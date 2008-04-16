namespace System.Web.Mvc {
    using System;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    // represents a result that performs a redirection given some values dictionary
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ActionRedirectResult : ActionResult {

        private RouteCollection _routes;

        public ActionRedirectResult(RouteValueDictionary values) {
            Values = values;
        }

        public RouteValueDictionary Values {
            get;
            private set;
        }

        // TODO: Should this property even be here?
        // Right now we're using this only for unit-testing purposes.
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

            VirtualPathData vpd = Routes.GetVirtualPath(context, Values);
            if (vpd == null || String.IsNullOrEmpty(vpd.VirtualPath)) {
                throw new InvalidOperationException(MvcResources.ActionRedirectResult_NoRouteMatched);
            }
            string target = vpd.VirtualPath;
            context.HttpContext.Response.Redirect(target);
        }

    }

}
