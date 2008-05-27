namespace System.Web.Mvc {
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IViewLocator {
        string GetViewLocation(RequestContext requestContext, string viewName);
        string GetMasterLocation(RequestContext requestContext, string masterName);
    }
}
