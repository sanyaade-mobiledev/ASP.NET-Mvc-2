using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.Diagnostics;
using JScriptPowerTools.Shared;

namespace JScriptIntelliSenseParaExtension
{
    internal class ScriptCompletionReplacementSource : ICompletionSource
    {
        ITextBuffer _textBuffer;
        ILanguageBlockManager _lbm;

        private bool IsHtmlFile { get { return _lbm != null; } }

        public ScriptCompletionReplacementSource(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;

            if (textBuffer.ContentType.TypeName.Equals("HTML", StringComparison.OrdinalIgnoreCase))
                _lbm = VsServiceManager.GetLanguageBlockManager(textBuffer);
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (IsHtmlFile && completionSets.Any())
            {
                var bottomSpan = completionSets.First().ApplicableTo;
                if (!JScriptEditorUtil.IsInJScriptLanguageBlock(_lbm, bottomSpan.GetStartPoint(bottomSpan.TextBuffer.CurrentSnapshot)))
                {
                    // This is an HTML statement completion session, so do nothing
                    return;
                }
            }

            // TODO: Reflect over the ShimCompletionSet to see where the Description value comes from
            //       as setting the property does not actually change the value.
            var newCompletionSets = completionSets
                .Select(cs => cs == null ? cs : new ScriptCompletionSet(
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