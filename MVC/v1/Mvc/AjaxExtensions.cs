namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class AjaxExtensions {

        public static bool IsMvcAjaxRequest(this HttpRequestBase request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            return request["__MVCAJAX"] == "true";
        }
    }
}
