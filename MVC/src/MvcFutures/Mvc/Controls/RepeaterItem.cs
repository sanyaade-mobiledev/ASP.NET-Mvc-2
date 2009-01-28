namespace Microsoft.Web.Mvc.Controls {
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class RepeaterItem : Control, IDataItemContainer, IViewDataContainer {
        private object _dataItem;
        private int _itemIndex;

        public RepeaterItem(int itemIndex, object dataItem) {
            _itemIndex = itemIndex;
            _dataItem = dataItem;
        }

        public object DataItem {
            get {
                return _dataItem;
            }
        }

        public int DataItemIndex {
            get {
                return _itemIndex;
            }
        }

        public int DisplayIndex {
            get {
                return _itemIndex;
            }
        }

        public ViewDataDictionary ViewData {
            get;
            set;
        }
    }
}
