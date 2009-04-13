using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData {
    /// <summary>
    /// Implements the IAutoFieldGenerator interface and supports advanced scenarios such as declarative column ordering,
    /// workaround for attribute localization issues.
    /// </summary>
    public class AdvancedFieldGenerator : IAutoFieldGenerator {

        private MetaTable _table;
        private ContainerType _containerType;

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
            _containerType = multiItemMode ? ContainerType.List : ContainerType.Item;
            SkipList = new List<MetaColumn>();
        }

        private bool IncludeField(MetaColumn column) {
            // Skip columns that are on the skip list
            return !SkipList.Contains(column);
        }

        #region IAutoFieldGenerator Members

        public ICollection GenerateFields(Control control) {
            DataBoundControlMode mode = GetMode(control);

            // Get all of table's columns, take only the ones that should be automatically included in a fields collection,
            // sort the result by the ColumnOrderAttribute, and for each column create a DynamicField
            var fields = from column in _table.GetScaffoldColumns(mode, _containerType)
                         where IncludeField(column)
                         select new DynamicField() {
                             DataField = column.Name,
                             HeaderText = column.DisplayName
                         };

            return fields.ToList();
        }

        #endregion

        private static DataBoundControlMode GetMode(Control control) {
            // Try to get the mode from the details view
            var detailsView = control as DetailsView;
            if (detailsView != null) {
                switch (detailsView.CurrentMode) {
                    case DetailsViewMode.Edit:
                        return DataBoundControlMode.Edit;
                    case DetailsViewMode.Insert:
                        return DataBoundControlMode.Insert;
                    default:
                        return DataBoundControlMode.ReadOnly;
                }
            }

            return DataBoundControlMode.ReadOnly;
        }
    }
}
