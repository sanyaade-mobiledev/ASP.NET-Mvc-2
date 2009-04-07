namespace Microsoft.Web.Preview.Configuration {
    using System.Configuration;
    using System.Web.Configuration;

    public class SearchSection : ConfigurationSection {
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers {
            get { return (ProviderSettingsCollection)base["providers"]; }
        }

        [ConfigurationProperty("enabled", DefaultValue = false)]
        public bool Enabled {
            get {
                return (bool)this["enabled"];
            }
        }

        internal static SearchSection GetConfigurationSection() {
            return (SearchSection)WebConfigurationManager.GetWebApplicationSection("microsoft.web.preview/search");
        }
    }
}
