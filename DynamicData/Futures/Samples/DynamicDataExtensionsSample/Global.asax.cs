using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Web.Routing;
using System.Web.DynamicData;
using Microsoft.Web.DynamicData.Extensions;
using System.ComponentModel.DataAnnotations;

namespace DynamicDataExtensionsSample {
    public class Global : System.Web.HttpApplication {
        public static void RegisterRoutes(RouteCollection routes) {
            MetaModel model = new MetaModel();

            // Example of adding metadata attributes programmatically
            InMemoryMetadataManager.AddColumnAttributes<Product>(p => p.UnitsInStock,
                new RangeAttribute(0, 1000) { ErrorMessage = "This field must be between {1} and {2} [from InMemoryMetadataManager]." }
                );

            model.RegisterContext(typeof(NorthwindDataContext), new ContextConfiguration() {
                ScaffoldAllTables = true,
                MetadataProviderFactory = (type => new InMemoryMetadataTypeDescriptionProvider(type, new AssociatedMetadataTypeTypeDescriptionProvider(type)))
            });

            // The following statement supports separate-page mode, where the List, Detail, Insert, and 
            // Update tasks are performed by using separate pages. To enable this mode, uncomment the following 
            // route definition, and comment out the route definitions in the combined-page mode section that follows.
            routes.Add(new DynamicDataRoute("{table}/{action}.aspx") {
                Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Model = model
            });

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
