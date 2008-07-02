using System;

namespace Microsoft.Web.DynamicData {
    /// <summary>
    /// Allows to specify the ordering of columns. Columns are will be sorted in increasing order based 
    /// on the Order value. Columns without this attribute have a default Order of 0. Negative values are
    /// allowed and can be used to place a column before all other columns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ColumnOrderAttribute : Attribute, IComparable {

        public static ColumnOrderAttribute Default = new ColumnOrderAttribute(0);

        public ColumnOrderAttribute(int order) {
            Order = order;
        }

        /// <summary>
        /// The ordering of a column. Can be negative.
        /// </summary>
        public int Order { get; private set; }

        #region IComparable Members

        public int CompareTo(object obj) {
            return Order - ((ColumnOrderAttribute)obj).Order;
        }

        #endregion
    }
}
