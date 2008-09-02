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

        protected internal bool TryUpdateModel(object model, string[] keys) {
            return TryUpdateModel(model, keys, null /* objectPrefix */);
        }

        protected internal bool TryUpdateModel(object model, string[] keys, string objectPrefix) {
            UpdateModelCore(model, keys, objectPrefix);
            return ViewData.ModelState.IsValid;
        }

        protected internal void UpdateModel(object model, string[] keys) {
            UpdateModel(model, keys, null /* objectPrefix */);
        }

        protected internal void UpdateModel(object model, string[] keys, string objectPrefix) {
            UpdateModelCore(model, keys, objectPrefix);

            if (!ViewData.ModelState.IsValid) {
                string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.Controller_UpdateModel_UpdateUnsuccessful,
                    model.GetType().FullName);
                throw new InvalidOperationException(message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We want to continue if a property setter errors out.")]
        private void UpdateModelCore(object model, string[] keys, string objectPrefix) {
            if (model == null) {
                throw new ArgumentNullException("model");
            }
            if (keys == null || keys.Length == 0) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "keys");
            }

            PropertyDescriptorCollection propCollection = TypeDescriptor.GetProperties(model);
            Dictionary<string, PropertyDescriptor> properties = new Dictionary<string, PropertyDescriptor>();
            foreach (string key in keys) {
                if (String.IsNullOrEmpty(key)) {
                    continue;
                }

                // locate the property descriptor for each property and prepend the prefix (if exists) to the key name
                PropertyDescriptor propDescriptor = propCollection.Find(key, true /* ignoreCase */);
                if (propDescriptor == null) {
                    string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.Controller_UpdateModel_PropertyNotFound,
                        model.GetType().FullName, key);
                    throw new ArgumentException(message, "keys");
                }

                string fieldName = (String.IsNullOrEmpty(objectPrefix)) ? key : objectPrefix + "." + key;
                properties[fieldName] = propDescriptor;
            }

            foreach (var property in properties) {
                string fieldName = property.Key;
                PropertyDescriptor propDescriptor = property.Value;

                IModelBinder converter = ModelBinders.GetBinder(propDescriptor.PropertyType);
                object convertedValue = converter.GetValue(ControllerContext, fieldName, propDescriptor.PropertyType, ViewData.ModelState);
                if (convertedValue != null) {
                    try {
                        propDescriptor.SetValue(model, convertedValue);
                    }
                    catch {
                        // want to use the current culture since this message is potentially displayed to the end user
                        string message = String.Format(CultureInfo.CurrentCulture, MvcResources.Common_ValueNotValidForProperty,
                            convertedValue, propDescriptor.Name);
                        string attemptedValue = Convert.ToString(convertedValue, CultureInfo.CurrentCulture);
                        ViewData.ModelState.AddModelError(fieldName, attemptedValue, message);
                    }
                }
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
