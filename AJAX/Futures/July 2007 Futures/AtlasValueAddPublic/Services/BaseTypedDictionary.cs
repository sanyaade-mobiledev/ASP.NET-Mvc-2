namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Reflection;

    internal abstract class BaseTypedDictionary : IEnumerable<KeyValuePair<string, object>> {
        private Dictionary<string, Type> _targetTypes;

        public BaseTypedDictionary(IEnumerable targetParameterInfos) {
            Debug.Assert(targetParameterInfos != null, "The targetParameterInfos parameter should never be null");
            _targetTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            foreach (object targetParameterInfo in targetParameterInfos) {
                ParameterInfo pinfo = targetParameterInfo as ParameterInfo;
                if (pinfo != null) {
                    _targetTypes.Add(pinfo.Name, pinfo.ParameterType);
                }
                else {
                    DataColumn col = targetParameterInfo as DataColumn;
                    Debug.Assert(col != null, "targetParameterInfos must be an array of ParameterInfo or DataColumn.");
                    _targetTypes.Add(col.ColumnName, col.DataType);
                }
            }
        }

        public abstract object this[string index] {
            get;
        }

        // This method takes the name, which can be a parameter name or a property name,
        // gets the corresponding type and uses type conversion on this type to transform
        // valueText into an object of the right type.
        protected object FromString(string name, string valueText) {
            TypeConverter converter = null;
            Type targetType;
            _targetTypes.TryGetValue(name, out targetType);
            if (targetType == null) {
                if (_targetTypes.Count == 1) {
                    Dictionary<string, Type>.ValueCollection.Enumerator values = _targetTypes.Values.GetEnumerator();
                    values.MoveNext();
                    PropertyInfo targetPInfo = DataService.GetPropertyInfo(values.Current, name);
                    if (targetPInfo == null) {
                        throw new InvalidOperationException();
                    }
                    converter = TypeDescriptor.GetConverter(targetPInfo.PropertyType);
                }
                else {
                    throw new InvalidOperationException();
                }
            }
            else {
                converter = TypeDescriptor.GetConverter(targetType);
            }
            return converter.ConvertFromInvariantString(valueText);
        }

        protected object CoerceObject(string name, object objectOrString) {
            string str = objectOrString as string;
            if (str == null) return objectOrString;
            return FromString(name, str);
        }

        protected abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

        public abstract bool TryGetValue(string key, out object value);

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
