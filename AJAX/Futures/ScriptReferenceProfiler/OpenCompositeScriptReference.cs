using System.Web.UI;

namespace ScriptReferenceProfiler {
    internal class OpenCompositeScriptReference : CompositeScriptReference {
        public string GetUrl(ScriptManager scriptManager) {
            return base.GetUrl(scriptManager, true);
        }
    }
}
