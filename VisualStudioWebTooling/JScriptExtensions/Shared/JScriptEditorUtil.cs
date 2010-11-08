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
    }
}