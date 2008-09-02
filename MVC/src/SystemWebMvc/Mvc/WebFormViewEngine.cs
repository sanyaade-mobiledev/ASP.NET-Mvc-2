namespace System.Web.Mvc {
    using System.Web;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class WebFormViewEngine : VirtualPathProviderViewEngine {

        public WebFormViewEngine() {
            MasterLocationFormats = new[] {
                "~/Views/{1}/{0}.master",
                "~/Views/Shared/{0}.master"
            };

            ViewLocationFormats = new[] {
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx"
            };

            PartialViewLocationFormats = ViewLocationFormats;
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath) {
            return new WebFormView(partialPath, null);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath) {
            return new WebFormView(viewPath, masterPath);
        }
    }
}
