namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class Controller : IActionFilter, IController, IDisposable {

        private RouteCollection _routeCollection;
        private IDictionary<string, object> _viewData;
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

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This property is settable so that unit tests can provide mock implementations.")]
        public TempDataDictionary TempData {
            get;
            set;
        }

        public IPrincipal User {
            get {
                return HttpContext == null ? null : HttpContext.User;
            }
        }

        public IDictionary<string, object> ViewData {
            get {
                if (_viewData == null) {
                    _viewData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
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

        private static ArgumentException CreateDuplicateEntriesException(string key, string parameterName) {
            return new ArgumentException(
                String.Format(CultureInfo.CurrentUICulture, MvcResources.Helper_DictionaryAlreadyContainsKey, key), parameterName);
        }

        // These methods exist to help steer people toward implementing the Dispose pattern correctly.
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
            TempData = new TempDataDictionary(controllerContext.HttpContext);

            string actionName = RouteData.GetRequiredString("action");
            ControllerActionInvoker invoker = ActionInvoker ?? new ControllerActionInvoker(controllerContext);
            if (!invoker.InvokeAction(actionName, new Dictionary<string, object>())) {
                HandleUnknownAction(actionName);
            }
        }

        protected virtual void HandleUnknownAction(string actionName) {
            throw new HttpException(404, String.Format(CultureInfo.CurrentUICulture,
                MvcResources.Controller_UnknownAction, actionName, GetType().FullName));
        }

        protected virtual void OnActionExecuting(ActionExecutingContext filterContext) {
        }

        protected virtual void OnActionExecuted(ActionExecutedContext filterContext) {
        }

        protected virtual void OnResultExecuted(ResultExecutedContext filterContext) {
        }

        protected virtual void OnResultExecuting(ResultExecutingContext filterContext) {
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Instance method for consistency with other helpers.")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#",
            Justification = "Response.Redirect() takes its URI as a string parameter.")]
        protected internal HttpRedirectResult Redirect(string url) {
            if (String.IsNullOrEmpty(url)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "url");
            }
            return new HttpRedirectResult(url);
        }

        protected internal ActionRedirectResult RedirectToAction(object values) {
            return new ActionRedirectResult(new RouteValueDictionary(values)) {
                Routes = RouteCollection
            };
        }

        protected internal ActionRedirectResult RedirectToAction(RouteValueDictionary values) {
            RouteValueDictionary newDict = (values != null) ? new RouteValueDictionary(values) : new RouteValueDictionary();
            return new ActionRedirectResult(newDict) {
                Routes = RouteCollection
            };
        }

        protected internal ActionRedirectResult RedirectToAction(string actionName) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            return new ActionRedirectResult(new RouteValueDictionary() { { "action", actionName } }) {
                Routes = RouteCollection
            };
        }

        protected internal ActionRedirectResult RedirectToAction(string actionName, object values) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            RouteValueDictionary newDict = new RouteValueDictionary(values);
            if (!TryAddValue(newDict, "action", actionName)) {
                throw CreateDuplicateEntriesException("action", "actionName");
            }
            return new ActionRedirectResult(newDict) {
                Routes = RouteCollection
            };
        }

        protected internal ActionRedirectResult RedirectToAction(string actionName, RouteValueDictionary values) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            RouteValueDictionary newDict = (values != null) ? new RouteValueDictionary(values) : new RouteValueDictionary();
            if (!TryAddValue(newDict, "action", actionName)) {
                throw CreateDuplicateEntriesException("action", "actionName");
            }
            return new ActionRedirectResult(newDict) {
                Routes = RouteCollection
            };
        }

        protected internal ActionRedirectResult RedirectToAction(string actionName, string controllerName) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            return new ActionRedirectResult(new RouteValueDictionary() { { "action", actionName }, { "controller", controllerName } }) {
                Routes = RouteCollection
            };
        }

        protected internal ActionRedirectResult RedirectToAction(string actionName, string controllerName, object values) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            RouteValueDictionary newDict = new RouteValueDictionary(values);
            if (!TryAddValue(newDict, "action", actionName)) {
                throw CreateDuplicateEntriesException("action", "actionName");
            }
            if (!TryAddValue(newDict, "controller", controllerName)) {
                throw CreateDuplicateEntriesException("controller", "controllerName");
            }
            return new ActionRedirectResult(newDict) {
                Routes = RouteCollection
            };
        }

        protected internal ActionRedirectResult RedirectToAction(string actionName, string controllerName, RouteValueDictionary values) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            RouteValueDictionary newDict = (values != null) ? new RouteValueDictionary(values) : new RouteValueDictionary();
            if (!TryAddValue(newDict, "action", actionName)) {
                throw CreateDuplicateEntriesException("action", "actionName");
            }
            if (!TryAddValue(newDict, "controller", controllerName)) {
                throw CreateDuplicateEntriesException("controller", "controllerName");
            }
            return new ActionRedirectResult(newDict) {
                Routes = RouteCollection
            };
        }

        protected internal RenderViewResult RenderView() {
            return RenderView(null /* viewName */, null /* masterName */, null /* viewData */);
        }

        protected internal RenderViewResult RenderView(object viewData) {
            return RenderView(null /* viewName */, null /* masterName */, viewData);
        }

        protected internal RenderViewResult RenderView(string viewName) {
            return RenderView(viewName, null /* masterName */, null /* viewData */);
        }

        protected internal RenderViewResult RenderView(string viewName, string masterName) {
            return RenderView(viewName, masterName, null /* viewData */);
        }

        protected internal RenderViewResult RenderView(string viewName, object viewData) {
            return RenderView(viewName, null /* masterName */, viewData);
        }

        protected internal RenderViewResult RenderView(string viewName, string masterName, object viewData) {
            return new RenderViewResult() {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = viewData ?? ViewData,
                ViewEngine = ViewEngine,
                TempData = TempData
            };
        }

        private static bool TryAddValue(IDictionary<string, object> dict, string key, object value) {
            if (dict.ContainsKey(key)) {
                return false;
            }
            else {
                dict[key] = value;
                return true;
            }
        }

        #region IActionFilter Members
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext) {
            OnActionExecuting(filterContext);
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext) {
            OnActionExecuted(filterContext);
        }

        void IActionFilter.OnResultExecuting(ResultExecutingContext filterContext) {
            OnResultExecuting(filterContext);
        }

        void IActionFilter.OnResultExecuted(ResultExecutedContext filterContext) {
            OnResultExecuted(filterContext);
        }
        #endregion

        #region IController Members
        void IController.Execute(ControllerContext controllerContext) {
            Execute(controllerContext);
        }
        #endregion

    }
    
}
