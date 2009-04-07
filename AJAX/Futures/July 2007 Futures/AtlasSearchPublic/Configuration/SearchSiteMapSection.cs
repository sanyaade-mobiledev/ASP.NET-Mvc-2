namespace Microsoft.Web.Preview.Configuration {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Text;
    using System.Web.Configuration;

    public class SearchSiteMapSection : ConfigurationSection {
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

        internal static SearchSiteMapSection GetConfigurationSection() {
            return (SearchSiteMapSection)WebConfigurationManager.GetWebApplicationSection("microsoft.web.preview/searchSiteMap");
        }
    }
}
