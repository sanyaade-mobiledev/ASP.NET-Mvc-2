namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class FilterAttribute : Attribute {

        private int _order = -1;

        public int Order {
            get {
                return _order;
            }
            set {
                if (value < -1) {
                    throw new ArgumentOutOfRangeException("value",
                        MvcResources.FilterAttribute_OrderOutOfRange);
                }
                _order = value;
            }
        }
    }
}
