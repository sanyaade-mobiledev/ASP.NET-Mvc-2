namespace System.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ValueProviderResult {

        private static readonly CultureInfo _staticCulture = CultureInfo.InvariantCulture;

        private CultureInfo _instanceCulture;

        // default constructor so that subclassed types can set the properties themselves
        protected ValueProviderResult() {
        }

        public ValueProviderResult(object rawValue, string attemptedValue, CultureInfo culture) {
            RawValue = rawValue;
            AttemptedValue = attemptedValue;
            Culture = culture;
        }

        public string AttemptedValue {
            get;
            protected set;
        }

        public CultureInfo Culture {
            get {
                if (_instanceCulture == null) {
                    _instanceCulture = _staticCulture;
                }
                return _instanceCulture;
            }
            protected set {
                _instanceCulture = value;
            }
        }

        public object RawValue {
            get;
            protected set;
        }

    }
}
