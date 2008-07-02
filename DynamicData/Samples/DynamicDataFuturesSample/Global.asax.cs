using System;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.Routing;
using Microsoft.Web.DynamicData;
using System.Collections.Generic;

namespace DynamicDataFuturesSample {
    public class Global : System.Web.HttpApplication {
        public static void RegisterRoutes(RouteCollection routes) {
            MetaModel model = new MetaModel();
            
            // use the AdvancedFieldTemplateFactory to get automatic support for extra field templates such as 'Enumeration'
            model.FieldTemplateFactory = new AdvancedFieldTemplateFactory();

            // Example of adding metadata attributes programmatically
            InMemoryMetadataManager.AddColumnAttributes<Product>(p => p.UnitsInStock,
                new RangeAttribute(0, 1000) { ErrorMessage = "This field must be between {1} and {2} [from InMemoryMetadataManager]." }
                );

            model.RegisterContext(typeof(NorthwindDataContext), new ContextConfiguration() {
                ScaffoldAllTables = true,
                MetadataProviderFactory = (type => new InMemoryMetadataTypeDescriptionProvider(type, new AssociatedMetadataTypeTypeDescriptionProvider(type)))
            });

            // Create a single route override for the Edit action on the Products table
            routes.Add(new DynamicDataRoute("MyProductsEditPage/{ProductID}.aspx") {
                Model = model,
                Table = "Products",
                Action = PageAction.Edit,
            });

            // Use a route to provide pretty URL, instead of having the Primary Key in the query string
            routes.Add(new PrettyDynamicDataRoute("{table}/{action}/{PK1}/{PK2}") {
                Model = model,
                Defaults = new RouteValueDictionary(new { action = PageAction.List, PK1 = "", PK2 = "" })
            });

            // The following statement supports separate-page mode, where the List, Detail, Insert, and 
            // Update tasks are performed by using separate pages. To enable this mode, uncomment the following 
            // route definition, and comment out the route definitions in the combined-page mode section that follows.
            //routes.Add(new DynamicDataRoute("{table}/{action}.aspx") {
            //    Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
            //    Model = model
            //});

            // The following statements support combined-page mode, where the List, Detail, Insert, and
            // Update tasks are performed by using the same page. To enable this mode, uncomment the
            // following routes and comment out the route definition in the separate-page mode section above.
            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.List,
            //    ViewName = "ListDetails",
            //    Model = model
            //});

            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx") {
            //    Action = PageAction.Details,
            //    ViewName = "ListDetails",
            //    Model = model
            //});
        }

        void Application_Start(object sender, EventArgs e) {
            RegisterRoutes(RouteTable.Routes);
        }

    }
}
