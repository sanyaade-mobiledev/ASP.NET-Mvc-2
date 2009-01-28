namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Serializable]
    public class ModelError {

        public ModelError(Exception exception)
            : this(exception, null /* errorMessage */) {
        }

        public ModelError(Exception exception, string errorMessage)
            : this(errorMessage) {
            if (exception == null) {
                throw new ArgumentNullException("exception");
            }

            Exception = exception;
        }

        public ModelError(string errorMessage) {
            ErrorMessage = errorMessage ?? String.Empty;
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
