namespace Microsoft.Web.Mvc {
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [FormCollectionBinder]
    public class FormCollection : NameValueCollection {

        public FormCollection() {
        }

        public FormCollection(NameValueCollection collection) {
            if (collection == null) {
                throw new ArgumentNullException("collection");
            }

            Add(collection);
        }

        private class FormCollectionBinderAttribute : CustomModelBinderAttribute {
            private static readonly FormCollectionInstantiator _instantiator = new FormCollectionInstantiator();
            public override IModelBinder GetBinder() {
                return _instantiator;
            }
        }

        private class FormCollectionInstantiator : IModelBinder {
            #region IModelBinder Members
            public object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState) {
                if (controllerContext == null) {
                    throw new ArgumentNullException("controllerContext");
                }
                if (modelType != typeof(FormCollection)) {
                    String message = String.Format(CultureInfo.CurrentUICulture, 
                        MvcResources.ModelBinder_WrongModelType, typeof(FormCollection).FullName);
                    throw new ArgumentException(message, "modelType");
                }

                return new FormCollection(controllerContext.HttpContext.Request.Form);
            }
            #endregion
        }

    }
}
