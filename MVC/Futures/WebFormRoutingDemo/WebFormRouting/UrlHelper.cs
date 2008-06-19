using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace WebFormRouting {
    public class UrlHelper 
    {
        IRoutablePage _page;
        RouteCollection _routes;

        public UrlHelper(IRoutablePage page, RouteCollection routes) 
        {
            _page = page;
            _routes = routes;
        }

        public string RouteUrl(object values) {
            return RouteUrl(new RouteValueDictionary(values));
        }

        public string RouteUrl(string name, object values) {
            return RouteUrl(name, new RouteValueDictionary(values));
        }

        public string RouteUrl(RouteValueDictionary values) {
            VirtualPathData vpd = _routes.GetVirtualPath(_page.RequestContext, values);
            if (vpd != null) {
                return vpd.VirtualPath;
            }
            return null;
        }

        public string RouteUrl(string name, RouteValueDictionary values) {
            VirtualPathData vpd = _routes.GetVirtualPath(_page.RequestContext, name, values);
            if (vpd != null) {
                return vpd.VirtualPath;
            }
            return null;
        }
    }
}
