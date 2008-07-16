namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ControllerActionInvoker {

        public ControllerActionInvoker(ControllerContext controllerContext) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }

            ControllerContext = controllerContext;
        }

        public ControllerContext ControllerContext {
            get;
            private set;
        }

        internal static object ConvertParameterType(object value, Type destinationType, string parameterName, string actionName) {
            if (value == null || destinationType.IsInstanceOfType(value)) {
                return value;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool canConvertFrom = converter.CanConvertFrom(value.GetType());
            if (!canConvertFrom) {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!(canConvertFrom || converter.CanConvertTo(destinationType))) {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    MvcResources.Controller_CannotConvertParameter,
                    parameterName, actionName, value, destinationType));
            }
            try {
                return canConvertFrom ?
                    converter.ConvertFrom(null /* context */, CultureInfo.InvariantCulture, value) :
                    converter.ConvertTo(null /* context */, CultureInfo.InvariantCulture, value, destinationType);
            }
            catch (Exception ex) {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    MvcResources.Controller_CannotConvertParameter,
                    parameterName, actionName, value, destinationType),
                    ex);
            }
        }

        private static object ExtractParameterFromDictionary(ParameterInfo parameterInfo, IDictionary<string, object> parameters) {
            object value;
            bool wasFound = parameters.TryGetValue(parameterInfo.Name, out value);

            // The key should always be present, even if the parameter value is null.
            if (!wasFound ||
                (!(value == null && TypeHelpers.TypeAllowsNullValue(parameterInfo.ParameterType)) &&
                !parameterInfo.ParameterType.IsInstanceOfType(value))) {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    MvcResources.ControllerActionInvoker_ParameterDictionaryIsInvalid,
                    parameterInfo.Member.Name));
            }
            return value;
        }

        private IList<T> FiltersToTypedList<T>(IList<FilterAttribute> filters) where T : class {
            List<T> typedList = new List<T>();

            // always add the controller (if it's of the right type) first
            T controllerFilter = ControllerContext.Controller as T;
            if (controllerFilter != null) {
                typedList.Add(controllerFilter);
            }

            // then add filters of the right type
            typedList.AddRange(filters.OfType<T>());
            return typedList;
        }

        protected virtual MethodInfo FindActionMethod(string actionName, IDictionary<string, object> values) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }

            // We have to loop through all the methods to make sure there isn't
            // a conflict. If we stop the loop the first time we find a match
            // we might miss some error cases.

            MemberInfo[] memberInfos = ControllerContext.Controller.GetType().GetMember(actionName, MemberTypes.Method,
                BindingFlags.IgnoreCase | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public);
            MethodInfo foundMatch = null;

            foreach (MethodInfo methodInfo in memberInfos) {

                // 1) Action methods must not have the non-action attribute in their inheritance chain, and
                // 2) special methods like constructors, property accessors, and event accessors cannot be action methods, and
                // 3) methods originally defined on Object (like ToString()) or Controller (like Dispose()) cannot be action methods.
                if (!methodInfo.IsDefined(typeof(NonActionAttribute), true /* inherit */) &&
                    !methodInfo.IsSpecialName &&
                    !methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof(Controller))) {
                    if (foundMatch != null) {
                        throw new InvalidOperationException(String.Format(
                            CultureInfo.CurrentUICulture, MvcResources.Controller_MoreThanOneAction,
                            actionName, ControllerContext.Controller.GetType()));
                    }
                    foundMatch = methodInfo;
                }
            }

            if (foundMatch != null) {
                if (foundMatch.ContainsGenericParameters) {
                    throw new InvalidOperationException(String.Format(
                        CultureInfo.CurrentUICulture, MvcResources.Controller_ActionCannotBeGeneric,
                        foundMatch.Name));
                }
            }

            return foundMatch;
        }

        protected virtual FilterInfo GetFiltersForActionMethod(MethodInfo methodInfo) {
            if (methodInfo == null) {
                throw new ArgumentNullException("methodInfo");
            }

            // Enumerable.OrderBy() is a stable sort, so this method preserves scope ordering.
            FilterAttribute[] typeFilters = (FilterAttribute[])methodInfo.ReflectedType.GetCustomAttributes(typeof(FilterAttribute), true /* inherit */);
            FilterAttribute[] methodFilters = (FilterAttribute[])methodInfo.GetCustomAttributes(typeof(FilterAttribute), true /* inherit */);
            List<FilterAttribute> orderedFilters = typeFilters.Concat(methodFilters).OrderBy(attr => attr.Order).ToList();

            FilterInfo filterInfo = new FilterInfo {
                ActionFilters = FiltersToTypedList<IActionFilter>(orderedFilters),
                AuthorizationFilters = FiltersToTypedList<IAuthorizationFilter>(orderedFilters),
                ExceptionFilters = FiltersToTypedList<IExceptionFilter>(orderedFilters),
                ResultFilters = FiltersToTypedList<IResultFilter>(orderedFilters)
            };
            return filterInfo;
        }

        protected virtual object GetParameterValue(ParameterInfo parameterInfo, IDictionary<string, object> values) {
            if (parameterInfo == null) {
                throw new ArgumentNullException("parameterInfo");
            }

            Type parameterType = parameterInfo.ParameterType;
            string parameterName = parameterInfo.Name;
            string actionName = parameterInfo.Member.Name;

            bool valueRequired = !TypeHelpers.TypeAllowsNullValue(parameterType);

            // Try to get a value for the parameter. We use this order of precedence:
            // 1. Explicitly-provided extra parameters in the call to InvokeAction()
            // 2. Values from the RouteData (could be from the typed-in URL or from the route's default values)
            // 3. Request values (query string, form post data, cookie)
            object parameterValue = null;
            if (!(values != null && values.TryGetValue(parameterName, out parameterValue))) {
                if (!(ControllerContext.RouteData != null && ControllerContext.RouteData.Values.TryGetValue(parameterName, out parameterValue))) {
                    if (ControllerContext.HttpContext != null && ControllerContext.HttpContext.Request != null) {
                        parameterValue = ControllerContext.HttpContext.Request[parameterName];
                    }
                }
            }

            if (parameterValue == null && valueRequired) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture, MvcResources.Controller_MissingParameter, parameterName, actionName));
            }

            try {
                return ConvertParameterType(parameterValue, parameterType, parameterName, actionName);
            }
            catch (Exception ex) {
                // Parameter value conversion errors are acceptable unless the value is required
                if (valueRequired) {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture, MvcResources.Controller_MissingParameter, parameterName, actionName), ex);
                }
            }
            return null;
        }

        protected virtual IDictionary<string, object> GetParameterValues(MethodInfo methodInfo, IDictionary<string, object> values) {
            if (methodInfo == null) {
                throw new ArgumentNullException("methodInfo");
            }

            Dictionary<string, object> parameterDict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                if (parameterInfo.IsOut || parameterInfo.ParameterType.IsByRef) {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture, MvcResources.Controller_ReferenceParametersNotSupported, parameterInfo.Name, methodInfo.Name));
                }
                parameterDict[parameterInfo.Name] = GetParameterValue(parameterInfo, values);
            }
            return parameterDict;
        }

        public virtual bool InvokeAction(string actionName, IDictionary<string, object> values) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }

            MethodInfo methodInfo = FindActionMethod(actionName, values);
            if (methodInfo != null) {
                IDictionary<string, object> parameters = GetParameterValues(methodInfo, values);
                FilterInfo filterInfo = GetFiltersForActionMethod(methodInfo);

                try {
                    AuthorizationContext authContext = InvokeAuthorizationFilters(methodInfo, filterInfo.AuthorizationFilters);
                    if (authContext.Cancel) {
                        // not authorized, so don't execute the action method or its filters
                        InvokeActionResult(authContext.Result ?? EmptyResult.Instance);
                    }
                    else {
                        ActionExecutedContext postActionContext = InvokeActionMethodWithFilters(methodInfo, parameters, filterInfo.ActionFilters);
                        InvokeActionResultWithFilters(postActionContext.Result, filterInfo.ResultFilters);
                    }
                }
                catch (Exception ex) {
                    // something blew up, so execute the exception filters
                    ExceptionContext exceptionContext = InvokeExceptionFilters(ex, filterInfo.ExceptionFilters);
                    if (!exceptionContext.ExceptionHandled) {
                        throw;
                    }
                    InvokeActionResult(exceptionContext.Result);
                }

                return true;
            }

            // notify controller that no method matched
            return false;
        }

        protected virtual ActionResult InvokeActionMethod(MethodInfo methodInfo, IDictionary<string, object> parameters) {
            if (methodInfo == null) {
                throw new ArgumentNullException("methodInfo");
            }
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }

            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            if (parameterInfos.Length != parameters.Count) {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    MvcResources.ControllerActionInvoker_ParameterDictionaryIsInvalid,
                    methodInfo.Name));
            }

            IController controller = ControllerContext.Controller;
            object[] parametersArray = (from parameterInfo
                                        in parameterInfos
                                        select ExtractParameterFromDictionary(parameterInfo, parameters)).ToArray();
            object returnValue = methodInfo.Invoke(controller, parametersArray);

            if (returnValue == null) {
                return new EmptyResult();
            }

            ActionResult actionResult = (returnValue as ActionResult) ??
                new ContentResult { Content = Convert.ToString(returnValue, CultureInfo.InvariantCulture) };
            return actionResult;
        }

        internal static ActionExecutedContext InvokeActionMethodFilter(IActionFilter filter, ActionExecutingContext preContext, Func<ActionExecutedContext> continuation) {
            filter.OnActionExecuting(preContext);
            if (preContext.Cancel) {
                return new ActionExecutedContext(preContext, preContext.ActionMethod, true /* canceled */, null /* exception */) {
                    Result = preContext.Result
                };
            }

            bool wasError = false;
            ActionExecutedContext postContext = null;
            try {
                postContext = continuation();
            }
            catch (Exception ex) {
                wasError = true;
                postContext = new ActionExecutedContext(preContext, preContext.ActionMethod, false /* canceled */, ex);
                filter.OnActionExecuted(postContext);
                if (!postContext.ExceptionHandled) {
                    throw;
                }
            }
            if (!wasError) {
                filter.OnActionExecuted(postContext);
            }
            return postContext;
        }

        protected virtual ActionExecutedContext InvokeActionMethodWithFilters(MethodInfo methodInfo, IDictionary<string, object> parameters, IList<IActionFilter> filters) {
            if (methodInfo == null) {
                throw new ArgumentNullException("methodInfo");
            }
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }
            if (filters == null) {
                throw new ArgumentNullException("filters");
            }

            ActionExecutingContext preContext = new ActionExecutingContext(ControllerContext, methodInfo, parameters);
            Func<ActionExecutedContext> continuation = () =>
                new ActionExecutedContext(ControllerContext, methodInfo, false /* canceled */, null /* exception */) {
                    Result = InvokeActionMethod(methodInfo, parameters)
                };

            // need to reverse the filter list because the continuations are built up backward
            Func<ActionExecutedContext> thunk = filters.Reverse().Aggregate(continuation,
                (next, filter) => () => InvokeActionMethodFilter(filter, preContext, next));
            return thunk();
        }

        protected virtual void InvokeActionResult(ActionResult actionResult) {
            if (actionResult == null) {
                throw new ArgumentNullException("actionResult");
            }
            actionResult.ExecuteResult(ControllerContext);
        }

        internal static ResultExecutedContext InvokeActionResultFilter(IResultFilter filter, ResultExecutingContext preContext, Func<ResultExecutedContext> continuation) {
            filter.OnResultExecuting(preContext);
            if (preContext.Cancel) {
                return new ResultExecutedContext(preContext, preContext.Result, true /* canceled */, null /* exception */);
            }

            bool wasError = false;
            ResultExecutedContext postContext = null;
            try {
                postContext = continuation();
            }
            catch (ThreadAbortException) {
                // This type of exception occurs as a result of Response.Redirect(), but we special-case so that
                // the filters don't see this as an error.
                postContext = new ResultExecutedContext(preContext, preContext.Result, false /* canceled */, null /* exception */);
                filter.OnResultExecuted(postContext);
                throw;
            }
            catch (Exception ex) {
                wasError = true;
                postContext = new ResultExecutedContext(preContext, preContext.Result, false /* canceled */, ex);
                filter.OnResultExecuted(postContext);
                if (!postContext.ExceptionHandled) {
                    throw;
                }
            }
            if (!wasError) {
                filter.OnResultExecuted(postContext);
            }
            return postContext;
        }

        protected virtual ResultExecutedContext InvokeActionResultWithFilters(ActionResult actionResult, IList<IResultFilter> filters) {
            if (actionResult == null) {
                throw new ArgumentNullException("actionResult");
            }
            if (filters == null) {
                throw new ArgumentNullException("filters");
            }

            ResultExecutingContext preContext = new ResultExecutingContext(ControllerContext, actionResult);
            Func<ResultExecutedContext> continuation = delegate {
                InvokeActionResult(actionResult);
                return new ResultExecutedContext(ControllerContext, preContext.Result, false /* canceled */, null /* exception */);
            };

            // need to reverse the filter list because the continuations are built up backward
            Func<ResultExecutedContext> thunk = filters.Reverse().Aggregate(continuation,
                (next, filter) => () => InvokeActionResultFilter(filter, preContext, next));
            return thunk();
        }

        protected virtual AuthorizationContext InvokeAuthorizationFilters(MethodInfo methodInfo, IList<IAuthorizationFilter> filters) {
            if (methodInfo == null) {
                throw new ArgumentNullException("methodInfo");
            }
            if (filters == null) {
                throw new ArgumentNullException("filters");
            }

            AuthorizationContext context = new AuthorizationContext(ControllerContext, methodInfo);
            foreach (IAuthorizationFilter filter in filters) {
                filter.OnAuthorization(context);
                // short-circuit evaluation
                if (context.Cancel) {
                    break;
                }
            }

            return context;
        }

        protected virtual ExceptionContext InvokeExceptionFilters(Exception exception, IList<IExceptionFilter> filters) {
            if (exception == null) {
                throw new ArgumentNullException("exception");
            }
            if (filters == null) {
                throw new ArgumentNullException("filters");
            }

            ExceptionContext context = new ExceptionContext(ControllerContext, exception);
            foreach (IExceptionFilter filter in filters) {
                filter.OnException(context);
            }

            return context;
        }
    }
}
