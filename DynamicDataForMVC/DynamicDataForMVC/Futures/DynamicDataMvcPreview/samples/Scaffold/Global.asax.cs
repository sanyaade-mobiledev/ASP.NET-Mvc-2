using System.Web;
using System.Web.DynamicData;
using Microsoft.Web.DynamicData.Mvc;
using System.Web.Mvc;
using System.Web.Routing;
using ScaffoldSample.Models;
using System.ComponentModel;

namespace ScaffoldSample {
    public class GlobalApplication : HttpApplication {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            MetaModel model = new MetaModel();
            model.DynamicDataFolderVirtualPath = "~/Views/Shared";
            model.RegisterContext(typeof(NorthwindDataContext), new ContextConfiguration { ScaffoldAllTables = true });

            routes.MapScaffoldRoute(
                "scaffold",
                "dd/{table}/{action}",
                model,
                new { action = "list" }
            );

            routes.MapRoute(
                "default",
                "{controller}/{action}/{id}",
                new { action = "Index", id = "" }
            );
        }

        protected void Application_Start() {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}