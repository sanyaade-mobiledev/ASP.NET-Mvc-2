namespace Microsoft.Web.Mvc {
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class RadioListExtensions {
        /// <summary>
        /// Creates an HTML Radio button list based on the passed-in datasource.
        /// </summary>
        /// <param name="dataSource">IEnumerable, IQueryable, DataSet, DataTable, or IDataReader</param>
        /// <param name="htmlName">The name of the control for the page</param>
        public static string[] RadioButtonList(this HtmlHelper helper, string htmlName, object dataSource) {
            return RadioButtonList(helper, htmlName, dataSource, "", "", null, null);
        }

        /// <summary>
        /// Creates an HTML Radio button list based on the passed-in datasource.
        /// </summary>
        /// <param name="dataSource">IEnumerable, IQueryable, DataSet, DataTable, or IDataReader</param>
        /// <param name="htmlName">The name of the control for the page</param>
        /// <param name="textField">The datasource field to use for the the display text</param>
        /// <param name="valueField">The datasource field to use for the the control value</param>
        /// <param name="selectedValue">The value that should be selected</param>
        /// <param name="htmlAttributes">Any attributes you want set on the tag. Use anonymous-type declaration for this: new{class=cssclass}</param>
        public static string[] RadioButtonList(this HtmlHelper helper, string htmlName, object dataSource, object selectedValue) {
            return RadioButtonList(helper, htmlName, dataSource, "", "", selectedValue, null);
        }

        public static string[] RadioButtonList(this HtmlHelper helper, string htmlName, object dataSource, string textField, string valueField) {
            return RadioButtonList(helper, htmlName, dataSource, textField, valueField, null, null);
        }

        public static string[] RadioButtonList(this HtmlHelper helper, string htmlName, object dataSource, string textField, string valueField, object selectedValue) {
            return RadioButtonList(helper, htmlName, dataSource, textField, valueField, selectedValue, null);
        }

        /// <summary>
        /// Creates an HTML Radio button list based on the passed-in datasource.
        /// </summary>
        /// <param name="dataSource">IEnumerable, IQueryable, DataSet, DataTable, or IDataReader</param>
        /// <param name="htmlName">The name of the control for the page</param>
        /// <param name="textField">The datasource field to use for the the display text</param>
        /// <param name="valueField">The datasource field to use for the the control value</param>
        /// <param name="selectedValue">The value that should be selected</param>
        /// <param name="htmlAttributes">Any attributes you want set on the tag. Use anonymous-type declaration for this: new{class=cssclass}</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "helper", Justification = "Required for Extension Method")]
        public static string[] RadioButtonList(this HtmlHelper helper, string htmlName, object dataSource, string textField, string valueField, object selectedValue, object htmlAttributes) {
            return RadioBuilder.RadioButtonList(htmlName, dataSource, textField, valueField, selectedValue, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Creates an HTML Radio button list based on the passed-in datasource.
        /// </summary>
        /// <param name="dataSource">IEnumerable, IQueryable, DataSet, DataTable, or IDataReader</param>
        /// <param name="htmlName">The name of the control for the page</param>
        /// <param name="textField">The datasource field to use for the the display text</param>
        /// <param name="valueField">The datasource field to use for the the control value</param>
        /// <param name="selectedValue">The value that should be selected</param>
        /// <param name="htmlAttributes">Dictionary of HTML Attribute settings</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "helper", Justification = "Required for Extension Method")]
        public static string[] RadioButtonList(this HtmlHelper helper, string htmlName, object dataSource, string textField, string valueField, object selectedValue, IDictionary<string, object> htmlAttributes) {
            return RadioBuilder.RadioButtonList(htmlName, dataSource, textField, valueField, selectedValue, (htmlAttributes == null) ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes));
        }
    }
}
