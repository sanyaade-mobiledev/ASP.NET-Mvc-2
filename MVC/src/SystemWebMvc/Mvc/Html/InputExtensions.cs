namespace System.Web.Mvc.Html {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class InputExtensions {
        public static string CheckBox(this HtmlHelper htmlHelper, string name) {
            return CheckBox(htmlHelper, name, (object)null /* htmlAttributes */);
        }

        public static string CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked) {
            return CheckBox(htmlHelper, name, isChecked, (object)null /* htmlAttributes */);
        }

        public static string CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, object htmlAttributes) {
            return CheckBox(htmlHelper, name, isChecked, new RouteValueDictionary(htmlAttributes));
        }

        public static string CheckBox(this HtmlHelper htmlHelper, string name, object htmlAttributes) {
            return CheckBox(htmlHelper, name, new RouteValueDictionary(htmlAttributes));
        }

        public static string CheckBox(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes) {
            return htmlHelper.InputHelper("checkbox", name, "true", true /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }

        public static string CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, IDictionary<string, object> htmlAttributes) {
            return htmlHelper.InputHelper("checkbox", name, "true", false /* useViewData */, isChecked, true /* setId */, htmlAttributes);
        }

        public static string Hidden(this HtmlHelper htmlHelper, string name) {
            return Hidden(htmlHelper, name, null /* value */);
        }

        public static string Hidden(this HtmlHelper htmlHelper, string name, object value) {
            return Hidden(htmlHelper, name, value, (object)null /* hmtlAttributes */);
        }

        public static string Hidden(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes) {
            return Hidden(htmlHelper, name, value, new RouteValueDictionary(htmlAttributes));
        }

        public static string Hidden(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes) {
            return InputHelper(htmlHelper, "hidden", name, value, (value == null) /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }

        public static string Password(this HtmlHelper htmlHelper, string name) {
            return Password(htmlHelper, name, null /* value */);
        }

        public static string Password(this HtmlHelper htmlHelper, string name, object value) {
            return Password(htmlHelper, name, value, (object)null /* htmlAttributes */);
        }

        public static string Password(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes) {
            return Password(htmlHelper, name, value, new RouteValueDictionary(htmlAttributes));
        }

        public static string Password(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes) {
            return InputHelper(htmlHelper, "password", name, value, (value == null) /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value) {
            return RadioButton(htmlHelper, name, value, (object)null /* htmlAttributes */);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes) {
            return RadioButton(htmlHelper, name, value, new RouteValueDictionary(htmlAttributes));
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes) {
            // Determine whether or not to render the checked attribute based on the contents of ViewData.
            string valueString = Convert.ToString(value, CultureInfo.CurrentUICulture);
            bool isChecked = (!String.IsNullOrEmpty(name)) && (String.Equals(htmlHelper.EvalString(name), valueString, StringComparison.OrdinalIgnoreCase));
            return RadioButton(htmlHelper, name, value, isChecked, htmlAttributes);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked) {
            return RadioButton(htmlHelper, name, value, isChecked, (object)null /* htmlAttributes */);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked, object htmlAttributes) {
            return RadioButton(htmlHelper, name, value, isChecked, new RouteValueDictionary(htmlAttributes));
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }
            if (value == null) {
                throw new ArgumentNullException("value");
            }
            return htmlHelper.InputHelper("radio", name, Convert.ToString(value, CultureInfo.CurrentUICulture), false, isChecked, true, htmlAttributes);
        }

        public static string TextBox(this HtmlHelper htmlHelper, string name) {
            return TextBox(htmlHelper, name, null /* value */);
        }

        public static string TextBox(this HtmlHelper htmlHelper, string name, object value) {
            return TextBox(htmlHelper, name, value, (object)null /* htmlAttributes */);
        }

        public static string TextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes) {
            return TextBox(htmlHelper, name, value, new RouteValueDictionary(htmlAttributes));
        }

        public static string TextBox(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes) {
            return InputHelper(htmlHelper, "text", name, value, (value == null) /* useViewData */, false /* isChecked */, true /* setId */, htmlAttributes);
        }

        private static string InputHelper(this HtmlHelper htmlHelper, string inputType, string name, object value, bool useViewData, bool isChecked, bool setId, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }

            bool isRadio = String.Equals("radio", inputType, StringComparison.OrdinalIgnoreCase);
            bool isCheckBox = String.Equals("checkbox", inputType, StringComparison.OrdinalIgnoreCase);

            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", inputType);
            tagBuilder.MergeAttribute("name", name);

            string attemptedValue = htmlHelper.GetModelAttemptedValue(name);

            if (isCheckBox) {
                // Helpers that take isChecked as parameter should never look at ViewData
                if (useViewData) {
                    isChecked = htmlHelper.EvalBoolean(name);
                }
                tagBuilder.MergeAttribute("value", Convert.ToString(value, CultureInfo.CurrentUICulture));
            }
            else {
                tagBuilder.MergeAttribute("value", attemptedValue ?? ((useViewData) ? htmlHelper.EvalString(name) : Convert.ToString(value, CultureInfo.CurrentUICulture)));
            }

            if (setId) {
                tagBuilder.MergeAttribute("id", name);
            }

            // Add attributes common to radio and checkbox
            if ((isRadio || isCheckBox) && (isChecked)) {
                tagBuilder.MergeAttribute("checked", "checked");
            }

            // If there are any errors for a named field, we add the css attribute.
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState)) {
                if (modelState.Errors.Count > 0) {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            if (isCheckBox) {
                // Render an additional <input type="hidden".../> for checkboxes. This
                // addresses scenarios where unchecked checkboxes are not sent in the request.
                // Sending a hidden input makes it possible to know that the checkbox was present
                // on the page when the request was submitted.
                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.SelfClosing));

                TagBuilder hiddenInput = new TagBuilder("input");
                hiddenInput.MergeAttributes(htmlAttributes);
                hiddenInput.MergeAttribute("type", "hidden");
                hiddenInput.MergeAttribute("name", name);
                hiddenInput.MergeAttribute("value", "false");
                inputItemBuilder.AppendLine(hiddenInput.ToString(TagRenderMode.SelfClosing));
                return inputItemBuilder.ToString();
            }

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }
    }
}
