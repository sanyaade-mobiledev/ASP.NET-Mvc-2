using System;
using System.Web.UI;

namespace ScriptReferenceProfiler {
    internal class OpenScriptReference : ScriptReference {
        public OpenScriptReference(ScriptReference reference) : base() {
            Assembly = reference.Assembly;
            IgnoreScriptPath = reference.IgnoreScriptPath;
            Name = reference.Name;
            NotifyScriptLoaded = false;
            Path = reference.Path;
            ResourceUICultures = reference.ResourceUICultures;
            ScriptMode = reference.ScriptMode;
        }

        public string GetUrl(ScriptManager scriptManager) {
            if (String.IsNullOrEmpty(Path)) {
                return base.GetUrl(scriptManager, false);
            }
            else {
                return scriptManager.ResolveClientUrl(Path);
            }
        }
    }
}
