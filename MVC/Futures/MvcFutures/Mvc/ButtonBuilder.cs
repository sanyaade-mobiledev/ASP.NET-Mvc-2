namespace Microsoft.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ButtonBuilder {
        /// <summary>
        /// Creates a form Submit Button
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="buttonText">Text for the button</param>
        /// <param name="htmlAttributes">Dictionary of settings</param>
        /// <returns>System.String</returns>
        public static IHtmlElement SubmitButton(string name, string buttonText, IDictionary<string, object> htmlAttributes) {
            TagBuilder buttonTag = new TagBuilder("input");
            IDictionary<string, string> attributes = TagBuilder.ToStringDictionary(htmlAttributes);

            TagBuilder.TryAddValue(attributes, "type", "submit");

            if (!attributes.ContainsKey("id") && name != null) {
                TagBuilder.TryAddValue(attributes, "id", name);
            }

            if (!String.IsNullOrEmpty(name)) {
                TagBuilder.TryAddValue(attributes, "name", name);
            }

            if (!String.IsNullOrEmpty(buttonText)) {
                TagBuilder.TryAddValue(attributes, "value", buttonText);
            }
            
            buttonTag.Attributes = attributes;
            return buttonTag;
        }

        /// <summary>
        /// Creates a form Submit Image
        /// </summary>
        /// <param name="htmlName">Name of the button</param>
        /// <param name="sourcUrl">The URL of the image to use (relative)</param>
        /// <param name="htmlAttributes">Dictionary of settings</param>
        /// <returns>System.String</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "The return value is not a regular URL since it may contain ~/ ASP.NET-specific characters")]
        public static IHtmlElement SubmitImage(string name, string sourceUrl, IDictionary<string, object> htmlAttributes) {
            TagBuilder buttonTag = new TagBuilder("input");
            IDictionary<string, string> attributes = TagBuilder.ToStringDictionary(htmlAttributes);

            TagBuilder.TryAddValue(attributes, "type", "image");

            if (!attributes.ContainsKey("id")) {
                TagBuilder.TryAddValue(attributes, "id", name);
            }

            if (!String.IsNullOrEmpty(name)) {
                TagBuilder.TryAddValue(attributes, "name", name);
            }

            if (!String.IsNullOrEmpty(sourceUrl)) {
                TagBuilder.TryAddValue(attributes, "src", sourceUrl);
            }

            buttonTag.Attributes = attributes;
            return buttonTag;
        }

        /// <summary>
        /// Creates a Button
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="buttonText">Text for the button</param>
        /// <param name="onClickMethod">The javascript method to call when the button is clicked</param>
        /// <param name="htmlAttributes">Dictionary of settings</param>
        /// <returns>System.String</returns>
        public static IHtmlElement Button(string name, string buttonText, string onClickMethod, IDictionary<string, object> htmlAttributes) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            
            TagBuilder buttonTag = new TagBuilder("button");
            IDictionary<string, string> attributes = TagBuilder.ToStringDictionary(htmlAttributes);

            if (!String.IsNullOrEmpty(name)) {
                TagBuilder.TryAddValue(attributes, "name", name);
            }

            if (!String.IsNullOrEmpty(buttonText)) {
                TagBuilder.TryAddValue(attributes, "value", buttonText);
            }

            
            if (!String.IsNullOrEmpty(onClickMethod)) {
                TagBuilder.TryAddValue(attributes, "onclick", onClickMethod);
            }

            buttonTag.Attributes = attributes;
            return buttonTag;
        }
    }
}
