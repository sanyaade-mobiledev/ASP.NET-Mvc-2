namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [CLSCompliant(false)]
    public sealed class AcceptVerbsAttribute : ActionSelectionAttribute {

        public AcceptVerbsAttribute(params string[] verbs) {
            if (verbs == null || verbs.Length == 0) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "verbs");
            }

            Verbs = new ReadOnlyCollection<string>(verbs);
        }

        public ICollection<string> Verbs {
            get;
            private set;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }

            string incomingVerb = controllerContext.HttpContext.Request.HttpMethod;
            return Verbs.Contains(incomingVerb, StringComparer.OrdinalIgnoreCase);
        }
    }
}
