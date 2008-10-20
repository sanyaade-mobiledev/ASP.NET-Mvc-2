namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Hosting;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class VirtualPathProviderViewEngine : IViewEngine {
        private static readonly string[] _emptyLocations = new string[0];

        private VirtualPathProvider _vpp;

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] MasterLocationFormats {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] PartialViewLocationFormats {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] ViewLocationFormats {
            get;
            set;
        }

        protected VirtualPathProvider VirtualPathProvider {
            get {
                if (_vpp == null) {
                    _vpp = HostingEnvironment.VirtualPathProvider;
                }
                return _vpp;
            }
            set {
                _vpp = value;
            }
        }

        protected abstract IView CreatePartialView(ControllerContext controllerContext, string partialPath);

        protected abstract IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath);
        
        public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(partialViewName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "partialViewName");
            }

            string[] searched;
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string partialPath = GetPath(PartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, controllerName, out searched);

            if (String.IsNullOrEmpty(partialPath)) {
                return new ViewEngineResult(searched);
            }

            return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
        }

        public virtual ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(viewName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "viewName");
            }

            string[] viewLocationsSearched;
            string[] masterLocationsSearched;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string viewPath = GetPath(ViewLocationFormats, "ViewLocationFormats", viewName, controllerName, out viewLocationsSearched);
            string masterPath = GetPath(MasterLocationFormats, "MasterLocationFormats", masterName, controllerName, out masterLocationsSearched);

            if (String.IsNullOrEmpty(viewPath) || (String.IsNullOrEmpty(masterPath) && !String.IsNullOrEmpty(masterName))) {
                return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }

        private string GetPath(string[] locations, string locationsPropertyName, string name, string controllerName, out string[] searchedLocations) {
            searchedLocations = _emptyLocations;

            if (String.IsNullOrEmpty(name)) {
                return String.Empty;
            }

            if (locations == null || locations.Length == 0) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                    MvcResources.Common_PropertyCannotBeNullOrEmpty, locationsPropertyName));
            }

            bool nameRepresentsPath = IsSpecificPath(name);

            return (nameRepresentsPath) ?
                GetPathFromSpecificName(name, ref searchedLocations) :
                GetPathFromGeneralName(locations, name, controllerName, ref searchedLocations);
        }

        private string GetPathFromGeneralName(string[] locations, string name, string controllerName, ref string[] searchedLocations) {
            string result = String.Empty;
            searchedLocations = new string[locations.Length];

            for (int i = 0; i < locations.Length; i++) {
                string virtualPath = String.Format(CultureInfo.InvariantCulture, locations[i], name, controllerName);

                if (VirtualPathProvider.FileExists(virtualPath)) {
                    searchedLocations = _emptyLocations;
                    result = virtualPath;
                    break;
                }

                searchedLocations[i] = virtualPath;
            }

            return result;
        }

        private string GetPathFromSpecificName(string name, ref string[] searchedLocations) {
            string result = name;

            if (!VirtualPathProvider.FileExists(name)) {
                result = String.Empty;
                searchedLocations = new[] { name };
            }

            return result;
        }

        private static bool IsSpecificPath(string name) {
            char c = name[0];
            return (c == '~' || c == '/');
        }

        public virtual void ReleaseView(ControllerContext controllerContext, IView view) {
            IDisposable disposable = view as IDisposable;
            if (disposable != null) {
                disposable.Dispose();
            }
        }
    }
}