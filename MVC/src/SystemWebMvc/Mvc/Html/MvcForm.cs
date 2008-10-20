namespace System.Web.Mvc.Html {
    using System;
    using System.Diagnostics.CodeAnalysis;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class MvcForm : IDisposable {
        private bool _disposed;
        private readonly HttpResponseBase _httpResponse;

        public MvcForm(HttpResponseBase httpResponse) {
            if (httpResponse == null) {
                throw new ArgumentNullException("httpResponse");
            }
            _httpResponse = httpResponse;
        }

        [SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
        public void Dispose() {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                _disposed = true;
                _httpResponse.Write("</form>");
            }
        }

        public void EndForm() {
            Dispose(true);
        }
    }
}
