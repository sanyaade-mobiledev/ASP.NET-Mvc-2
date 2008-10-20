namespace System.Web.Mvc {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DefaultModelBinder : IModelBinder {

        public virtual ModelBinderResult BindModel(ModelBindingContext bindingContext) {
            if (bindingContext == null) {
                throw new ArgumentNullException("bindingContext");
            }

            // see if the value provider already returns an instance of the requested data type; if so, we can short-circuit
            // the evaluation and just return that instance.
            if (!String.IsNullOrEmpty(bindingContext.ModelName)) {
                ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (result != null && bindingContext.ModelType.IsInstanceOfType(result.RawValue)) {
                    bindingContext.ModelState.SetAttemptedValue(bindingContext.ModelName, result.AttemptedValue);
                    return new ModelBinderResult(result.RawValue);
                }
            }

            if (IsSimpleType(bindingContext.ModelType)) {
                // basic types (int, etc.) generally have string -> type converters, so just use those converters
                ModelBinderResult simpleResult = GetSimpleType(bindingContext);
                if (simpleResult != null || !bindingContext.ModelType.IsArray) {
                    return simpleResult;
                }
            }

            if (bindingContext.ModelType.IsArray) {
                return new ModelBinderResult(CreateArray(bindingContext, bindingContext.ModelType.GetElementType()));
            }

            // the new context creates the model if one doesn't already exist
            Func<object> modelProvider = () => bindingContext.Model ?? CreateModel(bindingContext, bindingContext.ModelType);
            ModelBindingContext newContext = new ModelBindingContext(bindingContext, bindingContext.ValueProvider, bindingContext.ModelType, bindingContext.ModelName, modelProvider, bindingContext.ModelState, bindingContext.ShouldUpdateProperty);

            // if ICollection<T> where T is a simple type, use simple list binding logic rather than custom list binding logic
            object simpleCollection = TryUpdateSimpleCollection(newContext);
            if (simpleCollection != null) {
                return new ModelBinderResult(simpleCollection);
            }

            // the BindModelCore() method contains the user's custom logic (or our own, if not subclassed)
            return BindModelCore(newContext);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We note that the property setter threw and invalidate the ModelState as a result.")]
        protected virtual ModelBinderResult BindModelCore(ModelBindingContext bindingContext) {
            // special-case IDictionary<,> and ICollection<>
            Type dictionaryType = bindingContext.ModelType.GetInterfaces().FirstOrDefault(IsDictionaryInterface);
            if (dictionaryType != null) {
                Type[] genericArguments = dictionaryType.GetGenericArguments();
                Type keyType = genericArguments[0];
                Type valueType = genericArguments[1];
                ModelBinderResult dictionary = UpdateDictionary(bindingContext, keyType, valueType);
                return dictionary;
            }

            Type collectionType = bindingContext.ModelType.GetInterfaces().FirstOrDefault(IsCollectionInterface);
            if (collectionType != null) {
                Type itemType = collectionType.GetGenericArguments()[0];
                ModelBinderResult collection = UpdateCollection(bindingContext, itemType);
                return collection;
            }

            Predicate<string> propFilter;
            BindAttribute bindAttr = (BindAttribute)TypeDescriptor.GetAttributes(bindingContext.ModelType)[typeof(BindAttribute)];
            if (bindAttr != null) {
                propFilter = property => bindAttr.IsPropertyAllowed(property) && bindingContext.ShouldUpdateProperty(property);
            }
            else {
                propFilter = bindingContext.ShouldUpdateProperty;
            }

            bool triedToSetModelProperty = false;

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(bindingContext.ModelType);
            foreach (PropertyDescriptor property in properties) {

                // compare the current property against the list of allowed properties for this type
                if (!propFilter(property.Name)) {
                    continue;
                }

                // we can't update value type properties that are read-only
                if (property.PropertyType.IsValueType && property.IsReadOnly) {
                    continue;
                }

                // use PropertyType rather than GetType() since we only want to update properties as
                // seen by the typed reference to the model
                ModelStateDictionary propertyState = new ModelStateDictionary();
                ModelBindingContext propertyContext = new ModelBindingContext(bindingContext, bindingContext.ValueProvider, bindingContext.ModelType, bindingContext.ModelName, null /* modelProvider */, propertyState, null /* propertyFilter */);
                ModelBinderResult propertyResult = BindProperty(propertyContext, property.PropertyType, () => property.GetValue(bindingContext.Model), property.Name);
                bindingContext.ModelState.Merge(propertyState);

                // if there wasn't even a form value for this property, just move on
                if (propertyResult == null) {
                    continue;
                }

                triedToSetModelProperty = true;
                bool shouldCallSetValue = false;
                string subPropertyName = CreateSubPropertyName(bindingContext.ModelName, property.Name);

                if (propertyResult.Value != null) {
                    if (!property.IsReadOnly) {
                        shouldCallSetValue = true;
                    }
                }
                else {
                    if (TypeHelpers.TypeAllowsNullValue(property.PropertyType)) {
                        // user probably explicitly wanted this value to be set to null
                        shouldCallSetValue = true;
                    }
                    else {
                        if (propertyState.IsValid) {
                            // user didn't type a value, but we can't set a value type to null
                            bindingContext.ModelState.AddModelError(subPropertyName, MvcResources.DefaultModelBinder_ValueRequired);
                        }
                    }
                }

                if (shouldCallSetValue) {
                    try {
                        property.SetValue(bindingContext.Model, propertyResult.Value);
                    }
                    catch {
                        // note that there was an error and just move on to the next property

                        // use CurrentCulture since this error message is displayed to the user browsing the site
                        string message = String.Format(CultureInfo.CurrentCulture, MvcResources.Common_ValueNotValidForProperty, propertyResult.Value);
                        bindingContext.ModelState.AddModelError(subPropertyName, message);
                    }
                }
            }

            return (triedToSetModelProperty) ? new ModelBinderResult(bindingContext.Model) : null;
        }

        protected ModelBinderResult BindProperty(ModelBindingContext parentContext, Type propertyType, Func<object> propertyValueProvider, string propertyName) {
            // the property name as understood by the value provider
            string newName = CreateSubPropertyName(parentContext.ModelName, propertyName);

            IModelBinder binder = GetBinder(propertyType);
            ModelBindingContext newContext = new ModelBindingContext(parentContext, parentContext.ValueProvider, propertyType, newName, propertyValueProvider, parentContext.ModelState, null /* propertyFilter */);
            ModelBinderResult result = binder.BindModel(newContext);
            return result;
        }

        protected static object ConvertSimpleArrayType(CultureInfo culture, object value, Type destinationType) {
            if (value == null || destinationType.IsInstanceOfType(value)) {
                return value;
            }

            // array conversion results in four cases, as below
            Array valueAsArray = value as Array;
            if (destinationType.IsArray) {
                Type destinationElementType = destinationType.GetElementType();
                if (valueAsArray != null) {
                    // case 1: both destination + source type are arrays, so convert each element
                    IList converted = Array.CreateInstance(destinationElementType, valueAsArray.Length);
                    for (int i = 0; i < valueAsArray.Length; i++) {
                        converted[i] = ConvertSimpleType(culture, valueAsArray.GetValue(i), destinationElementType);
                    }
                    return converted;
                }
                else {
                    // case 2: destination type is array but source is single element, so wrap element in array + convert
                    object element = ConvertSimpleType(culture, value, destinationElementType);
                    IList converted = Array.CreateInstance(destinationElementType, 1);
                    converted[0] = element;
                    return converted;
                }
            }
            else if (valueAsArray != null) {
                // case 3: destination type is single element but source is array, so extract first element + convert
                if (valueAsArray.Length > 0) {
                    value = valueAsArray.GetValue(0);
                    return ConvertSimpleType(culture, value, destinationType);
                }
                else {
                    // case 3(a): source is empty array, so can't perform conversion
                    return null;
                }
            }
            // case 4: both destination + source type are single elements, so convert
            return ConvertSimpleType(culture, value, destinationType);
        }

        protected static object ConvertSimpleType(CultureInfo culture, object value, Type destinationType) {
            if (value == null || destinationType.IsInstanceOfType(value)) {
                return value;
            }

            // if this is a user-input value but the user didn't type anything, return no value
            string valueAsString = value as string;
            if (valueAsString != null && valueAsString.Length == 0) {
                return null;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool canConvertFrom = converter.CanConvertFrom(value.GetType());
            if (!canConvertFrom) {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!(canConvertFrom || converter.CanConvertTo(destinationType))) {
                string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.DefaultModelBinder_NoConverterExists,
                    value.GetType().FullName, destinationType.FullName);
                throw new InvalidOperationException(message);
            }

            try {
                object convertedValue = (canConvertFrom) ?
                     converter.ConvertFrom(null /* context */, culture, value) :
                     converter.ConvertTo(null /* context */, culture, value, destinationType);
                return convertedValue;
            }
            catch (Exception ex) {
                string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.DefaultModelBinder_ConversionThrew,
                    value.GetType().FullName, destinationType.FullName);
                throw new InvalidOperationException(message, ex);
            }
        }

        // this method is specifically for creating an array of types using the index wire format
        private Array CreateArray(ModelBindingContext bindingContext, Type elementType) {
            // special support for creating arrays - create and bind a list, then coerce it to an array
            Type listType = typeof(List<>).MakeGenericType(elementType);
            IList list = (IList)CreateModel(bindingContext, listType);

            ModelBindingContext newContext = new ModelBindingContext(bindingContext, bindingContext.ValueProvider, listType, bindingContext.ModelName, () => list, bindingContext.ModelState, null /* propertyFilter */);
            BindModelCore(newContext);

            Array array = Array.CreateInstance(elementType, list.Count);
            for (int i = 0; i < list.Count; i++) {
                array.SetValue(list[i], i);
            }
            return array;
        }

        protected virtual object CreateModel(ModelBindingContext bindingContext, Type modelType) {
            Type typeToCreate = modelType;

            // we can understand some collection interfaces, e.g. IList<>, IDictionary<,>
            if (modelType.IsGenericType) {
                Type genericTypeDefinition = modelType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(IDictionary<,>)) {
                    typeToCreate = typeof(Dictionary<,>).MakeGenericType(modelType.GetGenericArguments());
                }
                else if (genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IList<>)) {
                    typeToCreate = typeof(List<>).MakeGenericType(modelType.GetGenericArguments());
                }
            }

            // fallback to the type's default constructor
            return Activator.CreateInstance(typeToCreate);
        }

        protected static string CreateSubIndexName(string prefix, string indexName) {
            return (!String.IsNullOrEmpty(prefix)) ? prefix + "[" + indexName + "]" : "[" + indexName + "]";
        }

        protected static string CreateSubPropertyName(string prefix, string propertyName) {
            return (!String.IsNullOrEmpty(prefix)) ? prefix + "." + propertyName : propertyName;
        }

        protected virtual IModelBinder GetBinder(Type modelType) {
            IModelBinder binder = ModelBinders.GetBinder(modelType);
            return binder;
        }

        private static Type GetElementType(Type type) {
            // currently this method handles only T[] and T?
            if (type.IsArray) {
                return type.GetElementType();
            }

            // Nullable.GetUnderlyingType() returns null if the provided type is not nullable
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We want to replace the exception text with our own error message.")]
        protected static ModelBinderResult GetSimpleType(ModelBindingContext bindingContext) {
            ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (result == null) {
                return null;
            }

            bindingContext.ModelState.SetAttemptedValue(bindingContext.ModelName, result.AttemptedValue);
            try {
                object convertedValue = ConvertSimpleArrayType(result.Culture, result.RawValue, bindingContext.ModelType);
                return new ModelBinderResult(convertedValue);
            }
            catch {
                // need to use current culture since this message is potentially displayed to the end user
                string message = String.Format(CultureInfo.CurrentCulture, MvcResources.Common_ValueNotValidForProperty,
                    result.AttemptedValue);
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, message);
                return new ModelBinderResult(null);
            }
        }

        private static bool IsCollectionInterface(Type type) {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        private static bool IsDictionaryInterface(Type type) {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        protected static bool IsSimpleType(Type type) {
            // simple data types are structs (which likely have their own type converters) and strings
            Type elementType = GetElementType(type);
            return (elementType.IsValueType || elementType == typeof(string));
        }

        private static object TryUpdateSimpleCollection(ModelBindingContext bindingContext) {
            Type genericCollectionType = bindingContext.ModelType.GetInterfaces().FirstOrDefault(IsCollectionInterface);
            if (genericCollectionType == null) {
                // there is no ICollection<T> interface we can use
                return null;
            }

            Type elementType = genericCollectionType.GetGenericArguments()[0];
            if (!(elementType == typeof(string) || elementType.IsValueType)) {
                // T is a complex type
                return null;
            }

            ModelBindingContext newContext = new ModelBindingContext(bindingContext, bindingContext.ValueProvider, elementType.MakeArrayType(), bindingContext.ModelName, null /* modelProvider */, bindingContext.ModelState, null /* propertyFilter */);
            ModelBinderResult convertedResult = GetSimpleType(newContext);
            IList convertedValues = (convertedResult != null) ? convertedResult.Value as IList : null;
            if (convertedValues == null || convertedValues.Count == 0) {
                // we couldn't find any values using the simple list binding algorithm
                return null;
            }

            // if we got any items, put them in the collection and report success
            MethodInfo addMethod = genericCollectionType.GetMethod("Add");
            MethodInfo clearMethod = genericCollectionType.GetMethod("Clear");
            object collection = bindingContext.Model;

            clearMethod.Invoke(collection, null /* parameters */);
            foreach (object convertedValue in convertedValues) {
                addMethod.Invoke(collection, new object[] { convertedValue });
            }
            return collection;
        }

        private ModelBinderResult UpdateCollection(ModelBindingContext bindingContext, Type itemType) {
            // first, need to read the set of all unique indices
            string indicesFieldName = CreateSubPropertyName(bindingContext.ModelName, "index");
            ModelBindingContext indicesContext = new ModelBindingContext(bindingContext, bindingContext.ValueProvider, typeof(string[]), indicesFieldName, null /* modelProvider */, bindingContext.ModelState, null /* propertyFilter */);
            ModelBinderResult indicesResult = GetSimpleType(indicesContext);
            string[] indices = (indicesResult != null) ? indicesResult.Value as string[] : null;

            // loop through entries
            List<object> convertedValues = new List<object>();
            if (indices != null) {
                IModelBinder itemBinder = GetBinder(itemType);
                foreach (string index in indices) {
                    string itemName = CreateSubIndexName(bindingContext.ModelName, index);
                    ModelBindingContext itemContext = new ModelBindingContext(bindingContext, bindingContext.ValueProvider, itemType, itemName, null /* modelProvider */, bindingContext.ModelState, null /* propertyFilter */);
                    object convertedValue = itemBinder.BindModel(itemContext).Value;
                    convertedValues.Add(convertedValue);
                }
            }

            // if there weren't any entries in the list, return that we did nothing
            if (convertedValues.Count == 0) {
                return null;
            }

            // if there were entries in the list, replace the collection
            Type collectionType = typeof(ICollection<>).MakeGenericType(itemType);
            MethodInfo addMethod = collectionType.GetMethod("Add");
            MethodInfo clearMethod = collectionType.GetMethod("Clear");

            object collection = bindingContext.Model;
            clearMethod.Invoke(collection, null /* parameters */);
            foreach (object convertedValue in convertedValues) {
                addMethod.Invoke(collection, new object[] { convertedValue });
            }
            return new ModelBinderResult(collection);
        }

        private ModelBinderResult UpdateDictionary(ModelBindingContext bindingContext, Type keyType, Type valueType) {
            // first, need to read the set of all unique indices
            string indicesFieldName = CreateSubPropertyName(bindingContext.ModelName, "index");
            ModelBindingContext indicesContext = new ModelBindingContext(bindingContext, bindingContext.ValueProvider, typeof(string[]), indicesFieldName, null /* modelProvider */, bindingContext.ModelState, null /* propertyFilter */);
            ModelBinderResult indicesResult = GetSimpleType(indicesContext);
            string[] indices = (indicesResult != null) ? indicesResult.Value as string[] : null;
            Type kvpType = typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType);

            // loop through entries
            List<KeyValuePair<object, object>> convertedValues = new List<KeyValuePair<object, object>>();
            if (indices != null) {
                foreach (string index in indices) {
                    string itemName = CreateSubIndexName(bindingContext.ModelName, index);
                    ModelBindingContext itemContext = new ModelBindingContext(bindingContext, bindingContext.ValueProvider, kvpType, itemName, null /* modelProvider */, bindingContext.ModelState, null /* propertyFilter */);

                    object key = BindProperty(itemContext, keyType, null /* propertyValueProvider */, "key").Value;
                    object value = BindProperty(itemContext, valueType, null /* propertyValueProvider */, "value").Value;
                    convertedValues.Add(new KeyValuePair<object, object>(key, value));
                }
            }

            // if there weren't any entries in the list, return that we did nothing
            if (convertedValues.Count == 0) {
                return null;
            }

            // if there were entries in the list, replace the collection
            MethodInfo clearMethod = typeof(ICollection<>).MakeGenericType(kvpType).GetMethod("Clear");
            PropertyInfo itemProperty = typeof(IDictionary<,>).MakeGenericType(keyType, valueType).GetProperty("Item");

            object dictionary = bindingContext.Model;
            clearMethod.Invoke(dictionary, null /* parameters */);
            foreach (var convertedValue in convertedValues) {
                itemProperty.SetValue(dictionary, convertedValue.Value, new object[] { convertedValue.Key });
            }
            return new ModelBinderResult(dictionary);
        }

    }
}
