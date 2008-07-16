namespace System.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AjaxOptions {

        private InsertionMode _insertion = InsertionMode.Replace;
        private string _onBegin;
        private string _onFailure;
        private string _onSuccess;
        private string _updateTargetId;

        public InsertionMode InsertionMode {
            get {
                return _insertion;
            }
            set {
                switch (value) {
                    case InsertionMode.Replace:
                    case InsertionMode.InsertAfter:
                    case InsertionMode.InsertBefore:
                        _insertion = value;
                        return;

                    default:
                        throw new ArgumentOutOfRangeException("value", String.Format(CultureInfo.InvariantCulture,
                            MvcResources.Common_InvalidEnumValue, value, typeof(InsertionMode).FullName));
                }
            }
        }

        public string OnBegin {
            get {
                return _onBegin ?? String.Empty;
            }
            set {
                _onBegin = value;
            }
        }

        public string OnFailure {
            get {
                return _onFailure ?? String.Empty;
            }
            set {
                _onFailure = value;
            }
        }

        public string OnSuccess {
            get {
                return _onSuccess ?? String.Empty;
            }
            set {
                _onSuccess = value;
            }
        }

        public string UpdateTargetId {
            get {
                return _updateTargetId ?? String.Empty;
            }
            set {
                _updateTargetId = value;
            }
        }

        internal string ToJavascriptString() {
            // creates a string of the form { key1: value1, key2 : value2, ... }
            StringBuilder builder = new StringBuilder("{");
            builder.AppendFormat(CultureInfo.InvariantCulture, " insertionMode: {0},", (int)InsertionMode);
            builder.AppendFormat(CultureInfo.InvariantCulture, " updateTargetId: '{0}',", UpdateTargetId);
            if (!String.IsNullOrEmpty(OnBegin)) {
                builder.AppendFormat(CultureInfo.InvariantCulture, " onBegin: function(request) {{ {0} }},", OnBegin);
            }
            if (!String.IsNullOrEmpty(OnFailure)) {
                builder.AppendFormat(CultureInfo.InvariantCulture, " onFailure: function(request) {{ {0} }},", OnFailure);
            }
            if (!String.IsNullOrEmpty(OnSuccess)) {
                builder.AppendFormat(CultureInfo.InvariantCulture, " onSuccess: function(request) {{ {0} }},", OnSuccess);
            }

            builder.Length--;
            builder.Append(" }");
            return builder.ToString();
        }
    }

    public enum InsertionMode {
        Replace = 0,
        InsertBefore = 1,
        InsertAfter = 2
    }
}
