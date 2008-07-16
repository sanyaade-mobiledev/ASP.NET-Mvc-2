namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class Controller : IActionFilter, IAuthorizationFilter, IController, IDisposable, IExceptionFilter, IResultFilter {

        private RouteCollection _routeCollection;
        private TempDataDictionary _tempData;
        private ITempDataProvider _tempDataProvider;
        private ViewDataDictionary _viewData;
        private IViewEngine _viewEngine;

        public ControllerActionInvoker ActionInvoker {
            get;
            set;
        }

        public ControllerContext ControllerContext {
            get;
            set;
        }

        public HttpContextBase HttpContext {
            get {
                return ControllerContext == null ? null : ControllerContext.HttpContext;
            }
        }

        public HttpRequestBase Request {
            get {
                return HttpContext == null ? null : HttpContext.Request;
            }
        }

        public HttpResponseBase Response {
            get {
                return HttpContext == null ? null : HttpContext.Response;
            }
        }

        internal RouteCollection RouteCollection {
            get {
                if (_routeCollection == null) {
                    _routeCollection = RouteTable.Routes;
                }
                return _routeCollection;
            }
            set {
                _routeCollection = value;
            }
        }

        public RouteData RouteData {
            get {
                return ControllerContext == null ? null : ControllerContext.RouteData;
            }
        }

        public HttpServerUtilityBase Server {
            get {
                return HttpContext == null ? null : HttpContext.Server;
            }
        }

        public HttpSessionStateBase Session {
            get {
                return HttpContext == null ? null : HttpContext.Session;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
         Justification = "This property is settable so that unit tests can provide mock implementations.")]
        public TempDataDictionary TempData {
            get {
                if (_tempData == null) {
                    _tempData = new TempDataDictionary();
                }
                return _tempData;
            }
            set {
                _tempData = value;
            }
        }

        public ITempDataProvider TempDataProvider {
            get {
                if (_tempDataProvider == null) {
                    _tempDataProvider = new SessionStateTempDataProvider(ControllerContext.HttpContext);
                }
                return _tempDataProvider;
            }
            set {
                _tempDataProvider = value;
            }
        }

        public IPrincipal User {
            get {
                return HttpContext == null ? null : HttpContext.User;
            }
        }

        public ViewDataDictionary ViewData {
            get {
                if (_viewData == null) {
                    _viewData = new ViewDataDictionary();
                }
                return _viewData;
            }
        }

        public IViewEngine ViewEngine {
            get {
                if (_viewEngine == null) {
                    _viewEngine = new WebFormViewEngine();
                }
                return _viewEngine;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                _viewEngine = value;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#",
            Justification = "'Content' refers to ContentResult type; 'content' refers to ContentResult.Content property.")]
        protected internal ContentResult Content(string content) {
            return Content(content, null /* contentType */);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#",
            Justification = "'Content' refers to ContentResult type; 'content' refers to ContentResult.Content property.")]
        protected internal ContentResult Content(string content, string contentType) {
            return Content(content, contentType, null /* contentEncoding */);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#",
            Justification = "'Content' refers to ContentResult type; 'content' refers to ContentResult.Content property.")]
        protected internal virtual ContentResult Content(string content, string contentType, Encoding contentEncoding) {
            return new ContentResult {
                Content = content,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }

        // The default ControllerActionInvoker will never match methods defined on the Controller
        // class, so the Dispose() method is not web-callable.  However, in general, since
        // implicitly-implemented interface methods are public, they are web-callable unless
        // decorated with [NonAction].
        [SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
        public void Dispose() {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
        }

        protected internal virtual void Execute(ControllerContext controllerContext) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }

            ControllerContext = controllerContext;
            TempData.Load(TempDataProvider);

            try {
                string actionName = RouteData.GetRequiredString("action");
                ControllerActionInvoker invoker = ActionInvoker ?? new ControllerActionInvoker(controllerContext);
                if (!invoker.InvokeAction(actionName, new Dictionary<string, object>())) {
                    HandleUnknownAction(actionName);
                }
            }
            finally {
                TempData.Save(TempDataProvider);
            }
        }

        protected virtual void HandleUnknownAction(string actionName) {
            throw new HttpException(404, String.Format(CultureInfo.CurrentUICulture,
                MvcResources.Controller_UnknownAction, actionName, GetType().FullName));
        }

        protected internal JsonResult Json(object data) {
            return Json(data, null /* contentType */);
        }

        protected internal JsonResult Json(object data, string contentType) {
            return Json(data, contentType, null /* contentEncoding */);
        }

        protected internal virtual JsonResult Json(object data, string contentType, Encoding contentEncoding) {
            return new JsonResult {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }

        protected virtual void OnActionExecuting(ActionExecutingContext filterContext) {
        }

        protected virtual void OnActionExecuted(ActionExecutedContext filterContext) {
        }

        protected virtual void OnAuthorization(AuthorizationContext filterContext) {
        }

        protected virtual void OnException(ExceptionContext filterContext) {
        }

        protected virtual void OnResultExecuted(ResultExecutedContext filterContext) {
        }

        protected virtual void OnResultExecuting(ResultExecutingContext filterContext) {
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Instance method for consistency with other helpers.")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#",
            Justification = "Response.Redirect() takes its URI as a string parameter.")]
        protected internal virtual RedirectResult Redirect(string url) {
            if (String.IsNullOrEmpty(url)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "url");
            }
            return new RedirectResult(url);
        }

        protected internal RedirectToRouteResult RedirectToAction(string actionName) {
            return RedirectToAction(actionName, (RouteValueDictionary)null);
        }

        protected internal RedirectToRouteResult RedirectToAction(string actionName, object values) {
            return RedirectToAction(actionName, new RouteValueDictionary(values));
        }

        protected internal RedirectToRouteResult RedirectToAction(string actionName, RouteValueDictionary values) {
            return RedirectToAction(actionName, null /* controllerName */, values);
        }

        protected internal RedirectToRouteResult RedirectToAction(string actionName, string controllerName) {
            return RedirectToAction(actionName, controllerName, (RouteValueDictionary)null);
        }

        protected internal RedirectToRouteResult RedirectToAction(string actionName, string controllerName, object values) {
            return RedirectToAction(actionName, controllerName, new RouteValueDictionary(values));
        }

        protected internal virtual RedirectToRouteResult RedirectToAction(string actionName, string controllerName, RouteValueDictionary values) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }

            RouteValueDictionary newDict = new RouteValueDictionary();

            newDict["action"] = actionName;
            if (!String.IsNullOrEmpty(controllerName)) {
                newDict["controller"] = controllerName;
            }

            if (!newDict.ContainsKey("controller") && RouteData != null && RouteData.Values.ContainsKey("controller")) {
                newDict["controller"] = RouteData.Values["controller"];
            }

            if (values != null) {
                foreach (var entry in values) {
                    newDict[entry.Key] = entry.Value;
                 }
            }

            return new RedirectToRouteResult(newDict);
        }

        protected internal RedirectToRouteResult RedirectToRoute(object values) {
            return RedirectToRoute(new RouteValueDictionary(values));
        }

        protected internal RedirectToRouteResult RedirectToRoute(RouteValueDictionary values) {
            return RedirectToRoute(null /* routeName */, values);
        }

        protected internal RedirectToRouteResult RedirectToRoute(string routeName) {
            return RedirectToRoute(routeName, (RouteValueDictionary)null);
        }

        protected internal RedirectToRouteResult RedirectToRoute(string routeName, object values) {
            return RedirectToRoute(routeName, new RouteValueDictionary(values));
        }

        protected internal virtual RedirectToRouteResult RedirectToRoute(string routeName, RouteValueDictionary values) {
            RouteValueDictionary newDict = (values != null) ? new RouteValueDictionary(values) : new RouteValueDictionary();
            return new RedirectToRouteResult(routeName, newDict);
        }

        protected internal ViewResult View() {
            return View(null /* viewName */, null /* masterName */, null /* model */);
        }

        protected internal ViewResult View(object model) {
            return View(null /* viewName */, null /* masterName */, model);
        }

        protected internal ViewResult View(string viewName) {
            return View(viewName, null /* masterName */, null /* model */);
        }

        protected internal ViewResult View(string viewName, string masterName) {
            return View(viewName, masterName, null /* model */);
        }

        protected internal ViewResult View(string viewName, object model) {
            return View(viewName, null /* masterName */, model);
        }

        protected internal virtual ViewResult View(string viewName, string masterName, object model) {
            if (model != null) {
                ViewData.Model = model;
            }
            return new ViewResult() {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = ViewData,
                ViewEngine = ViewEngine,
                TempData = TempData
            };
        }

        #region IActionFilter Members
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext) {
            OnActionExecuting(filterContext);
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext) {
            OnActionExecuted(filterContext);
        }
        #endregion

        #region IAuthorizationFilter Members
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext) {
            OnAuthorization(filterContext);
        }
        #endregion

        #region IController Members
        void IController.Execute(ControllerContext controllerContext) {
            Execute(controllerContext);
        }
        #endregion

        #region IExceptionFilter Members
        void IExceptionFilter.OnException(ExceptionContext filterContext) {
            OnException(filterContext);
        }
        #endregion

        #region IResultFilter Members
        void IResultFilter.OnResultExecuting(ResultExecutingContext filterContext) {
            OnResultExecuting(filterContext);
        }

        void IResultFilter.OnResultExecuted(ResultExecutedContext filterContext) {
            OnResultExecuted(filterContext);
        }
        #endregion
    }
}
