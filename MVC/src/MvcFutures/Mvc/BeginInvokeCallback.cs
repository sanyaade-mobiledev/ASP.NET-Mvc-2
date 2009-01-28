namespace Microsoft.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public delegate IAsyncResult BeginInvokeCallback(AsyncCallback callback, object state);
    
}
