using System.Web;
using System;
namespace NORTHWNDModel {

    /// <summary>
    /// There are no comments for NorthwindClientEntities in the schema.
    /// </summary>
    public partial class NorthwindClientEntities : global::System.Data.Services.Client.DataServiceContext {
        public NorthwindClientEntities() : this(new Uri("http://localhost:59486/Northwind.svc/")) { }

        partial void OnContextCreated() {
            this.SendingRequest += new System.EventHandler<System.Data.Services.Client.SendingRequestEventArgs>(NorthwindClientEntities_SendingRequest);
        }

        void NorthwindClientEntities_SendingRequest(object sender, System.Data.Services.Client.SendingRequestEventArgs e) {
            // Write some logging information to the page to demonstrate the ADO.NET Data Service requests
            // that are being made.
            HttpContext.Current.Response.Write(e.Request.RequestUri + "<br>");
        }
    }
}
