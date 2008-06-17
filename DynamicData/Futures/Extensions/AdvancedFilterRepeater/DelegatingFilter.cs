using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData.Extensions {
    public interface ISelectionChangedAware {
        event EventHandler SelectionChanged;
    }

    public class DelegatingFilter : FilterUserControlBase, ISelectionChangedAware {

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
    }
}
