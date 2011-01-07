using System;
using System.ComponentModel.Composition;
using JScriptPowerTools.Shared;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace JScriptBraceMatchingExtension
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("jscript")]
    [ContentType("HTML")]
    [TagType(typeof(TextMarkerTag))]
    internal class JScriptBraceMatchingTaggerProvider : IViewTaggerProvider
    {
        [Import]
        internal IClassifierAggregatorService ClassifierAggregatorService { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer)
            where T : ITag
        {
            if (textView == null)
                return null;

            var classifier = VsServiceManager.GetScriptColorizer(buffer) ?? ClassifierAggregatorService.GetClassifier(buffer);

            return new JScriptBraceMatchingTagger(textView, buffer, classifier) as ITagger<T>;
        }
    }
}