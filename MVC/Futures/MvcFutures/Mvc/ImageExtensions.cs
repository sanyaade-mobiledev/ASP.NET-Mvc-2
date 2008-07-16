namespace Microsoft.Web.Mvc {
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System;
    using System.Collections.Generic;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ImageExtensions {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "The return value is not a regular URL since it may contain ~/ ASP.NET-specific characters")]
        public static string Image(this HtmlHelper helper, string imageRelativeUrl) {
            return Image(helper, imageRelativeUrl, null, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Required for Extension Method")]
        public static string Image(this HtmlHelper helper, string imageRelativeUrl, string alt) {
            return Image(helper, imageRelativeUrl, alt, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "The return value is not a regular URL since it may contain ~/ ASP.NET-specific characters")]
        public static string Image(this HtmlHelper helper, string imageRelativeUrl, string alt, object htmlAttributes) {
            return Image(helper, imageRelativeUrl, alt, TagBuilder.ToDictionary(htmlAttributes));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "The return value is not a regular URL since it may contain ~/ ASP.NET-specific characters")]
        public static string Image(this HtmlHelper helper, string imageRelativeUrl, object htmlAttributes) {
            return Image(helper, imageRelativeUrl, null, TagBuilder.ToDictionary(htmlAttributes));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "The return value is not a regular URL since it may contain ~/ ASP.NET-specific characters")]
        public static string Image(this HtmlHelper helper, string imageRelativeUrl, IDictionary<string, object> htmlAttributes) {
            return Image(helper, imageRelativeUrl, null, htmlAttributes);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "The return value is not a regular URL since it may contain ~/ ASP.NET-specific characters")]
        public static string Image(this HtmlHelper helper, string imageRelativeUrl, string alt, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(imageRelativeUrl)) {
                throw new ArgumentNullException("imageRelativeUrl"); // TODO: Use resource for message.
            }

            UrlHelper url = new UrlHelper(helper.ViewContext);
            string imageUrl = url.Content(imageRelativeUrl);
            return Image(imageUrl, alt, htmlAttributes).ToString();
        }

        public static IHtmlElement Image(string imageUrl, string alt, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(imageUrl)) {
                throw new ArgumentNullException("imageRelativeUrl"); // TODO: Use resource message
            }

            TagBuilder imageTag = new TagBuilder("img");
            IDictionary<string, string> attributes = TagBuilder.ToStringDictionary(htmlAttributes);

            if (!String.IsNullOrEmpty(imageUrl)) {
                TagBuilder.TryAddValue(attributes, "src", imageUrl);
            }

            if (!String.IsNullOrEmpty(alt)) {
                TagBuilder.TryAddValue(attributes, "alt", alt);
            }

            imageTag.Attributes = attributes;
            return imageTag;
        }
    }
}
