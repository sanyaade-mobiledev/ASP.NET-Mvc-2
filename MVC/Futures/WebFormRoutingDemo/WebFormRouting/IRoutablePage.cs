using System;
using System.Web;
using System.Web.Routing;

namespace WebFormRouting
{
    /// <summary>
    /// Pages don't have to implement this interface, but the ones that 
    /// do will be able to generate outgoing routing URLs.
    /// </summary>
    public interface IRoutablePage : IHttpHandler
    {
        RequestContext RequestContext { get; set; }
        HtmlHelper Html { get; }
        UrlHelper Url { get; }
    }
}
