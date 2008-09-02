namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    public partial class HtmlHelper {
        public string CheckBox(string name) {
            return CheckBox(name, (object)null /* htmlAttributes */);
        }

        public string CheckBox(string name, bool isChecked) {
            return CheckBox(name, isChecked, (object)null /* htmlAttributes */);
        }

        public string CheckBox(string name, bool isChecked, object htmlAttributes) {
            return CheckBox(name, isChecked, new RouteValueDictionary(htmlAttributes));
        }

        public string CheckBox(string name, object htmlAttributes) {
            return CheckBox(name, new RouteValueDictionary(htmlAttributes));
        }

        public string CheckBox(string name, IDictionary<string, object> htmlAttributes) {
            return InputHelper("checkbox", name, "true", true /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }

        public string CheckBox(string name, bool isChecked, IDictionary<string, object> htmlAttributes) {
            return InputHelper("checkbox", name, "true", false /* useViewData */, isChecked, true /* setId */, htmlAttributes);
        }
    }
}
