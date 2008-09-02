namespace Microsoft.Web.Mvc {
    using System;
    using System.Web.Mvc;
    using System.Web;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ContentTypeAttribute : ActionFilterAttribute {
        public ContentTypeAttribute(string contentType) {
            if (String.IsNullOrEmpty(contentType)) {
                throw new ArgumentException(Resources.MvcResources.Common_NullOrEmpty, "contentType");
            }
            
            ContentType = contentType;
        }

        public string ContentType {
            get;
            private set;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext) {
            filterContext.HttpContext.Response.ContentType = ContentType;
        }
    }
}

