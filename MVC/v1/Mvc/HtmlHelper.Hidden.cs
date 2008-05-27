namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc.Resources;

    public partial class HtmlHelper {
        public string Hidden(string name) {
            return Hidden(name, (IDictionary<string, object>)null);
        }

        public string Hidden(string name, object htmlAttributes) {
            return Hidden(name, ToDictionary(htmlAttributes));
        }

        public string Hidden(string name, IDictionary<string, object> htmlAttributes) {
            return InputHelper(name, true /* useViewData */, null /* defaultValue */, "hidden" /* inputType */, htmlAttributes);
        }

        public string Hidden(string name, string value) {
            return Hidden(name, value, (IDictionary<string, object>)null);
        }

        public string Hidden(string name, string value, object htmlAttributes) {
            return Hidden(name, value, ToDictionary(htmlAttributes));
        }

        public string Hidden(string name, string value, IDictionary<string, object> htmlAttributes) {
            return InputHelper(name, false /* useViewData */, value, "hidden" /* inputType */, htmlAttributes);
        }
    }
}
