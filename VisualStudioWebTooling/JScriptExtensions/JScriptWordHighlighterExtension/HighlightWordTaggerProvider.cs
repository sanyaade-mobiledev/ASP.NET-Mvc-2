using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace JScriptWordHighlighterExtension
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("jscript")]
    [TagType(typeof(TextMarkerTag))]
    internal class HighlightWordTaggerProvider : IViewTaggerProvider
    {
        [Import]
        internal ITextSearchService TextSearchService { get; set; }

        [Import]
        internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector { get; set; }

        [Import]
        internal IClassifierAggregatorService ClassifierAggregatorService { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer)
            where T : ITag
        {
            var textStructureNavigator = TextStructureNavigatorSelector.GetTextStructureNavigator(textView.TextBuffer);

            return new HighlightWordTagger(textView, buffer, TextSearchService, textStructureNavigator, ClassifierAggregatorService.GetClassifier(buffer)) as ITagger<T>;
        }
    }
}