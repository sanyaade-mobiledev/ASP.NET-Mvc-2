namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections;

    public class BridgeRequest {
        private string _method;
        public string Method {
            get {
                return _method;
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException("value");
                }
                _method = value;
            }
        }

        private IDictionary _args;


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Unknown.")]
        public IDictionary Args {
            get {
                return _args;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                _args = value;
            }
        }

        private string _serviceUrl;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Consistency.")]
        public string ServiceUrl {
            get {
                return _serviceUrl;
            }
            set {
                _serviceUrl = value;
            }
        }

        public BridgeRequest(string method, IDictionary args) {
            Method = method;

            // REVIEW: Treat null args as empty dictionary, enables Get bridge requests currently
            if (args == null) args = new Hashtable();
            Args = args;
        }
    }
}
