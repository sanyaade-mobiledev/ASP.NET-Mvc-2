namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewEngineCollection : Collection<IViewEngine> {

        public ViewEngineCollection() {
        }

        public ViewEngineCollection(IList<IViewEngine> list)
            : base(list) {
        }

    }
}
