using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData.Extensions {
    public class AdvancedFilterRepeater : FilterRepeater {

        protected override IEnumerable<MetaColumn> GetFilteredColumns() {
            // sort the filters by their filter order as specified in FilterAttribute.
            return Table.Columns.Where(c => IsFilterableColumn(c)).OrderBy(column => column, new FilterOrderComparer());
        }

        protected bool IsFilterableColumn(MetaColumn column) {
            if (column.IsCustomProperty) return false;

            var filterAttribute = column.Attributes.OfType<FilterAttribute>().FirstOrDefault();
            if (filterAttribute != null) return filterAttribute.Enabled;

            if (column is MetaForeignKeyColumn) return true;

            if (column.ColumnType == typeof(bool)) return true;

            return false;
        }

        private class FilterOrderComparer : IComparer<MetaColumn> {
            public int Compare(MetaColumn x, MetaColumn y) {
                return GetWeight(x) - GetWeight(y);
            }

            private int GetWeight(MetaColumn column) {
                var filterAttribute = column.Attributes.OfType<FilterAttribute>().FirstOrDefault();
                return filterAttribute != null ? filterAttribute.Order : Int32.MaxValue;
            }
        }

    }
}
