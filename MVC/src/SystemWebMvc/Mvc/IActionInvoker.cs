﻿namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IActionInvoker {
        bool InvokeAction(ControllerContext controllerContext, string actionName);
    }
}