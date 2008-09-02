namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    public partial class HtmlHelper {
        public string Password(string name) {
            return Password(name, (IDictionary<string, object>)null);
        }

        public string Password(string name, object htmlAttributes) {
            return Password(name, new RouteValueDictionary(htmlAttributes));
        }

        public string Password(string name, IDictionary<string, object> htmlAttributes) {
            return InputHelper("password", name, null /* value */, true /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }

        public string Password(string name, string value) {
            return Password(name, value, (IDictionary<string, object>)null);
        }

        public string Password(string name, string value, object htmlAttributes) {
            return Password(name, value, new RouteValueDictionary(htmlAttributes));
        }

        public string Password(string name, string value, IDictionary<string, object> htmlAttributes) {
            return InputHelper("password", name, value, false /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }
    }
}
