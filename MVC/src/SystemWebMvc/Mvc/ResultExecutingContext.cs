namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ResultExecutingContext : ControllerContext {

        public ResultExecutingContext(ControllerContext controllerContext, ActionResult result)
            : base(controllerContext) {

            if (result == null) {
                throw new ArgumentNullException("result");
            }

            Result = result;
        }

        public bool Cancel {
            get;
            set;
        }

        public ActionResult Result {
            get;
            private set;
        }

    }
}
