namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public partial class HtmlHelper {
        internal const string ValidationMessageCssClassName = "field-validation-error";
        internal const string ValidationSummaryCssClassName = "validation-summary-errors";
        internal const string ValidationInputCssClassName = "input-validation-error";

        private RouteCollection _routeCollection;

        public HtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) {
            if (viewContext == null) {
                throw new ArgumentNullException("viewContext");
            }
            if (viewDataContainer == null) {
                throw new ArgumentNullException("viewDataContainer");
            }
            ViewContext = viewContext;
            ViewDataContainer = viewDataContainer;
        }

        internal RouteCollection RouteCollection {
            get {
                if (_routeCollection == null) {
                    _routeCollection = RouteTable.Routes;
                }
                return _routeCollection;
            }
            set {
                _routeCollection = value;
            }
        }

        public ViewContext ViewContext {
            get;
            private set;
        }

        protected ViewDataDictionary ViewData {
            get {
                return ViewDataContainer.ViewData;
            }
        }

        public IViewDataContainer ViewDataContainer {
            get;
            private set;
        }

        public string ActionLink(string linkText, string actionName) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            string controllerName = ViewContext.RouteData.GetRequiredString("controller");
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public string ActionLink(string linkText, string actionName, object values) {
            return ActionLink(linkText, actionName, values, null);
        }

        public string ActionLink(string linkText, string actionName, object values, object htmlAttributes) {
            return ActionLink(linkText, actionName, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public string ActionLink(string linkText, string actionName, RouteValueDictionary values) {
            return ActionLink(linkText, actionName, values, new RouteValueDictionary());
        }

        public string ActionLink(string linkText, string actionName, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, null /* controllerName */, new RouteValueDictionary(values), htmlAttributes);
        }

        public string ActionLink(string linkText, string actionName, string controllerName) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public string ActionLink(string linkText, string actionName, string controllerName, object values, object htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public string ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, new RouteValueDictionary(values), htmlAttributes);
        }

        public string ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object values, object htmlAttributes) {
            return ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public string ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(actionName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "actionName");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return GenerateLink(linkText, null /* routeName */, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(values), htmlAttributes);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public string AttributeEncode(string html) {
            return (!String.IsNullOrEmpty(html)) ? HttpUtility.HtmlAttributeEncode(html) : String.Empty;
        }

        public string AttributeEncode(object value) {
            return AttributeEncode(Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "For consistency, all helpers are instance methods.")]
        public string Encode(string html) {
            return (!String.IsNullOrEmpty(html)) ? HttpUtility.HtmlEncode(html) : String.Empty;
        }

        public string Encode(object value) {
            return Encode(Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        private string EvalString(string key) {
            return Convert.ToString(ViewData.Eval(key), CultureInfo.InvariantCulture);
        }

        private bool EvalBoolean(string key) {
            return Convert.ToBoolean(ViewData.Eval(key), CultureInfo.InvariantCulture);
        }

        public IDisposable Form() {
            // generates <form action="{current url}" method="post">...</form>

            TagBuilder tagBuilder = new TagBuilder("form");
            tagBuilder.Attributes["action"] = ViewContext.HttpContext.Request.Url.ToString();
            tagBuilder.Attributes["method"] = "post";

            HttpResponseBase httpResponse = ViewContext.HttpContext.Response;
            httpResponse.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            return new FormDisposable(ViewContext.HttpContext.Response);
        }

        private string GenerateLink(string linkText, string routeName, string actionName, string controllerName, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, values, RouteCollection, ViewContext);
            TagBuilder tagBuilder = new TagBuilder("a") {
                InnerHtml = Encode(linkText)
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private string GenerateLink(string linkText, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            string url = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, values, RouteCollection, ViewContext);
            TagBuilder tagBuilder = new TagBuilder("a") {
                InnerHtml = Encode(linkText)
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private string GetModelAttemptedValue(string key) {
            ModelState modelState;
            if (ViewData.ModelState.TryGetValue(key, out modelState)) {
                return modelState.AttemptedValue;
            }
            return null;
        }

        public string RouteLink(string linkText, object values, object htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            return GenerateLink(linkText, null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public string RouteLink(string linkText, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return GenerateLink(linkText, null /* routeName */, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values), htmlAttributes);
        }

        public string RouteLink(string linkText, string routeName, object htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            return GenerateLink(linkText, routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(), new RouteValueDictionary(htmlAttributes));
        }

        public string RouteLink(string linkText, string routeName, object values, object htmlAttributes) {
            return RouteLink(linkText, routeName, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public string RouteLink(string linkText, string routeName, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return GenerateLink(linkText, routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(values), htmlAttributes);
        }

        public string RouteLink(string linkText, string routeName, string protocol, string hostName, string fragment, object values, object htmlAttributes) {
            return RouteLink(linkText, routeName, protocol, hostName, fragment, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public string RouteLink(string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary values, RouteValueDictionary htmlAttributes) {
            if (String.IsNullOrEmpty(linkText)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "linkText");
            }
            if (String.IsNullOrEmpty(routeName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "routeName");
            }
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            return GenerateLink(linkText, routeName, null /* actionName */, null /* controllerName */, protocol, hostName, fragment, new RouteValueDictionary(values), htmlAttributes);
        }

        public string ValidationMessage(string modelName) {
            return ValidationMessage(modelName, new { @class = ValidationMessageCssClassName });
        }

        public string ValidationMessage(string modelName, object htmlAttributes) {
            return ValidationMessage(modelName, new RouteValueDictionary(htmlAttributes));
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames",
            Justification = "'validationMessage' refers to the message that will be rendered by the ValidationMessage helper.")]
        public string ValidationMessage(string modelName, string validationMessage) {
            return ValidationMessage(modelName, validationMessage, new { @class = ValidationMessageCssClassName });
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames",
            Justification = "'validationMessage' refers to the message that will be rendered by the ValidationMessage helper.")]
        public string ValidationMessage(string modelName, string validationMessage, object htmlAttributes) {
            return ValidationMessage(modelName, validationMessage, new RouteValueDictionary(htmlAttributes));
        }

        public string ValidationMessage(string modelName, IDictionary<string, object> htmlAttributes) {
            return ValidationMessage(modelName, null /* validationMessage */, htmlAttributes);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames",
            Justification = "'validationMessage' refers to the message that will be rendered by the ValidationMessage helper.")]
        public string ValidationMessage(string modelName, string validationMessage, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(modelName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "modelName");
            }

            if (!ViewData.ModelState.ContainsKey(modelName)) {
                return null;
            }

            ModelState modelState = ViewData.ModelState[modelName];
            ModelErrorCollection modelErrors = (modelState == null) ? null : modelState.Errors;
            ModelError modelError = ((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors[0];

            if (modelError == null) {
                return null;
            }

            TagBuilder builder = new TagBuilder("span");
            builder.MergeAttributes(htmlAttributes);
            builder.MergeAttribute("class", ValidationMessageCssClassName);
            builder.SetInnerText(String.IsNullOrEmpty(validationMessage) ? modelError.ErrorMessage : validationMessage);

            return builder.ToString(TagRenderMode.Normal);
        }

        public string ValidationSummary() {
            return ValidationSummary(new { @class = ValidationSummaryCssClassName });
        }

        public string ValidationSummary(object htmlAttributes) {
            return ValidationSummary(new RouteValueDictionary(htmlAttributes));
        }

        public string ValidationSummary(IDictionary<string, object> htmlAttributes) {
            // Nothing to do if there aren't any errors
            if (ViewData.ModelState.IsValid) {
                return null;
            }

            StringBuilder htmlSummary = new StringBuilder();
            TagBuilder unorderedList = new TagBuilder("ul");
            unorderedList.MergeAttributes(htmlAttributes);
            unorderedList.MergeAttribute("class", ValidationSummaryCssClassName);

            foreach (var modelStateKvp in ViewData.ModelState) {
                foreach (var modelError in modelStateKvp.Value.Errors) {
                    TagBuilder listItem = new TagBuilder("li");
                    listItem.SetInnerText(modelError.ErrorMessage);
                    htmlSummary.AppendLine(listItem.ToString(TagRenderMode.Normal));
                }
            }

            unorderedList.InnerHtml = htmlSummary.ToString();

            return unorderedList.ToString(TagRenderMode.Normal);
        }

        private string InputHelper(string inputType, string name, string value, bool useViewData, bool isChecked, bool setId, IDictionary<string, object> htmlAttributes) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }

            bool isRadio = String.Equals("radio", inputType, StringComparison.OrdinalIgnoreCase);
            bool isCheckBox = String.Equals("checkbox", inputType, StringComparison.OrdinalIgnoreCase);

            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", inputType);
            tagBuilder.MergeAttribute("name", name);

            string attemptedValue = GetModelAttemptedValue(name);

            if (isCheckBox) {
                // Helpers that take isChecked as parameter should never look at ViewData
                if (useViewData) {
                    isChecked = EvalBoolean(name);
                }
                tagBuilder.MergeAttribute("value", value);
            }
            else {
                tagBuilder.MergeAttribute("value", attemptedValue ?? ((useViewData) ? EvalString(name) : value));
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
            if (ViewData.ModelState.TryGetValue(name, out modelState)) {
                if (modelState.Errors.Count > 0) {
                    tagBuilder.AddCssClass(ValidationInputCssClassName);
                }
            }

            if (isCheckBox) {
                // Render an additional <input type="hidden".../> for checkboxes. This
                // addresses scenarios where unchecked checkboxes are not sent in the request.
                // Sending a hidden input makes it possible to know that the checkbox was present
                // on the page when the request was submitted.
                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.AppendLine(tagBuilder.ToString(TagRenderMode.SelfClosing));
                inputItemBuilder.AppendLine(InputHelper("hidden", name, "false", false /* useViewData */, isChecked, false /* setId */, htmlAttributes));
                return inputItemBuilder.ToString();
            }

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private string TextAreaHelper(string name, bool useViewData, string value, int rows, int columns, IDictionary<string, object> htmlAttributes) {
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
            ModelState modelState;
            string attemptedValue = null;
            if (ViewData.ModelState.TryGetValue(name, out modelState)) {
                if (modelState.Errors.Count > 0) {
                    attemptedValue = modelState.AttemptedValue;
                    tagBuilder.AddCssClass(ValidationInputCssClassName);
                }
            }

            tagBuilder.SetInnerText(attemptedValue ?? ((useViewData) ? EvalString(name) : value));
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private sealed class FormDisposable : IDisposable {

            private bool _disposed;
            private readonly HttpResponseBase _httpResponse;

            public FormDisposable(HttpResponseBase httpResponse) {
                _httpResponse = httpResponse;
            }

            public void Dispose() {
                if (!_disposed) {
                    _disposed = true;
                    _httpResponse.Write("</form>");
                }
            }
        }

    }
}
