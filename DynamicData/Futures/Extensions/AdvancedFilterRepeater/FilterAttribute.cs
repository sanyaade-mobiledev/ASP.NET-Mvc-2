using System;

namespace Microsoft.Web.DynamicData.Extensions {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false)]
    public sealed class FilterAttribute : Attribute {

        public FilterAttribute() {
            Order = Int32.MaxValue;
            Enabled = true;
        }

        public string FilterControl { get; set; }
        
        // Lower values take precedence before greater values
        public int Order { get; set; }
        
        public bool Enabled { get; set; }
    }
}
