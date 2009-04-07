namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class ServiceRequest {
        Dictionary<string, object> _args = new Dictionary<string, object>();
        public Dictionary<string, object> Args {
            get {
                return _args;
            }
        }

        private ICredentials _creds;
        public ICredentials Credentials {
            get {
                return _creds;
            }
            set {
                _creds = value;
            }
        }

        private string _method;
        public string Method {
            get {
                return _method;
            }
            set {
                _method = value;
            }
        }
    }
}
