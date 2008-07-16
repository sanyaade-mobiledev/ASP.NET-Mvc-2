﻿namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class HttpUnauthorizedResult : ActionResult {

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }

            // 401 is the HTTP status code for unauthorized access - setting this
            // will cause the active authentication module to execute its default
            // unauthorized handler
            context.HttpContext.Response.StatusCode = 401;
        }
    }
}
