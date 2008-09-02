namespace System.Web.Mvc {
    using System.Globalization;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewMasterPage<TModel> : ViewMasterPage where TModel : class {

        public new ViewDataDictionary<TModel> ViewData {
            get {
                ViewDataDictionary<TModel> viewData = new ViewDataDictionary<TModel>(ViewPage.ViewData);
                return viewData;
            }
        }
    }
}
