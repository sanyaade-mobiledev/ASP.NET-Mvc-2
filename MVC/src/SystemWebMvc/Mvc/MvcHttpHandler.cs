namespace System.Web.Mvc {
    using System.Globalization;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;
    using System.Web.SessionState;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class MvcHttpHandler : UrlRoutingHandler, IRequiresSessionState {

        protected override void VerifyAndProcessRequest(IHttpHandler httpHandler, HttpContextBase httpContext) {
            if (httpHandler == null) {
                throw new ArgumentNullException("httpHandler");
            }
            MvcHandler mvcHandler = httpHandler as MvcHandler;
            if (mvcHandler == null) {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        MvcResources.MvcHttpHandler_DidNotReturnMvcHandler,
                        httpHandler.GetType().FullName),
                    "httpHandler");
            }

            mvcHandler.ProcessRequest(httpContext);
        }
    }
}
