using System.Web.Mvc;
using System.Web.Routing;

namespace DlrMvcLib {
    public static class RouteInitializer {
        public static void InitializeRoutes() {
            RegisterRoutes(RouteTable.Routes);
        }

        public static void InitializeViewEngines() {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new DlrViewEngine());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

    }
}
