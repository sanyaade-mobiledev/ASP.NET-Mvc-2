namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class HandleErrorInfo {

        public HandleErrorInfo(Exception exception, string controller, string action) {
            if (exception == null) {
                throw new ArgumentNullException("exception");
            }
            if (String.IsNullOrEmpty(controller)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controller");
            }
            if (string.IsNullOrEmpty(action)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "action");
            }

            Exception = exception;
            Controller = controller;
            Action = action;
        }

        public string Action {
            get;
            private set;
        }

        public string Controller {
            get;
            private set;
        }

        public Exception Exception {
            get;
            private set;
        }

    }
}
