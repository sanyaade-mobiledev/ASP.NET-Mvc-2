namespace Microsoft.Web.Preview.Diagnostics {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web.UI;
    using Microsoft.Web.Preview.Resources;

    [NonVisualControl]
    public class DiagnosticsManager : Control, IScriptControl {
        private ScriptManager _scriptManager;

        public DiagnosticsManager() {
        }

        private ScriptManager ScriptManager {
            get {
                if (_scriptManager == null) {
                    _scriptManager = System.Web.UI.ScriptManager.GetCurrent(this.Page);
                    if (_scriptManager == null) {
                        throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, PreviewWeb.Diagnostics_RequiresScriptManager, GetType().Name));
                    }
                }
                return _scriptManager;
            }
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)
        ]
        public override bool Visible {
            get {
                return true;
            }
            set {
                throw new NotImplementedException();
            }
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);
            ScriptManager.RegisterScriptControl(this);
        }

        protected override void Render(HtmlTextWriter writer) {
            base.Render(writer);

            // ScriptManager cannot be found in DesignMode, so do not attempt to register scripts.
            if (!DesignMode) {
                ScriptManager.RegisterScriptDescriptors(this);
            }
        }

        protected virtual IEnumerable<ScriptDescriptor> GetScriptDescriptors() {
            yield break;
        }

        protected virtual IEnumerable<ScriptReference> GetScriptReferences() {
            if (DiagnosticsService.IsEnabled() == true) {
                yield return new ScriptReference("PreviewDiagnostics.js", typeof(DiagnosticsManager).Assembly.FullName);
            }
            else {
                yield break;
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
