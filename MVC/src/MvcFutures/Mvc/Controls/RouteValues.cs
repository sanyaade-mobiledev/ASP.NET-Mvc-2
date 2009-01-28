﻿namespace Microsoft.Web.Mvc.Controls {
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;

    // TOOD: Should the attributes be in a list instead of a dictionary? The unit tests can break if the order changes

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class RouteValues : IAttributeAccessor {
        private IDictionary<string, string> _attributes;

        public IDictionary<string, string> Attributes {
            get {
                EnsureAttributes();
                return _attributes;
            }
        }

        private void EnsureAttributes() {
            if (_attributes == null) {
                _attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        protected virtual string GetAttribute(string key) {
            EnsureAttributes();
            string value;
            _attributes.TryGetValue(key, out value);
            return value;
        }

        protected virtual void SetAttribute(string key, string value) {
            EnsureAttributes();
            _attributes[key] = value;
        }

        #region IAttributeAccessor Members
        string IAttributeAccessor.GetAttribute(string key) {
            return GetAttribute(key);
        }

        void IAttributeAccessor.SetAttribute(string key, string value) {
            SetAttribute(key, value);
        }
        #endregion
    }
}
