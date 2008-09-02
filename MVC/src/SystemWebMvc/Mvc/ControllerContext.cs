namespace System.Web.Mvc {
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ControllerContext : RequestContext {
        protected internal ControllerContext(ControllerContext controllerContext)
            : this(GetControllerContext(controllerContext), GetControllerContext(controllerContext).Controller) {
        }

        public ControllerContext(HttpContextBase httpContext, RouteData routeData, ControllerBase controller)
            : base(httpContext, routeData) {
            if (controller == null) {
                throw new ArgumentNullException("controller");
            }
            Controller = controller;
        }

        public ControllerContext(RequestContext requestContext, ControllerBase controller)
            : this(GetRequestContext(requestContext).HttpContext, GetRequestContext(requestContext).RouteData, controller) {
        }

        public ControllerBase Controller {
            get;
            private set;
        }

        internal static RequestContext GetRequestContext(RequestContext requestContext) {
            if (requestContext == null) {
                throw new ArgumentNullException("requestContext");
            }
            return requestContext;
        }

        internal static ControllerContext GetControllerContext(ControllerContext controllerContext) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            return controllerContext;
        }
    }
}
