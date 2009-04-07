namespace Microsoft.Web.Preview.Services {
    using System;

    public class BridgeResponse {
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
