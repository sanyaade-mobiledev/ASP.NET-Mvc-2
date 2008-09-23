namespace DlrMvcLib {
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Routing;
    using IronPython.Runtime;

    public static class Routes {
        public static void AddDefaultNamespace(string ns) {
            ControllerBuilder.Current.DefaultNamespaces.Add(ns);
        }

        public static void MapRoute(string name, string url, PythonDictionary defaults) {
            var defaultValues = new RouteValueDictionary();
            foreach (var kvp in defaults) {
                defaultValues.Add(Convert.ToString(kvp.Key, CultureInfo.InvariantCulture), kvp.Value);
            }

            RouteTable.Routes.Add(name, new Route(url, defaultValues, new MvcRouteHandler())) ;
        }

        public static void IgnoreRoute(string url) {
            RouteTable.Routes.IgnoreRoute(url);
        }
    }
}
