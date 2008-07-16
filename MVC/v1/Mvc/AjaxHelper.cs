namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AjaxHelper {

        private RouteCollection _routeCollection;

        public AjaxHelper(ViewContext viewContext) {
            if (viewContext == null) {
                throw new ArgumentNullException("viewContext");
            }
            ViewContext = viewContext;
        }

        internal RouteCollection RouteCollection {
            get {
                return _routeCollection ?? RouteTable.Routes;
            }
            set {
                _routeCollection = value;
            }
        }

        public ViewContext ViewContext {
            get;
            private set;
        }

        public string ActionLink(string linkText, string actionName, AjaxOptions ajaxOptions) {
            return ActionLink(linkText, actionName, (string)null /* controllerName */, ajaxOptions);
        }

        public string ActionLink(string linkText, string actionName, object values, AjaxOptions ajaxOptions) {
            return ActionLink(linkText, actionName, (string)null /* controllerName */, values, ajaxOptions);
        }

        public string ActionLink(string linkText, string actionName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            return ActionLink(linkText, actionName, (string)null /* controllerName */, values, ajaxOptions, htmlAttributes);
        }

        public string ActionLink(string linkText, string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions) {
            return ActionLink(linkText, actionName, (string)null /* controllerName */, values, ajaxOptions);
        }

        public string ActionLink(string linkText, string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return ActionLink(linkText, actionName, (string)null /* controllerName */, values, ajaxOptions, htmlAttributes);
        }

        public string ActionLink(string linkText, string actionName, string controllerName, AjaxOptions ajaxOptions) {
            return ActionLink(linkText, actionName, controllerName, null /* values */, ajaxOptions, null /* htmlAttributes */);
        }

        public string ActionLink(string linkText, string actionName, string controllerName, object values, AjaxOptions ajaxOptions) {
            return ActionLink(linkText, actionName, controllerName, values, ajaxOptions, null /* htmlAttributes */);
        }

        public string ActionLink(string linkText, string actionName, string controllerName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            RouteValueDictionary newValues = new RouteValueDictionary(values);
            Dictionary<string, object> newAttributes = ObjectToCaseSensitiveDictionary(htmlAttributes);
            return ActionLink(linkText, actionName, controllerName, newValues, ajaxOptions, newAttributes);
        }

        public string ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions) {
            return ActionLink(linkText, actionName, controllerName, values, ajaxOptions, null /* htmlAttributes */);
        }

        public string ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (ajaxOptions == null) {
                throw new ArgumentNullException("ajaxOptions");
            }

            string targetUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, values ?? new RouteValueDictionary(), RouteCollection, ViewContext);

            TagBuilder tag = new TagBuilder("a") {
                InnerHtml = HttpUtility.HtmlEncode(linkText)
            };

            string onClickFormat = "Sys.Mvc.AsyncHyperlink.handleClick(this, {0}); return false;";
            string optionsString = ajaxOptions.ToJavascriptString();
            string onClick = String.Format(CultureInfo.InvariantCulture, onClickFormat, optionsString);

            tag.AddAttributes(TagBuilder.ToStringDictionary(htmlAttributes));
            tag.TryAddValue("href", targetUrl);
            tag.TryAddValue("onclick", onClick);
            tag.TagRenderMode = TagRenderMode.Normal;

            return tag.ToString();
        }

        public IDisposable Form(string actionName, AjaxOptions ajaxOptions) {
            return Form(actionName, (string)null /* controllerName */, ajaxOptions);
        }

        public IDisposable Form(string actionName, object values, AjaxOptions ajaxOptions) {
            return Form(actionName, (string)null /* controllerName */, values, ajaxOptions);
        }

        public IDisposable Form(string actionName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            return Form(actionName, (string)null /* controllerName */, values, ajaxOptions, htmlAttributes);
        }

        public IDisposable Form(string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions) {
            return Form(actionName, (string)null /* controllerName */, values, ajaxOptions);
        }

        public IDisposable Form(string actionName, RouteValueDictionary values, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            return Form(actionName, (string)null /* controllerName */, values, ajaxOptions, htmlAttributes);
        }

        public IDisposable Form(string actionName, string controllerName, AjaxOptions ajaxOptions) {
            return Form(actionName, controllerName, null /* values */, ajaxOptions, null /* htmlAttributes */);
        }

        public IDisposable Form(string actionName, string controllerName, object values, AjaxOptions ajaxOptions) {
            return Form(actionName, controllerName, values, ajaxOptions, null /* htmlAttributes */);
        }

        public IDisposable Form(string actionName, string controllerName, object values, AjaxOptions ajaxOptions, object htmlAttributes) {
            RouteValueDictionary newValues = new RouteValueDictionary(values);
            Dictionary<string, object> newAttributes = ObjectToCaseSensitiveDictionary(htmlAttributes);
            return Form(actionName, controllerName, newValues, ajaxOptions, newAttributes);
        }

        public IDisposable Form(string actionName, string controllerName, RouteValueDictionary values, AjaxOptions ajaxOptions) {
            return Form(actionName, controllerName, values, ajaxOptions, null /* htmlAttributes */);
        }

        public IDisposable Form(string actionName, string controllerName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (ajaxOptions == null) {
                throw new ArgumentNullException("ajaxOptions");
            }

            // get target URL
            string targetUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, valuesDictionary ?? new RouteValueDictionary(), RouteCollection, ViewContext);

            // JS code
            string onSubmitFormat = "Sys.Mvc.AsyncForm.handleSubmit(this, {0}); return false;";
            string optionsString = ajaxOptions.ToJavascriptString();
            string onSubmit = String.Format(CultureInfo.InvariantCulture, onSubmitFormat, optionsString);

            TagBuilder builder = new TagBuilder("form") {
                Attributes = new Dictionary<string, string>()
            };

            builder.TryAddValue("action", targetUrl);
            builder.TryAddValue("onsubmit", onSubmit);
            builder.AddAttributes(TagBuilder.ToStringDictionary(htmlAttributes));
            builder.TagRenderMode = TagRenderMode.StartTag;

            HttpResponseBase response = ViewContext.HttpContext.Response;
            response.Write(builder.ToString());
            return new DisposableForm(response);
        }

        internal static string InsertionModeToString(InsertionMode insertionMode) {
            switch (insertionMode) {
                case InsertionMode.Replace:
                    return "Sys.Mvc.InsertionMode.Replace";
                case InsertionMode.InsertBefore:
                    return "Sys.Mvc.InsertionMode.InsertBefore";
                case InsertionMode.InsertAfter:
                    return "Sys.Mvc.InsertionMode.InsertAfter";
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
