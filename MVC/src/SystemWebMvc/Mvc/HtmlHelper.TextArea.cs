namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    public partial class HtmlHelper {
        // These values are similar to the defaults used by WebForms
        // when using <asp:TextBox TextMode="MultiLine> without specifying
        // the Rows and Columns attributes.
        private const int TextAreaRows = 2;
        private const int TextAreaColumns = 20;

        public string TextArea(string name) {
            return TextArea(name, (object)null /* htmlAttributes */);
        }

        public string TextArea(string name, string value) {
            return TextArea(name, value, TextAreaRows, TextAreaColumns, null);
        }

        public string TextArea(string name, object htmlAttributes) {
            return TextArea(name, new RouteValueDictionary(htmlAttributes));
        }

        public string TextArea(string name, IDictionary<string, object> htmlAttributes) {
            return TextAreaHelper(name, true /* useViewData */, null /* value */, TextAreaRows, TextAreaColumns, htmlAttributes);
        }

        public string TextArea(string name, string value, object htmlAttributes) {
            return TextArea(name, value, TextAreaRows, TextAreaColumns, new RouteValueDictionary(htmlAttributes));
        }

        public string TextArea(string name, string value, IDictionary<string, object> htmlAttributes) {
            return TextArea(name, value, TextAreaRows, TextAreaColumns, htmlAttributes);
        }

        public string TextArea(string name, string value, int rows, int columns, object htmlAttributes) {
            return TextArea(name, value, rows, columns, new RouteValueDictionary(htmlAttributes));
        }

        public string TextArea(string name, string value, int rows, int columns, IDictionary<string, object> htmlAttributes) {
            return TextAreaHelper(name, false /* useViewData */, value, rows, columns, htmlAttributes);
        }
    }
}
