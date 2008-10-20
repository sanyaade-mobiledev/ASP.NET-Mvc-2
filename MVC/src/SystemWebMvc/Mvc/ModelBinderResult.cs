namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ModelBinderResult {

        public ModelBinderResult(object value) {
            Value = value;
        }

        public object Value {
            get;
            private set;
        }

    }
}
