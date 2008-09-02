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

        public ViewEngineResult(IView view) {
            if (view == null) {
                throw new ArgumentNullException("view");
            }

            View = view;
        }

        public IEnumerable<string> SearchedLocations {
            get;
            private set;
        }

        public IView View {
            get;
            private set;
        }
    }
}
