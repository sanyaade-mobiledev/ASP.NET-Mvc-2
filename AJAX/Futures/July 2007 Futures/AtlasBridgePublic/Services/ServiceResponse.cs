namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ServiceResponse {
        private object _response;
        public Object Response {
            get {
                return _response;
            }
            set {
                _response = value;
            }
        }
    }
}
