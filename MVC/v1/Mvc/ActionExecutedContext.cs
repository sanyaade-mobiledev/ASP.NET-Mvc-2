﻿namespace System.Web.Mvc {
    using System;
    using System.Reflection;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ActionExecutedContext : ControllerContext {

        public ActionExecutedContext(ControllerContext controllerContext, MethodInfo actionMethod, Exception exception)
            : base(controllerContext) {

            if (actionMethod == null) {
                throw new ArgumentNullException("actionMethod");
            }

            ActionMethod = actionMethod;
            Exception = exception;
        }

        public MethodInfo ActionMethod {
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
            get;
            set;
        }

    }
}