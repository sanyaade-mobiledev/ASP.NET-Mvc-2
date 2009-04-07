namespace Microsoft.Web.Preview.Search {
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Xml;

    public class AspNetSiteMapSearchSiteMapProvider : SearchSiteMapProviderBase {
        private const int MaxDepth = 6;

        public AspNetSiteMapSearchSiteMapProvider()
            : base() {
        }

        public override void WriteNodes(SearchSiteMapHandler handler, XmlTextWriter writer) {
            SiteMapNode root = System.Web.SiteMap.RootNode;
            WriteNode(root, writer, 0);
        }

        private void WriteNode(SiteMapNode node, XmlTextWriter writer, int depth) {
            if (depth > MaxDepth)
                return;

            writer.WriteStartElement("url");
            string appUrl = SearchSiteMapProviderBase.GenerateUrl(node.Url);
            writer.WriteElementString("loc", appUrl);

            string lastMod = node["lastModified"];
            if (String.IsNullOrEmpty(lastMod) && (HttpContext.Current != null)) {
                string physicalPath = HttpContext.Current.Request.MapPath(node.Url);
                if (File.Exists(physicalPath)) {
                    DateTime lastModified = File.GetLastWriteTimeUtc(physicalPath);
                    lastMod = lastModified.ToString("yyyy-MM-ddThh:mm:ss.fffZ", CultureInfo.InvariantCulture);
                }
            }
            if (!String.IsNullOrEmpty(lastMod)) {
                writer.WriteElementString("lastmod", lastMod);
            }

            string changeFreq = node["changeFrequency"];
            if (!String.IsNullOrEmpty(changeFreq)) {
                writer.WriteElementString("changefreq", changeFreq);
            }

            string priority = node["priority"];
            if (!String.IsNullOrEmpty(priority)) {
                writer.WriteElementString("priority", priority);
            }

            writer.WriteEndElement(); // url

            int subNodeDepth = depth + 1;
            foreach (SiteMapNode subNode in node.ChildNodes) {
                WriteNode(subNode, writer, subNodeDepth);
            }
        }
    }
}