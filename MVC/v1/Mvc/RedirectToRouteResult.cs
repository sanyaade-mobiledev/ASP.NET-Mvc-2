namespace System.Web.Mvc {
    using System;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    // represents a result that performs a redirection given some values dictionary
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class RedirectToRouteResult : ActionResult {

        private RouteCollection _routes;

        public RedirectToRouteResult(RouteValueDictionary values) :
            this(null, values) {
        }

        public RedirectToRouteResult(string routeName, RouteValueDictionary values) {
            RouteName = routeName ?? String.Empty;
            Values = values ?? new RouteValueDictionary();
        }

        public string RouteName {
            get;
            private set;
        }

        public RouteValueDictionary Values {
            get;
            private set;
        }

        internal RouteCollection Routes {
            get {
                if (_routes == null) {
                    _routes = RouteTable.Routes;
                }
                return _routes;
            }
            set {
                _routes = value;
            }
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }

            VirtualPathData vpd = Routes.GetVirtualPath(context, RouteName, Values);
            if (vpd == null || String.IsNullOrEmpty(vpd.VirtualPath)) {
                throw new InvalidOperationException(MvcResources.ActionRedirectResult_NoRouteMatched);
            }
            string target = vpd.VirtualPath;
            context.HttpContext.Response.Redirect(target);
        }
    }
}
