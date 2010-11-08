using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace JScriptOutliningExtension {

	[Export(typeof(ITaggerProvider))]
	[TagType(typeof(IOutliningRegionTag))]
	[ContentType("jscript")]
	sealed class OutliningTaggerProvider : ITaggerProvider {

		[Import]
		internal IClassifierAggregatorService ClassifierAggregatorService { get; set; }

		public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
			return buffer.Properties.GetOrCreateSingletonProperty(
				() => new OutliningTagger(buffer, ClassifierAggregatorService.GetClassifier(buffer))) as ITagger<T>;
		}
	}
}