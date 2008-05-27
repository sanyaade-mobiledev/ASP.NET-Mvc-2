﻿namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewContext : ControllerContext {

        public ViewContext(HttpContextBase httpContext, RouteData routeData, IController controller, string viewName, string masterName, ViewDataDictionary viewData, TempDataDictionary tempData)
            : base(httpContext, routeData, controller) {

            if (String.IsNullOrEmpty(viewName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "viewName");
            }
            ViewData = viewData;
            TempData = tempData;
            ViewName = viewName;
            MasterName = masterName;
        }

        public ViewContext(ControllerContext controllerContext, string viewName, string masterName, ViewDataDictionary viewData, TempDataDictionary tempData)
            : this(
            GetControllerContext(controllerContext).HttpContext,
            GetControllerContext(controllerContext).RouteData,
            GetControllerContext(controllerContext).Controller,
            viewName,
            masterName,
            viewData,
            tempData) {
        }

        public string MasterName {
            get;
            private set;
        }

        public TempDataDictionary TempData {
            get;
            private set;
        }

        public ViewDataDictionary ViewData {
            get;
            private set;
        }


        public string ViewName {
            get;
            private set;
        }

    }
}
