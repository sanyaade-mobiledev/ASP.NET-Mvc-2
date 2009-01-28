namespace Microsoft.Web.Mvc {
    using System;
    using System.Data.Linq;
    using System.Web;
    using System.Web.Mvc;

    // Register via a call in Global.asax.cs to 
    // ModelBinders.Binders.Add(typeof(Binary), new LinqBinaryModelBinder());
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class LinqBinaryModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            ValueProviderResult valueResult;
            bindingContext.ValueProvider.TryGetValue(bindingContext.ModelName, out valueResult);

            // case 1: there was no <input ... /> element containing this data
            if (valueResult == null) {
                return null;
            }

            string value = valueResult.AttemptedValue;

            // case 2: there was an <input ... /> element but it was left blank
            if (String.IsNullOrEmpty(value)) {
                return null;
            }

            // Future proofing. Right now, Binary.ToString() includes quotes around 
            // the base64 encoded value. This may get fixed in the future.
            string realValue = value.Replace("\"", string.Empty);
            var byteValue = new Binary(Convert.FromBase64String(realValue));
            return byteValue;
        }
    }
}
