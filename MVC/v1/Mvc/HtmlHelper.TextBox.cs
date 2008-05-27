namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc.Resources;

    public partial class HtmlHelper {
        public string TextBox(string name) {
            return TextBox(name, (IDictionary<string, object>)null);
        }

        public string TextBox(string name, object htmlAttributes) {
            return TextBox(name, ToDictionary(htmlAttributes));
        }

        public string TextBox(string name, IDictionary<string, object> htmlAttributes) {
            return InputHelper(name, true /* useViewData */, null /* defaultValue */, "text" /* inputType */, htmlAttributes);
        }

        public string TextBox(string name, string value) {
            return TextBox(name, value, (IDictionary<string, object>)null);
        }

        public string TextBox(string name, string value, object htmlAttributes) {
            return TextBox(name, value, ToDictionary(htmlAttributes));
        }

        public string TextBox(string name, string value, IDictionary<string, object> htmlAttributes) {
            return InputHelper(name, false /* useViewData */, value, "text" /* inputType */, htmlAttributes);
        }
    }
}
