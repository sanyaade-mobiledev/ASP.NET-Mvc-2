namespace Microsoft.Web.Preview.Services {
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class TypedDictionary : BaseTypedDictionary {
        private IDictionary<string, object> _dict;

        public TypedDictionary(IDictionary<string, object> dictionary, IEnumerable targetParameterInfos) :
            base(targetParameterInfos) {

            Debug.Assert((dictionary != null), "The dictionary parameter should never be null");
            _dict = dictionary;
        }

        public override bool TryGetValue(string key, out object value) {
            bool exists = _dict.TryGetValue(key, out value);
            if (!exists) return false;
            value = CoerceObject(key, value);
            return true;
        }

        public override object this[string key] {
            get {
                return CoerceObject(key, _dict[key]);
            }
        }

        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            foreach (KeyValuePair<string, object> item in _dict) {
                string itemValue = item.Value as string;
                if (itemValue != null) {
                    yield return new KeyValuePair<string, object>(item.Key, FromString(item.Key, itemValue));
                }
                else {
                    yield return item;
                }
            }
        }
    }
}
