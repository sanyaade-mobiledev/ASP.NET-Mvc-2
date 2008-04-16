namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.Mvc.Resources;

    // represents a result that renders a view
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class RenderViewResult : ActionResult {

        public string MasterName {
            get;
            set;
        }

        public object ViewData {
            get;
            set;
        }

        public IViewEngine ViewEngine {
            get;
            set;
        }

        public string ViewName {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This property is settable so that unit tests can provide mock implementations.")]
        public TempDataDictionary TempData {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            if (ViewEngine == null) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                    MvcResources.Common_PropertyCannotBeNullOrEmpty, "ViewEngine"));
            }

            string viewName = (!String.IsNullOrEmpty(ViewName)) ? ViewName : context.RouteData.GetRequiredString("action");
            ViewContext viewContext = new ViewContext(context, viewName, MasterName, ViewData, TempData);
            ViewEngine.RenderView(viewContext);
        }

    }

}
