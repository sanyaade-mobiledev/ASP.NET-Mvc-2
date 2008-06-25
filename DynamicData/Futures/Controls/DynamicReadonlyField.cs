using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData {
    public class DynamicReadonlyField : DynamicField {
        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex) {
            if (cellType == DataControlCellType.DataCell) {
                var control = new DynamicControl() { DataField = DataField };

                // Copy various properties into the control
                control.UIHint = UIHint;
                control.HtmlEncode = HtmlEncode;
                control.NullDisplayText = NullDisplayText;

                cell.Controls.Add(control);
            } else {
                base.InitializeCell(cell, cellType, rowState, rowIndex);
            }
        }
    }
}
