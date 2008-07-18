using System;
using System.Security.Permissions;
using System.Web;

namespace Microsoft.Web.DynamicData.Mvc {
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ModelPropertyError {
        public ModelPropertyError(string errorMessage) {
            ErrorMessage = errorMessage;
        }

        public ModelPropertyError(Exception exception) {
            Exception = exception;
            ErrorMessage = exception.Message;
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
