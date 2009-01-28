namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;
    using System.Web.UI;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class UrlHelper {

        // Session state URLs look like /AppPath/(S(...))/LocalPath, so an "app-relative" path
        // containing session state looks like ~/(S(...))/LocalPath. The Regex below matches
        // the "(S(...))/" part of that URL for easy removal.
        private static readonly Regex _sessionStateRegex = new Regex(@"(?<=^~/)\(S\([^/?]*\)\)/", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

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
            return GenerateUrl(null /* routeName */, actionName, null, (RouteValueDictionary)null /* routeValues */);
        }

        public string Action(string actionName, object routeValues) {
            return GenerateUrl(null /* routeName */, actionName, null /* controllerName */, new RouteValueDictionary(routeValues));
        }

        public string Action(string actionName, RouteValueDictionary routeValues) {
            return GenerateUrl(null /* routeName */, actionName, null /* controllerName */, routeValues);
        }

        public string Action(string actionName, string controllerName) {
            return GenerateUrl(null /* routeName */, actionName, controllerName, (RouteValueDictionary)null /* routeValues */);
        }

        public string Action(string actionName, string controllerName, object routeValues) {
            return GenerateUrl(null /* routeName */, actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        public string Action(string actionName, string controllerName, RouteValueDictionary routeValues) {
            return GenerateUrl(null /* routeName */, actionName, controllerName, routeValues);
        }

        public string Action(string actionName, string controllerName, object routeValues, string protocol) {
            return GenerateUrl(null /* routeName */, actionName, controllerName, protocol, null /* hostName */, null /* fragment */, new RouteValueDictionary(routeValues), RouteCollection, RequestContext);
        }

        public string Action(string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName) {
            return GenerateUrl(null /* routeName */, actionName, controllerName, protocol, hostName, null /* fragment */, routeValues, RouteCollection, RequestContext);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public string Content(string contentPath) {
            if (String.IsNullOrEmpty(contentPath)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "contentPath");
            }

            if (contentPath[0] == '~') {
                int idxQuery = contentPath.IndexOf('?');
                string filePath = (idxQuery >= 0) ? contentPath.Substring(0, idxQuery) : contentPath;
                string queryString = (idxQuery >= 0) ? contentPath.Substring(idxQuery) : null;

                string absoluteFilePath = VirtualPathUtility.ToAbsolute(filePath, RequestContext.HttpContext.Request.ApplicationPath);
                string relativeFilePath = MakeRelativeUrl(RequestContext.HttpContext.Request.Path, absoluteFilePath);
                return relativeFilePath + queryString;
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

        private string GenerateUrl(string routeName, string actionName, string controllerName, RouteValueDictionary routeValues) {
            return GenerateUrl(routeName, actionName, controllerName, routeValues, RouteCollection, RequestContext);
        }

        internal static string GenerateUrl(string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext) {
            bool generateFullyQualifiedUri = (!String.IsNullOrEmpty(protocol) || !String.IsNullOrEmpty(hostName));
            string url = GenerateUrl(routeName, actionName, controllerName, routeValues, routeCollection, requestContext, !generateFullyQualifiedUri);

            if (url != null) {
                if (!String.IsNullOrEmpty(fragment)) {
                    url = url + "#" + fragment;
                }

                if (generateFullyQualifiedUri) {
                    Uri requestUrl = requestContext.HttpContext.Request.Url;
                    protocol = (!String.IsNullOrEmpty(protocol)) ? protocol : Uri.UriSchemeHttp;
                    hostName = (!String.IsNullOrEmpty(hostName)) ? hostName : requestUrl.Host;

                    string port = String.Empty;
                    string requestProtocol = requestUrl.Scheme;

                    if (String.Equals(protocol, requestProtocol, StringComparison.OrdinalIgnoreCase)) {
                        port = requestUrl.IsDefaultPort ? String.Empty : (":" + Convert.ToString(requestUrl.Port, CultureInfo.InvariantCulture));
                    }

                    url = protocol + Uri.SchemeDelimiter + hostName + port + url;
                }
            }

            return url;
        }

        internal static string GenerateUrl(string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext) {
            return GenerateUrl(routeName, actionName, controllerName, routeValues, routeCollection, requestContext, true /* useResolutionService */);
        }

        private static string GenerateUrl(string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext, bool useResolutionService) {
            RouteValueDictionary mergedRouteValues = RouteValuesHelpers.MergeRouteValues(actionName, controllerName, requestContext.RouteData.Values, routeValues);

            VirtualPathData vpd = vpd = routeCollection.GetVirtualPath(requestContext, routeName, mergedRouteValues);

            if (vpd != null) {
                return (useResolutionService) ?
                    VirtualPathToClientUrl(vpd.VirtualPath, requestContext.HttpContext.Request.ApplicationPath, requestContext.HttpContext.Request.Path) :
                    vpd.VirtualPath;
            }
            return null;
        }

        internal static string MakeRelativeUrl(string fromPath, string toPath) {
            string relativeUrl = VirtualPathUtility.MakeRelative(fromPath, toPath);
            if (String.IsNullOrEmpty(relativeUrl) || relativeUrl[0] == '?') {
                // sometimes VirtualPathUtility.MakeRelative() will return an empty path when it meant to return '.'
                relativeUrl = "./" + relativeUrl;
            }
            return relativeUrl;
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(object routeValues) {
            return GenerateUrl(null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(routeValues));
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(RouteValueDictionary routeValues) {
            return GenerateUrl(null /* routeName */, null /* actionName */, null /* controllerName */, routeValues);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName) {
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, null /* routeValues */);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName, object routeValues) {
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(routeValues));
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName, RouteValueDictionary routeValues) {
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, routeValues);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName, object routeValues, string protocol) {
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, protocol, null /* hostName */, null /* fragment */, new RouteValueDictionary(routeValues), RouteCollection, RequestContext);
        }

        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "As the return value will used only for rendering, string return value is more appropriate.")]
        public string RouteUrl(string routeName, RouteValueDictionary routeValues, string protocol, string hostName) {
            return GenerateUrl(routeName, null /* actionName */, null /* controllerName */, protocol, hostName, null /* fragment */, routeValues, RouteCollection, RequestContext);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We can recover from exceptions thrown by VirtualPathUtility.ToAppRelative().")]
        private static string VirtualPathToClientUrl(string virtualPath, string applicationPath, string currentPath) {
            // We try to go from an absolute URL to an app-relative URL to a relative URL
            // to enable things like URL rewriting and session state.

            // VirtualPathUtility.ToAppRelative() bombs out if the path it's given contains a '?' character
            int queryStringIdx = virtualPath.IndexOf('?');
            string virtualPathWithoutQueryString = (queryStringIdx == -1) ? virtualPath : virtualPath.Substring(0, queryStringIdx);

            string appRelativePath = null;
            try {
                appRelativePath = VirtualPathUtility.ToAppRelative(virtualPathWithoutQueryString, applicationPath);
            }
            catch {
                // We might get an error if Routing returns a path of a form we're not expecting.
                // In that case, just return the original URL without running client resolution logic.
            }

            if (String.IsNullOrEmpty(appRelativePath)) {
                return virtualPath;
            }

            appRelativePath = _sessionStateRegex.Replace(appRelativePath, String.Empty);
            string destinationPath = VirtualPathUtility.ToAbsolute(appRelativePath, applicationPath);

            // Restore query string portion
            if (queryStringIdx >= 0) {
                destinationPath += virtualPath.Substring(queryStringIdx, virtualPath.Length - queryStringIdx);
            }

            string clientUrl = MakeRelativeUrl(currentPath, destinationPath);
            return clientUrl;
        }

    }
}
