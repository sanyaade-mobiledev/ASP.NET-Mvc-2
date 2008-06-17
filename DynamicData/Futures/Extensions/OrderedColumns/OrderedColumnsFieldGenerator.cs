using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;

namespace Microsoft.Web.DynamicData.Extensions {
    public class OrderedColumnsFieldGenerator : IAutoFieldGenerator {

        private MetaTable _table;

        public OrderedColumnsFieldGenerator(MetaTable table) {
            _table = table;
        }

        #region IAutoFieldGenerator Members

        public ICollection GenerateFields(Control control) {
            var columns = new List<MetaColumn>();
            foreach (MetaColumn column in _table.Columns) {
                // Skip columns that shouldn't be scaffolded
                if (!column.Scaffold) {
                    continue;
                }

                // Don't display long string in controls that show multiple items
                if (column.IsLongString)
                    continue;

                columns.Add(column);
            }

            var fields = from column in columns
                         orderby column.Attributes.OfType<ColumnOrderAttribute>().DefaultIfEmpty(ColumnOrderAttribute.Default).First()
                         select new DynamicField() { DataField = column.Name };

            return fields.ToList();
        }

        #endregion
    }
}
