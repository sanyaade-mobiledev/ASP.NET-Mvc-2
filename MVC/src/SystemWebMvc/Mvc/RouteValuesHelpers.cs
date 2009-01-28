namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    internal static class RouteValuesHelpers {
        public static RouteValueDictionary GetRouteValues(RouteValueDictionary routeValues) {
            return (routeValues != null) ? new RouteValueDictionary(routeValues) : new RouteValueDictionary();
        }

        public static RouteValueDictionary MergeRouteValues(string actionName, string controllerName, RouteValueDictionary implicitRouteValues, RouteValueDictionary routeValues) {
            // Create a new dictionary containing implicit and auto-generated values
            RouteValueDictionary mergedRouteValues = new RouteValueDictionary();

            object implicitValue;
            if (implicitRouteValues != null && implicitRouteValues.TryGetValue("action", out implicitValue)) {
                mergedRouteValues["action"] = implicitValue;
            }

            if (implicitRouteValues != null && implicitRouteValues.TryGetValue("controller", out implicitValue)) {
                mergedRouteValues["controller"] = implicitValue;
            }

            // Merge values from the user's dictionary/object
            if (routeValues != null) {
                foreach (KeyValuePair<string, object> routeElement in GetRouteValues(routeValues)) {
                    mergedRouteValues[routeElement.Key] = routeElement.Value;
                }
            }

            // Merge explicit parameters when not null
            if (actionName != null) {
                mergedRouteValues["action"] = actionName;
            }

            if (controllerName != null) {
                mergedRouteValues["controller"] = controllerName;
            }

            return mergedRouteValues;
        }
    }
}
