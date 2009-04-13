using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.DynamicData.ModelProviders;
using System.ComponentModel;
using System.Text;

namespace DynamicDataFuturesSample {
    public class CustomMetaColumn : MetaColumn {
        public CustomMetaColumn(MetaTable table, ColumnProvider columnProvider):
            base(table, columnProvider) {
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override AttributeCollection BuildAttributeCollection() {
            return CustomMetaColumn.AddDisplayName(Name, base.BuildAttributeCollection());
        }

        internal static AttributeCollection AddDisplayName(string name, AttributeCollection attributes) {
            var displayNameAttrib = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

            // If there is already a display name attribute, don't change anything
            if (displayNameAttrib != null) {
                return attributes;
            }

            // Add a friendlier display name attribute
            return AttributeCollection.FromExisting(
                attributes,
                new DisplayNameAttribute(MakeFriendlyName(name)));
        }

        // Turn a string like "QuantityPerUnit" int "Quantity Per Unit"
        // Also, change _ to spaces
        internal static string MakeFriendlyName(string name) {
            var builder = new StringBuilder();

            for (int i = 0; i < name.Length; i++) {

                char c = name[i];
                if (c == '_') {
                    c = ' ';
                }

                if (i > 0 && Char.IsUpper(c) && name[i - 1] != '_') {
                    builder.Append(' ');
                }
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
