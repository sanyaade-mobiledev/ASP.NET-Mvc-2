namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Hosting;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    // NOTE:
    //
    // This class makes a differentiation between null strings and empty strings.
    // The reason for this is that the ASP.NET cache cannot store null values, because
    // it returns null for cache lookups that fail. As such, we use null to mean "the
    // value is not found in the cache" and empty strings to mean "the cache is storing
    // an empty value".
    //
    // Wherever you find comparisons of null but not empty, or empty but not null, please
    // leave them as is, so that the system continues to function correctly.

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class VirtualPathProviderViewEngine : IViewEngine {

        private readonly string _cacheKeyPrefix_Master;
        private readonly string _cacheKeyPrefix_Partial;
        private readonly string _cacheKeyPrefix_View;
        private static readonly string[] _emptyLocations = new string[0];
        private VirtualPathProvider _vpp;

        protected VirtualPathProviderViewEngine() {
            if (HttpContext.Current == null || HttpContext.Current.IsDebuggingEnabled) {
                ViewLocationCache = new NullLocationCache();
            }
            else {
                ViewLocationCache = new AspNetCacheLocationCache();
            }

            string className = GetType().FullName;
            _cacheKeyPrefix_Master = className + ":Master:";
            _cacheKeyPrefix_Partial = className + ":Partial:";
            _cacheKeyPrefix_View = className + ":View:";
        }

        // TODO: This property should be exposed as part of the API once we refactor IViewLocationCache.
        internal IViewLocationCache ViewLocationCache {
            get;
            set;
        }

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

        [SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength",
            Justification = "null and String.Empty have different interpretations when locating a view from the cache.")]
        public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName) {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(partialViewName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "partialViewName");
            }

            string[] searched;
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string partialPath = GetPath(PartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, controllerName, _cacheKeyPrefix_Partial, out searched);

            if (partialPath == String.Empty) {
                return new ViewEngineResult(searched);
            }

            return new ViewEngineResult(CreatePartialView(controllerContext, partialPath));
        }

        [SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength",
            Justification = "null and String.Empty have different interpretations when locating a view from the cache.")]
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
            string viewPath = GetPath(ViewLocationFormats, "ViewLocationFormats", viewName, controllerName, _cacheKeyPrefix_View, out viewLocationsSearched);
            string masterPath = GetPath(MasterLocationFormats, "MasterLocationFormats", masterName, controllerName, _cacheKeyPrefix_Master, out masterLocationsSearched);

            if (viewPath == String.Empty || (masterPath == String.Empty && !String.IsNullOrEmpty(masterName))) {
                return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath));
        }

        private string GetPath(string[] locations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, out string[] searchedLocations) {
            searchedLocations = _emptyLocations;

            if (String.IsNullOrEmpty(name)) {
                return String.Empty;
            }

            if (locations == null || locations.Length == 0) {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                    MvcResources.Common_PropertyCannotBeNullOrEmpty, locationsPropertyName));
            }

            string cacheKey = cacheKeyPrefix + name;
            string result = ViewLocationCache.Get(cacheKey);

            if (result != null) {
                return result;
            }

            return IsSpecificPath(name)
                ? GetPathFromSpecificName(name, cacheKey, ref searchedLocations)
                : GetPathFromGeneralName(locations, name, controllerName, cacheKey, ref searchedLocations);
        }

        private string GetPathFromGeneralName(string[] locations, string name, string controllerName, string cacheKey, ref string[] searchedLocations) {
            string result = "";
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

            ViewLocationCache.Set(cacheKey, result);
            return result;
        }

        private string GetPathFromSpecificName(string name, string cacheKey, ref string[] searchedLocations) {
            string result = name;

            if (!VirtualPathProvider.FileExists(name)) {
                result = "";
                searchedLocations = new[] { name };
            }

            ViewLocationCache.Set(cacheKey, result);
            return result;
        }

        private static bool IsSpecificPath(string name) {
            char c = name[0];
            return (c == '~' || c == '/');
        }

        private class AspNetCacheLocationCache : IViewLocationCache {
            private readonly TimeSpan _slidingTimeout = new TimeSpan(0, 15, 0);

            public string Get(string cacheKey) {
                return (string)HttpContext.Current.Cache[cacheKey];
            }

            public void Set(string cacheKey, string virtualPath) {
                HttpContext.Current.Cache.Insert(cacheKey, virtualPath, null, Cache.NoAbsoluteExpiration, _slidingTimeout);
            }
        }

        private class NullLocationCache : IViewLocationCache {
            public string Get(string cacheKey) {
                return null;
            }

            public void Set(string cacheKey, string virtualPath) {
            }
        }
    }
}