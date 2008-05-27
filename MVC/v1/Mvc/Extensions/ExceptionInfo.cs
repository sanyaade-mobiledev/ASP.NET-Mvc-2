namespace System.Web.Mvc {
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ExceptionInfo {
        public string PropertyName {
            get;
            set;
        }

        public object AttemptedValue {
            get;
            set;
        }

        public string ErrorMessage {
            get;
            set;
        }
    }
}
