namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;
    using System.Collections;

    public class XmlBridgeTransformer : IBridgeResponseTransformer {
        public void Initialize(BridgeTransformData data) { }
        public object Transform(object results) {
            if (results == null) {
                throw new ArgumentNullException("results");
            }
            // REVIEW: there should be a cleaner way to do this
            XmlSerializer xs = new XmlSerializer(results.GetType());
            MemoryStream ms = new MemoryStream();
            using (XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8)) {
                xs.Serialize(writer, results);
                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms)) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
