namespace System.Web.Mvc {
    using System.Globalization;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewMasterPage<TModel> : ViewMasterPage where TModel : class {
        private AjaxHelper<TModel> _ajaxHelper;
        private HtmlHelper<TModel> _htmlHelper;
        private ViewDataDictionary<TModel> _viewData;

        public new AjaxHelper<TModel> Ajax {
            get {
                if (_ajaxHelper == null) {
                    _ajaxHelper = new AjaxHelper<TModel>(ViewContext, ViewPage);
                }
                return _ajaxHelper;
            }
        }

        public new HtmlHelper<TModel> Html {
            get {
                if (_htmlHelper == null) {
                    _htmlHelper = new HtmlHelper<TModel>(ViewContext, ViewPage);
                }
                return _htmlHelper;
            }
        }

        public new TModel Model {
            get {
                return ViewData.Model;
            }
        }

        public new ViewDataDictionary<TModel> ViewData {
            get {
                if (_viewData == null) {
                    _viewData = new ViewDataDictionary<TModel>(ViewPage.ViewData);
                }
                return _viewData;
            }
        }
    }
}
