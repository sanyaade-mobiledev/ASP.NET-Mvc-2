using System;
using System.Linq;
using System.Web.UI;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class Enumeration : System.Web.DynamicData.FieldTemplateUserControl {
        public override Control DataControl {
            get {
                return Literal1;
            }
        }

        public string EnumFieldValueString {
            get {
                if (FieldValue == null) {
                    return FieldValueString;
                }

                var attrib = Column.Attributes.OfType<EnumDataTypeAttribute>().SingleOrDefault();
                if (attrib != null) {
                    object enumValue = Enum.ToObject(attrib.EnumType, FieldValue);
                    return FormatFieldValue(enumValue);
                }

                return FieldValueString;
            }
        }
    }
}
