using System.ComponentModel.Composition;
using JScriptPowerTools.Shared;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace JScriptOutliningExtension
{
    [Export(typeof(ITaggerProvider))]
    [TagType(typeof(IOutliningRegionTag))]
    [ContentType("jscript")]
    [ContentType("HTML")]
    sealed class OutliningTaggerProvider : ITaggerProvider
    {
        [Import]
        internal IClassifierAggregatorService ClassifierAggregatorService { get; set; }

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            var classifier = VsServiceManager.GetScriptColorizer(buffer) ?? ClassifierAggregatorService.GetClassifier(buffer);

            return buffer.Properties.GetOrCreateSingletonProperty(
                () => new OutliningTagger(buffer, classifier)) as ITagger<T>;
        }
    }
}