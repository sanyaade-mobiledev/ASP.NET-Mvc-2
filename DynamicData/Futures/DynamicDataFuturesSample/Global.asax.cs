using System;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.Routing;
using Microsoft.Web.DynamicData;
using System.Collections.Generic;

namespace DynamicDataFuturesSample {
    public class Global : System.Web.HttpApplication {
        private static MetaModel s_defaultModel = new CustomMetaModel();
        public static MetaModel DefaultModel {
            get {
                return s_defaultModel;
            }
        }

        public static void RegisterRoutes(RouteCollection routes) {
            // Example of adding metadata attributes programmatically
            InMemoryMetadataManager.AddColumnAttributes<Product>(p => p.UnitsInStock,
                new RangeAttribute(0, 1000) { ErrorMessage = "This field must be between {1} and {2} [from InMemoryMetadataManager]." }
                );

            DefaultModel.RegisterContext(typeof(NorthwindDataContext), new ContextConfiguration() {
                ScaffoldAllTables = true,
                MetadataProviderFactory = (type => new InMemoryMetadataTypeDescriptionProvider(type, new AssociatedMetadataTypeTypeDescriptionProvider(type)))
            });

            // Create a special escape route for handling resource requests. The this route will be evaluated
            // for all requests but the contraint will only match requests with WebResource.axd on the end.
            // The StopRoutingHandler will instruct the routing system to stop processing this request and pass
            // it on to standard ASP.NET processing.
            routes.Add(new Route("{*resource}", new StopRoutingHandler()) {
                Constraints = new RouteValueDictionary(new  { resource = @".*WebResource\.axd" })
            });

            // Create a single route override for the Edit action on the Products table
            routes.Add(new DynamicDataRoute("MyProductsEditPage/{ProductID}") {
                Constraints = new RouteValueDictionary(new { ProductID = "[1-9][0-9]*" }), // use constraints to limit which requests get matched
                Model = DefaultModel,
                Table = "Products",
                Action = PageAction.Edit,
            });

            // Use a route to provide pretty URLs, instead of having the Primary Key in the query string
            routes.Add(new PrettyDynamicDataRoute("{table}/{action}/{PK1}/{PK2}") {
                Model = DefaultModel,
                Constraints = new RouteValueDictionary(new { action = "List|Details|Insert|Edit" }),
                Defaults = new RouteValueDictionary(new { action = PageAction.List, PK1 = "", PK2 = "" })
            });

            // The following statement supports separate-page mode, where the List, Detail, Insert, and 
            // Update tasks are performed by using separate pages. To enable this mode, uncomment the following 
            // route definition, and comment out the route definitions in the combined-page mode section that follows.
            //routes.Add(new DynamicDataRoute("{table}/{action}.aspx") {
            //    Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
            //    Model = DefaultModel
            //});

            // The following statements support combined-page mode, where the List, Detail, Insert, and
            // Update tasks are performed by using the same page. To enable this mode, uncomment the
            // following routes and comment out the route definition in the separate-page mode section above.
            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.List,
            //    ViewName = "ListDetails",
            //    Model = DefaultModel
            //});

            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.Details,
            //    ViewName = "ListDetails",
            //    Model = DefaultModel
            //});
        }

        void Application_Start(object sender, EventArgs e) {
            RegisterRoutes(RouteTable.Routes);
        }

    }
}
