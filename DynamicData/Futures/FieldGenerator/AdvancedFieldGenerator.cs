using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;

namespace Microsoft.Web.DynamicData {
    /// <summary>
    /// Implements the IAutoFieldGenerator interface and supports advanced scenarios such as declarative column ordering,
    /// workaround for attribute localization issues.
    /// </summary>
    public class AdvancedFieldGenerator : IAutoFieldGenerator {

        private MetaTable _table;
        private bool _multiItemMode;

        /// <summary>
        /// Allows to explicitly declare which columns should be skipped
        /// </summary>
        public List<MetaColumn> SkipList {
            get;
            set;
        }

        /// <summary>
        /// Creates a new AdvancedFieldGenerator.
        /// </summary>
        /// <param name="table">The table this class generates fields for.</param>
        /// <param name="multiItemMode"><value>true</value> to indicate a multi-item control such as GridView, <value>false</value> for a single-item control such as DetailsView.</param>
        public AdvancedFieldGenerator(MetaTable table, bool multiItemMode) {
            if (table == null) {
                throw new ArgumentNullException("table");
            }

            _table = table;
            _multiItemMode = multiItemMode;
            SkipList = new List<MetaColumn>();
        }

        private bool IncludeField(MetaColumn column) {
            // Skip columns that should not be scaffolded
            if (!column.GetScaffold())
                return false;

            // Don't display long strings in controls that show multiple items
            if (column.IsLongString && _multiItemMode)
                return false;

            // Skip columns that are on the skip list
            if (SkipList.Contains(column))
                return false;

            return true;
        }

        private ColumnOrderAttribute ColumnOrdering(MetaColumn column) {
            return column.Attributes.OfType<ColumnOrderAttribute>().DefaultIfEmpty(ColumnOrderAttribute.Default).First();
        }

        #region IAutoFieldGenerator Members

        public ICollection GenerateFields(Control control) {
            // Get all of table's columns, take only the ones that should be automatically included in a fields collection,
            // sort the result by the ColumnOrderAttribute, and for each column create a DynamicField
            var fields = from column in _table.Columns
                         where IncludeField(column)
                         orderby ColumnOrdering(column)
                         select new DynamicField() {
                             DataField = column.Name,
                             HeaderText = column.GetDisplayName() // use extension method to refetch display name to work around localization bug
                         };

            return fields.ToList();
        }

        #endregion
    }
}
