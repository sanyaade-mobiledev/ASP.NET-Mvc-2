namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using Microsoft.Web.Preview.Resources;

    public static class BridgeRestProxy {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Consistency.")]
        public static string MakeRestCall(string url, IDictionary argsDictionary, ICredentials credentials) {
            if (String.IsNullOrEmpty(url)) {
                throw new ArgumentException(PreviewWeb.BridgeRestProxy_EmptyUrl, "url");
            }

            string requestUrl = BuildRestRequest(url, argsDictionary);
            HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(new Uri(requestUrl));
            webRequest.Proxy = WebRequest.GetSystemWebProxy();
            if (credentials != null)
                webRequest.Credentials = credentials;
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            Stream stream = response.GetResponseStream();
            using (TextReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

        private static string BuildRestRequest(string url, IDictionary args) {
            StringBuilder request = new StringBuilder(url);
            if (args != null && args.Count > 0) {
                bool first = true;
                foreach (string key in args.Keys) {
                    if (first) {
                        request.Append('?');
                        first = false;
                    }
                    else {
                        request.Append('&');
                    }
                    // REVIEW: Do we need to url encode these guys?
                    request.Append(key);
                    request.Append('=');
                    request.Append(args[key]);
                }
            }
            return request.ToString();
        }
    }
}
