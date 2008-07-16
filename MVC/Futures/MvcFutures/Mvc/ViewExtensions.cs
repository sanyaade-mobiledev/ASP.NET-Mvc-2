namespace Microsoft.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Web.Mvc.Internal;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ViewExtensions {

        public static void RenderRoute(this HtmlHelper helper, RouteValueDictionary values) {
            var routeData = new RouteData();
            foreach (var kvp in values) {
                routeData.Values.Add(kvp.Key, kvp.Value);
            }
            var httpContext = helper.ViewContext.HttpContext;
            var requestContext = new RequestContext(httpContext, routeData);
            var handler = new RenderActionMvcHandler(requestContext);
            handler.ProcessRequestInternal(httpContext);
        }

        public static void RenderAction<TController>(this HtmlHelper helper, Expression<Action<TController>> action) where TController : Controller {
            RouteValueDictionary rvd = ExpressionHelper.GetRouteValuesFromExpression(action);
            helper.RenderRoute(rvd);
        }

        public static void RenderAction(this HtmlHelper helper, string actionName, string controllerName, object values) {
            helper.RenderAction(actionName, controllerName, new RouteValueDictionary(values));
        }

        public static void RenderAction(this HtmlHelper helper, string actionName, string controllerName, RouteValueDictionary values) {
            RouteValueDictionary rvd = null;
            if (values != null) {
                rvd = new RouteValueDictionary(values);
            }
            else {
                rvd = new RouteValueDictionary();
            }

            foreach (var entry in helper.ViewContext.RouteData.Values) {
                if (!rvd.ContainsKey(entry.Key)) {
                    rvd.Add(entry.Key, entry.Value);
                }
            }

            if (!rvd.ContainsKey("action")) {
                rvd["action"] = actionName;
            }

            if (!rvd.ContainsKey("controller") && !String.IsNullOrEmpty(controllerName)) {
                rvd["controller"] = controllerName;
            }
            
            helper.RenderRoute(rvd);
        }

        public static void RenderAction(this HtmlHelper helper, string actionName, string controllerName) {
            helper.RenderAction(actionName, controllerName, null);
        }

        public static void RenderAction(this HtmlHelper helper, string actionName) {
            helper.RenderAction(actionName, null);
        }

        private class RenderActionMvcHandler : MvcHandler {
            public RenderActionMvcHandler(RequestContext context) : base(context) { 
            }

            public void ProcessRequestInternal(HttpContextBase httpContext) {
                base.ProcessRequest(httpContext);
            }
        }
    }
}
