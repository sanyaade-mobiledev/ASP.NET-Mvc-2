using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text.Classification;
using JScriptPowerTools.Shared;

namespace JScriptWordHighlighterExtension
{
    internal class HighlightWordTagger : ITagger<HighlightWordTag>
    {
        ITextView View { get; set; }
        ITextBuffer SourceBuffer { get; set; }
        ITextSearchService TextSearchService { get; set; }
        ITextStructureNavigator TextStructureNavigator { get; set; }
        IClassifier Classifier { get; set; }
        NormalizedSnapshotSpanCollection WordSpans { get; set; }
        SnapshotSpan? CurrentWord { get; set; }
        SnapshotPoint RequestedPoint { get; set; }
        Dispatcher ViewDispatcher { get; set; }

        public HighlightWordTagger(ITextView view, ITextBuffer sourceBuffer, ITextSearchService textSearchService, ITextStructureNavigator textStructureNavigator, IClassifier classifier)
        {
            View = view;
            ViewDispatcher = ((IWpfTextView)view).VisualElement.Dispatcher;
            SourceBuffer = sourceBuffer;
            TextSearchService = textSearchService;
            TextStructureNavigator = textStructureNavigator;
            Classifier = classifier;

            WordSpans = new NormalizedSnapshotSpanCollection();
            CurrentWord = null;
            View.Caret.PositionChanged += View_Caret_PositionChanged;
            View.LayoutChanged += View_LayoutChanged;
        }

        private void View_Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            UpdateAtCaretPosition(e.NewPosition);
        }

