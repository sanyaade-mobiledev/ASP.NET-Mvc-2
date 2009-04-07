namespace Microsoft.Web.Preview.Configuration {
    using System.Configuration;

    public sealed class PreviewSectionGroup : ConfigurationSectionGroup {
        [ConfigurationProperty("search")]
        public SearchSection Search {
            get {
                return (SearchSection)Sections["search"];
            }
        }

        [ConfigurationProperty("searchSiteMap")]
        public SearchSiteMapSection SearchSiteMap {
            get {
                return (SearchSiteMapSection)Sections["searchSiteMap"];
            }
        }
    }
}