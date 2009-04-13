using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web;

namespace Microsoft.Web.DynamicData {
    /// <summary>
    /// The purpose of this class is to easily support routes that have the primary key as part of the path
    /// instead of the query string.  e.g. instead of /Categories/Details?CategoryID=3, allow /Categories/Details/3.
    /// It works by supporting special PK1, PK2, etc tokens that represent the primary key components.
    /// </summary>
    public class PrettyDynamicDataRoute : DynamicDataRoute {
        public PrettyDynamicDataRoute(string url)
            : base(url) {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            RouteData routeData = base.GetRouteData(httpContext);

            if (routeData != null) {
                // Change any reference to PK1, PK2, etc into the actual PK name
                FixUpRouteValues(routeData.Values, true /*fromFixedName*/);
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {

            // Change any reference to the actual PK name into PK1, PK2, etc...
            FixUpRouteValues(values, false /*fromFixedName*/);

            return base.GetVirtualPath(requestContext, values);
        }

        private void FixUpRouteValues(RouteValueDictionary routeValues, bool fromFixedName) {

            // First, try to get the table from the route values
            object tableName;
            if (!routeValues.TryGetValue("table", out tableName))
                return;

            MetaTable table;
            if (!Model.TryGetTable((string)tableName, out table))
                return;

            // Go through all the table's PK and make the required adjustment
            int index = 1;
            foreach (MetaColumn column in table.PrimaryKeyColumns) {

                // Assign from and to based on which direction we're fixing up
                string from, to;
                if (fromFixedName) {
                    from = "PK" + index;
                    to = column.Name;
                }
                else {
                    from = column.Name;
                    to = "PK" + index;
                }

                // If the route has that value, fix it up
                object val;
                if (routeValues.TryGetValue(from, out val)) {
                    routeValues[to] = val;
                    routeValues.Remove(from);
                }

                index++;
            }
        }
    }
}
