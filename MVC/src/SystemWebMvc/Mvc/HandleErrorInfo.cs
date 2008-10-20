﻿namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class HandleErrorInfo {

        public HandleErrorInfo(Exception exception, string controllerName, string actionName) {
            if (exception == null) {
                throw new ArgumentNullException("exception");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            if (string.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }

            Exception = exception;
            ControllerName = controllerName;
            ActionName = actionName;
        }

        public string ActionName {
            get;
            private set;
        }

        public string ControllerName {
            get;
            private set;
        }

        public Exception Exception {
            get;
            private set;
        }

    }
}
