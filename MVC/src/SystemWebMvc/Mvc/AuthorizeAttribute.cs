namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;

    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "This attribute is AllowMultiple = true and users might want to override behavior.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter {

        private string _roles;
        private string _users;

        public string Roles {
            get {
                return _roles ?? String.Empty;
            }
            set {
                _roles = value;
            }
        }

        public string Users {
            get {
                return _users ?? String.Empty;
            }
            set {
                _users = value;
            }
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext) {
            if (filterContext == null) {
                throw new ArgumentNullException("filterContext");
            }

            IPrincipal user = filterContext.HttpContext.User;
            if (!user.Identity.IsAuthenticated) {
                filterContext.Cancel = true;
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            if (!String.IsNullOrEmpty(Users)) {
                IEnumerable<string> validNames = SplitString(Users);
                bool wasMatch = validNames.Any(name => String.Equals(name, user.Identity.Name, StringComparison.OrdinalIgnoreCase));
                if (!wasMatch) {
                    filterContext.Cancel = true;
                    filterContext.Result = new HttpUnauthorizedResult();
                    return;
                }
            }

            if (!String.IsNullOrEmpty(Roles)) {
                IEnumerable<string> validRoles = SplitString(Roles);
                bool wasMatch = validRoles.Any(role => user.IsInRole(role));
                if (!wasMatch) {
                    filterContext.Cancel = true;
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        private static IEnumerable<string> SplitString(string original) {
            return from piece in original.Split(',')
                   let trimmed = piece.Trim()
                   where !String.IsNullOrEmpty(trimmed)
                   select trimmed;
        }
    }
}
