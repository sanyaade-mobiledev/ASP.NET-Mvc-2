namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    public partial class HtmlHelper {
        public string TextBox(string name) {
            return TextBox(name, (IDictionary<string, object>)null);
        }

        public string TextBox(string name, object htmlAttributes) {
            return TextBox(name, new RouteValueDictionary(htmlAttributes));
        }

        public string TextBox(string name, IDictionary<string, object> htmlAttributes) {
            return InputHelper("text", name, null /* value */, true /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }

        public string TextBox(string name, string value) {
            return TextBox(name, value, (IDictionary<string, object>)null);
        }

        public string TextBox(string name, string value, object htmlAttributes) {
            return TextBox(name, value, new RouteValueDictionary(htmlAttributes));
        }

        public string TextBox(string name, string value, IDictionary<string, object> htmlAttributes) {
            return InputHelper("text", name, value, false /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }
    }
}
