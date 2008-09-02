namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    public partial class HtmlHelper {
        public string Hidden(string name) {
            return Hidden(name, (IDictionary<string, object>)null);
        }

        public string Hidden(string name, object htmlAttributes) {
            return Hidden(name, new RouteValueDictionary(htmlAttributes));
        }

        public string Hidden(string name, IDictionary<string, object> htmlAttributes) {
            return InputHelper("hidden", name, null /* value */, true /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }

        public string Hidden(string name, string value) {
            return Hidden(name, value, (IDictionary<string, object>)null);
        }

        public string Hidden(string name, string value, object htmlAttributes) {
            return Hidden(name, value, new RouteValueDictionary(htmlAttributes));
        }

        public string Hidden(string name, string value, IDictionary<string, object> htmlAttributes) {
            return InputHelper("hidden", name, value, false /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }
    }
}
