namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ControllerActionInvoker : IActionInvoker {

        private readonly static ActionMethodDispatcherCache _staticDispatcherCache = new ActionMethodDispatcherCache();
        private readonly static ActionMethodSelectorCache _staticSelectorCache = new ActionMethodSelectorCache();

        private ActionMethodDispatcherCache _instanceDispatcherCache;
        private ActionMethodSelectorCache _instanceSelectorCache;

        public ControllerContext ControllerContext {
            get;
            protected set;
        }

        internal ActionMethodDispatcherCache DispatcherCache {
            get {
                if (_instanceDispatcherCache == null) {
                    _instanceDispatcherCache = _staticDispatcherCache;
                }
                return _instanceDispatcherCache;
            }
            set {
                _instanceDispatcherCache = value;
            }
        }

        internal ActionMethodSelectorCache SelectorCache {
            get {
                if (_instanceSelectorCache == null) {
                    _instanceSelectorCache = _staticSelectorCache;
                }
                return _instanceSelectorCache;
            }
            set {
                _instanceSelectorCache = value;
            }
        }

        protected virtual ActionResult CreateActionResult(object actionReturnValue) {
            if (actionReturnValue == null) {
                return new EmptyResult();
            }

            ActionResult actionResult = (actionReturnValue as ActionResult) ??
                new ContentResult { Content = Convert.ToString(actionReturnValue, CultureInfo.InvariantCulture) };
            return actionResult;
        }

        private static object ExtractParameterFromDictionary(ParameterInfo parameterInfo, IDictionary<string, object> parameters, MethodInfo methodInfo) {
            object value;
            bool wasFound = parameters.TryGetValue(parameterInfo.Name, out value);

            // The key should always be present, even if the parameter value is null.
            if (!wasFound ||
                (!(value == null && TypeHelpers.TypeAllowsNullValue(parameterInfo.ParameterType)) &&
                !parameterInfo.ParameterType.IsInstanceOfType(value))) {
                string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.ControllerActionInvoker_ParameterDictionaryContainsInvalidEntry,
                    parameterInfo.ParameterType, parameterInfo.Name, Convert.ToString(methodInfo, CultureInfo.CurrentUICulture), methodInfo.DeclaringType);
                throw new InvalidOperationException(message);
            }
            return value;
        }

        private IList<TFilter> FiltersToTypedList<TFilter>(IList<FilterAttribute> filters) where TFilter : class {
            List<TFilter> typedList = new List<TFilter>();

            // always add the controller (if it's of the right type) first
            TFilter controllerFilter = ControllerContext.Controller as TFilter;
            if (controllerFilter != null) {
                typedList.Add(controllerFilter);
            }

            // then add filters of the right type
            typedList.AddRange(filters.OfType<TFilter>());
            return typedList;
        }

        protected virtual MethodInfo FindActionMethod(string actionName) {
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }

            Type controllerType = ControllerContext.Controller.GetType();
            ActionMethodSelector selector = SelectorCache.GetSelector(controllerType);
            MethodInfo foundMatch = selector.FindActionMethod(ControllerContext, actionName);
            if (foundMatch != null) {
                if (foundMatch.ContainsGenericParameters) {
                    throw new InvalidOperationException(String.Format(
                        CultureInfo.CurrentUICulture, MvcResources.Controller_ActionCannotBeGeneric,
                        foundMatch));
                }
            }

            return foundMatch;
        }

        private static string GetFieldPrefix(ParameterInfo parameterInfo) {
            BindAttribute attr = (BindAttribute)Attribute.GetCustomAttribute(parameterInfo, typeof(BindAttribute));
            return ((attr != null) && (attr.Prefix != null)) ? attr.Prefix : parameterInfo.Name;
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

        private static IModelBinder GetModelBinder(ParameterInfo parameterInfo) {
            // first we look up attributes on the parameter itself
            CustomModelBinderAttribute[] attrs = (CustomModelBinderAttribute[])parameterInfo.GetCustomAttributes(typeof(CustomModelBinderAttribute), false /* inherit */);
            switch (attrs.Length) {
                case 0:
                    break;
                case 1:
                    IModelBinder converter = attrs[0].GetBinder();
                    return converter;
                default:
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                        MvcResources.ControllerActionInvoker_MultipleConverterAttributes,
                        parameterInfo.Name, parameterInfo.Member.Name));
            }

            // failing that, we retrieve the global GetConverter() method
            IModelBinder globalConverter = ModelBinders.GetBinder(parameterInfo.ParameterType);
            return globalConverter;
        }

        protected virtual object GetParameterValue(ParameterInfo parameterInfo) {
            if (parameterInfo == null) {
                throw new ArgumentNullException("parameterInfo");
            }

            Type parameterType = parameterInfo.ParameterType;
            IModelBinder converter = GetModelBinder(parameterInfo);
            string parameterName = GetFieldPrefix(parameterInfo);
            Predicate<string> propertyFilter = GetPropertyFilter(parameterInfo);

            ModelBindingContext bindingContext = new ModelBindingContext(ControllerContext, ControllerContext.Controller.ValueProvider, parameterType, parameterName, null /* modelProvider */, ControllerContext.Controller.ViewData.ModelState, propertyFilter);
            ModelBinderResult result = converter.BindModel(bindingContext);
            return (result != null) ? result.Value : null;
        }

        protected virtual IDictionary<string, object> GetParameterValues(MethodInfo methodInfo) {
            if (methodInfo == null) {
                throw new ArgumentNullException("methodInfo");
            }

            Dictionary<string, object> parameterDict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                if (parameterInfo.IsOut || parameterInfo.ParameterType.IsByRef) {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture, MvcResources.Controller_ReferenceParametersNotSupported, parameterInfo.Name, methodInfo.Name));
                }
                parameterDict[parameterInfo.Name] = GetParameterValue(parameterInfo);
            }
            return parameterDict;
        }

        private static Predicate<string> GetPropertyFilter(ParameterInfo parameterInfo) {
            BindAttribute attr = (BindAttribute)Attribute.GetCustomAttribute(parameterInfo, typeof(BindAttribute));
            return (attr != null) ? (Predicate<string>)attr.IsPropertyAllowed : null;
        }

        public virtual bool InvokeAction(ControllerContext controllerContext, string actionName) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }

            ControllerContext = controllerContext;

            MethodInfo methodInfo = FindActionMethod(actionName);
            if (methodInfo != null) {
                IDictionary<string, object> parameters = GetParameterValues(methodInfo);
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
                string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.ControllerActionInvoker_ParameterDictionaryCountIncorrect,
                    methodInfo);
                throw new InvalidOperationException(message);
            }

            object[] parametersArray = (from parameterInfo
                                        in parameterInfos
                                        select ExtractParameterFromDictionary(parameterInfo, parameters, methodInfo)).ToArray();

            ActionMethodDispatcher dispatcher = DispatcherCache.GetDispatcher(methodInfo);
            object actionReturnValue = dispatcher.Execute(ControllerContext.Controller, parametersArray);

            ActionResult actionResult = CreateActionResult(actionReturnValue);
            return actionResult;
        }

        internal static ActionExecutedContext InvokeActionMethodFilter(IActionFilter filter, ActionExecutingContext preContext, Func<ActionExecutedContext> continuation) {
            filter.OnActionExecuting(preContext);
            if (preContext.Result != null) {
                return new ActionExecutedContext(preContext, true /* canceled */, null /* exception */) {
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
                postContext = new ActionExecutedContext(preContext, false /* canceled */, ex);
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

            ActionExecutingContext preContext = new ActionExecutingContext(ControllerContext, parameters);
            Func<ActionExecutedContext> continuation = () =>
                new ActionExecutedContext(ControllerContext, false /* canceled */, null /* exception */) {
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
                return new ResultExecutedContext(ControllerContext, actionResult, false /* canceled */, null /* exception */);
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

            AuthorizationContext context = new AuthorizationContext(ControllerContext);
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
