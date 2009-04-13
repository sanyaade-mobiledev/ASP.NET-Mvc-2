using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

namespace DynamicDataProject
{
    public partial class EnumerationField : System.Web.DynamicData.FieldTemplateUserControl {
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

            Type enumType = Column.GetEnumType();
            if (enumType != null) {
                object enumValue = System.Enum.ToObject(enumType, FieldValue);
                return FormatFieldValue(enumValue);
            }

            return FieldValueString;
        }
    }
    }
}
