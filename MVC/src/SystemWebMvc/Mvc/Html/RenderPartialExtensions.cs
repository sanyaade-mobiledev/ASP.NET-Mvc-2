namespace System.Web.Mvc.Html {
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class RenderPartialExtensions {
        // Renders the partial view with the parent's view data
        public static void RenderPartial(this HtmlHelper htmlHelper, string partialViewName) {
            htmlHelper.RenderPartialInternal(partialViewName, htmlHelper.ViewData, ViewEngines.DefaultEngine);
        }

        // Renders the partial view with the given view data
        public static void RenderPartial(this HtmlHelper htmlHelper, string partialViewName, ViewDataDictionary viewData) {
            if (viewData == null) {
                throw new ArgumentNullException("viewData");
            }
            htmlHelper.RenderPartialInternal(partialViewName, viewData, ViewEngines.DefaultEngine);
        }

        // Renders the partial view with an empty view data and the given model
        public static void RenderPartial(this HtmlHelper htmlHelper, string partialViewName, object model) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            htmlHelper.RenderPartialInternal(partialViewName, new ViewDataDictionary(model), ViewEngines.DefaultEngine);
        }

        // Renders the partial view with a copy of the given view data plus the given model
        public static void RenderPartial(this HtmlHelper htmlHelper, string partialViewName, object model, ViewDataDictionary viewData) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            if (viewData == null) {
                throw new ArgumentNullException("viewData");
            }
            htmlHelper.RenderPartialInternal(partialViewName, new ViewDataDictionary(viewData) { Model = model }, ViewEngines.DefaultEngine);
        }
    }
}