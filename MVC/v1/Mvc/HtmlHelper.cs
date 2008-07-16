namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public partial class HtmlHelper {

        private RouteCollection _routeCollection;

        public HtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) {
            if (viewContext == null) {
                throw new ArgumentNullException("viewContext");
            }
            if (viewDataContainer == null) {
                throw new ArgumentNullException("viewDataContainer");
            }
            ViewContext = viewContext;
            ViewDataContainer = viewDataContainer;
        }

        internal RouteCollection RouteCollection {
            get {
                if (_routeCollection == null) {
                    _routeCollection = RouteTable.Routes;
                }
                return _routeCollection;
            }
            set {
                _routeCollection = value;
            }
        }

        public ViewContext ViewContext {
            get;
            private set;
        }

        protected ViewDataDictionary ViewData {
            get {
                return ViewDataContainer.ViewData;
            }
        }

        public IViewDataContainer ViewDataContainer {
            get;
            private set;
        }

        public string ActionLink(string linkText, string actionName) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            string controllerName = ViewContext.RouteData.GetRequiredString("controller");
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary());
        }

        public string ActionLink(string linkText, string actionName, object values) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, null /* controllerName */, new RouteValueDictionary(values));
        }

        public string ActionLink(string linkText, string actionName, RouteValueDictionary valuesDictionary) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, null /* controllerName */, new RouteValueDictionary(valuesDictionary));
        }

        public string ActionLink(string linkText, string actionName, string controllerName) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary());
        }

        public string ActionLink(string linkText, string actionName, string controllerName, object values) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(values));
        }

        public string ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary valuesDictionary) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(valuesDictionary));
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
            return Encode(Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        private string EvalString(string key) {
            return Convert.ToString(ViewData.Eval(key), CultureInfo.InvariantCulture);
        }

        private string GenerateLink(string linkText, string routeName, string actionName, string controllerName, RouteValueDictionary valuesDictionary) {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, valuesDictionary, RouteCollection, ViewContext);
            TagBuilder tag = new TagBuilder("a") {
                Attributes = new Dictionary<string, string> { { "href", url } },
                InnerHtml = Encode(linkText)
            };
            return tag.ToString();
        }

        public string RouteLink(string linkText, object values) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            return GenerateLink(linkText, null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values));
        }

        public string RouteLink(string linkText, RouteValueDictionary valuesDictionary) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateLink(linkText, null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(valuesDictionary));
        }

        public string RouteLink(string linkText, string routeName) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            return GenerateLink(linkText, routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary());
        }

        public string RouteLink(string linkText, string routeName, object values) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            return GenerateLink(linkText, routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values));
        }

        public string RouteLink(string linkText, string routeName, RouteValueDictionary valuesDictionary) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateLink(linkText, routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(valuesDictionary));
        }

        private string InputHelper(string name, bool useViewData, string defaultValue, string inputType, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }

            TagBuilder builder = new TagBuilder("input") {
                Attributes = TagBuilder.ToStringDictionary(htmlAttributes)
            };
            
            builder.TryAddValue("type", inputType);
            builder.TryAddValue("name", name);
            builder.TryAddValue("id", name);
            builder.TryAddValue("value", (useViewData) ? EvalString(name) : defaultValue);
            builder.TagRenderMode = TagRenderMode.SelfClosing;
            return builder.ToString();
        }
    }
}
