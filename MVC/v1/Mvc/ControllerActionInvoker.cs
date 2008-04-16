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
                return canConvertFrom ? converter.ConvertFrom(value) : converter.ConvertTo(value, destinationType);
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
                (!(value == null && TypeAllowsNullValue(parameterInfo.ParameterType)) &&
                !parameterInfo.ParameterType.IsInstanceOfType(value))) {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture,
                    MvcResources.ControllerActionInvoker_ParameterDictionaryIsInvalid,
                    parameterInfo.Member.Name));
            }
            return value;
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
                if (!methodInfo.IsDefined(typeof(NonActionAttribute), true) &&
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
                if (!typeof(ActionResult).IsAssignableFrom(foundMatch.ReturnType)) {
                    throw new InvalidOperationException(String.Format(
                        CultureInfo.CurrentUICulture, MvcResources.ControllerActionInvoker_MethodSignatureHasInvalidReturnType,
                        foundMatch.Name, foundMatch.DeclaringType, foundMatch.ReturnType));
                }
            }

            return foundMatch;
        }

        protected virtual IList<IActionFilter> GetActionFiltersForMember(MemberInfo memberInfo) {
            if (memberInfo == null) {
                throw new ArgumentNullException("memberInfo");
            }

            List<IActionFilter> unorderedFilters = new List<IActionFilter>();
            SortedList<int, IActionFilter> orderedFilters = new SortedList<int, IActionFilter>();

            ActionFilterAttribute[] attrs = (ActionFilterAttribute[])memberInfo.GetCustomAttributes(typeof(ActionFilterAttribute), false /* inherit */);
            foreach (ActionFilterAttribute filter in attrs) {
                // filters are allowed to have the same order only if the order is -1.  in that case,
                // they are processed before explicitly ordered filters but in no particular order in
                // relation to one another.
                if (filter.Order >= 0) {
                    if (orderedFilters.ContainsKey(filter.Order)) {
                        MethodBase methodInfo = memberInfo as MethodBase;
                        string exceptionMessage;
                        if (methodInfo != null) {
                            // Throw customized exception for action method
                            exceptionMessage = String.Format(CultureInfo.CurrentUICulture,
                                MvcResources.ControllerActionInvoker_FiltersOnMethodHaveDuplicateOrder,
                                methodInfo.Name, methodInfo.DeclaringType.FullName, filter.Order);
                        }
                        else {
                            // Throw customized exception for type
                            exceptionMessage = String.Format(CultureInfo.CurrentUICulture,
                                MvcResources.ControllerActionInvoker_FiltersOnTypeHaveDuplicateOrder,
                                ((Type)memberInfo).FullName, filter.Order);
                        }
                        throw new InvalidOperationException(exceptionMessage);
                    }
                    orderedFilters.Add(filter.Order, filter);
                }
                else {
                    unorderedFilters.Add(filter);
                }
            }

            // now append the ordered list to the unordered list to create the final list
            unorderedFilters.AddRange(orderedFilters.Values);
            return unorderedFilters;
        }

        protected virtual IList<IActionFilter> GetAllActionFilters(MethodInfo methodInfo) {
            if (methodInfo == null) {
                throw new ArgumentNullException("methodInfo");
            }

            // use a stack since we're building the member chain backward
            Stack<MemberInfo> memberChain = new Stack<MemberInfo>();

            // first, push the most derived action method, then its base method, and so forth
            memberChain.Push(methodInfo);
            MethodInfo baseMethod = methodInfo.GetBaseDefinition();
            Type curType = methodInfo.DeclaringType.BaseType;
            while (true) {
                MemberInfo[] memberInfos = curType.GetMember(methodInfo.Name, MemberTypes.Method,
                    BindingFlags.IgnoreCase | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public);
                MethodInfo foundMatch = null;
                foreach (MethodInfo possibleMatch in memberInfos) {
                    if (possibleMatch.GetBaseDefinition() == baseMethod) {
                        foundMatch = possibleMatch;
                        break;
                    }
                }
                if (foundMatch == null) {
                    // we've passed the declaring type of the base method
                    break;
                }
                if (foundMatch.DeclaringType == curType) {
                    // only push if there's no jump in the inheritance chain
                    memberChain.Push(foundMatch);
                }
                curType = curType.BaseType;
            }

            // second, push the current controller type, then its base type, and so forth
            curType = ControllerContext.Controller.GetType();
            while (curType != null) {
                memberChain.Push(curType);
                curType = curType.BaseType;
            }

            // now build the actual filter list up from the beginning.  add the current controller
            // if it implements IActionFilter, then process the memberInfo stack.
            List<IActionFilter> filterList = new List<IActionFilter>();
            IActionFilter controllerFilter = ControllerContext.Controller as IActionFilter;
            if (controllerFilter != null) {
                filterList.Add(controllerFilter);
            }
            foreach (MemberInfo memberInfo in memberChain) {
                filterList.AddRange(GetActionFiltersForMember(memberInfo));
            }

            return filterList;
        }

        protected virtual object GetParameterValue(ParameterInfo parameterInfo, IDictionary<string, object> values) {
            if (parameterInfo == null) {
                throw new ArgumentNullException("parameterInfo");
            }

            Type parameterType = parameterInfo.ParameterType;
            string parameterName = parameterInfo.Name;
            string actionName = parameterInfo.Member.Name;

            bool valueRequired = !TypeAllowsNullValue(parameterType);

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
                IList<IActionFilter> filters = GetAllActionFilters(methodInfo);

                ActionExecutedContext postContext = InvokeActionMethodWithFilters(methodInfo, parameters, filters);
                InvokeActionResultWithFilters(postContext.Result ?? new EmptyResult(), filters);

                // notify controller of completion
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

            // We need to check that the return value really was an ActionResult, as an implementor may have overridden
            // FindActionMethod() but forgotten to override this method.
            ActionResult actionResult = returnValue as ActionResult;
            if (returnValue != null && actionResult == null) {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.CurrentUICulture, MvcResources.ControllerActionInvoker_MethodReturnedWrongType,
                    methodInfo.Name, controller.GetType(), returnValue.GetType()));
            }
            return actionResult;
        }

        internal static ActionExecutedContext InvokeActionMethodFilter(IActionFilter filter, ActionExecutingContext preContext, Func<ActionExecutedContext> continuation) {
            filter.OnActionExecuting(preContext);
            if (preContext.Cancel) {
                return new ActionExecutedContext(preContext, preContext.ActionMethod, null /* exception */) {
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
                postContext = new ActionExecutedContext(preContext, preContext.ActionMethod, ex);
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
                new ActionExecutedContext(ControllerContext, methodInfo, null /* exception */) {
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

        internal static ResultExecutedContext InvokeActionResultFilter(IActionFilter filter, ResultExecutingContext preContext, Func<ResultExecutedContext> continuation) {
            filter.OnResultExecuting(preContext);
            if (preContext.Cancel) {
                return new ResultExecutedContext(preContext, preContext.Result, null /* exception */);
            }

            bool wasError = false;
            ResultExecutedContext postContext = null;
            try {
                postContext = continuation();
            }
            catch (ThreadAbortException) {
                // This type of exception occurs as a result of Response.Redirect(), but we special-case so that
                // the filters don't see this as an error.
                postContext = new ResultExecutedContext(preContext, preContext.Result, null /* exception */);
                filter.OnResultExecuted(postContext);
                throw;
            }
            catch (Exception ex) {
                wasError = true;
                postContext = new ResultExecutedContext(preContext, preContext.Result, ex);
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

        protected virtual ResultExecutedContext InvokeActionResultWithFilters(ActionResult actionResult, IList<IActionFilter> filters) {
            if (actionResult == null) {
                throw new ArgumentNullException("actionResult");
            }
            if (filters == null) {
                throw new ArgumentNullException("filters");
            }

            ResultExecutingContext preContext = new ResultExecutingContext(ControllerContext, actionResult);
            Func<ResultExecutedContext> continuation = delegate {
                InvokeActionResult(actionResult);
                return new ResultExecutedContext(ControllerContext, preContext.Result, null /* exception */);
            };

            // need to reverse the filter list because the continuations are built up backward
            Func<ResultExecutedContext> thunk = filters.Reverse().Aggregate(continuation,
                (next, filter) => () => InvokeActionResultFilter(filter, preContext, next));
            return thunk();
        }

        private static bool TypeAllowsNullValue(Type type) {
            // Only classes and Nullable<> types allow null values
            return (type.IsClass ||
                (type.IsGenericType &&
                !type.IsGenericTypeDefinition &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

    }
}
