namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class CompositeViewEngine : IViewEngine {

        public CompositeViewEngine(ICollection<IViewEngine> engineCollection) {
            if (engineCollection == null) {
                throw new ArgumentNullException("engineCollection");
            }

            EngineCollection = engineCollection;
        }

        public ICollection<IViewEngine> EngineCollection {
            get;
            private set;
        }

        private ViewEngineResult Find(Func<IViewEngine, ViewEngineResult> locator) {
            List<string> searched = new List<string>();

            foreach (IViewEngine engine in EngineCollection) {
                if (engine == null) {
                    continue;
                }

                ViewEngineResult result = locator(engine);

                if (result.View != null) {
                    return result;
                }

                searched.AddRange(result.SearchedLocations);
            }

            return new ViewEngineResult(searched);
        }

        public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "partialViewName");
            }

            return Find(e => e.FindPartialView(controllerContext, partialViewName));
        }

        public virtual ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "viewName");
            }

            return Find(e => e.FindView(controllerContext, viewName, masterName));
        }

    }
}
