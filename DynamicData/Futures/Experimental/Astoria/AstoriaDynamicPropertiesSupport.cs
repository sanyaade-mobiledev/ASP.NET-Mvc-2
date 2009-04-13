using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Web.Data.Services.Client {
    public class DynamicPropertiesSupport {
        private readonly XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";
        private readonly XNamespace AstoriaDataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        private readonly XNamespace AstoriaMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

        public DynamicPropertiesSupport(DataServiceContext context) {
            context.ReadingEntity += OnReadingEntity;
        }

        void OnReadingEntity(object sender, ReadingWritingEntityEventArgs e) {
            var bitbucketProp = e.Entity.GetType().GetProperty("DynamicProperties");

            IDictionary<string, object> bitbucket = null;
            if (bitbucketProp != null) {
                bitbucket = bitbucketProp.GetValue(e.Entity, null) as IDictionary<string, object>;
            }

            if (bitbucket != null) {
                var properties = e.Entity.GetType().GetProperties();

                var q = from p in e.Data.Element(AtomNamespace + "content")
                                        .Element(AstoriaMetadataNamespace + "properties")
                                        .Elements()
                        where properties.All(pp => pp.Name != p.Name.LocalName)
                        select new {
                            Name = p.Name.LocalName,
                            IsNull = string.Equals("true", p.Attribute(AstoriaMetadataNamespace + "null") == null ? null : p.Attribute(AstoriaMetadataNamespace + "null").Value, StringComparison.OrdinalIgnoreCase),
                            TypeName = p.Attribute(AstoriaMetadataNamespace + "type") == null ? null : p.Attribute(AstoriaMetadataNamespace + "type").Value,
                            p.Value
                        };

                foreach (var dp in q) {
                    bitbucket[dp.Name] = GetTypedEdmValue(dp.TypeName, dp.Value, dp.IsNull);
                }
            }
        }

        private static object GetTypedEdmValue(string type, string value, bool isnull) {
            if (isnull) return null;

            if (string.IsNullOrEmpty(type)) return value;

            // some types missing
            switch (type) {
                case "Edm.String": return value;
                case "Edm.Int16": return Convert.ChangeType(value, typeof(short));
                case "Edm.Int32": return Convert.ChangeType(value, typeof(int));
                case "Edm.Int64": return Convert.ChangeType(value, typeof(long));
                case "Edm.Boolean": return Convert.ChangeType(value, typeof(bool));
                case "Edm.Decimal": return Convert.ChangeType(value, typeof(decimal));
                case "Edm.DateTime": return XmlConvert.ToDateTime(value);
                case "Edm.Binary": return Convert.FromBase64String(value);

                default: throw new NotSupportedException("Not supported type " + type);
            }
        }
    }
}
