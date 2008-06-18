using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData.Extensions {
    public interface ISelectionChangedAware {
        event EventHandler SelectionChanged;
    }

    public class DelegatingFilter : FilterUserControlBase, ISelectionChangedAware, IControlParameterTarget {

        public event EventHandler SelectionChanged;

        private FilterUserControlBase child;

        public override string SelectedValue {
            get {
                return child.SelectedValue;
            }
        }

        public override DataKey SelectedDataKey {
            get {
                return child.SelectedDataKey;
            }
        }

        public object SelectedEnum {
            get {
                string value = SelectedValue;
                return String.IsNullOrEmpty(value) ? null : Enum.Parse(Column.ColumnType, value, false);
            }
        }

        protected void Page_Init(object sender, EventArgs e) {
            child = FilterFactory.Instance.GetFilterControl(this.Column);
            child.TableName = this.TableName;
            child.DataField = this.DataField;
            child.ContextTypeName = this.ContextTypeName;
         
            Controls.Add(child);
        }

        protected void Page_Load(object sender, EventArgs e) {
            var c = child as ISelectionChangedAware;
            if (c != null) {
                c.SelectionChanged += new EventHandler(DelegatingFilter_SelectionChanged);
            }
        }

        void DelegatingFilter_SelectionChanged(object sender, EventArgs e) {
            if (SelectionChanged != null) {
                SelectionChanged(sender, e);
            }
        }

        #region IControlParameterTarget Members

        MetaColumn IControlParameterTarget.FilteredColumn {
            get { return ((IControlParameterTarget)child).FilteredColumn; }
        }

        string IControlParameterTarget.GetPropertyNameExpression(string columnName) {
            return Column.ColumnType.IsEnum ? "SelectedEnum" : ((IControlParameterTarget)child).GetPropertyNameExpression(columnName);
        }

        MetaTable IControlParameterTarget.Table {
            get { return ((IControlParameterTarget)child).Table; }
        }

        #endregion
    }
}
