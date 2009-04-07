namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    internal sealed class ObjectDictionary : BaseTypedDictionary {
        private object _object;
        private PropertyInfo[] _propertyInfos;

        public ObjectDictionary(object source)
            : base(new ParameterInfo[] { }) {

            Debug.Assert((source != null), "The source parameter should never be null");
            _object = source;
            _propertyInfos = source.GetType().GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
        }

        public override bool TryGetValue(string key, out object value) {
            foreach (PropertyInfo info in _propertyInfos) {
                if (String.Equals(info.Name, key, StringComparison.OrdinalIgnoreCase)) {
                    value = info.GetValue(_object, null);
                    return true;
                }
            }
            value = null;
            return false;
        }

        public override object this[string key] {
            get {
                foreach (PropertyInfo info in _propertyInfos) {
                    if (String.Equals(info.Name, key, StringComparison.OrdinalIgnoreCase)) {
                        return info.GetValue(_object, null);
                    }
                }
                throw new ArgumentOutOfRangeException("key");
            }
        }

        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            foreach (PropertyInfo info in _propertyInfos) {
                yield return new KeyValuePair<string, object>(info.Name, info.GetValue(_object, null));
            }
        }
    }
}
