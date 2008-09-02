namespace Microsoft.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ComplexModelBinder : DefaultModelBinder {

        protected virtual IModelBinder GetBinder(Type modelType) {
            return ModelBinders.GetBinder(modelType);
        }

        public override object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(modelName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "modelName");
            }
            if (modelType == null) {
                throw new ArgumentNullException("modelType");
            }

            if (IsBasicType(modelType)) {
                return base.GetValue(controllerContext, modelName, modelType, modelState);
            }

            object o = Activator.CreateInstance(modelType);
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(o)) {
                string requestName = modelName + "." + prop.Name;
                Type propType = prop.PropertyType;
                IModelBinder converter = GetBinder(propType);
                object propValue = converter.GetValue(controllerContext, requestName, propType, modelState);
                prop.SetValue(o, propValue);
            }

            return o;
        }

        private static bool IsBasicType(Type type) {
            return (type.IsPrimitive ||
                type.IsEnum ||
                type == typeof(decimal) ||
                type == typeof(Guid) ||
                type == typeof(DateTime) ||
                type == typeof(string));
        }

    }
}
