namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AjaxHelper {
        private RouteCollection _routeCollection;

        public AjaxHelper(ViewContext viewContext) {
            if (viewContext == null) {
                throw new ArgumentNullException("viewContext");
            }
            ViewContext = viewContext;
        }

        internal RouteCollection RouteCollection {
            get {
                return _routeCollection ?? RouteTable.Routes;
            }
            set {
                _routeCollection = value;
            }
        }

        public ViewContext ViewContext {
            get;
            private set;
        }
    }
}
