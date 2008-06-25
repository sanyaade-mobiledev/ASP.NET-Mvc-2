using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Compilation;
using System.Linq;

namespace Microsoft.Web.DynamicData {
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

        public class FilterFactory {
            static FilterFactory() {
                Instance = new FilterFactory();
            }

            public static FilterFactory Instance {
                get;
                private set;
            }

            public string FilterFolderVirtualPath {
                get {
                    return "~/DynamicData/Filters/";
                }
            }

            public virtual FilterUserControlBase GetFilterControl(MetaColumn column) {
                if (column == null) {
                    throw new ArgumentNullException("column");
                }

                string filterTemplatePath = null;

                string filterControlName = "Default";

                FilterAttribute filterAttribute = column.Attributes.OfType<FilterAttribute>().FirstOrDefault();
                if (filterAttribute != null) {
                    if (!filterAttribute.Enabled) {
                        throw new InvalidOperationException(String.Format("The column '{0}' has a disabled filter", column.Name));
                    }

                    if (!String.IsNullOrEmpty(filterAttribute.FilterControl)) {
                        filterControlName = filterAttribute.FilterControl;
                    }
                }

                filterTemplatePath = VirtualPathUtility.Combine(FilterFolderVirtualPath, filterControlName + ".ascx");

                var filter = (FilterUserControlBase)BuildManager.CreateInstanceFromVirtualPath(
                    filterTemplatePath, typeof(FilterUserControlBase));

                return filter;
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
