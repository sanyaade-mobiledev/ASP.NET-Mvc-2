namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AuthorizationContext : ControllerContext {

        // parameterless constructor used for mocking
        public AuthorizationContext() {
        }

        public AuthorizationContext(ControllerContext controllerContext)
            : base(controllerContext) {
        }

        public ActionResult Result {
            get;
            set;
        }

    }
}
