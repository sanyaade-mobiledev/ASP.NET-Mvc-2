using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.DynamicData.ModelProviders;
using AttributeCollection = System.ComponentModel.AttributeCollection;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;

namespace DynamicDataFuturesSample {
    public class CustomMetaTable : MetaTable {
        public CustomMetaTable(MetaModel metaModel, TableProvider tableProvider) :
            base(metaModel, tableProvider) {
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override MetaColumn CreateColumn(ColumnProvider columnProvider) {
            return new CustomMetaColumn(this, columnProvider);
        }

        protected override MetaChildrenColumn CreateChildrenColumn(ColumnProvider columnProvider) {
            return new CustomMetaChildrenColumn(this, columnProvider);
        }

        protected override MetaForeignKeyColumn CreateForeignKeyColumn(ColumnProvider columnProvider) {
            return new CustomMetaForeignKeyColumn(this, columnProvider);
        }

        protected override AttributeCollection BuildAttributeCollection() {
            return CustomMetaColumn.AddDisplayName(Name, base.BuildAttributeCollection());
        }

        public override IEnumerable<MetaColumn> GetScaffoldColumns(DataBoundControlMode mode, ContainerType containerType) {
            return from column in base.GetScaffoldColumns(mode, containerType)
                   where IncludeField(column, mode)
                   select column;
        }

        private bool IncludeField(MetaColumn column, DataBoundControlMode mode) {

            // Exclude bool columns in Insert mode (just to test custom filtering)
            if (mode == DataBoundControlMode.Insert && column.ColumnType == typeof(bool))
                return false;

            return true;
        }

        public override string GetDisplayString(object row) {
            return "CUSTOM_" + base.GetDisplayString(row);
        }
    }
}
