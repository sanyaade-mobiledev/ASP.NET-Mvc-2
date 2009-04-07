namespace Microsoft.Web.Preview.Search {
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Xml;

    public class SearchSiteMapProviderBase : ProviderBase {
        public SearchSiteMapProviderBase()
            : base() {
        }

        public virtual IEnumerable DataQuery() {
            return null;
        }

        public virtual void WriteNodes(SearchSiteMapHandler handler, XmlTextWriter writer) {
        }

        [SuppressMessage("Microsoft.Design", "CA1055",
            Justification = "Consistent with other URL methods in ASP.NET.")]
        public static String GenerateUrl(String suffix) {
            String url;

            NameValueCollection vars = HttpContext.Current.Request.ServerVariables;
            string port = vars["SERVER_PORT"];
            port = (port == "80") ? String.Empty : ':' + port;
            string protocol = (vars["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";

            url = protocol + vars["SERVER_NAME"] + port + suffix;

            return url;
        }
    }
}