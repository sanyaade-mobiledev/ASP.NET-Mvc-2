namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class SessionStateTempDataProvider : ITempDataProvider {
        internal const string TempDataSessionStateKey = "__ControllerTempData";

        private HttpContextBase _httpContext;

        public HttpContextBase HttpContext {
            get {
                return _httpContext;
            }
        }

        public SessionStateTempDataProvider(HttpContextBase httpContext) {
            if (httpContext == null) {
                throw new ArgumentNullException("httpContext");
            }

            _httpContext = httpContext;
        }

        public virtual void SaveTempData(TempDataDictionary tempDataDictionary) {
            if (_httpContext.Session == null) {
                throw new InvalidOperationException(MvcResources.SessionStateTempDataProvider_SessionStateDisabled);
            }

            _httpContext.Session[TempDataSessionStateKey] = tempDataDictionary;
        }

        public virtual TempDataDictionary LoadTempData() {
            if (_httpContext.Session == null) {
                throw new InvalidOperationException(MvcResources.SessionStateTempDataProvider_SessionStateDisabled);
            }

            TempDataDictionary tempDataDictionary = _httpContext.Session[TempDataSessionStateKey] as TempDataDictionary;

            if (tempDataDictionary != null) {
                // If we got it from Session, remove it so that no other request gets it
                _httpContext.Session.Remove(TempDataSessionStateKey);
                return tempDataDictionary;
            }
            else {
                return new TempDataDictionary();
            }
        }
    }
}
