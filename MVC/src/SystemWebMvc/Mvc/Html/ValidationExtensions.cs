namespace System.Web.Mvc.Html {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ValidationExtensions {

        private static string GetUserErrorMessageOrDefault(ModelError error, ModelState modelState) {
            if (!String.IsNullOrEmpty(error.ErrorMessage)) {
                return error.ErrorMessage;
            }
            if (modelState == null) {
                return null;
            }

            string attemptedValue = (modelState.Value != null) ? modelState.Value.AttemptedValue : null;
            return String.Format(CultureInfo.CurrentCulture, MvcResources.Common_ValueNotValidForProperty, attemptedValue);
        }

        public static string ValidationMessage(this HtmlHelper htmlHelper, string modelName) {
            return ValidationMessage(htmlHelper, modelName, (object)null /* htmlAttributes */);
        }

        public static string ValidationMessage(this HtmlHelper htmlHelper, string modelName, object htmlAttributes) {
            return ValidationMessage(htmlHelper, modelName, new RouteValueDictionary(htmlAttributes));
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames",
            Justification = "'validationMessage' refers to the message that will be rendered by the ValidationMessage helper.")]
        public static string ValidationMessage(this HtmlHelper htmlHelper, string modelName, string validationMessage) {
            return ValidationMessage(htmlHelper, modelName, validationMessage, (object)null /* htmlAttributes */);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames",
            Justification = "'validationMessage' refers to the message that will be rendered by the ValidationMessage helper.")]
        public static string ValidationMessage(this HtmlHelper htmlHelper, string modelName, string validationMessage, object htmlAttributes) {
            return ValidationMessage(htmlHelper, modelName, validationMessage, new RouteValueDictionary(htmlAttributes));
        }

        public static string ValidationMessage(this HtmlHelper htmlHelper, string modelName, IDictionary<string, object> htmlAttributes) {
            return ValidationMessage(htmlHelper, modelName, null /* validationMessage */, htmlAttributes);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames",
            Justification = "'validationMessage' refers to the message that will be rendered by the ValidationMessage helper.")]
        public static string ValidationMessage(this HtmlHelper htmlHelper, string modelName, string validationMessage, IDictionary<string, object> htmlAttributes) {
            if (modelName == null) {
                throw new ArgumentNullException("modelName");
            }

            if (!htmlHelper.ViewData.ModelState.ContainsKey(modelName)) {
                return null;
            }

            ModelState modelState = htmlHelper.ViewData.ModelState[modelName];
            ModelErrorCollection modelErrors = (modelState == null) ? null : modelState.Errors;
            ModelError modelError = ((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors[0];

            if (modelError == null) {
                return null;
            }

            TagBuilder builder = new TagBuilder("span");
            builder.MergeAttributes(htmlAttributes);
            builder.MergeAttribute("class", HtmlHelper.ValidationMessageCssClassName);
            builder.SetInnerText(String.IsNullOrEmpty(validationMessage) ? GetUserErrorMessageOrDefault(modelError, modelState) : validationMessage);

            return builder.ToString(TagRenderMode.Normal);
        }

        public static string ValidationSummary(this HtmlHelper htmlHelper) {
            return ValidationSummary(htmlHelper, (object)null /* htmlAttributes */);
        }

        public static string ValidationSummary(this HtmlHelper htmlHelper, object htmlAttributes) {
            return ValidationSummary(htmlHelper, new RouteValueDictionary(htmlAttributes));
        }

        public static string ValidationSummary(this HtmlHelper htmlHelper, IDictionary<string, object> htmlAttributes) {
            // Nothing to do if there aren't any errors
            if (htmlHelper.ViewData.ModelState.IsValid) {
                return null;
            }

            StringBuilder htmlSummary = new StringBuilder();
            TagBuilder unorderedList = new TagBuilder("ul");
            unorderedList.MergeAttributes(htmlAttributes);
            unorderedList.MergeAttribute("class", HtmlHelper.ValidationSummaryCssClassName);

            foreach (ModelState modelState in htmlHelper.ViewData.ModelState.Values) {
                foreach (ModelError modelError in modelState.Errors) {
                    string errorText = GetUserErrorMessageOrDefault(modelError, null /* modelState */);
                    if (!String.IsNullOrEmpty(errorText)) {
                        TagBuilder listItem = new TagBuilder("li");
                        listItem.SetInnerText(errorText);
                        htmlSummary.AppendLine(listItem.ToString(TagRenderMode.Normal));
                    }
                }
            }

            unorderedList.InnerHtml = htmlSummary.ToString();

            return unorderedList.ToString(TagRenderMode.Normal);
        }
    }
}
