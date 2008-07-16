namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Routing;
    using System.Web.Mvc.Resources;

    internal class TagBuilder {

        private const string _attributeFormat = @" {0}=""{1}""";
        private const string _elementFormatSelfClosing = "<{0}{1} />";
        private const string _elementFormatNormal = "<{0}{1}>{2}</{0}>";
        private const string _elementFormatStartTag = "<{0}{1}>";
        private const string _elementFormatEndTag = "</{0}>";

        private IDictionary<string, string> _attributes;

        public TagBuilder(string tagName) {
            if (String.IsNullOrEmpty(tagName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "tagName");
            }

            TagName = tagName;
            TagRenderMode = TagRenderMode.Normal;
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

        public TagRenderMode TagRenderMode {
            get;
            set;
        }

        public string TagName {
            get;
            private set;
        }

        public void AddAttributes(Dictionary<string, string> attributes) {
            if (attributes != null) {
                foreach (string key in attributes.Keys) {
                    TryAddValue(key, attributes[key]);
                }
            }
        }

        private string GetAttributeString() {
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
            return attributesString;
        }

        public override string ToString() {
            switch (TagRenderMode) {
                case TagRenderMode.SelfClosing:
                    return String.Format(CultureInfo.InvariantCulture, _elementFormatSelfClosing, TagName, GetAttributeString());
                case TagRenderMode.Normal:
                    return String.Format(CultureInfo.InvariantCulture, _elementFormatNormal, TagName, GetAttributeString(), InnerHtml);
                case TagRenderMode.StartTag:
                    return String.Format(CultureInfo.InvariantCulture, _elementFormatStartTag, TagName, GetAttributeString());
                case TagRenderMode.EndTag:
                    return String.Format(CultureInfo.InvariantCulture, _elementFormatSelfClosing, TagName);
                default:
                    return (InnerHtml != null) ?
                        String.Format(CultureInfo.InvariantCulture, _elementFormatNormal, TagName, GetAttributeString(), InnerHtml) :
                        String.Format(CultureInfo.InvariantCulture, _elementFormatSelfClosing, TagName, GetAttributeString());
            }
        }

        public bool TryAddValue(string key, string value) {
            if (!Attributes.ContainsKey(key)) {
                Attributes[key] = value;
                return true;
            }
            return false;
        }

        internal static IDictionary<string, object> ToDictionary(object values) {
            return new RouteValueDictionary(values);
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
    }

    internal enum TagRenderMode {
        Normal,
        StartTag,
        EndTag,
        SelfClosing
    }
}
