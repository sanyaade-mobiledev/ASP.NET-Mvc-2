namespace System.Web.Mvc.Html {
    using System;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class LinkExtensions {
        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            string controllerName = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            return htmlHelper.GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object values) {
            return ActionLink(htmlHelper, linkText, actionName, values, null);
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object values, object htmlAttributes) {
            return ActionLink(htmlHelper, linkText, actionName, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary values) {
            return ActionLink(htmlHelper, linkText, actionName, values, new RouteValueDictionary());
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return htmlHelper.GenerateLink(linkText, null /* routeName */, actionName, null /* controllerName */, new RouteValueDictionary(values), htmlAttributes);
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            return htmlHelper.GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object values, object htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            return htmlHelper.GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return htmlHelper.GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(values), htmlAttributes);
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object values, object htmlAttributes) {
            return ActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public static string ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return htmlHelper.GenerateLink(linkText, null /* routeName */, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(values), htmlAttributes);
        }

        public static string GenerateLink(this HtmlHelper htmlHelper, string linkText, string routeName, string actionName, string controllerName, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, values, htmlHelper.RouteCollection, htmlHelper.ViewContext);
            TagBuilder tagBuilder = new TagBuilder("a") {
                InnerHtml = htmlHelper.Encode(linkText)
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        public static string GenerateLink(this HtmlHelper htmlHelper, string linkText, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, values, htmlHelper.RouteCollection, htmlHelper.ViewContext);
            TagBuilder tagBuilder = new TagBuilder("a") {
                InnerHtml = htmlHelper.Encode(linkText)
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, object values) {
            return RouteLink(htmlHelper, linkText, new RouteValueDictionary(values));
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary values) {
            return RouteLink(htmlHelper, linkText, values, new RouteValueDictionary());
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, string name, object values) {
            return RouteLink(htmlHelper, linkText, name, new RouteValueDictionary(values));
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, string name, RouteValueDictionary values) {
            return RouteLink(htmlHelper, linkText, name, values, new RouteValueDictionary());
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, object values, object htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            return htmlHelper.GenerateLink(linkText, null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return htmlHelper.GenerateLink(linkText, null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values), htmlAttributes);
        }        

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object values, object htmlAttributes) {
            return RouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return htmlHelper.GenerateLink(linkText, routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values), htmlAttributes);
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, object values, object htmlAttributes) {
            return RouteLink(htmlHelper, linkText, routeName, protocol, hostName, fragment, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public static string RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return htmlHelper.GenerateLink(linkText, routeName, null /* actionName */, null /* controllerName */, protocol, hostName, fragment, new RouteValueDictionary(values), htmlAttributes);
        }
    }
}
