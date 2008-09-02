namespace System.Web.Mvc {
    using System.Reflection;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class NonActionAttribute : ActionSelectionAttribute {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo) {
            return false;
        }
    }
}
