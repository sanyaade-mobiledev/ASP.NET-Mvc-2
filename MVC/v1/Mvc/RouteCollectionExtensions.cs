namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class RouteCollectionExtensions {

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL at it may contain special routing characters.")]
        public static void MapRoute(this RouteCollection routes, string name, string url) {
            MapRoute(routes, name, url, null /* defaults */, null /* constraints */);
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL at it may contain special routing characters.")]
        public static void MapRoute(this RouteCollection routes, string name, string url, object defaults) {
            MapRoute(routes, name, url, defaults, null /* constraints */);
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#",
            Justification = "This is not a regular URL at it may contain special routing characters.")]
        public static void MapRoute(this RouteCollection routes, string name, string url, object defaults, object constraints) {
            if (routes == null) {
                throw new ArgumentNullException("routes");
            }
            if (url == null) {
                throw new ArgumentNullException("url");
            }

            Route route = new Route(url, new MvcRouteHandler()) {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints)
            };

            if (String.IsNullOrEmpty(name)) {
                // Add unnamed route if no name given
                routes.Add(route);
            }
            else {
                routes.Add(name, route);
            }
        }

    }
}
