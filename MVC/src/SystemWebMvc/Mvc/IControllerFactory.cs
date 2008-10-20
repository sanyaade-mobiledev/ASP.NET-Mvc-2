﻿namespace System.Web.Mvc {
    using System.Web;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IControllerFactory {
        IController CreateController(RequestContext requestContext, string controllerName);
        void ReleaseController(IController controller);
    }
}