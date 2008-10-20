namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;
    using System.Text;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class HtmlHelper {
        internal const string ValidationMessageCssClassName = "field-validation-error";
        internal const string ValidationSummaryCssClassName = "validation-summary-errors";
        internal const string ValidationInputCssClassName = "input-validation-error";

        public HtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
            : this(viewContext, viewDataContainer, RouteTable.Routes) {
        }

        public HtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection) {
            if (viewContext == null) {
                throw new ArgumentNullException("viewContext");
            }
            if (viewDataContainer == null) {
                throw new ArgumentNullException("viewDataContainer");
            }
            if (routeCollection == null) {
                throw new ArgumentNullException("routeCollection");
            }
            ViewContext = viewContext;
            ViewDataContainer = viewDataContainer;
            RouteCollection = routeCollection;
        }

        public RouteCollection RouteCollection {
            get;
            private set;
        }

        public ViewContext ViewContext {
            get;
            private set;
        }

        public ViewDataDictionary ViewData {
            get {
                return ViewDataContainer.ViewData;
            }
        }

        public IViewDataContainer ViewDataContainer {
            get;
            private set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public string AttributeEncode(string html) {
            return (!String.IsNullOrEmpty(html)) ? HttpUtility.HtmlAttributeEncode(html) : String.Empty;
        }

        public string AttributeEncode(object value) {
            return AttributeEncode(Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public string Encode(string html) {
            return (!String.IsNullOrEmpty(html)) ? HttpUtility.HtmlEncode(html) : String.Empty;
        }

        public string Encode(object value) {
            return Encode(Convert.ToString(value, CultureInfo.CurrentCulture));
        }

        internal string EvalString(string key) {
            return Convert.ToString(ViewData.Eval(key), CultureInfo.CurrentCulture);
        }

        internal bool EvalBoolean(string key) {
            return Convert.ToBoolean(ViewData.Eval(key), CultureInfo.InvariantCulture);
        }

        internal static IView FindPartialView(IViewEngine engine, ViewContext viewContext, string partialViewName) {
            ViewEngineResult result = engine.FindPartialView(viewContext, partialViewName);
            if (result.View != null) {
                return result.View;
            }

            StringBuilder locationsText = new StringBuilder();
            foreach (string location in result.SearchedLocations) {
                locationsText.AppendLine();
                locationsText.Append(location);
            }

            throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                MvcResources.Common_PartialViewNotFound, partialViewName, locationsText));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public static string GetFormMethodString(FormMethod method) {
            switch (method) {
                case FormMethod.Get:
                    return "get";
                case FormMethod.Post:
                    return "post";
                default:
                    return "post";
            }
        }

        internal string GetModelAttemptedValue(string key) {
            ModelState modelState;
            if (ViewData.ModelState.TryGetValue(key, out modelState)) {
                return modelState.AttemptedValue;
            }
            return null;
        }

        internal virtual void RenderPartialInternal(string partialViewName, ViewDataDictionary viewData, IViewEngine engine) {
            if (String.IsNullOrEmpty(partialViewName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "partialViewName");
            }

            ViewContext newViewContext = new ViewContext(ViewContext, ViewContext.View, viewData, ViewContext.TempData);
            IView view = FindPartialView(engine, newViewContext, partialViewName);
            view.Render(newViewContext, ViewContext.HttpContext.Response.Output);
        }
    }
}
