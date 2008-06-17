using System;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.DynamicData;

namespace Microsoft.Web.DynamicData.Extensions {
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
}
