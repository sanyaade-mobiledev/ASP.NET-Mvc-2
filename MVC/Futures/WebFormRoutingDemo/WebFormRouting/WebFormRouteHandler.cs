using System;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.Routing;
using System.Web.Security;
using System.Security;

namespace WebFormRouting
{
    public class WebFormRouteHandler : IRouteHandler
    {
        public WebFormRouteHandler(string virtualPath) : this(virtualPath, true)
        {
        }

        public WebFormRouteHandler(string virtualPath, bool checkPhysicalUrlAccess)
        {
            this.VirtualPath = virtualPath;
            this.CheckPhysicalUrlAccess = checkPhysicalUrlAccess;
        }

        /// <summary>
        /// This is the full virtual path (using tilde syntax) to the WebForm page.
        /// </summary>
        /// <remarks>
        /// Needs to be thread safe so this is only settable via ctor.
        /// </remarks>
        public string VirtualPath { get; private set; }

        /// <summary>
        /// Because we're not actually rewriting the URL, ASP.NET's URL Auth will apply 
        /// to the incoming request URL and not the URL of the physical WebForm page.
        /// Setting this to true (default) will apply URL access rules against the 
        /// physical file.
        /// </summary>
        /// <value>True by default</value>
        public bool CheckPhysicalUrlAccess { get; set; }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string virtualPath = GetSubstitutedVirtualPath(requestContext);
            if (this.CheckPhysicalUrlAccess && !UrlAuthorizationModule.CheckUrlAccessForPrincipal(virtualPath, requestContext.HttpContext.User, requestContext.HttpContext.Request.HttpMethod))
                throw new SecurityException();

            var page = BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page)) as IHttpHandler;
            if (page != null)
            {
                //Pages that don't implement IRoutablePage won't have the RequestContext
                //available to them. Can't generate outgoing routing URLs without that context.
                var routablePage = page as IRoutablePage;
                if (routablePage != null)
                    routablePage.RequestContext = requestContext;
            }
            return page;
        }

        /// <summary>
        /// Gets the virtual path to the resource after applying substitutions based on route data.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public string GetSubstitutedVirtualPath(RequestContext requestContext) {
            if(!VirtualPath.Contains("{"))
                return VirtualPath;

            //Trim off ~/
            string virtualPath = VirtualPath.Substring(2);

            Route route = new Route(virtualPath, this);
            VirtualPathData vpd = route.GetVirtualPath(requestContext, requestContext.RouteData.Values);
            if (vpd == null)
                return VirtualPath;
            return "~/" + vpd.VirtualPath;
        }
    }
}
