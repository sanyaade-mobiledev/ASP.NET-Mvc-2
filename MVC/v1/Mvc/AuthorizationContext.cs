namespace System.Web.Mvc {
    using System;
    using System.Reflection;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AuthorizationContext : ControllerContext {

        public AuthorizationContext(ControllerContext controllerContext, MethodInfo actionMethod)
            : base(controllerContext) {

            if (actionMethod == null) {
                throw new ArgumentNullException("actionMethod");
            }

            ActionMethod = actionMethod;
        }

        public MethodInfo ActionMethod {
            get;
            private set;
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
