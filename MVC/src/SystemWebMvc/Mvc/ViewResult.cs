namespace System.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewResult : PartialViewResult {

        private string _masterName;

        public string MasterName {
            get {
                return _masterName ?? String.Empty;
            }
            set {
                _masterName = value;
            }
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            if (String.IsNullOrEmpty(ViewName)) {
                ViewName = context.RouteData.GetRequiredString("action");
            }

            if (View == null) {
                View = FindView(context);
            }

            ViewContext viewContext = new ViewContext(context, ViewName, ViewData, TempData);
            View.Render(viewContext, context.HttpContext.Response.Output);
        }

        private IView FindView(ControllerContext controllerContext) {
            ViewEngineResult result = ViewEngine.FindView(controllerContext, ViewName, MasterName);
            if (result.View != null) {
                return result.View;
            }

            // we need to generate an exception containing all the locations we searched
            StringBuilder locationsText = new StringBuilder();
            foreach (string location in result.SearchedLocations) {
                locationsText.AppendLine();
                locationsText.Append(location);
            }
            throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                MvcResources.Common_ViewNotFound, ViewName, locationsText));
        }

    }
}
