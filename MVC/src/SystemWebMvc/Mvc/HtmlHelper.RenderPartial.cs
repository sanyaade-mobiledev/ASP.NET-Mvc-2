namespace System.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc.Resources;

    public partial class HtmlHelper {

        private static IView FindPartialView(IViewEngine engine, ViewContext viewContext, string partialViewName) {
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

        // Renders the partial view with the parent's view data
        public void RenderPartial(string partialViewName) {
            RenderPartialInternal(partialViewName, ViewData, ViewEngines.DefaultEngine);
        }

        // Renders the partial view with the given view data
        public void RenderPartial(string partialViewName, ViewDataDictionary viewData) {
            if (viewData == null) {
                throw new ArgumentNullException("viewData");
            }
            RenderPartialInternal(partialViewName, viewData, ViewEngines.DefaultEngine);
        }

        // Renders the partial view with an empty view data and the given model
        public void RenderPartial(string partialViewName, object model) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            RenderPartialInternal(partialViewName, new ViewDataDictionary(model), ViewEngines.DefaultEngine);
        }

        // Renders the partial view with a copy of the given view data plus the given model
        public void RenderPartial(string partialViewName, object model, ViewDataDictionary viewData) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            if (viewData == null) {
                throw new ArgumentNullException("viewData");
            }
            RenderPartialInternal(partialViewName, new ViewDataDictionary(viewData) { Model = model }, ViewEngines.DefaultEngine);
        }

        internal virtual void RenderPartialInternal(string partialViewName, ViewDataDictionary viewData,
            IViewEngine engine) {

            if (String.IsNullOrEmpty(partialViewName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "partialViewName");
            }

            ViewContext newViewContext = new ViewContext(ViewContext, partialViewName, viewData, ViewContext.TempData);
            IView view = FindPartialView(engine, newViewContext, partialViewName);
            view.Render(newViewContext, ViewContext.HttpContext.Response.Output);
        }

    }
}