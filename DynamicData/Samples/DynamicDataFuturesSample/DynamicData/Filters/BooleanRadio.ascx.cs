using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class BooleanRadio_Filter : FilterUserControlBase, ISelectionChangedAware {
        public event EventHandler SelectionChanged {
            add {
                RadioButtonList1.SelectedIndexChanged += value;
            }
            remove {
                RadioButtonList1.SelectedIndexChanged -= value;
            }
        }

        protected void Page_Init(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                //if (!Column.IsRequired) {
                //    RadioButtonList1.Items.Add(new ListItem("none", ""));
                //}
                RadioButtonList1.Items.Add(new ListItem("True", bool.TrueString));
                RadioButtonList1.Items.Add(new ListItem("False", bool.FalseString));
            }
        }

        public override string SelectedValue {
            get {
                return RadioButtonList1.SelectedValue;
            }
        }
    }
}