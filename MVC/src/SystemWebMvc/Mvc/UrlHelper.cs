namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class UrlHelper {
        public UrlHelper(RequestContext requestContext)
            : this(requestContext, RouteTable.Routes) {
        }

        public UrlHelper(RequestContext requestContext, RouteCollection routeCollection) {
            if (requestContext == null) {
                throw new ArgumentNullException("requestContext");
            }
            if (routeCollection == null) {
                throw new ArgumentNullException("routeCollection");
            }
            RequestContext = requestContext;
            RouteCollection = routeCollection;
        }

        public RequestContext RequestContext {
            get;
            private set;
        }

        public RouteCollection RouteCollection {
            get;
            private set;
        }

        public string Action(string actionName) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            string controllerName = RequestContext.RouteData.GetRequiredString("controller");
            return GenerateUrl(null /* routeName */, actionName, controllerName, new RouteValueDictionary());
        }

        public string Action(string actionName, object values) {
            return Action(actionName, new RouteValueDictionary(values));
        }

        public string Action(string actionName, RouteValueDictionary valuesDictionary) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateUrl(null /* routeName */, actionName, null /* controllerName */, new RouteValueDictionary(valuesDictionary));
        }

        public string Action(string actionName, string controllerName) {
            return Action(actionName, controllerName, (object)null /* values */);
        }

        public string Action(string actionName, string controllerName, object values) {
            return Action(actionName, controllerName, new RouteValueDictionary(values));
        }

        public string Action(string actionName, string controllerName, RouteValueDictionary valuesDictionary) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateUrl(null /* routeName */, actionName, controllerName, new RouteValueDictionary(valuesDictionary));
        }

        public string Action(string actionName, string controllerName, object values, string protocol) {
            return Action(actionName, controllerName, new RouteValueDictionary(values), protocol, null /* hostName */);
        }

        public string Action(string actionName, string controllerName, RouteValueDictionary valuesDictionary, string protocol, string hostName) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateUrl(null /* routeName */, actionName, controllerName, protocol, hostName, null /* fragment */, new RouteValueDictionary(valuesDictionary), RouteCollection, RequestContext);
        }

        public string Content(string contentPath) {
            if (String.IsNullOrEmpty(contentPath)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "contentPath");
            }

            if (contentPath[0] == '~') {
                return VirtualPathUtility.ToAbsolute(contentPath, RequestContext.HttpContext.Request.ApplicationPath);
            }
            else {
                return contentPath;
            }
        }

        //REVIEW: Should we have an overload that takes Uri?
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameters as HttpUtility.UrlEncode()")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public string Encode(string url) {
            return HttpUtility.UrlEncode(url);
        }

        private string GenerateUrl(string routeName, string actionName, string controllerName, RouteValueDictionary valuesDictionary) {
            return GenerateUrl(routeName, actionName, controllerName, valuesDictionary, RouteCollection, RequestContext);
        }

        internal static string GenerateUrl(string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary valuesDictionary, RouteCollection routeCollection, RequestContext requestContext) {
            string url = GenerateUrl(routeName, actionName, controllerName, valuesDictionary, routeCollection, requestContext);

            if (url != null) {
                if (!String.IsNullOrEmpty(fragment)) {
                    url = url + "#" + fragment;
                }

                if (!String.IsNullOrEmpty(protocol) || !String.IsNullOrEmpty(hostName)) {
                    Uri requestUrl = requestContext.HttpContext.Request.Url;
                    protocol = (!String.IsNullOrEmpty(protocol)) ? protocol : Uri.UriSchemeHttp;
                    hostName = (!String.IsNullOrEmpty(hostName)) ? hostName : requestUrl.Host;

                    string port = String.Empty;
                    string requestProtocol = requestUrl.Scheme;

                    if (String.Equals(protocol, requestProtocol, StringComparison.OrdinalIgnoreCase)) {
                        port = requestUrl.IsDefaultPort ? String.Empty : (":" + Convert.ToString(requestUrl.Port, CultureInfo.CurrentUICulture));
                    }

                    url = protocol + Uri.SchemeDelimiter + hostName + port + url;
                }
            }

            return url;
        }

        internal static string GenerateUrl(string routeName, string actionName, string controllerName, RouteValueDictionary valuesDictionary, RouteCollection routeCollection, RequestContext requestContext) {
            if (actionName != null) {
                if (valuesDictionary.ContainsKey("action")) {
                    throw new ArgumentException(
                        String.Format(
                            CultureInfo.CurrentUICulture,
                            MvcResources.Helper_DictionaryAlreadyContainsKey,
                            "action"),
                        "actionName");
                }
                valuesDictionary.Add("action", actionName);
            }
            if (controllerName != null) {
                if (valuesDictionary.ContainsKey("controller")) {
                    throw new ArgumentException(
                        String.Format(
                            CultureInfo.CurrentUICulture,
                            MvcResources.Helper_DictionaryAlreadyContainsKey,
                            "controller"),
                        "controllerName");
                }
                valuesDictionary.Add("controller", controllerName);
            }

            VirtualPathData vpd;
            if (routeName != null) {
                vpd = routeCollection.GetVirtualPath(requestContext, routeName, valuesDictionary);
            }
            else {
                vpd = routeCollection.GetVirtualPath(requestContext, valuesDictionary);
            }

            if (vpd != null) {
                return vpd.VirtualPath;
            }
            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(object values) {
            return GenerateUrl(null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values));
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(RouteValueDictionary valuesDictionary) {
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateUrl(null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(valuesDictionary));
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName) {
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary());
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName, object values) {
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values));
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName, RouteValueDictionary valuesDictionary) {
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(valuesDictionary));
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName, object values, string protocol) {
            return RouteUrl(routeName, new RouteValueDictionary(values), protocol, null /* hostName */);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName, RouteValueDictionary valuesDictionary, string protocol, string hostName) {
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            if (valuesDictionary == null) {
                throw new ArgumentNullException("valuesDictionary");
            }
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, protocol, hostName, null /* fragment */, new RouteValueDictionary(valuesDictionary), RouteCollection, RequestContext);
        }
    }
}
