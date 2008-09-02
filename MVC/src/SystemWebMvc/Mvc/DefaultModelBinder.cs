namespace System.Web.Mvc {
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DefaultModelBinder : IModelBinder {

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Conversion errors are not always fatal.")]
        protected internal virtual object ConvertType(CultureInfo culture, object value, Type destinationType) {
            if (destinationType == null) {
                throw new ArgumentNullException("destinationType");
            }
            if (value == null || destinationType.IsInstanceOfType(value)) {
                return value;
            }

            // array conversion results in four cases, as below
            Array valueAsArray = value as Array;
            if (destinationType.IsArray) {
                Type destinationElementType = destinationType.GetElementType();
                if (valueAsArray != null) {
                    // case 1: both destination + source type are arrays, so convert each element
                    IList valueAsList = (IList)valueAsArray;
                    IList converted = Array.CreateInstance(destinationElementType, valueAsList.Count);
                    for (int i = 0; i < valueAsList.Count; i++) {
                        converted[i] = ConvertType(culture, valueAsList[i], destinationElementType);
                    }
                    return converted;
                }
                else {
                    // case 2: destination type is array but source is single element, so wrap element in array + convert
                    object element = ConvertType(culture, value, destinationElementType);
                    IList converted = Array.CreateInstance(destinationElementType, 1);
                    converted[0] = element;
                    return converted;
                }
            }
            else if (valueAsArray != null) {
                // case 3: destination type is single element but source is array, so extract first element + convert
                IList valueAsList = (IList)valueAsArray;
                if (valueAsList.Count > 0) {
                    value = valueAsList[0];
                }
                // .. fallthrough to case 4
            }
            // case 4: both destination + source type are single elements, so convert

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
                CultureInfo cultureToUse = culture ?? CultureInfo.CurrentCulture;
                object convertedValue = (canConvertFrom) ?
                     converter.ConvertFrom(null /* context */, cultureToUse, value) :
                     converter.ConvertTo(null /* context */, cultureToUse, value, destinationType);
                return convertedValue;
            }
            catch (Exception ex) {
                string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.DefaultModelBinder_ConversionThrew,
                    value.GetType().FullName, destinationType.FullName);
                throw new InvalidOperationException(message, ex);
            }
        }

        public virtual object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(modelName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "modelName");
            }
            if (modelType == null) {
                throw new ArgumentNullException("modelType");
            }

            // Try to get a value for the parameter. We use this order of precedence:
            // 1. Values from the RouteData (could be from the typed-in URL or from the route's default values)
            // 2. URI query string
            // 3. Request form submission (should be culture-aware)

            object parameterValue = null;
            CultureInfo culture = CultureInfo.InvariantCulture;
            string attemptedValue = String.Empty;

            if (controllerContext.RouteData != null && controllerContext.RouteData.Values.TryGetValue(modelName, out parameterValue)) {
                attemptedValue = Convert.ToString(parameterValue, CultureInfo.InvariantCulture);
            }
            else {
                HttpRequestBase request = controllerContext.HttpContext.Request;
                if (request != null) {
                    if (request.QueryString != null) {
                        parameterValue = request.QueryString.GetValues(modelName);
                        attemptedValue = request.QueryString[modelName];
                    }
                    if (parameterValue == null && request.Form != null) {
                        culture = CultureInfo.CurrentCulture;
                        parameterValue = request.Form.GetValues(modelName);
                        attemptedValue = request.Form[modelName];
                    }
                }
            }

            try {
                object convertedValue = ConvertType(culture, parameterValue, modelType);
                return convertedValue;
            }
            catch {
                if (modelState == null) {
                    throw;
                }

                // need to use current culture since this message is potentially displayed to the end user
                string message = String.Format(CultureInfo.CurrentCulture, MvcResources.Common_ValueNotValidForProperty,
                    attemptedValue, modelName);
                modelState.AddModelError(modelName, attemptedValue, message);
                return null;
            }
        }

    }
}
