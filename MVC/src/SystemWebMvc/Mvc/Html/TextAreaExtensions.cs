namespace System.Web.Mvc.Html {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class TextAreaExtensions {
        // These values are similar to the defaults used by WebForms
        // when using <asp:TextBox TextMode="MultiLine"> without specifying
        // the Rows and Columns attributes.
        private const int TextAreaRows = 2;
        private const int TextAreaColumns = 20;

        public static string TextArea(this HtmlHelper htmlHelper, string name) {
            return TextArea(htmlHelper, name, (object)null /* htmlAttributes */);
        }

        public static string TextArea(this HtmlHelper htmlHelper, string name, string value) {
            return TextArea(htmlHelper, name, value, TextAreaRows, TextAreaColumns, null);
        }

        public static string TextArea(this HtmlHelper htmlHelper, string name, object htmlAttributes) {
            return TextArea(htmlHelper, name, new RouteValueDictionary(htmlAttributes));
        }

        public static string TextArea(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes) {
            return TextAreaHelper(htmlHelper, name, true /* useViewData */, null /* value */, TextAreaRows, TextAreaColumns, htmlAttributes);
        }

        public static string TextArea(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes) {
            return TextArea(htmlHelper, name, value, TextAreaRows, TextAreaColumns, new RouteValueDictionary(htmlAttributes));
        }

        public static string TextArea(this HtmlHelper htmlHelper, string name, string value, IDictionary<string, object> htmlAttributes) {
            return TextArea(htmlHelper, name, value, TextAreaRows, TextAreaColumns, htmlAttributes);
        }

        public static string TextArea(this HtmlHelper htmlHelper, string name, string value, int rows, int columns, object htmlAttributes) {
            return TextArea(htmlHelper, name, value, rows, columns, new RouteValueDictionary(htmlAttributes));
        }

        public static string TextArea(this HtmlHelper htmlHelper, string name, string value, int rows, int columns, IDictionary<string, object> htmlAttributes) {
            return TextAreaHelper(htmlHelper, name, (value == null) /* useViewData */, value, rows, columns, htmlAttributes);
        }

        private static string TextAreaHelper(this HtmlHelper htmlHelper, string name, bool useViewData, string value, int rows, int columns, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }
            if (rows <= 0) {
                throw new ArgumentOutOfRangeException("rows", MvcResources.HtmlHelper_TextAreaParameterOutOfRange);
            }
            if (columns <= 0) {
                throw new ArgumentOutOfRangeException("columns", MvcResources.HtmlHelper_TextAreaParameterOutOfRange);
            }

            value = value ?? String.Empty;

            TagBuilder tagBuilder = new TagBuilder("textarea");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("rows", rows.ToString(CultureInfo.InvariantCulture));
            tagBuilder.MergeAttribute("cols", columns.ToString(CultureInfo.InvariantCulture));
            tagBuilder.MergeAttribute("id", name);

            // If there are any errors for a named field, we add the css attribute.
            string attemptedValue = htmlHelper.GetModelAttemptedValue(name);
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState)) {
                if (modelState.Errors.Count > 0) {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            // The first newline is always trimmed when a TextArea is rendered, so we add an extra one
            // in case the value being rendered is something like "\r\nHello".
            tagBuilder.SetInnerText(Environment.NewLine + (attemptedValue ?? ((useViewData) ? htmlHelper.EvalString(name) : value)));
            return tagBuilder.ToString(TagRenderMode.Normal);
        }
    }
}
