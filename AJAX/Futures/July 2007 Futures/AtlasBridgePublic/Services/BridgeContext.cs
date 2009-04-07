namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;

    public class BridgeContext {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Consistency.")]
        public BridgeContext(BridgeRequest request, string bridgeUrl) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }
            _request = request;
            _bridgeUrl = bridgeUrl;
        }

        // ClientScript request
        BridgeRequest _request;
        public BridgeRequest BridgeRequest {
            get {
                return _request;
            }
        }

        private string _bridgeUrl;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Consistency.")]
        public string BridgeUrl {
            get {
                return _bridgeUrl;
            }
        }

        Dictionary<string, object> _resultsChain = new Dictionary<string, object>();
        public Dictionary<string, object> ResultsChain {
            get {
                return _resultsChain;
            }
        }

        // Request that will be made to the bridged service
        ServiceRequest _serviceRequest = new ServiceRequest();
        public ServiceRequest ServiceRequest {
            get {
                return _serviceRequest;
            }
        }

        // Final response back to clientscript
        BridgeResponse _response = new BridgeResponse();
        public BridgeResponse BridgeResponse {
            get {
                return _response;
            }
        }

        // Response from the bridged service
        ServiceResponse _serviceResponse = new ServiceResponse();
        public ServiceResponse ServiceResponse {
            get {
                return _serviceResponse;
            }
        }
    }
}
