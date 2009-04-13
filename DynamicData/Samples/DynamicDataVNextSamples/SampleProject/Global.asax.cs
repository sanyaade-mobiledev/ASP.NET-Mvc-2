using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.DynamicData;
using System.Web.Routing;

namespace DynamicDataProject {
    public class Global : System.Web.HttpApplication {
        public static MetaModel L2Smodel { get; private set; }
        public static MetaModel EFmodel { get; private set; }

        public static void RegisterRoutes(RouteCollection routes) {
            L2Smodel = new MetaModel();
            L2Smodel.DynamicDataFolderVirtualPath = "~/DynamicDataL2S";
            L2Smodel.RegisterContext(typeof(NorthwindDataContext), new ContextConfiguration() { ScaffoldAllTables = true });
            L2Smodel.RegisterContext(typeof(InheritanceDataContext), new ContextConfiguration() { ScaffoldAllTables = true });
            RegisterModelRoutes(routes, L2Smodel, "L2S", true);

            EFmodel = new MetaModel();
            EFmodel.DynamicDataFolderVirtualPath = "~/DynamicDataEF";
            EFmodel.RegisterContext(typeof(DynamicDataEFProject.NORTHWNDEntities), new ContextConfiguration() { ScaffoldAllTables = true });
            EFmodel.RegisterContext(typeof(DynamicDataEFProject.InheritanceEntities), new ContextConfiguration() { ScaffoldAllTables = true });
            RegisterModelRoutes(routes, EFmodel, "EF", true);
        }

        public static void RegisterModelRoutes(RouteCollection routes, MetaModel model, string prefix, bool useSeperatePages) {
            if (useSeperatePages) {
                // The following statement supports separate-page mode, where the List, Detail, Insert, and 
                // Update tasks are performed by using separate pages. To enable this mode, uncomment the following 
                // route definition, and comment out the route definitions in the combined-page mode section that follows.
                routes.Add(new DynamicDataRoute(prefix + "/{table}/{action}.aspx") {
                    Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                    Model = model
                });
            } else {

                // The following statements support combined-page mode, where the List, Detail, Insert, and
                // Update tasks are performed by using the same page. To enable this mode, uncomment the
                // following routes and comment out the route definition in the separate-page mode section above.
                routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
                    Action = PageAction.List,
                    ViewName = "ListDetails",
                    Model = model
                });

                routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
                    Action = PageAction.Details,
                    ViewName = "ListDetails",
                    Model = model
                });
            }
        }

    void Application_Start(object sender, EventArgs e) {
            RegisterRoutes(RouteTable.Routes);
        }

    }
}
