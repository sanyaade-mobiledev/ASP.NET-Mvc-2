namespace System.Web.Mvc {
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IViewLocationCache {
        string GetViewLocation(HttpContextBase httpContext, string key);
        void InsertViewLocation(HttpContextBase httpContext, string key, string virtualPath);
    }
}
