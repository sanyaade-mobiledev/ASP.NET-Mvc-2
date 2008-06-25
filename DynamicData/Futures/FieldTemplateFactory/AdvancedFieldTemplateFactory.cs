using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData{
    public class AdvancedFieldTemplateFactory : FieldTemplateFactory {
        private const string EnumerationField = "Enumeration";

        public override string GetFieldTemplateVirtualPath(MetaColumn column, DataBoundControlMode mode, string uiHint) {

            // make sure we don't override a custom UIHint
            if (String.IsNullOrEmpty(uiHint)) {
                if (column.ColumnType.IsEnum) {
                    uiHint = EnumerationField;
                }
            }

            return base.GetFieldTemplateVirtualPath(column, mode, uiHint);
        }
    }
}
