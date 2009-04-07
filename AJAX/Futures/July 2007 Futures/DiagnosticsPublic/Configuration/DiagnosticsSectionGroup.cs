namespace Microsoft.Web.Preview.Configuration {
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Configuration;

    public sealed class DiagnosticsSectionGroup : ConfigurationSectionGroup {
        [ConfigurationProperty("diagnostics")]
        public DiagnosticsSection Diagnostics {
            get {
                return (DiagnosticsSection)Sections["diagnostics"];
            }
        }
    }
}