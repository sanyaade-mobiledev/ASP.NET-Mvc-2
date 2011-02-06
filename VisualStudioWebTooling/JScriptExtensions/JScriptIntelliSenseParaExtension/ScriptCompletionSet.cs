using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace JScriptIntelliSenseParaExtension
{
    internal class ScriptCompletionSet : CompletionSet
    {
        public ScriptCompletionSet()
            : base()
        { }

        public ScriptCompletionSet(string moniker, string displayName, ITrackingSpan applicableTo, IEnumerable<Completion> completions, IEnumerable<Completion> completionBuilders)
            : base(moniker, displayName, applicableTo, completions, completionBuilders)
        { }

        public override void SelectBestMatch()
        {
            base.SelectBestMatch(CompletionMatchType.MatchDisplayText, true);
        }
    }
}
