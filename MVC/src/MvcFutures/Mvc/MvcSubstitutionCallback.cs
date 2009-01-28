namespace Microsoft.Web.Mvc {
    using System.Web;
    
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public delegate string MvcSubstitutionCallback(HttpContextBase httpContext);
}
