namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc.Resources;

    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "Unsealed since there is no abstract base class for developers to subclass to get similar behavior.")]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionNameAttribute : Attribute {

        public ActionNameAttribute(string name) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }

            Name = name;
        }

        public string Name {
            get;
            private set;
        }

        public virtual bool IsValidForRequest(ControllerContext controllerContext, string action, MethodInfo methodInfo) {
            return String.Equals(action, Name, StringComparison.OrdinalIgnoreCase);
        }

    }
}
