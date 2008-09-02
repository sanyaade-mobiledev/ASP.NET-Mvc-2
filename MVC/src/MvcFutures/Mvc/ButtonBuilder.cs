namespace Microsoft.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ButtonBuilder {
        /// <summary>
        /// Creates a form Submit Button
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="buttonText">Text for the button</param>
        /// <param name="htmlAttributes">Dictionary of settings</param>
        /// <returns>System.String</returns>
        public static TagBuilder SubmitButton(string name, string buttonText, IDictionary<string, object> htmlAttributes) {
            TagBuilder buttonTag = new TagBuilder("input");
            
            buttonTag.MergeAttribute("type", "submit");
            if (!buttonTag.Attributes.ContainsKey("id") && name != null) {
                buttonTag.MergeAttribute("id", name);
            }

            if (!String.IsNullOrEmpty(name)) {
                buttonTag.MergeAttribute("name", name);
            }

            if (!String.IsNullOrEmpty(buttonText)) {
                buttonTag.MergeAttribute("value", buttonText);
            }
            
            buttonTag.MergeAttributes(htmlAttributes, true);
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
        public static TagBuilder SubmitImage(string name, string sourceUrl, IDictionary<string, object> htmlAttributes) {
            TagBuilder buttonTag = new TagBuilder("input");

            buttonTag.MergeAttribute("type", "image");
            
            if (!buttonTag.Attributes.ContainsKey("id")) {
                buttonTag.MergeAttribute("id", name);
            }

            if (!String.IsNullOrEmpty(name)) {
                buttonTag.MergeAttribute("name", name);
            }

            if (!String.IsNullOrEmpty(sourceUrl)) {
                buttonTag.MergeAttribute("src", sourceUrl);
            }
            buttonTag.MergeAttributes(htmlAttributes, true);
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
        public static TagBuilder Button(string name, string buttonText, string onClickMethod, IDictionary<string, object> htmlAttributes) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            
            TagBuilder buttonTag = new TagBuilder("button");

            if (!String.IsNullOrEmpty(name)) {
                buttonTag.MergeAttribute("name", name);
            }

            if (!String.IsNullOrEmpty(buttonText)) {
                buttonTag.MergeAttribute("value", buttonText);
            }

            
            if (!String.IsNullOrEmpty(onClickMethod)) {
                buttonTag.MergeAttribute("onclick", onClickMethod);
            }

            buttonTag.MergeAttributes(htmlAttributes, true);
            return buttonTag;
        }
    }
}
