using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ScriptReferenceProfiler {
    [ToolboxData("<{0}:ScriptReferenceProfiler runat=\"server\"/>")]
    public class ScriptReferenceProfiler : WebControl {

        private List<ScriptReference> _references = new List<ScriptReference>();
        private ScriptManager _scriptManager;

        [
        DefaultValue(null),
        Category("Behavior"),
        MergableProperty(false),
        NotifyParentProperty(true),
        TypeConverter(typeof(StringArrayConverter)),
        ]
        public string[] UICultures { get; set; }

        private string GetCompositeScriptUrl(ScriptMode scriptMode) {
            return GetCompositeScriptUrl(scriptMode, null);
        }

        private string GetCompositeScriptUrl(ScriptMode scriptMode, string culture) {
            string url = null;
            var previousCulture = CultureInfo.CurrentUICulture;
            var previousScriptMode = _scriptManager.ScriptMode;
            try {
                if (!String.IsNullOrEmpty(culture)) {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
                }
                _scriptManager.ScriptMode = scriptMode;
                var ocsr = new OpenCompositeScriptReference();
                //ocsr.ScriptMode = scriptMode;
                foreach (var reference in _references) {
                    ocsr.Scripts.Add(reference);
                }
                url = ocsr.GetUrl(_scriptManager);
            }
            finally {
                if (previousCulture != CultureInfo.CurrentUICulture) {
                    Thread.CurrentThread.CurrentUICulture = previousCulture;
                }
                if (previousScriptMode != _scriptManager.ScriptMode) {
                    _scriptManager.ScriptMode = previousScriptMode;
                }
            }
            return url;
        }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            _scriptManager = ScriptManager.GetCurrent(Page);
            if (_scriptManager != null) {
                _scriptManager.ResolveScriptReference += new EventHandler<ScriptReferenceEventArgs>(ResolveScriptReferenceHandler);
            }
        }

        private List<ScriptReference> RemoveDuplicates(List<ScriptReference> references) {
            HybridDictionary uniqueScriptDict = new HybridDictionary(references.Count);
            List<ScriptReference> filteredScriptList = new List<ScriptReference>(references.Count);
            foreach (var reference in references) {
                var key = (String.IsNullOrEmpty(reference.Name)) ?
                    reference.Path : reference.Name + ":" + reference.Assembly;
                if (!uniqueScriptDict.Contains(key)) {
                    uniqueScriptDict.Add(key, reference);
                    filteredScriptList.Add(reference);
                }
            }
            return filteredScriptList;
        }

        private void ResolveScriptReferenceHandler(object sender, ScriptReferenceEventArgs e) {
            var clonedReference = string.IsNullOrEmpty(e.Script.Path) ?
                new ScriptReference(e.Script.Name, e.Script.Assembly) :
                new ScriptReference(e.Script.Path);
            clonedReference.IgnoreScriptPath = e.Script.IgnoreScriptPath;
            clonedReference.ResourceUICultures = e.Script.ResourceUICultures;
            clonedReference.ScriptMode = ScriptMode.Auto;
            _references.Add(clonedReference);
        }

        protected override void RenderContents(HtmlTextWriter output) {
            if (_references.Count == 0) {
                output.WriteLine("No script reference was registered with this page.");
                return;
            }
            _references = RemoveDuplicates(_references);
            output.WriteLine(String.Format("{0} references found on this page:", _references.Count));
            output.RenderBeginTag(HtmlTextWriterTag.Pre);
            foreach (var reference in _references) {
                output.WriteEncodedText("<asp:ScriptReference ");
                if (!String.IsNullOrEmpty(reference.Path)) {
                    output.Write("path=\"");
                    output.WriteEncodedText(reference.Path);
                    output.Write('\"');
                }
                else {
                    output.Write("name=\"");
                    output.WriteEncodedText(reference.Name);
                    output.Write('\"');
                    if (!String.IsNullOrEmpty(reference.Assembly)) {
                        output.Write(" assembly=\"");
                        output.WriteEncodedText(reference.Assembly);
                        output.Write('\"');
                    }
                }
                output.WriteEncodedText("/>");
                output.WriteLine();
            }
            output.RenderEndTag();
            output.WriteLine("Download composite script: ");
            output.AddAttribute(HtmlTextWriterAttribute.Href, GetCompositeScriptUrl(ScriptMode.Debug));
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write("debug version");
            output.RenderEndTag();
            output.Write(' ');
            output.AddAttribute(HtmlTextWriterAttribute.Href, GetCompositeScriptUrl(ScriptMode.Release));
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write("release version");
            output.RenderEndTag();
            if (UICultures != null) {
                output.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var culture in UICultures) {
                    output.RenderBeginTag(HtmlTextWriterTag.Li);
                    output.Write(culture);
                    output.Write(":  ");
                    output.AddAttribute(HtmlTextWriterAttribute.Href, GetCompositeScriptUrl(ScriptMode.Debug, culture));
                    output.RenderBeginTag(HtmlTextWriterTag.A);
                    output.Write("debug version");
                    output.RenderEndTag();
                    output.Write(' ');
                    output.AddAttribute(HtmlTextWriterAttribute.Href, GetCompositeScriptUrl(ScriptMode.Release, culture));
                    output.RenderBeginTag(HtmlTextWriterTag.A);
                    output.Write("release version");
                    output.RenderEndTag();
                    output.RenderEndTag();
                }
                output.RenderEndTag();
            }
        }
    }
}
