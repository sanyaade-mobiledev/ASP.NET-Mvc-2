using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace WebFormRouting
{
    public static class WebFormRouteExtensions
    {
        public static void MapWebFormRoute(this RouteCollection routes, string url, string virtualPath) 
        {
            routes.MapWebFormRoute(null, url, virtualPath, false);
        }

        public static void MapWebFormRoute(this RouteCollection routes, string name, string url, string virtualPath) 
        {
            routes.MapWebFormRoute(name, url, virtualPath, false);
        }

        public static void MapWebFormRoute(this RouteCollection routes, string url, string virtualPath, bool checkPhysicalUrlAccess) 
        {
            routes.MapWebFormRoute(null, url, virtualPath, checkPhysicalUrlAccess);
        }

        public static void MapWebFormRoute(this RouteCollection routes, string name, string url, string virtualPath, bool checkPhysicalUrlAccess)
        {
            routes.Add(name, new WebFormRoute(url, virtualPath, checkPhysicalUrlAccess));
        }

        public static string GetUrl(this IRoutablePage page, string name)
        {
           var pathInfo = RouteTable.Routes.GetVirtualPath(page.RequestContext, name, new RouteValueDictionary());
           if (pathInfo != null)
               return pathInfo.VirtualPath;
           
           return null;
        }


    }
}
