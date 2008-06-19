using System;
using System.Web.UI;
using System.Web.Routing;
using System.Web;

namespace WebFormRouting
{
    /// <summary>
    /// Handy base class for routable pages.
    /// </summary>
    public abstract class RoutablePage : Page, IRoutablePage
    {
        public RoutablePage() {
            this.Html = new HtmlHelper(this, RouteTable.Routes);
            this.Url = new UrlHelper(this, RouteTable.Routes);
        }

        public RequestContext RequestContext {get; set; }

        public RouteData RouteData 
        {
            get 
            {
                if (RequestContext == null)
                {
                    //Try to manafacture one.
                    var context = new HttpContextWrapper2(HttpContext.Current);
                    var requestContext = new RequestContext(context, new RouteData());
                    this.RequestContext = requestContext;
                }
                
                return this.RequestContext.RouteData;
            }
        }

        public HtmlHelper Html {get; private set; }
        public UrlHelper Url { get; private set; }
    }
}
