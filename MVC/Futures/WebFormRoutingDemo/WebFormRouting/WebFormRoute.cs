using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace WebFormRouting {
    public class WebFormRoute : Route {
        public WebFormRoute(string url, string virtualPath) 
            : base(url, new WebFormRouteHandler(virtualPath)) { 
        }

        public WebFormRoute(string url, string virtualPath, bool checkPhysicalUrlAccess) 
            : base(url, new WebFormRouteHandler(virtualPath, checkPhysicalUrlAccess)) { 
        }

        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext) {
            return base.GetRouteData(httpContext);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            return base.GetVirtualPath(requestContext, values);
        }
    }
}
