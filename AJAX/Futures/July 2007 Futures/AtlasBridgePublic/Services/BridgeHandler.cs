namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Reflection;
    using System.Web;
    using System.Web.Compilation;
    using System.Web.Configuration;
    using System.Web.Script.Serialization;
    using System.Web.UI;
    using Microsoft.Web.Preview.Resources;

    public class BridgeHandler {
        private BridgeContext _context;
        protected BridgeContext Context {
            get {
                if (_context == null) {
                    throw new InvalidOperationException("Accessing context too early, it is null");
                }
                return _context;
            }
        }

        protected BridgeRequest BridgeRequest {
            get {
                return Context.BridgeRequest;
            }
        }

        protected BridgeResponse BridgeResponse {
            get {
                return Context.BridgeResponse;
            }
        }

        protected ServiceRequest ServiceRequest {
            get {
                return Context.ServiceRequest;
            }
        }

        protected ServiceResponse ServiceResponse {
            get {
                return Context.ServiceResponse;
            }
        }

        private string _bridgeXml;
        protected string BridgeXml {
            get {
                return _bridgeXml;
            }
            set {
                _bridgeXml = value;
            }
        }

        private BridgeService _bridgeService;
        internal BridgeService BridgeService {
            get {
                // Only build the service once this way.
                if (_bridgeService == null) {
                    // Try the cache first
                    _bridgeService = BridgeService.GetService(VirtualPath);
                    if (_bridgeService == null) {
                        _bridgeService = new BridgeService(BridgeXml);
                        BridgeService.UpdateCache(VirtualPath, _bridgeService);
                    }
                }
                return _bridgeService;
            }
        }

        private string _virtualPath;
        protected string VirtualPath {
            get {
                return _virtualPath;
            }
            set {
                _virtualPath = value;
            }
        }

        protected void BuildBridgeContext(string bridgePath, BridgeRequest request) {
            _context = new BridgeContext(request, bridgePath);
        }

        private BridgeMethodInfo GetMethodInfoForCall(string method) {
            BridgeMethodInfo methodInfo = null;
            if (!BridgeService.ServiceInfo.Methods.TryGetValue(method, out methodInfo)) {
                throw new ArgumentException("No such method registered: " + method);
            }
            return methodInfo;
        }

        // Shortest possible string that can contain an expression
        private const int MinimumExpressionLength = 10;
        private static char[] s_sep = { ':' };
        internal static object TryParseExpression(string param, BridgeContext context) {
            string exp = param;
            if (param.Length > MinimumExpressionLength && param.StartsWith("%", StringComparison.Ordinal) && param.EndsWith("%", StringComparison.Ordinal)) {
                exp = param.Substring(1, param.Length - 2).Trim();
            }
            else {
                return param;
            }

            string[] pieces = exp.Split(s_sep);
            if (pieces.Length > 1) {
                switch (pieces[0].Trim()) {
                    case "args":
                        string key = pieces[1].Trim();
                        if (context != null && context.BridgeRequest.Args.Contains(key)) {
                            return context.BridgeRequest.Args[key];
                        }
                        else {
                            throw new ArgumentException("Failed to find object in Args: " + key + " for expression: " + param);
                        }
                    case "appsettings":
                        return WebConfigurationManager.AppSettings[pieces[1].Trim()];
                    case "resultschain":
                        if (context != null && pieces.Length == 3) {
                            ICollection results = context.ResultsChain[pieces[1].Trim()] as ICollection;
                            if (results == null || results.Count == 0)
                                throw new ArgumentException("Results expression only works for a non empty Collection of IDictionary: " + param);

                            IEnumerator iterator = results.GetEnumerator();
                            iterator.MoveNext();
                            IDictionary dict = iterator.Current as IDictionary;
                            if (dict == null)
                                throw new ArgumentException("Results expression only works for a List of IDictionary: " + param);
                            string propertyKey = pieces[2].Trim();
                            string result = dict[propertyKey] as string;
                            if (result != null) {
                                return result;
                            }
                            else {
                                throw new ArgumentException("Failed to parse expression: Could not find: " + propertyKey + " in results for: " + param);
                            }
                        }
                        break;
                    case "querystring":
                        return HttpContext.Current.Request.QueryString[pieces[1].Trim()];
                    default:
                        throw new ArgumentException("Unknown expression type: " + param);
                }
            }
            throw new ArgumentException("Failed to parse expression, make sure its well formed: " + param);
        }

        // Helper function that resolve the config parameter values, client args, and expressions
        private static void ResolveParameters(Dictionary<string, BridgeParameterInfo> parameterInfo, Dictionary<string, object> args, BridgeContext context) {
            foreach (BridgeParameterInfo paramInfo in parameterInfo.Values) {
                object paramValue = null;

                // Fill in server side values as default always
                if (paramInfo.Value != null) {
                    paramValue = paramInfo.Value;
                }

                // REVIEW: what happens if doesn't exist and no default, should probably throw here?  or just use null (using null for now)
                if (!paramInfo.ServerOnly) {
                    if (context.BridgeRequest.Args.Contains(paramInfo.Name)) {
                        paramValue = context.BridgeRequest.Args[paramInfo.Name];
                    }
                }

                // Handle expressions of form value= "% appsettings | querystring : key %"
                string paramValueString = paramValue as string;
                if (paramValueString != null) {
                    paramValue = TryParseExpression(paramValueString, context);
                }

                args[paramInfo.ServerName] = paramValue;
            }
        }

        // This is the signature which we code gen the actual calls to the method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "3#", Justification = "Consistency.")]
        public virtual object CallServiceClassMethod(string method, Dictionary<string, object> args, ICredentials credentials, string url) {
            return null;
        }


        // Used by the generated proxy methods to convert parameter types to the right type
        protected static object ConvertToType(object argValue, Type parameterType) {
            string name = Assembly.CreateQualifiedName(typeof(JavaScriptSerializer).Assembly.FullName, "System.Web.Script.Serialization.ObjectConverter");
            Type conv = Type.GetType(name);
            MethodInfo method = conv.GetMethod("ConvertObjectToType", BindingFlags.NonPublic | BindingFlags.Static);
            return method.Invoke(null, new object[] { argValue, parameterType, new JavaScriptSerializer() });
        }

        public virtual void TransformRequest() {
            BridgeMethodInfo methodInfo = GetMethodInfoForCall(BridgeRequest.Method);

            // Set the server method name
            ServiceRequest.Method = methodInfo.ServerName;

            // Apply the parameter mapping/expressions
            ResolveParameters(methodInfo.Parameters, ServiceRequest.Args, Context);

            // Apply the credentials now only if the credentials aren't set already
            BridgeCredentialInfo credInfo = methodInfo.Credentials;
            if (ServiceRequest.Credentials == null && credInfo != null) {
                ServiceRequest.Credentials = new NetworkCredential((string)TryParseExpression(credInfo.User, Context), (string)TryParseExpression(credInfo.Password, Context), (string)TryParseExpression(credInfo.Domain, Context));
            }
        }

        // Hook to fullfill the request from the cache, return value of true stops execution of the bridge pipeline and returns what's in BridgeResponse.Response
        public virtual bool ResolveRequestCache() {
            BridgeMethodInfo methodInfo = GetMethodInfoForCall(BridgeRequest.Method);
            // First cache that has a hit we use
            foreach (IBridgeRequestCache cache in methodInfo.CacheInstances) {
                object rawResponse = cache.Lookup(Context);
                if (rawResponse != null) {
                    BridgeResponse.Response = rawResponse;
                    return true;
                }
            }
            return false;
        }
        public virtual void ProcessRequest() {
            // Resolve the url before we make the call
            // If the ServiceUrl is specified on the BridgeRequest, use that instead
            string url = BridgeRequest.ServiceUrl;
            if (string.IsNullOrEmpty(url)) {
                url = BridgeService.ServiceInfo.ServiceUrl;
            }

            // Now handle expressions
            if (!string.IsNullOrEmpty(url)) {
                url = BridgeHandler.TryParseExpression(url, Context) as string;
                if (url == null) {
                    throw new ArgumentException("The expression for url must evaluate to a string object.");
                }
            }

            // For now we special case the REST proxy
            if (string.Equals(BridgeService.ServiceInfo.ServiceClass, "Microsoft.Web.Preview.Services.BridgeRestProxy")) {
                ServiceResponse.Response = BridgeRestProxy.MakeRestCall(url, ServiceRequest.Args, ServiceRequest.Credentials);
            }
            else {
                // This method gets code gen'ed to call the actual proxy method (see BridgeBuildProvider.cs)
                ServiceResponse.Response = CallServiceClassMethod(ServiceRequest.Method, ServiceRequest.Args, ServiceRequest.Credentials, url);
            }
        }
        public virtual void TransformResponse() {
            object rawResponse = ServiceResponse.Response;
            BridgeMethodInfo methodInfo = GetMethodInfoForCall(BridgeRequest.Method);
            foreach (Pair pair in methodInfo.ResponseTransforms) {
                Type type;
                string typeStr = (string)pair.First;
                IBridgeResponseTransformer transformer = BridgeService.CreateInstance(typeStr, out type) as IBridgeResponseTransformer;
                if (transformer == null) {
                    throw new InvalidOperationException(typeStr + " is not of type IBridgeResponseTransformer");
                }
                transformer.Initialize((BridgeTransformData)pair.Second);
                rawResponse = transformer.Transform(rawResponse);
            }
            BridgeResponse.Response = rawResponse;
        }

        // This is where the transformed response is placed into the cache
        public virtual void UpdateResponseCache() {
            BridgeMethodInfo methodInfo = GetMethodInfoForCall(BridgeRequest.Method);
            // First cache that has a hit we use
            foreach (IBridgeRequestCache cache in methodInfo.CacheInstances) {
                cache.Put(Context);
            }
        }

        // Entrypoint to the pipeline
        protected virtual void ExecuteBridgePipeline() {
            // Make the calls to all other bridge methods first
            BridgeMethodInfo methodInfo = GetMethodInfoForCall(BridgeRequest.Method);
            foreach (BridgeChainRequestInfo callInfo in methodInfo.BridgeChainRequests) {
                Dictionary<string, object> args = new Dictionary<string, object>();
                ResolveParameters(callInfo.Parameters, args, Context);
                BridgeRequest request = new BridgeRequest(callInfo.Method, args);
                object results = BridgeHandler.Invoke(callInfo.BridgeUrl, request);
                Context.ResultsChain[callInfo.Name] = results;
            }

            TransformRequest();

            // Stop execution if we have a cache hit
            if (ResolveRequestCache()) return;

            ProcessRequest();
            TransformResponse();
            UpdateResponseCache();
        }

        // Public invocation of the entry point
        public object Invoke(BridgeRequest request) {
            BuildBridgeContext(VirtualPath, request);
            ExecuteBridgePipeline();
            return BridgeResponse.Response;
        }

        // For bridge to bridge scenario, or invoking bridge statically
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Consistency.")]
        public static object Invoke(string bridgeUrl, BridgeRequest request) {
            bridgeUrl = VirtualPathUtility.ToAbsolute(bridgeUrl);

            Type type = BuildManager.GetCompiledType(bridgeUrl);
            if (type == null || !typeof(BridgeHandler).IsAssignableFrom(type)) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, PreviewWeb.BridgeHandler_CannotLoad, bridgeUrl));
            }
            BridgeHandler bridge = Activator.CreateInstance(type) as BridgeHandler;
            if (bridge == null) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, PreviewWeb.BridgeHandler_CannotCreate, bridgeUrl));
            }
            return bridge.Invoke(request);
        }

    }
}
