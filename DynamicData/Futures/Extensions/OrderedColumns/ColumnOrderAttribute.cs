using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Web.DynamicData.Extensions {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ColumnOrderAttribute : Attribute, IComparable {

        public static ColumnOrderAttribute Default = new ColumnOrderAttribute(0);

        public ColumnOrderAttribute(int order) { Order = order; }
        public int Order { get; private set; }


        #region IComparable Members

        public int CompareTo(object obj) {
            return Order - ((ColumnOrderAttribute)obj).Order;
        }

        #endregion
    }
}
