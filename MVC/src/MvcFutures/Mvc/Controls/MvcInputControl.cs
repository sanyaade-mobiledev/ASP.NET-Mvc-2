namespace Microsoft.Web.Mvc.Controls {
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class MvcInputControl : MvcControl {
        private string _name;

        protected MvcInputControl(string inputType) {
            if (String.IsNullOrEmpty(inputType)) {
                throw new ArgumentException("Null or empty", "inputType");
            }
            InputType = inputType;
        }

        [Browsable(false)]
        public string InputType {
            get;
            private set;
        }

        [DefaultValue("")]
        public string Name {
            get {
                return _name ?? String.Empty;
            }
            set {
                _name = value;
            }
        }

        protected override void Render(HtmlTextWriter writer) {
            if (!DesignMode && String.IsNullOrEmpty(Name)) {
                throw new InvalidOperationException("The Name property must be specified.");
            }

            foreach (var attribute in Attributes) {
                writer.AddAttribute(attribute.Key, attribute.Value);
            }

            if (!Attributes.ContainsKey("type")) {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, InputType);
            }
            if (!Attributes.ContainsKey("name")) {
                writer.AddAttribute(HtmlTextWriterAttribute.Name, Name);
            }
            if (!Attributes.ContainsKey("id")) {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, Name);
            }

            if (!Attributes.ContainsKey("value")) {
                string value = null;
                if (DesignMode) {
                    value = "Text";
                }
                else {
                    if (ViewData != null) {
                        value = Convert.ToString(ViewData.Eval(Name), CultureInfo.InvariantCulture);
                    }
                }
                if (!String.IsNullOrEmpty(value)) {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, value);
                }
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Input);

            writer.RenderEndTag();
        }
    }
}
