namespace System.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DefaultValueProvider : IValueProvider {

        public ControllerContext ControllerContext {
            get;
            private set;
        }

        public DefaultValueProvider(ControllerContext controllerContext) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }

            ControllerContext = controllerContext;
        }

        public virtual ValueProviderResult GetValue(string name) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }

            // Try to get a value for the parameter. We use this order of precedence:
            // 1. Values from the RouteData (could be from the typed-in URL or from the route's default values)
            // 2. URI query string
            // 3. Request form submission (should be culture-aware)

            object rawValue = null;
            CultureInfo culture = CultureInfo.InvariantCulture;
            string attemptedValue = null;

            if (ControllerContext.RouteData != null && ControllerContext.RouteData.Values.TryGetValue(name, out rawValue)) {
                attemptedValue = Convert.ToString(rawValue, CultureInfo.InvariantCulture);
            }
            else {
                HttpRequestBase request = ControllerContext.HttpContext.Request;
                if (request != null) {
                    if (request.QueryString != null) {
                        rawValue = request.QueryString.GetValues(name);
                        attemptedValue = request.QueryString[name];
                    }
                    if (rawValue == null && request.Form != null) {
                        culture = CultureInfo.CurrentCulture;
                        rawValue = request.Form.GetValues(name);
                        attemptedValue = request.Form[name];
                    }
                }
            }

            return (rawValue != null) ? new ValueProviderResult(rawValue, attemptedValue, culture) : null;
        }

    }
}
