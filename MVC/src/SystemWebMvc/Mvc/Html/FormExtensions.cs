namespace System.Web.Mvc.Html {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class FormExtensions {
        public static MvcForm BeginForm(this HtmlHelper htmlHelper) {
            // generates <form action="{current url}" method="post">...</form>

            TagBuilder tagBuilder = new TagBuilder("form");

            tagBuilder.Attributes["action"] = htmlHelper.ViewContext.HttpContext.Request.Url.ToString();
            tagBuilder.Attributes["method"] = HtmlHelper.GetFormMethodString(FormMethod.Post);

            HttpResponseBase httpResponse = htmlHelper.ViewContext.HttpContext.Response;
            httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcForm(htmlHelper.ViewContext.HttpContext.Response);
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, object values) {
            return BeginForm(htmlHelper, null, null, new RouteValueDictionary(values), FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, RouteValueDictionary values) {
            return BeginForm(htmlHelper, null, null, GetRouteValues(values), FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName) {
            return BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(), FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object values) {
            return BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(values), FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary values) {
            return BeginForm(htmlHelper, actionName, controllerName, GetRouteValues(values), FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method) {
            return BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(), method, new RouteValueDictionary());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object values, FormMethod method) {
            return BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(values), method, new RouteValueDictionary());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary values, FormMethod method) {
            return BeginForm(htmlHelper, actionName, controllerName, GetRouteValues(values), method, new RouteValueDictionary());
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method, object htmlAttributes) {
            return BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(), method, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method, IDictionary<string, object> htmlAttributes) {
            return BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(), method, htmlAttributes);
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object values, FormMethod method, object htmlAttributes) {
            return BeginForm(htmlHelper, actionName, controllerName, new RouteValueDictionary(values), method, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary values, FormMethod method, IDictionary<string, object> htmlAttributes) {
            TagBuilder tagBuilder = new TagBuilder("form");
            tagBuilder.MergeAttributes(htmlAttributes);
            string formAction = UrlHelper.GenerateUrl(null /* routeName */, actionName, controllerName, GetRouteValues(values), htmlHelper.RouteCollection, htmlHelper.ViewContext);
            tagBuilder.MergeAttribute("action", formAction);
            tagBuilder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method));

            HttpResponseBase httpResponse = htmlHelper.ViewContext.HttpContext.Response;
            httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new MvcForm(htmlHelper.ViewContext.HttpContext.Response);
        }

        public static void EndForm(this HtmlHelper htmlHelper) {
            HttpResponseBase httpResponse = htmlHelper.ViewContext.HttpContext.Response;
            httpResponse.Write("</form>");
        }

        private static RouteValueDictionary GetRouteValues(RouteValueDictionary values) {
            return values != null ? new RouteValueDictionary(values) : new RouteValueDictionary();
        }
    }
}
