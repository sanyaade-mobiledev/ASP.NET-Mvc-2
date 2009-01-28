namespace Microsoft.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IAsyncController : IController {

        IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state);
        void EndExecute(IAsyncResult asyncResult);

    }
}
