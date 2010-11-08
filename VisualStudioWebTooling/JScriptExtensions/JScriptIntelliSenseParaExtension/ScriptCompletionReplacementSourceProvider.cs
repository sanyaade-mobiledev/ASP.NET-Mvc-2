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
    [Export(typeof(ICompletionSourceProvider))]
    [Name("JScript IntelliSense <para> Extension - Completion")]
    [Order(Before = "default")]
    [ContentType("jscript")]
    internal class ScriptCompletionReplacementSourceProvider : ICompletionSourceProvider
    {
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new ScriptCompletionReplacementSource(textBuffer);
        }
    }
}
