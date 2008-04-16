namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ResultExecutedContext : ControllerContext {

        public ResultExecutedContext(ControllerContext controllerContext, ActionResult result, Exception exception)
            : base(controllerContext) {

            if (result == null) {
                throw new ArgumentNullException("result");
            }

            Result = result;
            Exception = exception;
        }

        public Exception Exception {
            get;
            private set;
        }

        public bool ExceptionHandled {
            get;
            set;
        }

        public ActionResult Result {
            get;
            private set;
        }

    }
}