        private void View_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            // If a new snapshot wasn't generated, then skip this layout
            if (e.NewSnapshot != e.OldSnapshot)
            {
                UpdateAtCaretPosition(View.Caret.Position);
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
        private void OnTagsChanged(SnapshotSpan span)
        {
            if (TagsChanged != null)
            {
                TagsChanged(this, new SnapshotSpanEventArgs(span));
            }
        }

        void UpdateAtCaretPosition(CaretPosition caretPosition)
        {
            SnapshotPoint? point = caretPosition.Point.GetPoint(SourceBuffer, caretPosition.Affinity);

            if (!point.HasValue)
                return;

            // If the new caret position is still within the current word (and on the same snapshot), we don't need to check it
            if (CurrentWord.HasValue
                && CurrentWord.Value.Snapshot == SourceBuffer.CurrentSnapshot
                && point.Value >= CurrentWord.Value.Start
                && point.Value <= CurrentWord.Value.End)
            {
                return;
            }

            RequestedPoint = point.Value;

            UpdateWordAdornments();
        }

        #region ShimLanguageNavigator workarounds
        // The ShimLanguageNavigator that the editor uses to map from ITextStructureNavigator to the
        // old language service interfaces has a bug in inline views, where it uses the wrong ITextSnapshot
        // in a TranslateTo operation. This workaround doesn't try to correct the problem, but
        // merely wraps all calls inside a Try/Catch block and returning a default value instead of throwing.
        //
        // See Dev10 #906779 for more details. This bug has been fixed post-Dev10 RTM.
        TextExtent GetExtentOfWord(SnapshotPoint point)
        {
            var topPoint = View.BufferGraph.MapUpToSnapshot(point, PointTrackingMode.Positive, PositionAffinity.Successor, View.TextSnapshot);

            if (!topPoint.HasValue)
            {
                Debug.Fail("Unexpected: can't map point to view.");
                return new TextExtent(new SnapshotSpan(point, point), false);
            }

            var topExtent = TextStructureNavigator.GetExtentOfWord(topPoint.Value);
            var spans = View.BufferGraph.MapDownToSnapshot(topExtent.Span, SpanTrackingMode.EdgeExclusive, point.Snapshot);
            if (!spans.Any())
            {
                Debug.Fail("Unexpected: can't map back down to view.");
                return new TextExtent(new SnapshotSpan(point, point), false);
            }

            return new TextExtent(new SnapshotSpan(spans[0].Start, spans[spans.Count - 1].End), topExtent.IsSignificant);
        }

        IEnumerable<SnapshotSpan> FindAll(SnapshotSpan currentWord)
        {
            // Search on the *wrong* snapshot, on purpose, and map the results down to the correct snapshot
            var findData = new FindData(currentWord.GetText(), View.TextSnapshot);
            findData.FindOptions = FindOptions.WholeWord | FindOptions.MatchCase;

            var snapshot = currentWord.Snapshot;
            return TextSearchService.FindAll(findData)
                                    .Select(s => View.BufferGraph.MapDownToSnapshot(s, SpanTrackingMode.EdgeExclusive, snapshot))
                                    .Where(spans => spans.Any())
                                    .Select(spans => new SnapshotSpan(spans[0].Start, spans[spans.Count - 1].End));
        }
        #endregion
        
        void UpdateWordAdornments()
        {
            var currentRequest = RequestedPoint;
            var wordSpans = new List<SnapshotSpan>();
            var word = GetExtentOfWord(currentRequest);

            // Find all words in the buffer like the one the caret is on
            bool foundWord = true;
            // If we've selected something not worth highlighting, we might have missed a "word" by a little bit
            if (!WordExtentIsValid(currentRequest, word))
            {
                // Before we retry, make sure it is worthwhile
                if (word.Span.Start != currentRequest
                     || currentRequest == currentRequest.GetContainingLine().Start
                     || char.IsWhiteSpace((currentRequest - 1).GetChar()))
                {
                    foundWord = false;
                }
                else
                {
                    // Try again, one character previous. 
                    // If the caret is at the end of a word, pick up the word.
                    word = GetExtentOfWord(currentRequest - 1);

                    // If the word still isn't valid, we're done
                    if (!WordExtentIsValid(currentRequest, word))
                        foundWord = false;
                }
            }

            SynchronousUpdate(currentRequest, new NormalizedSnapshotSpanCollection(), null);
            if (!foundWord)
                return;

            SnapshotSpan currentWord = word.Span;
            
            // If this is the current word, and the caret moved within a word, we're done.
            if (CurrentWord.HasValue && currentWord == CurrentWord)
                return;

            // If the word is not an identifier, then don't search for matches.
            if (!JScriptEditorUtil.IsClassifiedAs(Classifier, currentWord.Start, JScriptClassifications.Identifier))
                return;

            var request = new FindMatchesRequest { WordSpan = currentWord, RequestPoint = currentRequest };
            FindAllMatches(request);
        }

        sealed class FindMatchesRequest
        {
            public SnapshotSpan WordSpan;
            public SnapshotPoint RequestPoint;
        }

        DispatcherTimer Timer { get; set; }
        void FindAllMatches(FindMatchesRequest request)
        {
            if (Timer != null)
                Timer.Stop();

            // Delay finding matches for 1 second
            Timer = new DispatcherTimer(DispatcherPriority.Background, ViewDispatcher)
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            Timer.Tick += (sender, args) =>
            {
                Timer.Stop();
                ThreadPool.QueueUserWorkItem(FindAllMatches_Background, request);
            };

            Timer.Start();
        }

        void FindAllMatches_Background(object state)
        {
            var request = (FindMatchesRequest)state;
            if (request.RequestPoint != RequestedPoint)
                return;

            // Find the new spans
            var wordSpans = FindAll(request.WordSpan);

            if (wordSpans.Count() == 1)
                return;

            // If another change hasn't happened, do a real update
            if (request.RequestPoint == RequestedPoint)
                SynchronousUpdate(request.RequestPoint, wordSpans, request.WordSpan);
        }

        private static bool WordExtentIsValid(SnapshotPoint currentRequest, TextExtent word)
        {
            return word.IsSignificant && currentRequest.Snapshot.GetText(word.Span).Any(c => char.IsLetter(c));
        }

        private void SynchronousUpdate(SnapshotPoint currentRequest, IEnumerable<SnapshotSpan> newSpans, SnapshotSpan? newCurrentWord)
        {
            ViewDispatcher.BeginInvoke(new Action(() =>
                {
                    if (currentRequest != RequestedPoint)
                        return;

                    WordSpans = new NormalizedSnapshotSpanCollection(
                        newSpans.Where(s => JScriptEditorUtil.IsClassifiedAsJavaScript(Classifier, s.Start)));
                    
                    CurrentWord = newCurrentWord;

                    OnTagsChanged(new SnapshotSpan(View.TextBuffer.CurrentSnapshot, 0, View.TextBuffer.CurrentSnapshot.Length));
                }));
        }

        public IEnumerable<ITagSpan<HighlightWordTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (CurrentWord == null)
                yield break;

            // Hold on to a "snapshot" of the word spans and current word, so that we maintain the same
            // collection throughout
            SnapshotSpan currentWord = CurrentWord.Value;
            NormalizedSnapshotSpanCollection wordSpans = WordSpans;

            if (spans.Count == 0 || WordSpans.Count == 0)
                yield break;

            // If the requested snapshot isn't the same as the one our words are on, translate our spans to the expected snapshot
            if (spans[0].Snapshot != wordSpans[0].Snapshot)
            {
                wordSpans = new NormalizedSnapshotSpanCollection(
                    wordSpans.Select(span => span.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive)));

                currentWord = currentWord.TranslateTo(spans[0].Snapshot, SpanTrackingMode.EdgeExclusive);
            }

            // First, yield back the word the cursor is under (if it overlaps)
            // Note that we'll yield back the same word again in the wordspans collection;
            // the duplication here is expected.
            if (spans.OverlapsWith(new NormalizedSnapshotSpanCollection(currentWord)))
                yield return new TagSpan<HighlightWordTag>(currentWord, new HighlightWordTag());

            // Second, yield all the other words in the file
            foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, wordSpans))
            {
                yield return new TagSpan<HighlightWordTag>(span, new HighlightWordTag());
            }
        }
    }
}