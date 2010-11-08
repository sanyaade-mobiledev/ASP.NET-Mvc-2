using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;

namespace JScriptIntelliSenseParaExtension
{
    internal class ScriptCompletionReplacementSource : ICompletionSource
    {
        ITextBuffer _textBuffer;

        public ScriptCompletionReplacementSource(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            var newCompletionSets = completionSets
                .Select(cs => cs == null ? cs : new CompletionSet(
                    cs.Moniker,
                    cs.DisplayName,
                    cs.ApplicableTo,
                    cs.Completions
                        .Select(c => c == null ? c : new Completion(
                            c.DisplayText,
                            c.InsertionText,
                            DocCommentHelper.ProcessParaTags(c.Description),
                            c.IconSource,
                            c.IconAutomationText))
                        .ToList(),
                    cs.CompletionBuilders))
                .ToList();
            
            completionSets.Clear();

            newCompletionSets.ForEach(cs => completionSets.Add(cs));
        }

        private bool _isDisposed;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }
    }
}