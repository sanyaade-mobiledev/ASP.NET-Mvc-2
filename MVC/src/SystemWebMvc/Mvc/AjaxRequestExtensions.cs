namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class AjaxRequestExtensions {

        public static bool IsAjaxRequest(this HttpRequestBase request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }
            
            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}
