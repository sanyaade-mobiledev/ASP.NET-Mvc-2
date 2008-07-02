using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData {
    public class AdvancedFilterRepeater : FilterRepeater {

        protected override IEnumerable<MetaColumn> GetFilteredColumns() {
            // sort the filters by their filter order as specified in FilterAttribute.
            return Table.Columns.Where(c => IsFilterableColumn(c)).OrderBy(column => FilterOrdering(column));
        }

        protected bool IsFilterableColumn(MetaColumn column) {
            // don't filter custom properties by default
            if (column.IsCustomProperty) return false;

            // honor FilterAttribute.Enabled value, if present
            var filterAttribute = column.Attributes.OfType<FilterAttribute>().FirstOrDefault();
            if (filterAttribute != null) return filterAttribute.Enabled;

            // always filter FK columns and bools
            if (column is MetaForeignKeyColumn) return true;
            if (column.ColumnType == typeof(bool)) return true;

            return false;
        }

        private FilterAttribute FilterOrdering(MetaColumn column) {
            return column.Attributes.OfType<FilterAttribute>().DefaultIfEmpty(FilterAttribute.Default).First();
        }
    }
}
