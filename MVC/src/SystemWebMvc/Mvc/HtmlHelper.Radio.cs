namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    public partial class HtmlHelper {
        public string RadioButton(string name) {
            return RadioButton(name, false /* isChecked */);
        }

        public string RadioButton(string name, bool isChecked) {
            return RadioButton(name, isChecked, (object)null /* htmlAttributes */);
        }

        public string RadioButton(string name, bool isChecked, object htmlAttributes) {
            return RadioButton(name, isChecked, new RouteValueDictionary(htmlAttributes));
        }

        public string RadioButton(string name, bool isChecked, IDictionary<string, object> htmlAttributes) {
            return InputHelper("radio", name, null /* value */, true /* useViewData */, isChecked, true /* setId */, htmlAttributes);
        }

        public string RadioButton(string name, string value, bool isChecked) {
            return RadioButton(name, value, isChecked, (object)null /* htmlAttributes */);
        }

        public string RadioButton(string name, string value, bool isChecked, object htmlAttributes) {
            return RadioButton(name, value, isChecked, new RouteValueDictionary(htmlAttributes));
        }

        public string RadioButton(string name, string value, bool isChecked, IDictionary<string, object> htmlAttributes) {
            return InputHelper("radio", name, value, false /* useViewData */, isChecked, true /* setId */, htmlAttributes);
        }
    }
}
