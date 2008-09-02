namespace Microsoft.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class FormCollectionModelBinder : DefaultModelBinder {

        public FormCollectionModelBinder(FormCollection collection) {
            if (collection == null) {
                throw new ArgumentNullException("collection");
            }

            Collection = collection;
        }

        public FormCollection Collection {
            get;
            private set;
        }

        public override object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState) {
            if (String.IsNullOrEmpty(modelName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "modelName");
            }
            if (modelType == null) {
                throw new ArgumentNullException("modelType");
            }

            string[] parameterValue = Collection.GetValues(modelName);
            string attemptedValue = Collection[modelName];

            try {
                // form conversions should be culture-aware
                object convertedValue = ConvertType(CultureInfo.CurrentCulture, parameterValue, modelType);
                return convertedValue;
            }
            catch {
                if (modelState == null) {
                    throw;
                }

                string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.DefaultModelBinder_CouldNotConvert,
                    attemptedValue, modelType.FullName);
                modelState.AddModelError(modelName, attemptedValue, message);
                return null;
            }
        }

    }
}
