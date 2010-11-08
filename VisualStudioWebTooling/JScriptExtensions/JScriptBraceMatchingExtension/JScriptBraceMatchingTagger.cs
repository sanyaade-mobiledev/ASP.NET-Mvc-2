using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Classification;
using JScriptPowerTools.Shared;

namespace JScriptBraceMatchingExtension
{
    internal class JScriptBraceMatchingTagger : ITagger<TextMarkerTag>
    {
        private readonly Dictionary<char, char> _braceList =
            new Dictionary<char, char>()
            {
                {'{', '}'},
                {'[', ']'},
                {'(', ')'}
            };
        private string _tagType = "bracehighlight";
        private IClassifier _classifier;
        private ITextView _view;
        private ITextBuffer _sourceBuffer;
        private SnapshotPoint? _currentCharPoint;
        private int _viewScreensToSearchOver = 20;
        private readonly JScriptBraceMatcher _braceMatcher = new JScriptBraceMatcher();

        internal JScriptBraceMatchingTagger(ITextView view, ITextBuffer sourceBuffer, IClassifier classifier)
        {
            _view = view;
            _sourceBuffer = sourceBuffer;
            _currentCharPoint = null;
            _classifier = classifier;

            _view.TextBuffer.Changed += TextBuffer_Changed;
            _view.Caret.PositionChanged += Caret_PositionChanged;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
        private void OnTagsChanged(SnapshotSpan span)
        {
            if (TagsChanged != null)
            {
                TagsChanged(this, new SnapshotSpanEventArgs(span));
            }
        }

        private void TextBuffer_Changed(object sender, TextContentChangedEventArgs e)
        {
#if DEBUG
            Util.Log("TextBuffer_Changed");
#endif
            UpdateAtCaretPosition(_view.Caret.Position);
        }

        private void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
#if DEBUG
            Util.Log("Caret_PositionChanged");
#endif
            UpdateAtCaretPosition(e.NewPosition);
        }

        private void UpdateAtCaretPosition(CaretPosition caretPosition)
        {
            _currentCharPoint = caretPosition.Point.GetPoint(_sourceBuffer, caretPosition.Affinity);

            if (!_currentCharPoint.HasValue)
                return;

            OnTagsChanged(new SnapshotSpan(_sourceBuffer.CurrentSnapshot, 0, _sourceBuffer.CurrentSnapshot.Length));
        }

        public IEnumerable<ITagSpan<TextMarkerTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
#if DEBUG
            Util.Log("GetTags called");
#endif
            if (spans.Count == 0) // there is no content in the buffer
                yield break;

            // don't do anything if the current SnapshotPoint is not initialized or after the end of the buffer
            if (!_currentCharPoint.HasValue || _currentCharPoint.Value.Position > _currentCharPoint.Value.Snapshot.Length)
                yield break;

            // hold on to a snapshot of the current character
            var currentCharPoint = _currentCharPoint.Value;

            // if the requested snapshot isn't the same as the one the brace is on, translate our spans to the expected snapshot
            if (spans[0].Snapshot != currentCharPoint.Snapshot)
            {
                currentCharPoint = currentCharPoint.TranslateTo(spans[0].Snapshot, PointTrackingMode.Positive);
            }

            // get the current char and the previous char
            char? currentChar = currentCharPoint.Position != currentCharPoint.Snapshot.Length ? (char?)currentCharPoint.GetChar() : null;
            var prevCharPoint = currentCharPoint == 0 ? currentCharPoint : currentCharPoint - 1; // if currentChar is 0 (beginning of buffer), don't move it back
            var prevChar = prevCharPoint.GetChar();
            
            var maxLines = _view.TextViewLines.Count * _viewScreensToSearchOver;
            var maxLinesReached = false;
            char closeChar;

            if (currentChar.HasValue && _braceList.TryGetValue(currentChar.Value, out closeChar)) // the value is the open brace
            {
                // if the current char is a comment, string or operator then don't try matching
                // Note: The editor incorrectly classifies braces in regex literals as operators.
                //       We are using this fact to detect if the brace is in a regex literal and thus ignore it.
                if (JScriptEditorUtil.IsClassifiedAs(_classifier, currentCharPoint,
                        JScriptClassifications.Comment,
                        JScriptClassifications.String,
                        JScriptClassifications.Operator))
                    yield break;

                var currPairSpan = new SnapshotSpan();
                if (JScriptBraceMatcher.FindMatchingCloseChar(_classifier, currentCharPoint, currentChar.Value, closeChar, maxLines, ref maxLinesReached, out currPairSpan) == true ||
                    maxLinesReached)
                {
                    yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(currentCharPoint, 1), new TextMarkerTag(_tagType));
                    if (!maxLinesReached)
                        yield return new TagSpan<TextMarkerTag>(currPairSpan, new TextMarkerTag(_tagType));
                }
            }

            var openChar = _braceList.Where(kvp => kvp.Value.Equals(prevChar))
                                     .Select(n => n.Key)
                                     .FirstOrDefault();

            if (openChar != default(char)) // the value is the close brace, which is the *previous* character 
            {
                // if the prev char is a comment or string then don't try matching
                // Note: The editor incorrectly classifies braces in regex literals as operators.
                //       We are using this fact to detect if the brace is in a regex literal and thus ignore it.
                if (JScriptEditorUtil.IsClassifiedAs(_classifier, prevCharPoint,
                        JScriptClassifications.Comment,
                        JScriptClassifications.String,
                        JScriptClassifications.Operator))
                    yield break;

                var prevPairSpan = new SnapshotSpan();
                maxLinesReached = false;

                if (JScriptBraceMatcher.FindMatchingOpenChar(_classifier, prevCharPoint, openChar, prevChar, maxLines, ref maxLinesReached, out prevPairSpan) == true ||
                    maxLinesReached)
                {
                    yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(prevCharPoint, 1), new TextMarkerTag(_tagType));
                    if (!maxLinesReached)
                        yield return new TagSpan<TextMarkerTag>(prevPairSpan, new TextMarkerTag(_tagType));
                }
            }
        }
    }
}