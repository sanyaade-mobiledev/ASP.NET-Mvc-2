namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Compilation;
    using System.Web.Script.Services;
    using System.Web.UI;
    using System.Xml;
    using Microsoft.Web.Preview.Resources;

    internal class BridgeService {
        private static Dictionary<string, BridgeService> s_cache = new Dictionary<string, BridgeService>();
        public static BridgeService GetService(string virtualPath) {
            BridgeService service = null;
            if (s_cache.TryGetValue(virtualPath, out service)) {
                return service;
            }
            return null;
        }
        public static void UpdateCache(string virtualPath, BridgeService service) {
            lock (s_cache) {
                s_cache[virtualPath] = service;
            }
        }

        private string _namespace;
        public String Namespace {
            get {
                return _namespace;
            }
            set {
                _namespace = value;
            }
        }

        private string _className;
        public String Classname {
            get {
                return _className;
            }
            set {
                _className = value;
            }
        }

        private string _language;
        public String Language {
            get {
                return _language;
            }
            set {
                _language = value;
            }
        }

        private string _partialClassFile;
        public String PartialClassFile {
            get {
                return _partialClassFile;
            }
            set {
                _partialClassFile = value;
            }
        }

        private ServiceInfo _serviceInfo;
        public ServiceInfo ServiceInfo {
            get {
                return _serviceInfo;
            }
            set {
                _serviceInfo = value;
            }
        }

        public BridgeService(string xml) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Initialize(doc);
        }

        private void Initialize(XmlDocument document) {
            XmlNode rootNode = document.SelectSingleNode("/bridge");
            if (rootNode == null)
                throw new InvalidOperationException("No <bridge> node found");

            Namespace = GetAttributeValue(rootNode, "namespace", true);
            Classname = GetAttributeValue(rootNode, "className", true);
            PartialClassFile = GetAttributeValue(rootNode, "partialClassFile", false);
            Language = GetAttributeValue(rootNode, "language", false);
            if (String.IsNullOrEmpty(Language)) {
                Language = "C#";
            }

            ServiceInfo = BuildServiceInfo(rootNode);
        }

        private static string GetAttributeValue(XmlNode node, string attr, bool required) {
            XmlAttribute attrib = node.Attributes[attr];
            if (attrib == null) {
                if (required) {
                    throw new InvalidOperationException("Attribute: " + attr + " is required.");
                }
                else {
                    return null;
                }
            }
            return attrib.Value;
        }

        private static Dictionary<string, BridgeParameterInfo> BuildParams(XmlNode methodNode) {
            XmlNodeList paramNodes = methodNode.SelectNodes("parameter");
            Dictionary<string, BridgeParameterInfo> parameters = new Dictionary<string, BridgeParameterInfo>(paramNodes.Count);
            foreach (XmlNode paramNode in paramNodes) {
                BridgeParameterInfo param = new BridgeParameterInfo();
                param.Name = GetAttributeValue(paramNode, "name", true);
                param.Value = GetAttributeValue(paramNode, "value", false);
                param.ServerName = GetAttributeValue(paramNode, "serverName", false);
                string serverOnly = GetAttributeValue(paramNode, "serverOnly", false);
                param.ServerOnly = (serverOnly != null && string.Equals(serverOnly, "true", StringComparison.OrdinalIgnoreCase));
                parameters.Add(param.Name, param);
            }
            return parameters;
        }

        /*
          <data>
            <attribute name="selector" value="bs:Items/bs:Item" />
            <dictionary name="namespaceMapping">
              <item name="bs" value="http://webservices.amazon.com/AWSECommerceService/2005-10-05" />
            </dictionary>
            <dictionary name="selectedNodes">
              <item name="Title" value="bs:ItemAttributes/bs:Title" />
              <item name="Product" value="bs:ItemAttributes/bs:ProductGroup" />
              <item name="Author" value="bs:ItemAttributes/bs:Author" />
              <item name="ASIN" value="bs:ASIN" />
            </dictionary>
          </data>
         */
        private static BridgeTransformData BuildTransformData(XmlNode dataNode) {
            BridgeTransformData data = new BridgeTransformData();
            if (dataNode != null) {
                XmlNodeList attrNodes = dataNode.SelectNodes("attribute");
                foreach (XmlNode node in attrNodes) {
                    string key = GetAttributeValue(node, "name", true);
                    string value = GetAttributeValue(node, "value", true);
                    data.Attributes[key] = value;
                }

                XmlNodeList dictNodes = dataNode.SelectNodes("dictionary");
                foreach (XmlNode dictNode in dictNodes) {
                    string name = GetAttributeValue(dictNode, "name", true);
                    XmlNodeList items = dictNode.SelectNodes("item");
                    Dictionary<string, string> d = new Dictionary<string, string>(items.Count);

                    foreach (XmlNode itemNode in items) {
                        string key = GetAttributeValue(itemNode, "name", true);
                        string value = GetAttributeValue(itemNode, "value", true);
                        d[key] = value;
                    }
                    data.Dictionaries[name] = d;
                }

            }
            return data;
        }

        private static void BuildTransforms(XmlNode transformNode, BridgeMethodInfo method) {
            if (transformNode == null) return;
            XmlNodeList transformNodes = transformNode.SelectNodes("transform");
            foreach (XmlNode node in transformNodes) {
                string typeStr = GetAttributeValue(node, "type", true);
                XmlNode dataNode = GetSingleNode(node, "data", false);
                BridgeTransformData data = BuildTransformData(dataNode);
                method.ResponseTransforms.Add(new Pair(typeStr, data));
            }
        }

        private static void BuildCaches(XmlNode cacheNode, BridgeMethodInfo method) {
            if (cacheNode == null) return;
            XmlNodeList cacheNodes = cacheNode.SelectNodes("cache");
            foreach (XmlNode node in cacheNodes) {
                string typeStr = GetAttributeValue(node, "type", true);
                XmlNode dataNode = GetSingleNode(node, "data", false);
                BridgeTransformData data = BuildTransformData(dataNode);
                method.Caches.Add(new Pair(typeStr, data));
            }
        }
        private static void BuildMethodRequestChain(XmlNode callNode, BridgeMethodInfo method) {
            XmlNodeList requests = callNode.SelectNodes("request");
            foreach (XmlNode node in requests) {
                BridgeChainRequestInfo callInfo = new BridgeChainRequestInfo();
                callInfo.Name = GetAttributeValue(node, "name", true);
                callInfo.BridgeUrl = GetAttributeValue(node, "bridgeUrl", true);
                callInfo.Method = GetAttributeValue(node, "method", true);
                method.BridgeChainRequests.Add(callInfo);

                XmlNode inputNode = GetSingleNode(node, "input", false);
                if (inputNode == null) {
                    callInfo.Parameters = new Dictionary<string, BridgeParameterInfo>(0);
                }
                else {
                    callInfo.Parameters = BuildParams(inputNode);
                }

            }
        }

        private static BridgeCredentialInfo ParseCredentials(XmlNode authNode) {
            BridgeCredentialInfo cred = null;
            XmlNode credNode = authNode.SelectSingleNode("credentials");
            if (credNode != null) {
                cred = new BridgeCredentialInfo();
                cred.User = GetAttributeValue(credNode, "username", true);
                cred.Password = GetAttributeValue(credNode, "password", true);
                cred.Domain = GetAttributeValue(credNode, "domain", true);
            }
            return cred;
        }

        private static XmlNode GetSingleNode(XmlNode node, string name, bool required) {
            XmlNode childNode = node.SelectSingleNode(name);
            if (required && childNode == null) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, PreviewWeb.BridgeService_NodeRequired, name));
            }
            return childNode;
        }
        private static XmlNode GetSingleNode(XmlNode node, string name) {
            return GetSingleNode(node, name, true);
        }

        private static void BuildMethodInfo(ServiceInfo serviceInfo, XmlNode node) {
            XmlNodeList methodNodes = node.SelectNodes("method");
            foreach (XmlNode methodNode in methodNodes) {
                string name = GetAttributeValue(methodNode, "name", true);
                string serverName = GetAttributeValue(methodNode, "serverName", false);
                if (serverName == null) serverName = name;

                string getEnabled = GetAttributeValue(methodNode, "getEnabled", false);
                bool get = (getEnabled != null && String.Equals(getEnabled, "true", StringComparison.OrdinalIgnoreCase));

                ResponseFormat mode = ResponseFormat.Json;
                string responseFormat = GetAttributeValue(methodNode, "responseFormat", false);
                if (responseFormat != null && String.Equals(responseFormat, "xml", StringComparison.OrdinalIgnoreCase)) {
                    mode = ResponseFormat.Xml;
                }

                BridgeMethodInfo methodInfo = new BridgeMethodInfo(name, serverName, get, mode);
                XmlNode inputNode = GetSingleNode(methodNode, "input", false);
                if (inputNode == null) {
                    methodInfo.Parameters = new Dictionary<string, BridgeParameterInfo>(0);
                }
                else {
                    methodInfo.Parameters = BuildParams(inputNode);
                }

                XmlNode requests = GetSingleNode(methodNode, "requestChain", false);
                if (requests != null) {
                    BuildMethodRequestChain(requests, methodInfo);
                }

                // REVIEW: Only used on Soap stuff currently
                XmlNodeList includeNodes = methodNode.SelectNodes("xmlinclude");
                foreach (XmlNode includeNode in includeNodes) {
                    methodInfo.XmlIncludes.Add(GetAttributeValue(includeNode, "type", true));
                }

                XmlNode transformNode = GetSingleNode(methodNode, "transforms", false);
                BuildTransforms(transformNode, methodInfo);

                XmlNode cachingNode = GetSingleNode(methodNode, "caching", false);
                BuildCaches(cachingNode, methodInfo);

                XmlNode authNode = methodNode.SelectSingleNode("authentication");
                if (authNode != null) {
                    methodInfo.Credentials = ParseCredentials(authNode);
                }

                serviceInfo.Methods[name] = methodInfo;
            }
        }

        private static ServiceInfo BuildServiceInfo(XmlNode node) {
            ServiceInfo serviceInfo = new ServiceInfo();
            XmlNode proxyNode = GetSingleNode(node, "proxy");
            serviceInfo.ServiceClass = GetAttributeValue(proxyNode, "type", true);
            serviceInfo.ServiceUrl = GetAttributeValue(proxyNode, "serviceUrl", false);
            BuildMethodInfo(serviceInfo, node);
            return serviceInfo;
        }

        // Cache the types once, but create a new instance each invocation
        private static Dictionary<string, Type> s_typeCache = new Dictionary<string, Type>();
        internal static Type GetType(string type) {
            Type serviceType;
            if (!s_typeCache.TryGetValue(type, out serviceType)) {
                serviceType = BuildManager.GetType(type, true);
                if (serviceType == null) {
                    throw new ArgumentException("cannot find Type: " + type);
                }
                s_typeCache[type] = serviceType;
            }
            return serviceType;
        }
        internal static object CreateInstance(string type, out Type serviceType) {
            serviceType = GetType(type);
            object serviceObject = Activator.CreateInstance(serviceType);
            if (serviceObject == null) {
                throw new ArgumentException("cannot instantiate Type: " + type);
            }
            return serviceObject;
        }
    }

    internal class ServiceInfo {
        private Dictionary<string, BridgeMethodInfo> _methods = new Dictionary<string, BridgeMethodInfo>();
        public Dictionary<string, BridgeMethodInfo> Methods {
            get {
                return _methods;
            }
        }

        private string _url;
        public string ServiceUrl {
            get {
                return _url;
            }
            set {
                _url = value;
            }
        }


        // REVIEW: Rename this proxy?
        private string _serviceClass;
        public string ServiceClass {
            get {
                return _serviceClass;
            }
            set {
                _serviceClass = value;
            }
        }

        public ServiceInfo() {
        }
    }

    internal class BridgeMethodInfo {
        public BridgeMethodInfo(string name, string serverName, bool getEnabled, ResponseFormat responseFormat) {
            Name = name;
            ServerName = serverName;
            GetEnabled = getEnabled;
            ResponseFormat = responseFormat;
        }

        private BridgeCredentialInfo _creds;
        public BridgeCredentialInfo Credentials {
            get {
                return _creds;
            }
            set {
                _creds = value;
            }
        }

        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

        private string _serverName;
        public string ServerName {
            get {
                return _serverName;
            }
            set {
                _serverName = value;
            }
        }

        private bool _getEnabled;
        public bool GetEnabled {
            get {
                return _getEnabled;
            }
            set {
                _getEnabled = value;
            }
        }

        private ResponseFormat _responseFormat;
        public ResponseFormat ResponseFormat {
            get {
                return _responseFormat;
            }
            set {
                _responseFormat = value;
            }
        }

        private Dictionary<string, BridgeParameterInfo> _parameters;
        public Dictionary<string, BridgeParameterInfo> Parameters {
            get {
                return _parameters;
            }
            set {
                _parameters = value;
            }
        }

        private List<string> _xmlIncludes = new List<string>();
        public List<string> XmlIncludes {
            get {
                return _xmlIncludes;
            }
        }

        private List<BridgeChainRequestInfo> _requests = new List<BridgeChainRequestInfo>();
        public List<BridgeChainRequestInfo> BridgeChainRequests {
            get {
                return _requests;
            }
        }

        private List<Pair> _responseTransforms = new List<Pair>();
        public List<Pair> ResponseTransforms {
            get {
                return _responseTransforms;
            }
        }

        private List<Pair> _caches = new List<Pair>();
        public List<Pair> Caches {
            get {
                return _caches;
            }
        }

        private List<IBridgeRequestCache> _cacheInstances;
        public List<IBridgeRequestCache> CacheInstances {
            get {
                if (_cacheInstances == null) {
                    _cacheInstances = new List<IBridgeRequestCache>(Caches.Count);
                    foreach (Pair pair in Caches) {
                        Type type;
                        string typeStr = (string)pair.First;
                        IBridgeRequestCache cache = BridgeService.CreateInstance(typeStr, out type) as IBridgeRequestCache;
                        if (cache == null) {
                            throw new InvalidOperationException(typeStr + " is not of type IBridgeRequestCache");
                        }
                        cache.Initialize((BridgeTransformData)pair.Second);
                        _cacheInstances.Add(cache);
                    }

                }
                return _cacheInstances;
            }
        }

    }

    internal class BridgeCredentialInfo {
        private string _user;
        public string User {
            get {
                return _user;
            }
            set {
                _user = value;
            }
        }

        private string _domain;
        public string Domain {
            get {
                return _domain;
            }
            set {
                _domain = value;
            }
        }

        private string _password;
        public string Password {
            get {
                return _password;
            }
            set {
                _password = value;
            }
        }
    }

    internal class BridgeChainRequestInfo {
        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

        private string _bridgeUrl;
        public string BridgeUrl {
            get {
                return _bridgeUrl;
            }
            set {
                _bridgeUrl = value;
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

        private Dictionary<string, BridgeParameterInfo> _parameters;
        public Dictionary<string, BridgeParameterInfo> Parameters {
            get {
                return _parameters;
            }
            set {
                _parameters = value;
            }
        }
    }

    internal class BridgeParameterInfo {
        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

        private string _serverName;
        public string ServerName {
            get {
                if (_serverName == null)
                    return Name;
                return _serverName;
            }
            set {
                _serverName = value;
            }
        }

        private string _value;
        public string Value {
            get {
                return _value;
            }
            set {
                _value = value;
            }
        }

        private bool _serverOnly;
        public bool ServerOnly {
            get {
                return _serverOnly;
            }
            set {
                _serverOnly = value;
            }
        }
    }

    // shared with IBridgeRequestCache
    public class BridgeTransformData {
        private Dictionary<string, Dictionary<string, string>> _dictionaries = new Dictionary<string, Dictionary<string, string>>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Fixing this would require new types and new OM.")]
        public Dictionary<string, Dictionary<string, string>> Dictionaries {
            get {
                return _dictionaries;
            }
        }

        private Dictionary<string, string> _attributes = new Dictionary<string, string>();
        public Dictionary<string, string> Attributes {
            get {
                return _attributes;
            }
        }
    }
}
