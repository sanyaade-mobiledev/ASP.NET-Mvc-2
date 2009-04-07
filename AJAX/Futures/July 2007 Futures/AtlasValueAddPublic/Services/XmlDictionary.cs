namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml;

    internal sealed class XmlDictionary : BaseTypedDictionary {
        private XmlNode[] _nodes;

        public XmlDictionary(XmlNode[] nodes, IEnumerable targetParameterInfos) : base(targetParameterInfos) {
            Debug.Assert((nodes != null), "The nodes parameter should never be null");
            _nodes = nodes;
        }

        public override bool TryGetValue(string key, out object value) {
            foreach (XmlNode node in _nodes) {
                if (String.Equals(node.Name, key, StringComparison.OrdinalIgnoreCase)) {
                    value = FromString(node.Name, node.InnerText);
                    return true;
                }
            }
            value = null;
            return false;
        }

        public override object this[string key] {
            get {
                foreach (XmlNode node in _nodes) {
                    if (String.Equals(node.Name, key, StringComparison.OrdinalIgnoreCase)) {
                        return FromString(node.Name, node.InnerText);
                    }
                }
                throw new ArgumentOutOfRangeException("key");;
            }
        }

        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            foreach (XmlNode node in _nodes) {
                yield return new KeyValuePair<string, object>(node.Name, FromString(node.Name, node.InnerText));
            }
        }
    }
}
