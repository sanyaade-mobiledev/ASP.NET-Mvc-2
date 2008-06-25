using System;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Web.DynamicData {
    internal class SimpleDynamicDataSourceView : DataSourceView {

        private SimpleDynamicDataSource _owner;

        public SimpleDynamicDataSourceView(SimpleDynamicDataSource owner, string viewName)
            : base(owner, viewName) {
            _owner = owner;
        }
        
        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments) {
            return new object[] { _owner.DataObject };
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues) {

            object newObject;

            try {
                newObject = BuildDataObject(_owner.DataObjectType, values);
            }
            catch (LinqDataSourceValidationException e) {
                // allow user to handle conversion or dlinq property validation exceptions.
                _owner.OnException(new DynamicValidatorEventArgs(e, DynamicDataSourceOperation.Update));
                throw;
            }

            var eventArgs = new SimpleDynamicDataSourceCompleteEventArgs() { NewObject = newObject };

            _owner.OnComplete(eventArgs);

            return 1;
        }

        private object BuildDataObject(Type dataObjectType, IDictionary inputParameters) {
            object dataObject = _owner.DataObject;

            Dictionary<string, Exception> convertOrValidateExceptions = null;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(dataObject);
            foreach (DictionaryEntry de in inputParameters) {
                string propName = (de.Key == null ? String.Empty : de.Key.ToString());
                PropertyDescriptor property = props.Find(propName, /*ignoreCase*/true);
                // NOTE: No longer throws when a property is not found or is read only.  This makes
                // Delete, Insert and Update operations more optimistic, allowing scenarios such as:
                // 1) Deletes and Updates after projecting data in the Selecting event.
                // 2) Deletes and Updates after selecting children of the data object type in the
                //    Selecting event.
                if ((property != null) && (!property.IsReadOnly)) {
                    try {
                        object value = BuildObjectValue(de.Value, property.PropertyType, propName);
                        property.SetValue(dataObject, value);
                    }
                    catch (Exception e) {
                        if (convertOrValidateExceptions == null) {
                            convertOrValidateExceptions = new Dictionary<string, Exception>(
                                StringComparer.OrdinalIgnoreCase);
                        }
                        convertOrValidateExceptions[property.Name] = e;
                    }
                }
            }

            // package up conversion or dlinq validation exceptions into single exception.
            if (convertOrValidateExceptions != null) {
                // Include the text of the first exception as part of the full exception,
                // to make it less cryptic in scenarios where it gets shown.
                throw new LinqDataSourceValidationException(String.Format(CultureInfo.InvariantCulture,
                    "AtlasWeb.LinqDataSourceView_ValidationFailed", dataObjectType, convertOrValidateExceptions.Values.First().Message),
                    convertOrValidateExceptions);
            }

            return dataObject;
        }

        internal static object BuildObjectValue(object value, Type destinationType, string paramName) {
            // NOTE: This method came from LinqDataSource with no changes made.
            // Only consider converting the type if the value is non-null and the types don't match
            if ((value != null) && (!destinationType.IsInstanceOfType(value))) {
                Type innerDestinationType = destinationType;
                bool isNullable = false;
                if (destinationType.IsGenericType &&
                    (destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))) {
                    innerDestinationType = destinationType.GetGenericArguments()[0];
                    isNullable = true;
                }
                else {
                    if (destinationType.IsByRef) {
                        innerDestinationType = destinationType.GetElementType();
                    }
                }

                // Try to convert from for example string to DateTime, so that
                // afterwards we can convert DateTime to Nullable<DateTime>

                // If the value is a string, we attempt to use a TypeConverter to convert it
                value = ConvertType(value, innerDestinationType, paramName);

                // Special-case the value when the destination is Nullable<T>
                if (isNullable) {
                    Type paramValueType = value.GetType();
                    if (innerDestinationType != paramValueType) {
                        // Throw if for example, we are trying to convert from int to Nullable<bool>
                        throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                            "AtlasWeb.LinqDataSourceView_CannotConvertType", paramName, paramValueType.FullName,
                            String.Format(CultureInfo.InvariantCulture, "Nullable<{0}>",
                            destinationType.GetGenericArguments()[0].FullName)));
                    }
                }
            }
            return value;
        }

        private static object ConvertType(object value, Type type, string paramName) {
            // NOTE: This method came from LinqDataSource with no changes made.
            string s = value as string;
            if (s != null) {
                // Get the type converter for the destination type
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter != null) {
                    // Perform the conversion
                    try {
                        value = converter.ConvertFromString(s);
                    }
                    catch (NotSupportedException) {
                        throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                            "AtlasWeb.LinqDataSourceView_CannotConvertType", paramName, typeof(string).FullName,
                            type.FullName));
                    }
                    catch (FormatException) {
                        throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                            "AtlasWeb.LinqDataSourceView_CannotConvertType", paramName, typeof(string).FullName,
                            type.FullName));
                    }
                }
            }
            return value;
        }
    }

}

