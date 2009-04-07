namespace Microsoft.Web.Preview.UI.Controls.WebParts {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Web.UI;
    using Microsoft.Web.Preview.Resources;

    public class WebPartManager : System.Web.UI.WebControls.WebParts.WebPartManager, IScriptControl {
        private ScriptManager _scriptManager;

        public WebPartManager() {
        }

        internal WebPartManager(ScriptManager scriptManager) {
            Debug.Assert(scriptManager != null);
            _scriptManager = scriptManager;
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

        private ScriptManager ScriptManager {
            get {
                if (_scriptManager == null) {
                    _scriptManager = System.Web.UI.ScriptManager.GetCurrent(BasePage);
                    if (_scriptManager == null) {
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                            PreviewWeb.Common_ScriptManagerRequired, ID));
                    }
                }
                return _scriptManager;
            }
        }

        protected override bool CheckRenderClientScript() {
            // Atlas does not render differently on downlevel browsers.  Therefore, RenderClientScript
            // does not need to check the browser capabilities.
            return EnableClientScript;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Matches IScriptControl interface.")]
        protected virtual IEnumerable<ScriptDescriptor> GetScriptDescriptors() {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Sys.Preview.UI.Controls.WebParts.WebPartManager", ClientID);
            descriptor.AddProperty("allowPageDesign", DisplayMode.AllowPageDesign);
            yield return descriptor;
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

        protected override void RegisterClientScript() {
            // WebParts.js only supports IE; Registering before base
            // to replace script with WebParts substitution.
            Type type = typeof(WebPartManager);
            String url = BasePage.ClientScript.GetWebResourceUrl(type, "WebParts.js");
            type = typeof(System.Web.UI.WebControls.WebParts.WebPartManager);

            ScriptManager.RegisterClientScriptInclude(this, type, "WebParts.js", url); // replaces "WebParts.js" key.
            base.RegisterClientScript();
        }

        protected override void Render(HtmlTextWriter writer) {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null) {
                scriptManager.RegisterScriptDescriptors(this);
            }
        }

        #region IScriptControl Members
        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors() {
            return GetScriptDescriptors();
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences() {
            return GetScriptReferences();
        }
        #endregion
    }
}
