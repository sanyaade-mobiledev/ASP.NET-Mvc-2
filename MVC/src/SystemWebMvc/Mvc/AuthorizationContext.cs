namespace System.Web.Mvc {
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AuthorizationContext : ControllerContext {

        public AuthorizationContext(ControllerContext controllerContext)
            : base(controllerContext) {
        }

        public bool Cancel {
            get;
            set;
        }

        public ActionResult Result {
            get;
            set;
        }
    }
}
