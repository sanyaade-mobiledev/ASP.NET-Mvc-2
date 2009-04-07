namespace Microsoft.Web.Preview.UI.Controls.WebParts {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.UI;
    using Microsoft.Web.Preview.Resources;

    public class WebPartZone : System.Web.UI.WebControls.WebParts.WebPartZone, IScriptControl, IWebPartZone {
        public WebPartZone() {
        }

        private Page BasePage {
            get {
                Page page = Page;
                if (page == null) {
                    throw new InvalidOperationException(PreviewWeb.Common_PageCannotBeNull);
                }
                return page;
            }
        }

        private System.Web.UI.WebControls.WebParts.WebPartManager BaseWebPartManager {
            get {
                System.Web.UI.WebControls.WebParts.WebPartManager webPartManager = base.WebPartManager;
                if (webPartManager == null) {
                    throw new InvalidOperationException(
                        "You must enable Web Parts by adding a WebPartManager to your page.  " +
                        "The WebPartManager must be placed before any Web Part controls on the page.");
                }
                return webPartManager;
            }
        }

        public override System.Web.UI.WebControls.WebParts.WebPartVerbRenderMode WebPartVerbRenderMode {
            get {
                if (DesignMode) {
                    return base.WebPartVerbRenderMode;
                }

                bool browserSupportsMenu =
                    (BasePage.Request.Browser != null) &&
                    BasePage.Request.Browser.Win32 &&
                    (BasePage.Request.Browser.MSDomVersion.CompareTo(new Version(5, 5)) >= 0);

                if (browserSupportsMenu) {
                    return base.WebPartVerbRenderMode;
                }
                else {
                    return System.Web.UI.WebControls.WebParts.WebPartVerbRenderMode.TitleBar;
                }
            }
            set {
                base.WebPartVerbRenderMode = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Matches IScriptControl interface.")]
        protected virtual IEnumerable<ScriptDescriptor> GetScriptDescriptors() {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Sys.Preview.UI.Controls.WebParts.WebPartZone", ClientID);
            descriptor.AddProperty("uniqueId", UniqueID);
            descriptor.AddComponentProperty("webPartManager", BaseWebPartManager.ClientID);
            descriptor.AddProperty("allowLayoutChange", AllowLayoutChange);
            yield return descriptor;

            AtlasWebPartChrome chrome = new AtlasWebPartChrome(this, BaseWebPartManager);

            System.Web.UI.WebControls.WebParts.WebPartCollection webParts = WebParts;
            for (int i = 0; i < webParts.Count; i++) {
                System.Web.UI.WebControls.WebParts.WebPart webPart = webParts[i];
                ScriptControlDescriptor wpDescriptor = new ScriptControlDescriptor("Sys.Preview.UI.Controls.WebParts.WebPart", chrome.GetWebPartChromeClientID(webPart));

                // Only render titleElement in xml-script if the titleElement was actually rendered
                // in the HTML by WebPartChrome.
                System.Web.UI.WebControls.WebParts.PartChromeType effectiveChromeType =
                    GetEffectiveChromeType(webPart);
                if (effectiveChromeType == System.Web.UI.WebControls.WebParts.PartChromeType.TitleOnly ||
                    effectiveChromeType == System.Web.UI.WebControls.WebParts.PartChromeType.TitleAndBorder) {
                    wpDescriptor.AddElementProperty("titleElement", chrome.GetWebPartTitleClientID(webPart));
                }

                wpDescriptor.AddComponentProperty("zone", ClientID);
                wpDescriptor.AddProperty("zoneIndex", i.ToString());
                wpDescriptor.AddProperty("allowZoneChange", webPart.AllowZoneChange);
                yield return wpDescriptor;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Matches IScriptControl interface.")]
        protected virtual IEnumerable<ScriptReference> GetScriptReferences() {
            yield return new ScriptReference(FrameworkScript.PreviewMainScriptResource, FrameworkScript.PreviewAssembly);
            yield return new ScriptReference(FrameworkScript.PreviewDragDropScriptResource, FrameworkScript.PreviewAssembly);
            yield return new ScriptReference(FrameworkScript.PreviewWebPartsScriptResource, FrameworkScript.PreviewAssembly);
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);
            ScriptManager scriptManager = ScriptManager.GetCurrent(BasePage);
            if (scriptManager != null) {
                scriptManager.RegisterScriptControl(this);
            }
        }

        protected override void Render(HtmlTextWriter writer) {
            base.Render(writer);
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null) {
                scriptManager.RegisterScriptDescriptors(this);
            }
        }

        protected override void RenderDropCue(System.Web.UI.HtmlTextWriter writer) {
            base.RenderDropCue(new FixPaddingHtmlTextWriter(writer));
        }

        #region IScriptControl Members
        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors() {
            return GetScriptDescriptors();
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences() {
            return GetScriptReferences();
        }
        #endregion

        private class AtlasWebPartChrome : System.Web.UI.WebControls.WebParts.WebPartChrome {
            public AtlasWebPartChrome(System.Web.UI.WebControls.WebParts.WebPartZoneBase zone,
                                 System.Web.UI.WebControls.WebParts.WebPartManager manager)
                : base(zone, manager) {
            }

            public new string GetWebPartChromeClientID(System.Web.UI.WebControls.WebParts.WebPart webPart) {
                return base.GetWebPartChromeClientID(webPart);
            }

            public new string GetWebPartTitleClientID(System.Web.UI.WebControls.WebParts.WebPart webPart) {
                return base.GetWebPartTitleClientID(webPart);
            }
        }

        // Fix FireFox bug in WebPartZoneBase.cs
        private class FixPaddingHtmlTextWriter : System.Web.UI.HtmlTextWriter {
            public FixPaddingHtmlTextWriter(TextWriter writer)
                : base(writer) {
            }

            public override void AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle key, string value) {
                if ((key == System.Web.UI.HtmlTextWriterStyle.PaddingBottom ||
                     key == System.Web.UI.HtmlTextWriterStyle.PaddingLeft ||
                     key == System.Web.UI.HtmlTextWriterStyle.PaddingRight ||
                     key == System.Web.UI.HtmlTextWriterStyle.PaddingTop) &&
                    (value == "1")) {
                    base.AddStyleAttribute(key, "1px");
                }
                else {
                    base.AddStyleAttribute(key, value);
                }
            }
        }
    }
}
