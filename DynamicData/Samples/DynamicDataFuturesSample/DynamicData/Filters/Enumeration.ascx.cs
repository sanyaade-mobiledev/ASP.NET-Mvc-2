using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using Microsoft.Web.DynamicData;
using System.Collections.Specialized;
using System.Globalization;

namespace DynamicDataFuturesSample {
    public partial class Enumeration_Filter : FilterUserControlBase, ISelectionChangedAware {
        protected void Page_Load(object sender, EventArgs e) {
            if (DropDownList1.Items.Count < 2) {
                foreach (object name in Enum.GetValues(Column.ColumnType)) {
                    DropDownList1.Items.Add(new ListItem(Enum.GetName(Column.ColumnType, name), name.ToString()));
                }
            }
        }

        public override string SelectedValue {
            get {
                return DropDownList1.SelectedValue;
            }
        }

        #region ISelectionChangedAware Members

        public event EventHandler SelectionChanged {
            add {
                DropDownList1.SelectedIndexChanged += value;
            }
            remove {
                DropDownList1.SelectedIndexChanged -= value;
            }
        }

        #endregion
    }
}