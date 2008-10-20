namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class Controller : ControllerBase, IActionFilter, IAuthorizationFilter, IDisposable, IExceptionFilter, IResultFilter {

        private IActionInvoker _actionInvoker;
        private RouteCollection _routeCollection;
        private ITempDataProvider _tempDataProvider;

        public IActionInvoker ActionInvoker {
            get {
                if (_actionInvoker == null) {
                    _actionInvoker = new ControllerActionInvoker();
                }
                return _actionInvoker;
            }
            set {
                _actionInvoker = value;
            }
        }

        public HttpContextBase HttpContext {
            get {
                return ControllerContext == null ? null : ControllerContext.HttpContext;
            }
        }

        public ModelStateDictionary ModelState {
            get {
                return ViewData.ModelState;
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

        public ITempDataProvider TempDataProvider {
            get {
                if (_tempDataProvider == null) {
                    _tempDataProvider = new SessionStateTempDataProvider();
                }
                return _tempDataProvider;
            }
            set {
                _tempDataProvider = value;
            }
        }

        public UrlHelper Url {
            get;
            set;
        }

        public IPrincipal User {
            get {
                return HttpContext == null ? null : HttpContext.User;
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

        // The default invoker will never match methods defined on the Controller type, so
        // the Dispose() method is not web-callable.  However, in general, since implicitly-
        // implemented interface methods are public, they are web-callable unless decorated with
        // [NonAction].
        [SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
        public void Dispose() {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
        }

        protected override void ExecuteCore() {
            TempData.Load(ControllerContext, TempDataProvider);

            try {
                string actionName = RouteData.GetRequiredString("action");
                if (!ActionInvoker.InvokeAction(ControllerContext, actionName)) {
                    HandleUnknownAction(actionName);
                }
            }
            finally {
                TempData.Save(ControllerContext, TempDataProvider);
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

        protected override void Initialize(RequestContext requestContext) {
            base.Initialize(requestContext);
            Url = new UrlHelper(requestContext);
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

        protected internal PartialViewResult PartialView() {
            return PartialView(null /* viewName */, null /* model */);
        }

        protected internal PartialViewResult PartialView(object model) {
            return PartialView(null /* viewName */, model);
        }

        protected internal PartialViewResult PartialView(string viewName) {
            return PartialView(viewName, null /* model */);
        }

        protected internal virtual PartialViewResult PartialView(string viewName, object model) {
            if (model != null) {
                ViewData.Model = model;
            }

            return new PartialViewResult {
                ViewName = viewName,
                ViewData = ViewData,
                TempData = TempData
            };
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

        protected internal bool TryUpdateModel<TModel>(TModel model) where TModel : class {
            return TryUpdateModel(model, null, null, null, ValueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, string prefix) where TModel : class {
            return TryUpdateModel(model, prefix, null, null, ValueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, string[] includeProperties) where TModel : class {
            return TryUpdateModel(model, null, includeProperties, null, ValueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties) where TModel : class {
            return TryUpdateModel(model, prefix, includeProperties, null, ValueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) where TModel : class {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties, ValueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, IValueProvider valueProvider) where TModel : class {
            return TryUpdateModel(model, null, null, null, valueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, string prefix, IValueProvider valueProvider) where TModel : class {
            return TryUpdateModel(model, prefix, null, null, valueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, string[] includeProperties, IValueProvider valueProvider) where TModel : class {
            return TryUpdateModel(model, null, includeProperties, null, valueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, IValueProvider valueProvider) where TModel : class {
            return TryUpdateModel(model, prefix, includeProperties, null, valueProvider);
        }

        protected internal bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties, IValueProvider valueProvider) where TModel : class {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            if (valueProvider == null) {
                throw new ArgumentNullException("valueProvider");
            }

            Predicate<string> propertyFilter = propertyName => BindAttribute.IsPropertyAllowed(propertyName, includeProperties, excludeProperties);
            IModelBinder binder = ModelBinders.GetBinder(typeof(TModel));

            ModelBindingContext bindingContext = new ModelBindingContext(ControllerContext, valueProvider, typeof(TModel), prefix, () => model, ModelState, propertyFilter);
            binder.BindModel(bindingContext);
            return ModelState.IsValid;
        }

        protected internal void UpdateModel<TModel>(TModel model) where TModel : class {
            UpdateModel(model, null, null, null, ValueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, string prefix) where TModel : class {
            UpdateModel(model, prefix, null, null, ValueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, string[] includeProperties) where TModel : class {
            UpdateModel(model, null, includeProperties, null, ValueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, string prefix, string[] includeProperties) where TModel : class {
            UpdateModel(model, prefix, includeProperties, null, ValueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) where TModel : class {
            UpdateModel(model, prefix, includeProperties, excludeProperties, ValueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, IValueProvider valueProvider) where TModel : class {
            UpdateModel(model, null, null, null, valueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, string prefix, IValueProvider valueProvider) where TModel : class {
            UpdateModel(model, prefix, null, null, valueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, string[] includeProperties, IValueProvider valueProvider) where TModel : class {
            UpdateModel(model, null, includeProperties, null, valueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, IValueProvider valueProvider) where TModel : class {
            UpdateModel(model, prefix, includeProperties, null, valueProvider);
        }

        protected internal void UpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties, IValueProvider valueProvider) where TModel : class {
            bool success = TryUpdateModel(model, prefix, includeProperties, excludeProperties, valueProvider);
            if (!success) {
                string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.Controller_UpdateModel_UpdateUnsuccessful,
                    typeof(TModel).FullName);
                throw new InvalidOperationException(message);
            }
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

            return new ViewResult {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = ViewData,
                TempData = TempData
            };
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#",
            Justification = "The method name 'View' is a convenient shorthand for 'CreateViewResult'.")]
        protected internal ViewResult View(IView view) {
            return View(view, null /* model */);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#",
            Justification = "The method name 'View' is a convenient shorthand for 'CreateViewResult'.")]
        protected internal virtual ViewResult View(IView view, object model) {
            if (model != null) {
                ViewData.Model = model;
            }

            return new ViewResult {
                View = view,
                ViewData = ViewData,
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
