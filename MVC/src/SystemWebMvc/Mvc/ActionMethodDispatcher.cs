namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;

    // The methods in this class don't perform error checking; that is the responsibility of the
    // caller.
    internal class ActionMethodDispatcher {

        private delegate object ActionExecutor(ControllerBase controller, object[] parameters);
        private delegate void VoidActionExecutor(ControllerBase controller, object[] parameters);

        private ActionExecutor _executor;

        public ActionMethodDispatcher(MethodInfo methodInfo) {
            _executor = GetExecutor(methodInfo);
            MethodInfo = methodInfo;
        }

        public MethodInfo MethodInfo {
            get;
            private set;
        }

        public ActionResult Execute(ControllerBase controller, object[] parameters) {
            object result = _executor(controller, parameters);
            ActionResult actionResult = ObjectToActionResult(result);
            return actionResult;
        }

        private static ActionExecutor GetExecutor(MethodInfo methodInfo) {
            // Parameters to executor
            ParameterExpression controllerParameter = Expression.Parameter(typeof(ControllerBase), "controller");
            ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            // Build parameter list
            List<Expression> parameters = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++) {
                ParameterInfo paramInfo = paramInfos[i];
                BinaryExpression valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(valueObj, paramInfo.ParameterType);

                // valueCast is "(Ti) parameters[i]"
                parameters.Add(valueCast);
            }

            // Call method
            UnaryExpression controllerCast = Expression.Convert(controllerParameter, methodInfo.ReflectedType);
            MethodCallExpression methodCall = Expression.Call(controllerCast, methodInfo, parameters);

            // methodCall is "((TController) controller) method((T0) parameters[0], (T1) parameters[1], ...)"
            // Create function
            if (methodCall.Type == typeof(void)) {
                Expression<VoidActionExecutor> lambda = Expression.Lambda<VoidActionExecutor>(methodCall, controllerParameter, parametersParameter);
                VoidActionExecutor voidExecutor = lambda.Compile();
                return WrapVoidAction(voidExecutor);
            }
            else {
                // must coerce methodCall to match ActionExecutor signature
                UnaryExpression castMethodCall = Expression.Convert(methodCall, typeof(object));
                Expression<ActionExecutor> lambda = Expression.Lambda<ActionExecutor>(castMethodCall, controllerParameter, parametersParameter);
                return lambda.Compile();
            }
        }

        public static ActionResult ObjectToActionResult(object result) {
            if (result == null) {
                return new EmptyResult();
            }

            ActionResult actionResult = (result as ActionResult) ??
                new ContentResult { Content = Convert.ToString(result, CultureInfo.InvariantCulture) };
            return actionResult;
        }

        private static ActionExecutor WrapVoidAction(VoidActionExecutor executor) {
            return delegate(ControllerBase controller, object[] parameters) {
                executor(controller, parameters);
                return null;
            };
        }

    }
}
