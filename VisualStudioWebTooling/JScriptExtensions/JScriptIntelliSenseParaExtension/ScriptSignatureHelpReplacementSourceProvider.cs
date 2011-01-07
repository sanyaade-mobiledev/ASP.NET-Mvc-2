using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text;

namespace JScriptIntelliSenseParaExtension
{
    [Export(typeof(ISignatureHelpSourceProvider))]
    [Name("JScript IntelliSense <para> Extension")]
    [Order(Before = "default")]
    [ContentType("jscript")]
    [ContentType("HTML")]
    internal class ScriptSignatureHelpReplacementSourceProvider : ISignatureHelpSourceProvider
    {
        public ISignatureHelpSource TryCreateSignatureHelpSource(ITextBuffer textBuffer)
        {
            return new ScriptSignatureHelpReplacementSource();
        }
    }
}