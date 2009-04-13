using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.DynamicData.ModelProviders;
using System.ComponentModel;

namespace DynamicDataFuturesSample{
    public class CustomMetaForeignKeyColumn : MetaForeignKeyColumn {
        public CustomMetaForeignKeyColumn(MetaTable table, ColumnProvider columnProvider) :
            base(table, columnProvider) {
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override AttributeCollection BuildAttributeCollection() {
            return CustomMetaColumn.AddDisplayName(Name, base.BuildAttributeCollection());
        }
    }
}
