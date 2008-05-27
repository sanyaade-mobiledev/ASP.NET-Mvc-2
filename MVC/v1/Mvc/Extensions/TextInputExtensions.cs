namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class TextInputExtensions {
        /// <summary>
        /// Creates a scrollable text box for form input
        /// </summary>
        /// <param name="htmlName">The name and ID of the control</param>
        /// <param name="value">The text for the control</param>
        public static string TextArea(this HtmlHelper helper, string htmlName, object value) {
            //set defaults for rows/cols
            //using Phi :) ratio cols=3*Phi*rows
            return TextArea(helper, htmlName, value, 12, 58, null);
        }

        /// <summary>
        /// Creates a scrollable text box for form input
        /// </summary>
        /// <param name="htmlName">The name and ID of the control</param>
        /// <param name="value">The text for the control</param>
        /// <param name="htmlAttributes">Any attributes you want set on the tag. Use anonymous-type declaration for this: new{class=cssclass}</param>
        public static string TextArea(this HtmlHelper helper, string htmlName, object value, object htmlAttributes) {
            //set defaults for rows/cols
            //using Phi :) ratio
            return TextArea(helper, htmlName, value, 12, 58, htmlAttributes);
        }

        /// <summary>
        /// Creates a scrollable text box for form input
        /// </summary>
        /// <param name="htmlName">The name and ID of the control</param>
        /// <param name="value">The text for the control</param>
        /// <param name="htmlAttributes">Dictionary of HTML Settings</param>
        public static string TextArea(this HtmlHelper helper, string htmlName, object value, IDictionary<string, object> htmlAttributes) {
            //set defaults for rows/cols
            //using Phi :) ratio
            return TextArea(helper, htmlName, value, 12, 58, htmlAttributes);
        }

        /// <summary>
        /// Creates a scrollable text box for form input
        /// </summary>
        /// <param name="htmlName">The name and ID of the control</param>
        /// <param name="value">The text for the control</param>
        /// <param name="rows">The height</param>
        /// <param name="cols">The width</param>
        public static string TextArea(this HtmlHelper helper, string htmlName, object value, int rows, int cols) {
            return TextArea(helper, htmlName, value, rows, cols, null);
        }

        /// <summary>
        /// Creates a scrollable text box for form input
        /// </summary>
        /// <param name="htmlName">The name and ID of the control</param>
        /// <param name="value">The text for the control</param>
        /// <param name="rows">The height</param>
        /// <param name="cols">The width</param>
        /// <param name="maxlength">The input limit for the text area</param>
        /// <param name="htmlAttributes">Any attributes you want set on the tag. Use anonymous-type declaration for this: new{_class=cssclass}</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "maxlength", Justification = "Spelling is appropriate for use (HTML reference)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "helper", Justification = "Required for Extension Method")]
        public static string TextArea(this HtmlHelper helper, string htmlName, object value, int rows, int cols, object htmlAttributes) {
            return TextInputBuilder.TextArea(htmlName, value, rows, cols, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Creates a scrollable text box for form input
        /// </summary>
        /// <param name="htmlName">The name and ID of the control</param>
        /// <param name="value">The text for the control</param>
        /// <param name="rows">The height</param>
        /// <param name="cols">The width</param>
        /// <param name="maxlength">The input limit for the text area</param>
        /// <param name="htmlAttributes">Dictionary of HTML Settings</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "maxlength", Justification = "Spelling is appropriate for use (HTML reference)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "helper", Justification = "Required for Extension Method")]
        public static string TextArea(this HtmlHelper helper, string htmlName, object value, int rows, int cols, IDictionary<string, object> htmlAttributes) {
            return TextInputBuilder.TextArea(htmlName, value, rows, cols, (htmlAttributes == null) ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes));
        }
    }
}
