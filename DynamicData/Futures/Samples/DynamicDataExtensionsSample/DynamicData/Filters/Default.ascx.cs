using System;
using System.Web.DynamicData;
using System.Web.UI;
using Microsoft.Web.DynamicData.Extensions;

namespace DynamicDataExtensionsSample {
    public partial class Default_Filter : FilterUserControlBase, ISelectionChangedAware {
        public event EventHandler SelectionChanged {
            add {
                DropDownList1.SelectedIndexChanged += value;
            }
            remove {
                DropDownList1.SelectedIndexChanged -= value;
            }
        }

        public override string SelectedValue {
            get {
                return DropDownList1.SelectedValue;
            }
        }

        protected void Page_Init(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                PopulateListControl(DropDownList1);

                // Set the initial value if there is one
                if (!String.IsNullOrEmpty(InitialValue))
                    DropDownList1.SelectedValue = InitialValue;
            }
        }
    }
}