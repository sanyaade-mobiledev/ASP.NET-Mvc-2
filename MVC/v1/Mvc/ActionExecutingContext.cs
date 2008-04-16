namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ActionExecutingContext : ControllerContext {

        public ActionExecutingContext(ControllerContext controllerContext, MethodInfo actionMethod, IDictionary<string, object> actionParameters)
            : base(controllerContext) {

            if (actionMethod == null) {
                throw new ArgumentNullException("actionMethod");
            }
            if (actionParameters == null) {
                throw new ArgumentNullException("actionParameters");
            }

            ActionMethod = actionMethod;
            ActionParameters = actionParameters;
        }

        public MethodInfo ActionMethod {
            get;
            private set;
        }

        public IDictionary<string, object> ActionParameters {
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
