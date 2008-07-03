using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Handlers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

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

        public string GetCompositeScript(string[] urls) {
            var script = new StringBuilder();
            IHttpHandler handler = new ScriptResourceHandler();
            var absoluteUri = Page.Request.Url.AbsoluteUri;
            var fileNameIndex = absoluteUri.LastIndexOf(Page.Request.Url.Segments[2]);
            var scriptResourcePath = absoluteUri.Substring(0, fileNameIndex) + "ScriptResource.axd";
            foreach(var url in urls) {
                // Is script resource-based?
                if (url.IndexOf("/scriptresource.axd", StringComparison.OrdinalIgnoreCase) != -1) {
                    var queryStringIndex = url.IndexOf('?');
                    var queryString = url.Substring(queryStringIndex + 1);
                    var request = new HttpRequest("scriptresource.axd", scriptResourcePath, queryString);
                    using (var textWriter = new StringWriter(script)) {
                        HttpResponse response = new HttpResponse(textWriter);
                        HttpContext context = new HttpContext(request, response);
                        handler.ProcessRequest(context);
                    }
                    script.AppendLine();
                }
                else {
                    using (var textReader = new StreamReader(Page.Server.MapPath(url), true)) {
                        script.AppendLine(textReader.ReadToEnd());
                    }
                }
            }
            script.AppendLine("if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();");
            return script.ToString();
        }

        private List<string> GetCompositeScriptData(ScriptMode scriptMode) {
            return GetCompositeScriptData(scriptMode, null);
        }

        private List<string> GetCompositeScriptData(ScriptMode scriptMode, string culture) {
            List<string> data = new List<string>();
            var previousCulture = CultureInfo.CurrentUICulture;
            var previousScriptMode = _scriptManager.ScriptMode;
            try {
                if (!String.IsNullOrEmpty(culture)) {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
                }
                _scriptManager.ScriptMode = scriptMode;
                foreach (var reference in _references) {
                    var openReference = new OpenScriptReference(reference);
                    data.Add(openReference.GetUrl(_scriptManager));
                }
            }
            finally {
                if (previousCulture != CultureInfo.CurrentUICulture) {
                    Thread.CurrentThread.CurrentUICulture = previousCulture;
                }
                if (previousScriptMode != _scriptManager.ScriptMode) {
                    _scriptManager.ScriptMode = previousScriptMode;
                }
            }
            return data;
        }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            var request = Page.Request;
            var scriptReference = request.Form["__SCRIPTREFERENCEPROFILER"];
            if (!String.IsNullOrEmpty(scriptReference)) {
                var response = Page.Response;
                response.Clear();
                response.AddHeader("Content-Type", "text/javascript");
                var culture = request.Form["__SCRIPTREFERENCEPROFILERCULTURE"];
                response.AddHeader("Content-Disposition", "attachment; filename=" +
                    Path.GetFileNameWithoutExtension(request.FilePath) + 
                    (request.Form["__SCRIPTREFERENCEPROFILERDEBUG"] == "debug" ? ".debug" : "") + 
                    (String.IsNullOrEmpty(culture) ? "" : "." + culture) + 
                    ".js");
                response.Write(GetCompositeScript(request.Form["__SCRIPTREFERENCEPROFILERCONTENTS"].Split(' ')));
                response.Flush();
                response.End();
                return;
            }
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
            // Render script reference tags
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
            // Render script
            output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            output.RenderBeginTag(HtmlTextWriterTag.Script);
            var serializer = new JavaScriptSerializer();
            output.Write(@"Sys.Application.add_init(function() {
    Sys._scriptReferenceProfilerScripts = {
        debug: " + serializer.Serialize(GetCompositeScriptData(ScriptMode.Debug)));
            output.WriteLine(@",
        release: " + serializer.Serialize(GetCompositeScriptData(ScriptMode.Release)));
            if (UICultures != null) {
                foreach (var culture in UICultures) {
                    output.WriteLine(@",
        ""debug-" + culture + @""": " + serializer.Serialize(GetCompositeScriptData(ScriptMode.Debug, culture)));
                    output.WriteLine(@",
        ""release-" + culture + @""": " + serializer.Serialize(GetCompositeScriptData(ScriptMode.Release, culture)));
                }
            }
            output.WriteLine(@"
    };
    Sys._scriptReferenceProfilerSend = function(version, culture) {
        var field = document.createElement(""input"");
        field.name = ""__SCRIPTREFERENCEPROFILER"";
        field.value = ""true"";
        field.type= ""hidden"";
        theForm.appendChild(field);
        if (culture) {
            field = document.createElement(""input"");
            field.name = ""__SCRIPTREFERENCEPROFILERCULTURE"";
            field.value = culture;
            field.type= ""hidden"";
            theForm.appendChild(field);
        }
        if (version === ""debug"") {
            field = document.createElement(""input"");
            field.name = ""__SCRIPTREFERENCEPROFILERDEBUG"";
            field.value = ""debug"";
            field.type= ""hidden"";
            theForm.appendChild(field);
        }
        field = document.createElement(""input"");
        field.name = ""__SCRIPTREFERENCEPROFILERCONTENTS"";
        field.value = Sys._scriptReferenceProfilerScripts[version + (culture ? ""-"" + culture : """")].join("" "");
        field.type= ""hidden"";
        theForm.appendChild(field);
        theForm.submit();
    }
});");
            output.RenderEndTag();
            // Render links
            output.WriteLine("Download composite script: ");
            output.AddAttribute(HtmlTextWriterAttribute.Href, @"javascript:Sys._scriptReferenceProfilerSend('debug');");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write("debug version");
            output.RenderEndTag();
            output.Write(' ');
            output.AddAttribute(HtmlTextWriterAttribute.Href, @"javascript:Sys._scriptReferenceProfilerSend('release');");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write("release version");
            output.RenderEndTag();
            if (UICultures != null) {
                output.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var culture in UICultures) {
                    output.RenderBeginTag(HtmlTextWriterTag.Li);
                    output.Write(culture);
                    output.Write(":  ");
                    output.AddAttribute(HtmlTextWriterAttribute.Href, @"javascript:Sys._scriptReferenceProfilerSend('debug', '" + culture + @"');");
                    output.RenderBeginTag(HtmlTextWriterTag.A);
                    output.Write("debug version");
                    output.RenderEndTag();
                    output.Write(' ');
                    output.AddAttribute(HtmlTextWriterAttribute.Href, @"javascript:Sys._scriptReferenceProfilerSend('release', '" + culture + @"');");
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
