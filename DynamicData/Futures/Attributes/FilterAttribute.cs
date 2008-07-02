using System;

namespace Microsoft.Web.DynamicData {
    /// <summary>
    /// An attribute used to specify the filtering behavior for a column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false)]
    public sealed class FilterAttribute : Attribute, IComparable<FilterAttribute> {

        internal static FilterAttribute Default = new FilterAttribute();

        public FilterAttribute() {
            Enabled = true;
            Order = 0;
        }

        /// <summary>
        /// Value indicating which filter control to use.
        /// </summary>
        public string FilterControl { get; set; }
        
        /// <summary>
        /// The ordering of a filter. Negative values are allowed.
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Enable or disable filtering on this column.
        /// </summary>
        public bool Enabled { get; set; }

        #region IComparable<FilterAttribute> Members

        public int CompareTo(FilterAttribute other) {
            return Order - ((FilterAttribute)other).Order;
        }

        #endregion
    }
}
