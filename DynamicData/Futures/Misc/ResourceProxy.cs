using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Microsoft.Web.DynamicData {
    internal class ResourceProxy {

        private PropertyInfo prop { get; set; }

        public ResourceProxy(Type resourceManager, string resource) {
            prop = resourceManager.GetProperty(resource, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (prop == null) {
                throw new InvalidOperationException(String.Format("Could not find resource '{0}' on type '{1}'", resource, resourceManager.FullName));
            }
        }

        public string GetResource() {
            return (string)prop.GetValue(null, null);
        }
    }
}
