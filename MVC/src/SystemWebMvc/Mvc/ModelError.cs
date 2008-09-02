namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ModelError {

        public ModelError(Exception exception) {
            if (exception == null) {
                throw new ArgumentNullException("exception");
            }

            Exception = exception;
            ErrorMessage = exception.Message;
        }

        public ModelError(string errorMessage) {
            if (String.IsNullOrEmpty(errorMessage)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "errorMessage");
            }

            ErrorMessage = errorMessage;
        }

        public Exception Exception {
            get;
            private set;
        }

        public string ErrorMessage {
            get;
            private set;
        }
    }
}
