namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc.Resources;
    using System.Web.UI;

    // Much of the code in this file is borrowed from two locations:
    // Page.InitOutputCache(): ndp\fx\src\xsp\System\Web\UI\Page.cs
    // OutputCacheParameters: ndp\fx\src\xsp\System\Web\UI\OutputCacheSettings.cs

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class OutputCacheAttribute : ActionFilterAttribute {

        private static readonly char[] _varySeparator = new char[] { ';' };

        private string _cacheProfile;
        private int _duration;
        private OutputCacheProperty _setProperties;
        private OutputCacheLocation _location;
        private bool _noStore;
        private string _varyByContentEncoding;
        private string _varyByCustom;
        private string _varyByHeader;
        private string _varyByParam;

        public string CacheProfile {
            get {
                return _cacheProfile ?? String.Empty;
            }
            set {
                _cacheProfile = value;
            }
        }

        public int Duration {
            get {
                return _duration;
            }
            set {
                _setProperties |= OutputCacheProperty.Duration;
                _duration = value;
            }
        }

        public OutputCacheLocation Location {
            get {
                return _location;
            }
            set {
                _setProperties |= OutputCacheProperty.Location;
                _location = value;
            }
        }

        public bool NoStore {
            get {
                return _noStore;
            }
            set {
                _setProperties |= OutputCacheProperty.NoStore;
                _noStore = value;
            }
        }

        public string VaryByContentEncoding {
            get {
                return _varyByContentEncoding ?? String.Empty;
            }
            set {
                _setProperties |= OutputCacheProperty.VaryByContentEncoding;
                _varyByContentEncoding = value;
            }
        }

        public string VaryByCustom {
            get {
                return _varyByCustom ?? String.Empty;
            }
            set {
                _setProperties |= OutputCacheProperty.VaryByCustom;
                _varyByCustom = value;
            }
        }

        public string VaryByHeader {
            get {
                return _varyByHeader ?? String.Empty;
            }
            set {
                _setProperties |= OutputCacheProperty.VaryByHeader;
                _varyByHeader = value;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Param",
            Justification = "Matches the @ OutputCache page directive.")]
        public string VaryByParam {
            get {
                return _varyByParam ?? String.Empty;
            }
            set {
                _setProperties |= OutputCacheProperty.VaryByParam;
                _varyByParam = value;
            }
        }

        private OutputCacheProfile GetProfile() {
            OutputCacheSettingsSection settings = (OutputCacheSettingsSection)WebConfigurationManager.GetSection("system.web/caching/outputCacheSettings");
            OutputCacheProfile profile = settings.OutputCacheProfiles[CacheProfile];
            if (profile == null) {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.CurrentUICulture,
                        MvcResources.OutputCacheAttribute_ProfileNotFound, CacheProfile));
            }
            return profile;
        }

        private bool IsPropertySet(OutputCacheProperty flag) {
            return (_setProperties & flag) == flag;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext) {
            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;

            OutputCacheInitializer initializer = new OutputCacheInitializer {
                Timestamp = filterContext.HttpContext.Timestamp
            };

            // read default values from the configuration file
            if (!String.IsNullOrEmpty(CacheProfile)) {
                OutputCacheProfile profile = GetProfile();
                if (!profile.Enabled) {
                    return;
                }
                initializer.ApplyProfile(profile);
            }

            // overwrite configuration values with explicitly-set values
            if (IsPropertySet(OutputCacheProperty.Duration)) {
                initializer.Duration = Duration;
            }
            if (IsPropertySet(OutputCacheProperty.Location)) {
                initializer.Location = Location;
            }
            if (IsPropertySet(OutputCacheProperty.NoStore)) {
                initializer.NoStore = NoStore;
            }
            if (IsPropertySet(OutputCacheProperty.VaryByContentEncoding)) {
                initializer.VaryByContentEncoding = VaryByContentEncoding;
            }
            if (IsPropertySet(OutputCacheProperty.VaryByCustom)) {
                initializer.VaryByCustom = VaryByCustom;
            }
            if (IsPropertySet(OutputCacheProperty.VaryByHeader)) {
                initializer.VaryByHeader = VaryByHeader;
            }
            if (IsPropertySet(OutputCacheProperty.VaryByParam)) {
                initializer.VaryByParam = VaryByParam;
            }

            // set default location if not initially specified
            if (initializer.Location == (OutputCacheLocation)(-1)) {
                initializer.Location = OutputCacheLocation.Any;
            }

            initializer.InitializeCache(cache);
        }

        internal sealed class OutputCacheInitializer {

            public int Duration {
                get;
                set;
            }

            public OutputCacheLocation Location {
                get;
                set;
            }

            public bool NoStore {
                get;
                set;
            }

            public DateTime Timestamp {
                get;
                set;
            }

            public string VaryByContentEncoding {
                get;
                set;
            }

            public string VaryByCustom {
                get;
                set;
            }

            public string VaryByHeader {
                get;
                set;
            }

            public string VaryByParam {
                get;
                set;
            }

            public void ApplyProfile(OutputCacheProfile profile) {
                Duration = profile.Duration;
                Location = profile.Location;
                NoStore = profile.NoStore;
                VaryByContentEncoding = profile.VaryByContentEncoding;
                VaryByCustom = profile.VaryByCustom;
                VaryByHeader = profile.VaryByHeader;
                VaryByParam = profile.VaryByParam;
            }

            public void InitializeCache(HttpCachePolicyBase cache) {
                // sanity-check values only if they will actually be used
                if (Location != OutputCacheLocation.None) {
                    // duration must be non-negative
                    if (Duration < 0) {
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                            MvcResources.OutputCacheAttribute_InvalidParameter, "Duration"));
                    }
                }

                if (NoStore) {
                    cache.SetNoStore();
                }

                SetCacheability(cache);

                if (Location != OutputCacheLocation.None) {
                    cache.SetExpires(Timestamp.AddSeconds(Duration));
                    cache.SetMaxAge(new TimeSpan(0, 0, Duration));
                    cache.SetValidUntilExpires(true);
                    cache.SetLastModified(Timestamp);

                    // a client cached item won't be cached on the server or a proxy, so it
                    // doesn't need a Varies header
                    if (Location != OutputCacheLocation.Client) {
                        if (!String.IsNullOrEmpty(VaryByContentEncoding)) {
                            string[] a = VaryByContentEncoding.Split(_varySeparator);
                            foreach (string s in a) {
                                cache.VaryByContentEncodings[s.Trim()] = true;
                            }
                        }
                        if (!String.IsNullOrEmpty(VaryByHeader)) {
                            string[] a = VaryByHeader.Split(_varySeparator);
                            foreach (string s in a) {
                                cache.VaryByHeaders[s.Trim()] = true;
                            }
                        }

                        // only items cached on the server need VaryByCustom and VaryByParam
                        if (Location != OutputCacheLocation.Downstream) {
                            if (!String.IsNullOrEmpty(VaryByCustom)) {
                                cache.SetVaryByCustom(VaryByCustom);
                            }

                            if (!String.IsNullOrEmpty(VaryByParam)) {
                                string[] a = VaryByParam.Split(_varySeparator);
                                foreach (string s in a) {
                                    cache.VaryByParams[s.Trim()] = true;
                                }
                            }
                            else {
                                cache.VaryByParams.IgnoreParams = true;
                            }
                        }
                    }
                }
            }

            private void SetCacheability(HttpCachePolicyBase cache) {
                HttpCacheability cacheability;
                switch (Location) {
                    case OutputCacheLocation.Any:
                        cacheability = HttpCacheability.Public;
                        break;
                    case OutputCacheLocation.Server:
                        cacheability = HttpCacheability.ServerAndNoCache;
                        break;
                    case OutputCacheLocation.ServerAndClient:
                        cacheability = HttpCacheability.ServerAndPrivate;
                        break;
                    case OutputCacheLocation.Client:
                        cacheability = HttpCacheability.Private;
                        break;
                    case OutputCacheLocation.Downstream:
                        cacheability = HttpCacheability.Public;
                        cache.SetNoServerCaching();
                        break;
                    case OutputCacheLocation.None:
                        cacheability = HttpCacheability.NoCache;
                        break;
                    default:
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                            MvcResources.OutputCacheAttribute_InvalidParameter, "Location"));
                }
                cache.SetCacheability(cacheability);
            }
        }

        [Flags]
        private enum OutputCacheProperty {
            None = 0,
            Duration = 1 << 0,
            Location = 1 << 1,
            NoStore = 1 << 2,
            VaryByContentEncoding = 1 << 3,
            VaryByCustom = 1 << 4,
            VaryByHeader = 1 << 5,
            VaryByParam = 1 << 6
        }
    }
}
