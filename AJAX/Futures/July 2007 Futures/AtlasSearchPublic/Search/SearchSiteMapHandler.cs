namespace Microsoft.Web.Preview.Search {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Xml;
    using Microsoft.Web.Preview.Configuration;

    public class SearchSiteMapHandler : IHttpHandler {
        static SearchSiteMapSection _searchSiteMapSection;
        static List<SearchSiteMapProviderBase> _searchSiteMapProviders;

        public SearchSiteMapHandler() {
        }

        [
        ConfigurationPermission(SecurityAction.Assert, Unrestricted = true),
        SuppressMessage("Microsoft.Security", "CA2106",
            Justification = "The method instantiate providers from the information in the configuration section but doesn't directly expose information nor takes parameters or allow for modification of the critical information.")
        ]
        internal static List<SearchSiteMapProviderBase> InitFromConfig() {
            List<SearchSiteMapProviderBase> list = new List<SearchSiteMapProviderBase>();

            SearchSiteMapSection sectionSiteMapSection = SearchSiteMapSection.GetConfigurationSection();
            foreach (ProviderSettings ps in sectionSiteMapSection.Providers) {
                SearchSiteMapProviderBase _provider = ProvidersHelper.InstantiateProvider(ps, Type.GetType(ps.Type)) as SearchSiteMapProviderBase;
                list.Add(_provider);
            }

            return list;
        }

        internal static List<SearchSiteMapProviderBase> GetSearchSiteMapProviders() {
            if (_searchSiteMapProviders == null) {
                _searchSiteMapProviders = InitFromConfig();
            }

            return _searchSiteMapProviders;
        }

        [
        ConfigurationPermission(SecurityAction.Assert, Unrestricted = true),
        SuppressMessage("Microsoft.Security", "CA2106",
            Justification = "The method only exposes the presence of the SearchSiteMap section. No information can be passed in.")
        ]
        internal static bool IsEnabled() {
            if (_searchSiteMapSection == null) {
                _searchSiteMapSection = SearchSiteMapSection.GetConfigurationSection();
            }
            return _searchSiteMapSection.Enabled;
        }

        public virtual IEnumerable DataQuery() {
            return null;
        }

        public void ProcessRequest(HttpContext context) {
            if (!IsEnabled()) {
                return;
            }

            List<SearchSiteMapProviderBase> searchSiteMapProviders = GetSearchSiteMapProviders();
            if (searchSiteMapProviders.Count == 0) {
                return;
            }

            HttpResponse response = context.Response;
            response.ContentType = "text/xml";

            //Render mode - sitemapindex or sitemap?
            if (context.Request.QueryString.Count == 0) {
                //sitemapindex
                String handlerName = Path.GetFileName(HttpContext.Current.Request.PhysicalPath);

                using (XmlTextWriter writer = new XmlTextWriter(response.OutputStream, Encoding.UTF8)) {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument();
                    writer.WriteStartElement("sitemapindex");
                    writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    string appUrl = SearchSiteMapProviderBase.GenerateUrl(HttpContext.Current.Request.ApplicationPath + "/");

                    //Write out sitemap node in sitemapindex
                    foreach (SearchSiteMapProviderBase searchSiteMapProvider in searchSiteMapProviders) {
                        writer.WriteStartElement("sitemap");

                        writer.WriteElementString("loc", appUrl + handlerName + "?sitemap=" + HttpUtility.UrlEncode(searchSiteMapProvider.Name));

                        DateTime lastModified = DateTime.UtcNow;
                        String lastMod = lastModified.ToString("yyyy-MM-ddThh:mm:ss.fffZ", CultureInfo.InvariantCulture);
                        writer.WriteElementString("lastmod", lastMod);

                        writer.WriteEndElement(); // sitemap
                    }

                    writer.WriteEndElement(); // sitemapindex
                    writer.WriteEndDocument();
                }
            }
            else {
                //sitemap
                String sitemap = context.Request.QueryString["sitemap"];

                if (String.IsNullOrEmpty(sitemap) == false) {
                    using (XmlTextWriter writer = new XmlTextWriter(response.OutputStream, Encoding.UTF8)) {
                        writer.Formatting = Formatting.Indented;
                        writer.WriteStartDocument();
                        writer.WriteStartElement("urlset");
                        writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                        foreach (SearchSiteMapProviderBase searchSiteMapProvider in _searchSiteMapProviders) {
                            if (String.Compare(searchSiteMapProvider.Name, sitemap, StringComparison.Ordinal) == 0) {
                                searchSiteMapProvider.WriteNodes(this, writer);
                            }
                        }

                        writer.WriteEndElement(); // urlset
                        writer.WriteEndDocument();
                    }
                }
            }
        }

        public bool IsReusable {
            get {
                return true;
            }
        }
    }
}
