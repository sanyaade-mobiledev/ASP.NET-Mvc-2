namespace Microsoft.Web.Preview.Configuration {
    using System.Configuration;
    using System.Web.Configuration;

    public class DiagnosticsSection : ConfigurationSection {
        [ConfigurationProperty("enabled", DefaultValue = false)]
        public bool Enabled {
            get {
                return (bool)this["enabled"];
            }
        }

        internal static DiagnosticsSection GetConfigurationSection() {
            return (DiagnosticsSection)WebConfigurationManager.GetWebApplicationSection("microsoft.web.preview/diagnostics");
        }
    }
}
