namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewEngineResult {

        public ViewEngineResult(IEnumerable<string> searchedLocations) {
            if (searchedLocations == null) {
                throw new ArgumentNullException("searchedLocations");
            }

            SearchedLocations = searchedLocations;
        }

        public ViewEngineResult(IView view, IViewEngine viewEngine) {
            if (view == null) {
                throw new ArgumentNullException("view");
            }
            if (viewEngine == null) {
                throw new ArgumentNullException("viewEngine");
            }

            View = view;
            ViewEngine = viewEngine;
        }

        public IEnumerable<string> SearchedLocations {
            get;
            private set;
        }

        public IView View {
            get;
            private set;
        }

        public IViewEngine ViewEngine {
            get;
            private set;
        }
    }
}
