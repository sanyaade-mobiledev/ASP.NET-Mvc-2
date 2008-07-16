namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc.Resources;

    public partial class HtmlHelper {
        public string Password(string name) {
            return Password(name, (IDictionary<string, object>)null);
        }

        public string Password(string name, object htmlAttributes) {
            return Password(name, TagBuilder.ToDictionary(htmlAttributes));
        }

        public string Password(string name, IDictionary<string, object> htmlAttributes) {
            return InputHelper(name, true /* useViewData */, null /* defaultValue */, "password" /* inputType */, htmlAttributes);
        }

        public string Password(string name, string value) {
            return Password(name, value, (IDictionary<string, object>)null);
        }

        public string Password(string name, string value, object htmlAttributes) {
            return Password(name, value, TagBuilder.ToDictionary(htmlAttributes));
        }

        public string Password(string name, string value, IDictionary<string, object> htmlAttributes) {
            return InputHelper(name, false /* useViewData */, value, "password" /* inputType */, htmlAttributes);
        }
    }
}
