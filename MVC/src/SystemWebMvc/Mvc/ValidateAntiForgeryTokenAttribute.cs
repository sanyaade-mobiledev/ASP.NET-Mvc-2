namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter {

        private const string TokenName = HtmlHelper.AntiForgeryTokenFieldName;

        private string _salt;
        private AntiForgeryTokenSerializer _serializer;

        public string Salt {
            get {
                return _salt ?? String.Empty;
            }
            set {
                _salt = value;
            }
        }

        internal AntiForgeryTokenSerializer Serializer {
            get {
                if (_serializer == null) {
                    _serializer = new AntiForgeryTokenSerializer();
                }
                return _serializer;
            }
            set {
                _serializer = value;
            }
        }

        private bool ValidateFormToken(AntiForgeryToken token) {
            return (String.Equals(Salt, token.Salt, StringComparison.Ordinal));
        }

        private static HttpAntiForgeryException CreateValidationException() {
            return new HttpAntiForgeryException(MvcResources.AntiForgeryToken_ValidationFailed);
        }

        public void OnAuthorization(AuthorizationContext filterContext) {
            if (filterContext == null) {
                throw new ArgumentNullException("filterContext");
            }

            HttpCookie cookie = filterContext.HttpContext.Request.Cookies[TokenName];
            if (cookie == null || String.IsNullOrEmpty(cookie.Value)) {
                // error: cookie token is missing
                throw CreateValidationException();
            }
            AntiForgeryToken cookieToken = Serializer.Deserialize(cookie.Value);

            string formValue = filterContext.HttpContext.Request.Form[TokenName];
            if (String.IsNullOrEmpty(formValue)) {
                // error: form token is missing
                throw CreateValidationException();
            }
            AntiForgeryToken formToken = Serializer.Deserialize(formValue);

            if (!String.Equals(cookieToken.Value, formToken.Value, StringComparison.Ordinal)) {
                // error: form token does not match cookie token
                throw CreateValidationException();
            }

            if (!ValidateFormToken(formToken)) {
                // error: custom validation failed
                throw CreateValidationException();
            }
        }

    }
}
