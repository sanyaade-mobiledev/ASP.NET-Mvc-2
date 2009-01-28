namespace System.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AjaxHelper<TModel> : AjaxHelper where TModel : class {
        private ViewDataDictionary<TModel> _viewData;

        public AjaxHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
            : this(viewContext, viewDataContainer, RouteTable.Routes) {
        }

        public AjaxHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection)
            : base(viewContext, viewDataContainer, routeCollection) {

            _viewData = new ViewDataDictionary<TModel>(viewDataContainer.ViewData);
        }

        public new ViewDataDictionary<TModel> ViewData {
            get {
                return _viewData;
            }
        }
    }
}
