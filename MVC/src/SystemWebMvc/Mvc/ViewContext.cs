namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewContext : ControllerContext {

        public ViewContext(HttpContextBase httpContext, RouteData routeData, ControllerBase controller, IView view, ViewDataDictionary viewData, TempDataDictionary tempData)
            : base(httpContext, routeData, controller) {
            if (view == null) {
                throw new ArgumentNullException("view");
            }

            ViewData = viewData;
            TempData = tempData;
            View = view;
        }

        public ViewContext(ControllerContext controllerContext, IView view, ViewDataDictionary viewData, TempDataDictionary tempData)
            : this(GetControllerContext(controllerContext).HttpContext,
                   GetControllerContext(controllerContext).RouteData,
                   GetControllerContext(controllerContext).Controller,
                   view,
                   viewData,
                   tempData) {
        }

        public TempDataDictionary TempData {
            get;
            private set;
        }

        public IView View {
            get;
            private set;
        }

        public ViewDataDictionary ViewData {
            get;
            private set;
        }
    }
}
