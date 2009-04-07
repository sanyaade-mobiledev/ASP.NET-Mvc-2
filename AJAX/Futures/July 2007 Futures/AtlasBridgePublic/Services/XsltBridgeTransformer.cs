namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.Hosting;
    using System.Xml;
    using System.Xml.Xsl;
    using Microsoft.Web.Preview.Resources;

    public class XsltBridgeTransformer : IBridgeResponseTransformer {
        private string _xsltVirtualPath = String.Empty;

        public void Initialize(BridgeTransformData data) {
            _xsltVirtualPath = data.Attributes["stylesheetFile"];
        }

        public object Transform(object results) {
            string xml = results as string;
            if (xml == null) {
                throw new ArgumentException(PreviewWeb.XsltBridgeTransformer_StringOnly, "results");
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            XslCompiledTransform xslt = new XslCompiledTransform();

            // REVIEW: Is passing in CurrentCulture for the StringWriter correct? Previously
            // it wasn't passing in anything, and CurrentCulture is the default anyway.
            using (StringWriter output = new StringWriter(CultureInfo.CurrentCulture)) {
                // Make it absolute to get around an ASP.NET issue
                _xsltVirtualPath = VirtualPathUtility.ToAbsolute(_xsltVirtualPath);
                using (Stream file = VirtualPathProvider.OpenFile(_xsltVirtualPath)) {
                    using (XmlReader reader = XmlReader.Create(file)) {
                        xslt.Load(reader);
                        xslt.Transform(xmlDocument, null, output);
                    }
                }
                return output.GetStringBuilder().ToString();
            }
        }
    }
}
