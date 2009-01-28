namespace Microsoft.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class MvcAsyncRouteHandler : MvcRouteHandler {

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext) {
            return new MvcAsyncHandler(requestContext);
        }

    }
}
