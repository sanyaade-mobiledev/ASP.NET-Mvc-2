namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Microsoft.Web.Preview.Resources;

    public class XPathBridgeTransformer : IBridgeResponseTransformer {
        private Dictionary<string, string> _namespaceMapping;
        private string _selector;
        private Dictionary<string, string> _selectedNodeMap;

        public void Initialize(BridgeTransformData data) {
            if (!data.Dictionaries.TryGetValue("namespaceMapping", out _namespaceMapping)) {
                _namespaceMapping = new Dictionary<string, string>();
            }
            if (!data.Dictionaries.TryGetValue("selectedNodes", out _selectedNodeMap)) {
                throw new ArgumentException(PreviewWeb.XPathBridgeTransformer_RequiresSelectNodes, "data");
            }
            if (!data.Attributes.TryGetValue("selector", out _selector)) {
                throw new ArgumentException(PreviewWeb.XPathBridgeTransformer_RequiresSelectorAttribute, "data");
            }
        }

        private object BuildOutputStructure(XmlDocument xmlDocument) {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());

            foreach (string ns in _namespaceMapping.Keys) {
                nsmgr.AddNamespace(ns, (string)_namespaceMapping[ns]);
            }

            XmlNodeList resultNodes = xmlDocument.DocumentElement.SelectNodes(_selector, nsmgr);
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>(resultNodes.Count);

            foreach (XmlNode node in resultNodes) {
                Dictionary<string, string> item = new Dictionary<string, string>();
                foreach (string key in _selectedNodeMap.Keys) {
                    XmlNode leafNode = node.SelectSingleNode(_selectedNodeMap[key], nsmgr);
                    if (leafNode != null) {
                        item[key] = leafNode.InnerText;
                    }
                }
                data.Add(item);
            }

            return data;
        }
        public object Transform(object results) {
            string xml = results as string;
            if (xml == null) {
                throw new ArgumentException(PreviewWeb.XPathBridgeTransformer_StringOnly, "results");
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return BuildOutputStructure(xmlDocument);
        }
    }
}
