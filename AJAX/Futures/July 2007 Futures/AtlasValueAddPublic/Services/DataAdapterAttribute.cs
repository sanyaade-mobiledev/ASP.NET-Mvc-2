namespace Microsoft.Web.Preview.Services {
    using System;
    using Microsoft.Web.Preview.Resources;

    [AttributeUsage(AttributeTargets.Class)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments",
      Justification = "The 'getDataMethodName' parameter gets publicly exposed through the 'GetDataMethodNames' property.")]
    public sealed class DataAdapterAttribute : Attribute {
        private readonly Type _dataAdapterType;
        private readonly string[] _getDataMethodNames;
        private readonly string _updateMethodName;

        private DataAdapterAttribute() { }

        private DataAdapterAttribute(Type dataAdapterType) {
            if (dataAdapterType == null) {
                throw new ArgumentNullException("dataAdapterType");
            }
            _dataAdapterType = dataAdapterType;
        }

        public DataAdapterAttribute(Type dataAdapterType, string[] getDataMethodNames)
            : this(dataAdapterType) {

            if (getDataMethodNames == null) {
                throw new ArgumentNullException("getDataMethodNames");
            }
            if (getDataMethodNames.Length == 0) {
                throw new ArgumentException(PreviewWeb.DataAdapterAttribute_AtLeastOneName, "getDataMethodNames");
            }
            _getDataMethodNames = getDataMethodNames;
        }

        public DataAdapterAttribute(Type dataAdapterType, string getDataMethodName)
            : this(dataAdapterType, new string[] { getDataMethodName }) { }

        public DataAdapterAttribute(Type dataAdapterType, string[] getDataMethodNames, string updateMethodName)
            : this(dataAdapterType, getDataMethodNames) {

            _updateMethodName = updateMethodName;
        }

        public DataAdapterAttribute(Type dataAdapterType, string getDataMethodName, string updateMethodName)
            : this(dataAdapterType, new string[] { getDataMethodName }, updateMethodName) { }

        public Type DataAdapterType {
            get {
                return _dataAdapterType;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
            Justification = "If we ship this attribute in the core DLL we need to fix many other aspects " +
            "of the code as well, so for now we can exclude this.")]
        public string[] GetDataMethodNames {
            get {
                return _getDataMethodNames;
            }
        }

        public string UpdateMethodName {
            get {
                return _updateMethodName;
            }
        }

        public override bool Equals(object obj) {
            DataAdapterAttribute other = obj as DataAdapterAttribute;

            if (other != null) {
                if (_getDataMethodNames.Length != other._getDataMethodNames.Length) {
                    return false;
                }
                for (int i = 0; i < _getDataMethodNames.Length; i++) {
                    if (_getDataMethodNames[i] != other._getDataMethodNames[i]) {
                        return false;
                    }
                }

                return (_dataAdapterType == other.DataAdapterType) &&
                    String.Equals(_updateMethodName, other._updateMethodName, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }


        public override int GetHashCode() {
            return (_dataAdapterType.GetHashCode() ^ _getDataMethodNames.GetHashCode() ^ _updateMethodName.GetHashCode());
        }
    }
}
