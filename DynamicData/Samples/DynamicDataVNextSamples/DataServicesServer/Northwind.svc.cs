using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace DataServiceSite {
    public class Northwind : DataService<NORTHWNDEntities> {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config) {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // For testing purposes use "*" to indicate all entity sets/service operations.
            // "*" should NOT be used in production systems.

            // Example for entity sets (this example uses "AllRead" which allows reads but not writes)
            config.SetEntitySetAccessRule("*", EntitySetRights.All);

            // Example for service operations
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
        }

        // Query interceptors, change interceptors and service operations go here.
    }
}
