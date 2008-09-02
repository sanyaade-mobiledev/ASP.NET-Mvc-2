namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class PartialViewResult : ActionResult {

        private TempDataDictionary _tempData;
        private ViewDataDictionary _viewData;
        private IViewEngine _viewEngine;
        private string _viewName;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This entire type is meant to be mutable.")]
        public TempDataDictionary TempData {
            get {
                if (_tempData == null) {
                    _tempData = new TempDataDictionary();
                }
                return _tempData;
            }
            set {
                _tempData = value;
            }
        }

        public IView View {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This entire type is meant to be mutable.")]
        public ViewDataDictionary ViewData {
            get {
                if (_viewData == null) {
                    _viewData = new ViewDataDictionary();
                }
                return _viewData;
            }
            set {
                _viewData = value;
            }
        }

        public IViewEngine ViewEngine {
            get {
                return _viewEngine ?? ViewEngines.DefaultEngine;
            }
            set {
                _viewEngine = value;
            }
        }

        public string ViewName {
            get {
                return _viewName ?? String.Empty;
            }
            set {
                _viewName = value;
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
                View = FindPartialView(context);
            }

            ViewContext viewContext = new ViewContext(context, ViewName, ViewData, TempData);
            View.Render(viewContext, context.HttpContext.Response.Output);
        }

        private IView FindPartialView(ControllerContext controllerContext) {
            ViewEngineResult result = ViewEngine.FindPartialView(controllerContext, ViewName);
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
                MvcResources.Common_PartialViewNotFound, ViewName, locationsText));
        }
    }
}
