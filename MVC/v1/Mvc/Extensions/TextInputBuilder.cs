namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class TextInputBuilder {
        public static string TextArea(string htmlName, object value, int rows, int cols, RouteValueDictionary htmlAttributes) {

            if (rows == 0)
                rows = 12;
            if (cols == 0)
                cols = 58;

            value = value ?? string.Empty;

            htmlAttributes = htmlAttributes ?? new RouteValueDictionary();
            htmlAttributes.Add("value", HttpUtility.HtmlEncode(value.ToString()));

            htmlAttributes.Add("rows", rows);
            htmlAttributes.Add("cols", cols);

            return TextArea(htmlName, htmlAttributes);
        }

        public static string TextArea(string htmlName, RouteValueDictionary htmlAttributes) {
            htmlAttributes = htmlAttributes ?? new RouteValueDictionary();
            string textAreaTag = TagBuilder2.CreateTag(HtmlTagType.TextArea, htmlName, htmlAttributes);
            return textAreaTag;
        }
    }
}
