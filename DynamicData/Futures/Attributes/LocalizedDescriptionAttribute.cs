using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Microsoft.Web.DynamicData {
    public class LocalizedDescriptionAttribute : DescriptionAttribute {
        private ResourceProxy ResourceProxy { get; set; }

        public LocalizedDescriptionAttribute(Type resourceManager, String resource)
            : base("Need to localize") {

            ResourceProxy = new ResourceProxy(resourceManager, resource);
        }

        public override string Description {
            get {
                return ResourceProxy.GetResource();
            }
        }
    }
}
