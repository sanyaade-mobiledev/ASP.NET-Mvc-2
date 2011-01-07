using System;
using System.Collections.Generic;
using System.Linq;
using JScriptPowerTools.Shared;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

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
        private int _viewScreensToSearchOver = 20; // This seems like a good number :)
        private readonly JScriptBraceMatcher _braceMatcher = new JScriptBraceMatcher();
        private ILanguageBlockManager _lbm;

        private bool IsHtmlFile { get { return _lbm != null; } }

        internal JScriptBraceMatchingTagger(ITextView view, ITextBuffer sourceBuffer, IClassifier classifier)
        {
            _view = view;
            _sourceBuffer = sourceBuffer;
            _currentCharPoint = null;
            _classifier = classifier;

            _view.TextBuffer.Changed += TextBuffer_Changed;
            _view.Caret.PositionChanged += Caret_PositionChanged;

            if (sourceBuffer.ContentType.TypeName.Equals("HTML", StringComparison.OrdinalIgnoreCase))
                _lbm = VsServiceManager.GetLanguageBlockManager(_sourceBuffer);
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

        private bool _maxLinesReached = false;

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
            var currentChar = currentCharPoint.Position != currentCharPoint.Snapshot.Length ? (char?)currentCharPoint.GetChar() : null;
            var prevCharPoint = currentCharPoint == 0 ? currentCharPoint : currentCharPoint - 1; // if currentChar is 0 (beginning of buffer), don't move it back
            var prevChar = prevCharPoint != 0 ? prevCharPoint.GetChar() : default(char?); // Can't get prevChar if point is 0
            
            var maxLines = _view.TextViewLines.Count * _viewScreensToSearchOver;
            char closeChar;

            // If neither current or previous char is in a JS language block, return
            if (IsHtmlFile && !JScriptEditorUtil.IsInJScriptLanguageBlock(_lbm, currentCharPoint)
                           && !JScriptEditorUtil.IsInJScriptLanguageBlock(_lbm, prevCharPoint))
                yield break;

            // Check if char to right of cursor is an opening brace
            if (currentChar.HasValue && _braceList.TryGetValue(currentChar.Value, out closeChar)) // the value is the open brace
            {
                foreach (var s in GetTagSpans(currentCharPoint, currentChar.Value, closeChar, maxLines))
                {
                    yield return s;
                }
            }

            // Check if char to left of cursor is a closing brace
            var openChar = _braceList.Where(kvp => kvp.Value.Equals(prevChar))
                                     .Select(n => n.Key)
                                     .FirstOrDefault();

            if (openChar != default(char)) // the value is the close brace, which is the *previous* character 
            {
                foreach (var s in GetTagSpans(prevCharPoint, prevChar.Value, openChar, maxLines))
                {
                    yield return s;
                }
            }
        }

        private IEnumerable<TagSpan<TextMarkerTag>> GetTagSpans(SnapshotPoint bracePoint, char brace, char matchingBrace, int maxLines)
        {
            if (BraceShouldBeIgnored(bracePoint))
                yield break;

            var pairSpan = new SnapshotSpan();

            var findingClose = _braceList.ContainsKey(brace);
            _maxLinesReached = false;

            if (JScriptBraceMatcher.FindMatchingBrace(_lbm, _classifier, bracePoint, findingClose, brace, matchingBrace, maxLines, ref _maxLinesReached, out pairSpan) == true ||
                _maxLinesReached)
            {
                yield return new TagSpan<TextMarkerTag>(new SnapshotSpan(bracePoint, 1), new TextMarkerTag(_tagType));
                if (!_maxLinesReached)
                    yield return new TagSpan<TextMarkerTag>(pairSpan, new TextMarkerTag(_tagType));
            }
        }

        private bool BraceShouldBeIgnored(SnapshotPoint bracePoint)
        {
            // if the prev char is a comment, string or operator then don't try matching
            // Note: The editor incorrectly classifies braces in regex literals as operators.
            //       We are using this fact to detect if the brace is in a regex literal and thus ignore it.
            //       Genuine braces have no classification at all.
            return JScriptEditorUtil.IsClassifiedAs(_classifier, bracePoint,
                JScriptClassifications.Comment,
                JScriptClassifications.String,
                JScriptClassifications.Operator);
        }
    }
}