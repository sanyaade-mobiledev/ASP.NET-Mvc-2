namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web.Mvc.Resources;

    internal class ActionMethodSelector {

        public ActionMethodSelector(Type controllerType) {
            ControllerType = controllerType;
            PopulateLookupTables();
        }

        public Type ControllerType {
            get;
            private set;
        }

        public MethodInfo[] AliasedMethods {
            get;
            private set;
        }

        public ILookup<string, MethodInfo> NonAliasedMethods {
            get;
            private set;
        }

        private InvalidOperationException CreateAmbiguousMatchException(List<MethodInfo> ambiguousMethods, string action) {
            StringBuilder builder = new StringBuilder();
            foreach (MethodInfo methodInfo in ambiguousMethods) {
                builder.AppendLine();
                builder.Append(Convert.ToString(methodInfo, CultureInfo.CurrentUICulture));
            }
            string message = String.Format(CultureInfo.CurrentUICulture, MvcResources.ActionMethodSelector_AmbigiousMatch,
                action, ControllerType.FullName, builder);
            return new InvalidOperationException(message);
        }

        public MethodInfo FindActionMethod(ControllerContext controllerContext, string action) {
            List<MethodInfo> methodsMatchingName = GetMatchingAliasedMethods(controllerContext, action);
            methodsMatchingName.AddRange(NonAliasedMethods[action]);
            List<MethodInfo> finalMethods = RunSelectionFilters(controllerContext, methodsMatchingName);

            switch (finalMethods.Count) {
                case 0:
                    return null;

                case 1:
                    return finalMethods[0];

                default:
                    throw CreateAmbiguousMatchException(finalMethods, action);
            }
        }

        internal List<MethodInfo> GetMatchingAliasedMethods(ControllerContext controllerContext, string action) {
            // find all aliased methods which are opting in to this request
            // to opt in, all attributes defined on the method must return true

            var methods = from methodInfo in AliasedMethods
                          let attrs = (ActionNameAttribute[])methodInfo.GetCustomAttributes(typeof(ActionNameAttribute), true /* inherit */)
                          where attrs.All(attr => attr.IsValidForRequest(controllerContext, action, methodInfo))
                          select methodInfo;
            return methods.ToList();
        }

        private static bool IsMethodDecoratedWithAliasingAttribute(MethodInfo methodInfo) {
            return methodInfo.IsDefined(typeof(ActionNameAttribute), true /* inherit */);
        }

        private static bool IsValidActionMethod(MethodInfo methodInfo) {
            return !(methodInfo.IsSpecialName ||
                     methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof(Controller)));
        }

        private void PopulateLookupTables() {
            MethodInfo[] allMethods = ControllerType.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public);
            MethodInfo[] actionMethods = Array.FindAll(allMethods, IsValidActionMethod);

            AliasedMethods = Array.FindAll(actionMethods, IsMethodDecoratedWithAliasingAttribute);
            NonAliasedMethods = actionMethods.Except(AliasedMethods).ToLookup(method => method.Name, StringComparer.OrdinalIgnoreCase);
        }

        private static List<MethodInfo> RunSelectionFilters(ControllerContext controllerContext, List<MethodInfo> methodInfos) {
            // remove all methods which are opting out of this request
            // to opt out, at least one attribute defined on the method must return false

            var methods = from methodInfo in methodInfos
                          let attrs = (ActionSelectionAttribute[])methodInfo.GetCustomAttributes(typeof(ActionSelectionAttribute), true /* inherit */)
                          where (attrs == null || attrs.All(attr => attr.IsValidForRequest(controllerContext, methodInfo)))
                          select methodInfo;
            return methods.ToList();
        }

    }
}
