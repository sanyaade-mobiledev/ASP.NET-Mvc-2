namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ActionExecutingContext : ControllerContext {

        public ActionExecutingContext(ControllerContext controllerContext, IDictionary<string, object> actionParameters)
            : base(controllerContext) {

            if (actionParameters == null) {
                throw new ArgumentNullException("actionParameters");
            }

            ActionParameters = actionParameters;
        }

        public IDictionary<string, object> ActionParameters {
            get;
            private set;
        }

        public ActionResult Result {
            get;
            set;
        }

    }
}
