namespace System.Web.Mvc {
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ViewEngines {

        private readonly static ViewEngineCollection _engines = new ViewEngineCollection {
            new WebFormViewEngine() 
        };

        public static ViewEngineCollection Engines {
            get {
                return _engines;
            }
        }
    }
}
