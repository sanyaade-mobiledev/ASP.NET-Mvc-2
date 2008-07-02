using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace DynamicDataFuturesSample {
    public partial class DateAjaxCalendar_EditField : System.Web.DynamicData.FieldTemplateUserControl {
        protected void Page_Load(object sender, EventArgs e) {
            SetUpValidator(RequiredFieldValidator1);
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
