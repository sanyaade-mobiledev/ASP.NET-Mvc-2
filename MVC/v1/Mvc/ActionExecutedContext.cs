namespace System.Web.Mvc {
    using System;
    using System.Reflection;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ActionExecutedContext : ControllerContext {

        private ActionResult _result;

        public ActionExecutedContext(ControllerContext controllerContext, MethodInfo actionMethod, bool canceled, Exception exception)
            : base(controllerContext) {

            if (actionMethod == null) {
                throw new ArgumentNullException("actionMethod");
            }

            ActionMethod = actionMethod;
            Canceled = canceled;
            Exception = exception;
        }

        public MethodInfo ActionMethod {
            get;
            private set;
        }

        public bool Canceled {
            get;
            private set;
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
            get {
                return _result ?? EmptyResult.Instance;
            }
            set {
                _result = value;
            }
        }
    }
}
