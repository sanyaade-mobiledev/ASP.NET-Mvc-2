using System.ComponentModel;
using System;

namespace Microsoft.Web.DynamicData {
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute {
        private ResourceProxy ResourceProxy { get; set; }

        public LocalizedDisplayNameAttribute(Type resourceManager, String resource)
            : base("Need to localize") {
            ResourceProxy = new ResourceProxy(resourceManager, resource);
        }

        public override string DisplayName {
            get {
                return ResourceProxy.GetResource();
            }
        }
    }
}
