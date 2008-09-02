namespace System.Web.Mvc {
    using System.Reflection;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class MethodSelectionAttribute : Attribute {
        public virtual MethodSelectionResult OnMethodSelecting(ControllerContext controllerContext, string action, MethodInfo methodInfo) {
            return MethodSelectionResult.Continue;
        }

        public virtual MethodSelectionResult OnMethodSelected(ControllerContext controllerContext, string action, MethodInfo methodInfo) {
            return MethodSelectionResult.Continue;
        }
    }
}
