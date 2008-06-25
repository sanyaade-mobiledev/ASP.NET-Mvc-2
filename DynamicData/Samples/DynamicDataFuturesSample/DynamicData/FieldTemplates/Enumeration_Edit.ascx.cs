﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class Enumeration_EditField : System.Web.DynamicData.FieldTemplateUserControl {
        protected void Page_Load(object sender, EventArgs e) {
            DropDownList1.ToolTip = Column.GetDescription();

            if (DropDownList1.Items.Count == 0) {
                DropDownList1.Items.Add(new ListItem("[Not Set]", String.Empty));
                foreach (string name in Enum.GetNames(Column.ColumnType)) {
                    DropDownList1.Items.Add(new ListItem(name));
                }
            }

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(DynamicValidator1);
        }

        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);

            if (Mode == DataBoundControlMode.Edit) {
                string fieldValue = FieldValueString;
                ListItem item = DropDownList1.Items.FindByValue(fieldValue);
                if (item != null) {
                    DropDownList1.SelectedValue = fieldValue;
                }
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary) {
            string value = DropDownList1.SelectedValue;
            if (value == String.Empty) {
                value = null;
            }
            dictionary[Column.Name] = ConvertEditedValue(value);
        }

        public override Control DataControl {
            get {
                return DropDownList1;
            }
        }
    }
}