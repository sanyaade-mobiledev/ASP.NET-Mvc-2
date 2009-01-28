namespace Microsoft.Web.Mvc.Controls {
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class Label : MvcControl {
        private string _name;
        private int _truncateLength = -1;
        private string _truncateText = "...";

        [DefaultValue(EncodeType.Html)]
        public EncodeType EncodeType {
            get;
            set;
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

        [DefaultValue(-1)]
        [Description("The length of the text at which to truncate the value. Set to -1 to never truncate.")]
        public int TruncateLength {
            get {
                return _truncateLength;
            }
            set {
                if (value < -1) {
                    throw new ArgumentOutOfRangeException("value", "The TruncateLength property must be greater than or equal to -1.");
                }
                _truncateLength = value;
            }
        }

        [DefaultValue("...")]
        [Description("The text to display at the end of the string if it is truncated. This text is never encoded.")]
        public string TruncateText {
            get {
                return _truncateText ?? String.Empty;
            }
            set {
                _truncateText = value;
            }
        }

        protected override void Render(HtmlTextWriter writer) {
            if (!DesignMode && String.IsNullOrEmpty(Name)) {
                throw new InvalidOperationException("The Name property must be specified.");
            }

            string value = String.Empty;
            if (ViewData != null) {
                value = Convert.ToString(ViewData.Eval(Name), CultureInfo.InvariantCulture);
            }

            if (!Attributes.ContainsKey("name")) {
                writer.AddAttribute(HtmlTextWriterAttribute.Name, Name);
            }
            if (!Attributes.ContainsKey("id")) {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, Name);
            }

            bool wasTruncated = false;
            if ((TruncateLength >= 0) && (value.Length > TruncateLength)) {
                value = value.Substring(0, TruncateLength);
                wasTruncated = true;
            }

            switch (EncodeType) {
                case EncodeType.Html:
                    writer.Write(HttpUtility.HtmlEncode(value));
                    break;
                case EncodeType.HtmlAttribute:
                    writer.Write(HttpUtility.HtmlAttributeEncode(value));
                    break;
                case EncodeType.None:
                    writer.Write(value);
                    break;
            }

            if (wasTruncated) {
                writer.Write(TruncateText);
            }
        }
    }
}
