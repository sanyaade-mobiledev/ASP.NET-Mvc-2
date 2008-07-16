using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class Enumeration_Filter : FilterUserControlBase, ISelectionChangedAware {
        private Type _enumType;
        private bool? _flagsMode;

        protected void Page_Load(object sender, EventArgs e) {
            // need to turn on the active control
            ActiveListControl.Enabled = true;
            ActiveListControl.Visible = true;

            // items count differs depending on the mode
            bool needToFill = FlagsMode ? ActiveListControl.Items.Count == 0 : ActiveListControl.Items.Count == 1;

            if (needToFill) {
                DynamicDataFutures.FillEnumListControl(ActiveListControl, EnumType);
            }
        }

        public override string SelectedValue {
            get {
                if (FlagsMode) {
                    // in flags mode we need to enumerate over all selected values and build a compoud enumeration value out of it
                    // use long as the type since it's the widest type
                    // REVIEW: what if the underlying enum type is ulong?
                    long value = 0;
                    bool anythingSelected = false;
                    foreach (ListItem item in CheckBoxList1.Items) {
                        if (item.Selected) {
                            value += long.Parse(item.Value);
                            anythingSelected = true;
                        }
                    }
                    if (anythingSelected) {
                        object enumValue = Enum.ToObject(EnumType, value);
                        return DynamicDataFutures.GetUnderlyingTypeValueString(EnumType, enumValue);
                    } else {
                        return String.Empty;
                    }
                } else {
                    return DropDownList1.SelectedValue;
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

        private bool FlagsMode {
            get {
                if (_flagsMode == null) {
                    _flagsMode = DynamicDataFutures.IsEnumTypeInFlagsMode(EnumType);
                }
                return _flagsMode.Value;
            }
        }

        private ListControl ActiveListControl {
            get {
                if (FlagsMode) {
                    return CheckBoxList1;
                } else {
                    return DropDownList1;
                }
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