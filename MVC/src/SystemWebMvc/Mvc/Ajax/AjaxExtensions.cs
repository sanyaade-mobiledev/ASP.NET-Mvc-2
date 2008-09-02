namespace System.Web.Mvc.Ajax {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class AjaxExtensions {
        private const string LinkOnClickFormat = "Sys.Mvc.AsyncHyperlink.handleClick(this, new Sys.UI.DomEvent(event), {0});";
        private const string FormOnSubmitFormat = "Sys.Mvc.AsyncForm.handleSubmit(this, new Sys.UI.DomEvent(event), {0});";

        public static bool IsMvcAjaxRequest(this HttpRequestBase request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            return request["__MVCASYNCPOST"] == "true";
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, AjaxOptions ajaxOptions) {
            return ActionLink(ajaxHelper, linkText, actionName, (string)null /* controllerName */, ajaxOptions);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, object values, AjaxOptions ajaxOptions) {
            return ActionLink(ajaxHelper, linkText, actionName, (string)null /* controllerName */, values, ajaxOptions);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            return ActionLink(ajaxHelper, linkText, actionName, (string)null /* controllerName */, values, ajaxOptions, htmlAttributes);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions) {
            return ActionLink(ajaxHelper, linkText, actionName, (string)null /* controllerName */, values, ajaxOptions);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return ActionLink(ajaxHelper, linkText, actionName, (string)null /* controllerName */, values, ajaxOptions, htmlAttributes);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, AjaxOptions ajaxOptions) {
            return ActionLink(ajaxHelper, linkText, actionName, controllerName, null /* values */, ajaxOptions, null /* htmlAttributes */);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object values, AjaxOptions ajaxOptions) {
            return ActionLink(ajaxHelper, linkText, actionName, controllerName, values, ajaxOptions, null /* htmlAttributes */);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            RouteValueDictionary newValues = new RouteValueDictionary(values);
            Dictionary<string, object> newAttributes = ObjectToCaseSensitiveDictionary(htmlAttributes);
            return ActionLink(ajaxHelper, linkText, actionName, controllerName, newValues, ajaxOptions, newAttributes);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions) {
            return ActionLink(ajaxHelper, linkText, actionName, controllerName, values, ajaxOptions, null /* htmlAttributes */);
        }

        public static string ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (ajaxOptions == null) {
                throw new ArgumentNullException("ajaxOptions");
            }

            string targetUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, values ?? new RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext);

            return GenerateLink(linkText, targetUrl, ajaxOptions, htmlAttributes);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, AjaxOptions ajaxOptions) {
            return Form(ajaxHelper, actionName, (string)null /* controllerName */, ajaxOptions);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, object values, AjaxOptions ajaxOptions) {
            return Form(ajaxHelper, actionName, (string)null /* controllerName */, values, ajaxOptions);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            return Form(ajaxHelper, actionName, (string)null /* controllerName */, values, ajaxOptions, htmlAttributes);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions) {
            return Form(ajaxHelper, actionName, (string)null /* controllerName */, values, ajaxOptions);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return Form(ajaxHelper, actionName, (string)null /* controllerName */, values, ajaxOptions, htmlAttributes);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, string controllerName, AjaxOptions ajaxOptions) {
            return Form(ajaxHelper, actionName, controllerName, null /* values */, ajaxOptions, null /* htmlAttributes */);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, string controllerName, object values, AjaxOptions ajaxOptions) {
            return Form(ajaxHelper, actionName, controllerName, values, ajaxOptions, null /* htmlAttributes */);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, string controllerName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            RouteValueDictionary newValues = new RouteValueDictionary(values);
            Dictionary<string, object> newAttributes = ObjectToCaseSensitiveDictionary(htmlAttributes);
            return Form(ajaxHelper, actionName, controllerName, newValues, ajaxOptions, newAttributes);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions) {
            return Form(ajaxHelper, actionName, controllerName, values, ajaxOptions, null /* htmlAttributes */);
        }

        public static IDisposable Form(this AjaxHelper ajaxHelper, string actionName, string controllerName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (ajaxOptions == null) {
                throw new ArgumentNullException("ajaxOptions");
            }

            // get target URL
            string targetUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, valuesDictionary ?? new RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext);

            TagBuilder builder = new TagBuilder("form");

            builder.MergeAttributes(htmlAttributes);
            builder.MergeAttribute("action", targetUrl);
            builder.MergeAttribute("method", "post");
            builder.MergeAttribute("onsubmit", GenerateAjaxScript(ajaxOptions, FormOnSubmitFormat));

            HttpResponseBase response = ajaxHelper.ViewContext.HttpContext.Response;
            response.Write(builder.ToString(TagRenderMode.StartTag));
            return new DisposableForm(response);
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, object values, AjaxOptions ajaxOptions) {
            return RouteLink(ajaxHelper, linkText, null /* routeName */, new RouteValueDictionary(values), ajaxOptions,
                             new Dictionary<string, object>());
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, null /* routeName */, new RouteValueDictionary(values), ajaxOptions,
                             ObjectToCaseSensitiveDictionary(htmlAttributes));
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, object values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, null /* routeName */, new RouteValueDictionary(values), ajaxOptions,
                             htmlAttributes);
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions) {
            return RouteLink(ajaxHelper, linkText, null /* routeName */, valuesDictionary, ajaxOptions,
                             new Dictionary<string, object>());
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, object htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, null /* routeName */, valuesDictionary, ajaxOptions,
                             ObjectToCaseSensitiveDictionary(htmlAttributes));
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, null /* routeName */, valuesDictionary, ajaxOptions, htmlAttributes);
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, AjaxOptions ajaxOptions) {
            return RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(), ajaxOptions,
                             new Dictionary<string, object>());
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, AjaxOptions ajaxOptions, object htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(), ajaxOptions, ObjectToCaseSensitiveDictionary(htmlAttributes));
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(), ajaxOptions, htmlAttributes);
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, object values, AjaxOptions ajaxOptions) {
            return RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(values), ajaxOptions,
                             new Dictionary<string, object>());
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(values), ajaxOptions,
                             ObjectToCaseSensitiveDictionary(htmlAttributes));
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, object values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, routeName, new RouteValueDictionary(values), ajaxOptions, htmlAttributes);
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions) {
            return RouteLink(ajaxHelper, linkText, routeName, valuesDictionary, ajaxOptions, new Dictionary<string, object>());
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, object htmlAttributes) {
            return RouteLink(ajaxHelper, linkText, routeName, valuesDictionary, ajaxOptions, ObjectToCaseSensitiveDictionary(htmlAttributes));
        }

        public static string RouteLink(this AjaxHelper ajaxHelper, string linkText, string routeName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (ajaxOptions == null) {
                throw new ArgumentNullException("ajaxOptions");
            }

            string targetUrl = UrlHelper.GenerateUrl(routeName, null /* actionName */, null /* controllerName */, valuesDictionary ?? new RouteValueDictionary(), ajaxHelper.RouteCollection, ajaxHelper.ViewContext);

            return GenerateLink(linkText, targetUrl, ajaxOptions, htmlAttributes);
        }

        internal static string InsertionModeToString(InsertionMode insertionMode) {
            switch (insertionMode) {
                case InsertionMode.Replace:
                    return "Sys.Mvc.InsertionMode.replace";
                case InsertionMode.InsertBefore:
                    return "Sys.Mvc.InsertionMode.insertBefore";
                case InsertionMode.InsertAfter:
                    return "Sys.Mvc.InsertionMode.insertAfter";
                default:
                    return ((int)insertionMode).ToString(CultureInfo.InvariantCulture);
            }
        }

        private static Dictionary<string, object> ObjectToCaseSensitiveDictionary(object values) {
            Dictionary<string, object> dict = new Dictionary<string, object>(StringComparer.Ordinal);
            if (values != null) {
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(values)) {
                    object val = prop.GetValue(values);
                    dict[prop.Name] = val;
                }
            }
            return dict;
        }

        private static string GenerateLink(string linkText, string targetUrl, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            TagBuilder tag = new TagBuilder("a") {
                InnerHtml = HttpUtility.HtmlEncode(linkText)
            };

            tag.MergeAttributes(htmlAttributes);
            tag.MergeAttribute("href", targetUrl);
            tag.MergeAttribute("onclick", GenerateAjaxScript(ajaxOptions, LinkOnClickFormat));

            return tag.ToString(TagRenderMode.Normal);
        }

        private static string GenerateAjaxScript(AjaxOptions ajaxOptions, string scriptFormat) {
            string optionsString = ajaxOptions.ToJavascriptString();
            return String.Format(CultureInfo.InvariantCulture, scriptFormat, optionsString);
        }

        private sealed class DisposableForm : IDisposable {
            private HttpResponseBase _response;
            public DisposableForm(HttpResponseBase response) {
                _response = response;
            }
            public void Dispose() {
                _response.Write("</form>");
            }
        }
    }
}
