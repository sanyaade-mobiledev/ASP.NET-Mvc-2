using System;
using System.Data;
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
using System.ComponentModel.DataAnnotations;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample.DynamicData.FieldTemplates {
    public partial class IntegerSlider_EditField : System.Web.DynamicData.FieldTemplateUserControl {
        protected void Page_Load(object sender, EventArgs e) {
            var metadata = MetadataAttributes.OfType<RangeAttribute>().FirstOrDefault();
            if (metadata != null) {
                SliderExtender1.Minimum = (int)metadata.Minimum;
                SliderExtender1.Maximum = (int)metadata.Maximum;
            }
            TextBox1.ToolTip = Column.GetDescription();
            SetUpValidator(DynamicValidator1);
        }

        protected override void ExtractValues(IOrderedDictionary dictionary) {
            dictionary[Column.Name] = ConvertEditedValue(TextBox1.Text);
        }

        public override Control DataControl {
            get {
                return TextBox1;
            }
        }
    }
}
