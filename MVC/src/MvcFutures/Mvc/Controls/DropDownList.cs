namespace Microsoft.Web.Mvc.Controls {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    // TODO: Have ListBoxBase class to use with DropDownList and ListBox?
    // TODO: Do we need a way to explicitly specify the items? And only get the selected value(s) from ViewData?

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DropDownList : MvcControl {
        private string _name;

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

            if (!Attributes.ContainsKey("name")) {
                if (!String.IsNullOrEmpty(Name)) {
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, Name);
                }
            }
            if (!Attributes.ContainsKey("id")) {
                if (!String.IsNullOrEmpty(Name)) {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, Name);
                }
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Select);

            if (DesignMode) {
                writer.RenderBeginTag(HtmlTextWriterTag.Option);
                writer.WriteEncodedText("Sample Item");
                writer.RenderEndTag();
            }
            else {
                IEnumerable<SelectListItem> items = ListBoxHelper.GetSelectData<SelectList>(ViewData, Name);

                foreach (var listItem in items) {
                    if (!String.IsNullOrEmpty(listItem.Value)) {
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, listItem.Value);
                    }
                    if (listItem.Selected) {
                        writer.AddAttribute(HtmlTextWriterAttribute.Selected, "selected");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Option);
                    writer.WriteEncodedText(listItem.Text);
                    writer.RenderEndTag();
                }
            }

            writer.RenderEndTag();
        }
    }
}
