namespace Microsoft.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Routing;

    // TODO: Make the *real* TagBuilder public and delete this copy.
    internal class TagBuilder : IHtmlElement {

        private const string _attributeFormat = @" {0}=""{1}""";
        private const string _elementFormatClosed = "<{0}{1} />";
        private const string _elementFormatOpen = "<{0}{1}>{2}</{0}>";

        private IDictionary<string, string> _attributes;

        public TagBuilder(string tagName) {
            if (String.IsNullOrEmpty(tagName)) {
                throw new ArgumentException("// TODO: NULL OR EMPTY", "tagName");
            }

            TagName = tagName;
        }

        public IDictionary<string, string> Attributes {
            get {
                if (_attributes == null) {
                    _attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                return _attributes;
            }
            set {
                _attributes = value;
            }
        }

        public string InnerHtml {
            get;
            set;
        }

        public string TagName {
            get;
            private set;
        }

        public override string ToString() {
            string attributesString = String.Empty;
            if (Attributes != null) {
                StringBuilder sb = new StringBuilder();
                foreach (var attribute in Attributes) {
                    string key = attribute.Key;
                    string value = HttpUtility.HtmlAttributeEncode(attribute.Value);
                    sb.AppendFormat(CultureInfo.InvariantCulture, _attributeFormat, key, value);
                }
                attributesString = sb.ToString();
            }

            return (InnerHtml != null) ?
                String.Format(CultureInfo.InvariantCulture, _elementFormatOpen, TagName, attributesString, InnerHtml) :
                String.Format(CultureInfo.InvariantCulture, _elementFormatClosed, TagName, attributesString);
        }

        internal static Dictionary<string, string> ToStringDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary) {
            if (dictionary == null) {
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            return dictionary.ToDictionary(
                entry => Convert.ToString(entry.Key, CultureInfo.InvariantCulture),
                entry => Convert.ToString(entry.Value, CultureInfo.InvariantCulture),
                StringComparer.OrdinalIgnoreCase);
        }

        internal static bool TryAddValue<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key, TValue value) {
            if (dict.ContainsKey(key)) {
                return false;
            }
            else {
                dict[key] = value;
                return true;
            }
        }

        internal static IDictionary<string, object> ToDictionary(object values) {
            return new RouteValueDictionary(values);
        }
    }
}