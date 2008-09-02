namespace System.Web.Mvc {
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewUserControl<TModel> : ViewUserControl where TModel : class {
        private ViewDataDictionary<TModel> _viewData;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public new ViewDataDictionary<TModel> ViewData {
            get {
                EnsureViewData();
                return _viewData;
            }
            set {
                SetViewData(value);
            }
        }

        protected override void SetViewData(ViewDataDictionary viewData) {
            _viewData = new ViewDataDictionary<TModel>(viewData);

            base.SetViewData(_viewData);
        }
    }
}
