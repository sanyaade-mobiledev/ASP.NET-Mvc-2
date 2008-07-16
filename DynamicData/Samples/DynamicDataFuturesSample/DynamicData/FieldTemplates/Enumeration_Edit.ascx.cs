using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class Enumeration_EditField : System.Web.DynamicData.FieldTemplateUserControl {
        private Type _enumType;

        protected void Page_Load(object sender, EventArgs e) {
            DropDownList1.ToolTip = Column.GetDescription();

            if (DropDownList1.Items.Count == 0) {
                if (Mode == DataBoundControlMode.Insert || !Column.IsRequired) {
                    DropDownList1.Items.Add(new ListItem("[Not Set]", String.Empty));
                }

                DynamicDataFutures.FillEnumListControl(DropDownList1, EnumType);
            }

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(DynamicValidator1);
        }

        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);

            if (Mode == DataBoundControlMode.Edit && FieldValue != null) {
                string fieldValue = DynamicDataFutures.GetUnderlyingTypeValueString(EnumType, FieldValue);
                ListItem item = DropDownList1.Items.FindByValue(fieldValue);
                if (item != null) {
                    DropDownList1.SelectedValue = fieldValue;
                }
            }
        }

        private Type EnumType {
            get {
                if (_enumType == null) {
                    _enumType = Column.GetEnumType();
                }
                return _enumType;
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
