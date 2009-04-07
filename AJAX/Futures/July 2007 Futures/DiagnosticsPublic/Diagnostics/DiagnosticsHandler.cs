namespace Microsoft.Web.Preview.Diagnostics {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    //@@ Scaffolding placeholder to add Diagnostics.axd UI support
    public class DiagnosticsHandler : IHttpHandler {
        #region IHttpHandler Members

        public bool IsReusable {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            context.Response.Write("<html><body><h1>Diagnostics</h1></body></html>");
        }

        #endregion
    }
}
