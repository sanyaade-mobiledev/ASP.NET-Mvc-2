﻿namespace System.Web.Mvc {
    using System.Web;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IViewEngine {
        ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache);
        ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache);
        void ReleaseView(ControllerContext controllerContext, IView view);
    }
}
