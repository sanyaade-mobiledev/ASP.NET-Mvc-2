using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Classification;

namespace JScriptBraceMatchingExtension
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("jscript")]
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

            return new JScriptBraceMatchingTagger(textView, buffer, ClassifierAggregatorService.GetClassifier(buffer)) as ITagger<T>;
        }
    }
}