namespace System.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class HandleErrorAttribute : FilterAttribute, IExceptionFilter {

        private const string _defaultView = "Error";

        private Type _exceptionType = typeof(Exception);
        private string _view;

        public Type ExceptionType {
            get {
                return _exceptionType;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                if (!typeof(Exception).IsAssignableFrom(value)) {
                    throw new ArgumentException(String.Format(CultureInfo.CurrentUICulture,
                        MvcResources.ExceptionViewAttribute_NonExceptionType, value.FullName));
                }

                _exceptionType = value;
            }
        }

        public string View {
            get {
                return (!String.IsNullOrEmpty(_view)) ? _view : _defaultView;
            }
            set {
                _view = value;
            }
        }

        public void OnException(ExceptionContext filterContext) {
            if (filterContext == null) {
                throw new ArgumentNullException("filterContext");
            }

            // TODO: It's somewhat of a pain that filters can't assume anything about the controller
            // since we're just exposing the instance of an IController, which really isn't that
            // useful of a type.  We need to investigate removing the need to perform the cast.
            Controller controller = filterContext.Controller as Controller;
            if (controller == null || filterContext.ExceptionHandled) {
                return;
            }

            Exception exception = filterContext.Exception;

            // If this is not an HTTP 500 (for example, if somebody throws an HTTP 404 from an action method),
            // ignore it.
            if (new HttpException(null, exception).GetHttpCode() != 500) {
                return;
            }

            // Action method exceptions will be wrapped in a TargetInvocationException since they're invoked
            // using reflection, so we have to unwrap it.
            if (exception is TargetInvocationException) {
                exception = exception.InnerException;
            }

            if (!ExceptionType.IsInstanceOfType(exception)) {
                return;
            }

            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            filterContext.Result = new ViewResult() {
                TempData = controller.TempData,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                ViewEngine = controller.ViewEngine,
                ViewName = View,
            };
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
        }
    }
}
