using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.UI;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
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
