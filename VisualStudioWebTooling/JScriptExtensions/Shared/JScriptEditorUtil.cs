using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text;

namespace JScriptPowerTools.Shared
{
    public class JScriptEditorUtil
    {
        public static bool IsClassifiedAs(IClassifier classifier, SnapshotPoint startPoint, params string[] names)
        {
            if (startPoint == null) return false;

            var spans = classifier.GetClassificationSpans(new SnapshotSpan(startPoint, 1));
            var span = spans.FirstOrDefault();
            if (span == null) return false;

            if (names.Contains(span.ClassificationType.Classification))
                return true;

            return false;
        }

        public static bool IsClassifiedAsJavaScript(IClassifier classifier, SnapshotPoint startPoint)
        {
            if (startPoint == null) return false;

            var spans = classifier.GetClassificationSpans(new SnapshotSpan(startPoint, 1));
            var span = spans.FirstOrDefault();
            if (span == null) return false;

            return span.ClassificationType.Classification.StartsWith("Script ", StringComparison.OrdinalIgnoreCase);
        }

        private static Guid _jsLangServiceGuid = Guid.Parse("59e2f421-410a-4fc9-9803-1f4e79216be8");
        private static Guid _cssLangServiceGuid = Guid.Parse("a764e898-518d-11d2-9a89-00c04f79efc3");

        public static bool IsInJScriptLanguageBlock(ILanguageBlockManager lbm, SnapshotPoint point)
        {
            var lbColl = lbm.GetLanguageBlockCollection();
            var lbCount = lbColl.GetCount();
            for (var i = 0; i < lbCount; i++)
            {
                var lb = lbColl.GetAtIndex(i);
                int start, length;
                bool isEndInclusive;
                lb.GetExtent(out start, out length, out isEndInclusive);
                int end = isEndInclusive ? start + length : start + length - 1;

                if (point >= start && point <= end)
                {
                    // Current point is in this language block
                    return (lb.GetContainedLanguage().GetLanguageServiceGuid() == _jsLangServiceGuid);
                }
            }
            // Looped through all blocks and point was not in any of them
            return false;
        }

        public static List<BlockSpan> GetJavaScriptBlockSpans(ILanguageBlockManager languageBlockManager)
        {
            var javaScriptBlockSpans = new List<BlockSpan>();

            var languageBlockCollection = languageBlockManager.GetLanguageBlockCollection();
            int languageBlockCount = languageBlockCollection.GetCount();
            for (int i = 0; i < languageBlockCount; i++)
            {
                var languageBlock = languageBlockCollection.GetAtIndex(i);
                if (languageBlock.GetContainedLanguage().GetLanguageServiceGuid() == _jsLangServiceGuid)
                {
                    int start, length;
                    bool isEndInclusive;

                    languageBlock.GetExtent(out start, out length, out isEndInclusive);
                    javaScriptBlockSpans.Add(new BlockSpan(start, length, isEndInclusive));
                }
            }

            // Looped through all blocks and point was not in any of them
            return javaScriptBlockSpans;
        }
    }

    public class BlockSpan
    {
        public int Start { get; set; }
        public int End { get; set; }

        public BlockSpan(int start, int length, bool isEndInclusive)
        {
            Start = start;
            End = isEndInclusive ? start + length : start + length - 1;
        }

        public bool Contains(int position)
        {
            return Start <= position && position <= End;
        }
    }
}