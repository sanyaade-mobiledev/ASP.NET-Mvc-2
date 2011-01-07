using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using JScriptPowerTools.Shared;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;

namespace JScriptOutliningExtension
{
    sealed class OutliningTagger : ITagger<IOutliningRegionTag>, IDisposable
    {
        ITextBuffer _buffer;
        IClassifier _classifier;
        List<ITrackingSpan> _sections;
        DispatcherTimer _timer;

        /// <summary>
        /// Used only in markup files to figure out the spans of the JavaScript blocks
        /// </summary>
        ILanguageBlockManager _lbm;

        /// <summary>
        /// Spans of all JavaScript blocks in a markup files (null in JavaScript files)
        /// </summary>
        List<BlockSpan> _javaScriptBlockSpans;

        /// <summary>
        /// Starts of spans of all JavaScript blocks in a markup files (null in JavaScript files)
        /// </summary>
        HashSet<int> _javaScriptBlockStarts = new HashSet<int>();

        /// <summary>
        /// Ends of spans of all JavaScript blocks in a markup files (null in JavaScript files)
        /// </summary>
        HashSet<int> _javaScriptBlockEnds = new HashSet<int>();

        private bool IsHtmlFile { get { return _lbm != null; } }

        bool _fullReparse = true; // We always want full reparse the very first time!

        ITrackingSpan _editSpan = null;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public OutliningTagger(ITextBuffer buffer, IClassifier classifier)
        {
            _buffer = buffer;
            _classifier = classifier;
            _sections = new List<ITrackingSpan>();

            if (buffer.ContentType.TypeName.Equals("HTML", StringComparison.OrdinalIgnoreCase))
                _lbm = VsServiceManager.GetLanguageBlockManager(buffer);

            _timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
            _timer.Interval = TimeSpan.FromMilliseconds(3000); // We'll force a reparse after 3 secs of no buffer changes
            _timer.Tick += (sender, args) =>
            {
                _timer.Stop();
                ReparseFile();
            };
            buffer.Changed += BufferChanged;
            ReparseFile(); // Force an initial full parse
            _timer.Start();
        }

        public void Dispose()
        {
            _buffer.Changed -= BufferChanged;
        }

        private void BufferChanged(object s, TextContentChangedEventArgs e)
        {
            CalculateChange(e);

            // Reset the timer every time the buffer changes.
            _timer.Stop(); // is the stop neccesary?
            _timer.Start();
        }

        void CalculateChange(TextContentChangedEventArgs e)
        {

#if DEBUG
            Util.LogTextChanges(e.Changes);
#endif

            if (_fullReparse == true)
            {
                return;
            }

            if (e.Changes.Count != 1)
            {
                _fullReparse = true;
                return;
            }

            ITextChange textChange = e.Changes[0];

            if (_editSpan == null)
            {
                _editSpan = _buffer.CurrentSnapshot.CreateTrackingSpan(
                    textChange.NewPosition,
                    textChange.NewEnd - textChange.NewPosition,
                    SpanTrackingMode.EdgeInclusive);
#if DEBUG
                Util.Log("Created new edit span (" +
                    _editSpan.GetStartPoint(_buffer.CurrentSnapshot).Position + "," +
                    _editSpan.GetEndPoint(_buffer.CurrentSnapshot).Position + ") ",
                    _buffer.CurrentSnapshot.GetText(
                        _editSpan.GetStartPoint(_buffer.CurrentSnapshot).Position,
                        _editSpan.GetEndPoint(_buffer.CurrentSnapshot).Position -
                        _editSpan.GetStartPoint(_buffer.CurrentSnapshot).Position));
#endif
            }
            else
            {
                int oldEditStartPosition = _editSpan.GetStartPoint(_buffer.CurrentSnapshot).Position;
                int oldEditEndPosition = _editSpan.GetEndPoint(_buffer.CurrentSnapshot).Position;
                // In many cases, new edit is auto-merged with old edit by tracking span. To be more'
                // specific, in all cases when new edit is adjacent to the old edit, it will be
                // auto-merged. We need to create a new tracking span only if the new edit was non-adjacent
                // to the old edit (i.e. a few characters before the old edit or a few characters after
                // the old edit).
                if (textChange.NewPosition < oldEditStartPosition ||
                    textChange.NewPosition > oldEditEndPosition)
                {
                    int newEditStartPosition = Math.Min(textChange.NewPosition, oldEditStartPosition);
                    int newEditEndPosition = Math.Max(textChange.NewEnd, oldEditEndPosition);
                    _editSpan = _buffer.CurrentSnapshot.CreateTrackingSpan(
                        newEditStartPosition,
                        newEditEndPosition - newEditStartPosition,
                        SpanTrackingMode.EdgeInclusive);
                }
#if DEBUG
                Util.Log("Updated edit span (" +
                    _editSpan.GetStartPoint(_buffer.CurrentSnapshot).Position + "," +
                    _editSpan.GetEndPoint(_buffer.CurrentSnapshot).Position + ") ",
                    _buffer.CurrentSnapshot.GetText(
                        _editSpan.GetStartPoint(_buffer.CurrentSnapshot).Position,
                        _editSpan.GetEndPoint(_buffer.CurrentSnapshot).Position -
                        _editSpan.GetStartPoint(_buffer.CurrentSnapshot).Position));
#endif
            }
        }

        /// <summary>
        /// Returns the index of the smallest outlining span fully containing 
        /// the edit
        /// </summary>
        /// <returns></returns>
        int GetContainingOutliningSpanIndex()
        {
            int containingSpanIndex = -1;

            if (!_fullReparse && _editSpan != null)
            {
                int combinedEditStart = _editSpan.GetStartPoint(_buffer.CurrentSnapshot).Position;
                int combinedEditEnd = _editSpan.GetEndPoint(_buffer.CurrentSnapshot).Position;

                for (int i = _sections.Count - 1; i >= 0; i--)
                {
                    int spanStart = _sections[i].GetStartPoint(_buffer.CurrentSnapshot).Position;
                    int spanEnd = _sections[i].GetEndPoint(_buffer.CurrentSnapshot).Position;
#if DEBUG
                    Util.Log("Span " + i + ": (" + spanStart + "," + spanEnd + ")");
#endif
                    if (spanEnd < combinedEditStart)
                    {
#if DEBUG
                        Util.Log("Not going beyond span " + i);
#endif
                        break;
                    }

                    if (spanStart < combinedEditStart && spanEnd > combinedEditEnd)
                    {
                        containingSpanIndex = i;
                    }
                }
            }

            return containingSpanIndex;
        }

        public IEnumerable<ITagSpan<IOutliningRegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (_sections == null || _sections.Count == 0 || spans.Count == 0)
                yield break;

            var snapshot = spans[0].Snapshot;

            foreach (var section in _sections)
            {
                var sectionSpan = section.GetSpan(snapshot);

                if (spans.IntersectsWith(new NormalizedSnapshotSpanCollection(sectionSpan)))
                {
                    var firstLine = sectionSpan.GetText();
                    firstLine = "{}";

                    var collapsedHintText = sectionSpan.Length <= 250 ?
                        sectionSpan.GetText() :
                        snapshot.GetText(sectionSpan.Start, 249) + "…";

                    var tag = new OutliningRegionTag(firstLine, collapsedHintText);
                    yield return new TagSpan<IOutliningRegionTag>(sectionSpan, tag);
                }
            }
        }

        private void UpdateJavaScriptBlockSpans()
        {
            if (IsHtmlFile)
                _javaScriptBlockSpans = JScriptEditorUtil.GetJavaScriptBlockSpans(_lbm);
            else
            {
                if (_javaScriptBlockSpans == null)
                    _javaScriptBlockSpans = new List<BlockSpan>(1);
                else
                    _javaScriptBlockSpans.Clear();

                _javaScriptBlockSpans.Add(new BlockSpan(0, _buffer.CurrentSnapshot.Length, false));
            }

            _javaScriptBlockStarts.Clear();
            _javaScriptBlockEnds.Clear();

            foreach (BlockSpan javaScriptBlockSpan in _javaScriptBlockSpans)
            {
                _javaScriptBlockStarts.Add(javaScriptBlockSpan.Start);
                _javaScriptBlockEnds.Add(javaScriptBlockSpan.End);
            }
        }

        private bool IsInJavaScriptBlock(int position)
        {
            foreach (BlockSpan javaScriptBlockSpan in _javaScriptBlockSpans)
            {
                if (javaScriptBlockSpan.Contains(position))
                {
                    return true;
                }
            }

            return false;
        }

        void ReparseFile()
        {
            UpdateJavaScriptBlockSpans();

            // If we are in a markup file with no script blocks, return immediately.
            if (_javaScriptBlockSpans.Count == 0)
                return;

            int containingSpanIndex = -1;

            int reparseRangeStart = 0;
            int reparseRangeEnd = 0;

            try
            {
                if (_fullReparse)
                {
                    reparseRangeStart = 0;
                    reparseRangeEnd = _buffer.CurrentSnapshot.Length;
#if DEBUG
                    Util.Log("ReparseFile() - Full Reparse.");
#endif
                }
                else
                {
                    containingSpanIndex = GetContainingOutliningSpanIndex();
                    if (containingSpanIndex >= 0)
                    {
                        reparseRangeStart = _sections[containingSpanIndex].GetStartPoint(_buffer.CurrentSnapshot).Position;
                        reparseRangeEnd = _sections[containingSpanIndex].GetEndPoint(_buffer.CurrentSnapshot).Position;
                    }
                    else
                    {
                        // The edit must've happened outside of any existing section. However, it might've created new ones.
                        // E.g. if you create a new top-level function, it will be outside of any existing sections but will created at least one new one.
                        // We probably could support this, but there are weird cases where the edit actually just deleted the top-level section containing it.
                        // We we can just insert new sections, we need to remove old sections first. For now, we will just be safe and do full reparse here.
                        // Certainly a possibility for future perf optimization though.

                        _fullReparse = true;
                        reparseRangeStart = 0;
                        reparseRangeEnd = _buffer.CurrentSnapshot.Length;
                    }
#if DEBUG
                    Util.Log("ReparseFile() - partial reparse in range (" + reparseRangeStart + "," + reparseRangeEnd + ") ", _buffer.CurrentSnapshot.GetText().Substring(reparseRangeStart, reparseRangeEnd - reparseRangeStart));
#endif
                }

                if (reparseRangeEnd > reparseRangeStart)
                {
                    ReparseFile(reparseRangeStart, reparseRangeEnd, containingSpanIndex);
                }
                else
                {
#if DEBUG
                    Util.Log("ReparseFile() - Nothing to do - change must've happened outside of any sections.");
#endif
                }
            }
            finally
            {
                _fullReparse = false;
                _editSpan = null;
            }
        }

        void ReparseFile(int reparseRangeStart, int reparseRangeEnd, int containingSpanIndex)
        {
            var startTime = DateTime.Now.Ticks;

            var snapshot = _buffer.CurrentSnapshot;

#if DEBUG
            Util.Log("Entered ReparseFile(" + reparseRangeStart + "," + reparseRangeEnd + "," + containingSpanIndex + ")");
#endif

            var unbalanced = false;
            var currentSections = ReparseRange(snapshot, reparseRangeStart, reparseRangeEnd, out unbalanced);
#if DEBUG
            if (unbalanced)
            {
                Util.Log("Unbalanced curlies found in range (" + reparseRangeStart + "," + reparseRangeEnd + ") ", snapshot.GetText(reparseRangeStart, reparseRangeEnd - reparseRangeStart));
            }
#endif

            if (containingSpanIndex == -1)
            {
                // For now we treat this as a full reparse case even if it was a top-level edit not necessarily requiring full reparse.
                _sections = currentSections;

#if TopLevelEditIncrementalParse

                // Bring back this code when/if we support incremental reparse for top-level edits. See comments elsewhere in this
                // file about potential issues/complexity.

                if (_fullReparse) {
                    _sections = currentSections;
                }
                else {
                    // This is the case where edit happened outside of any current section. Provided the count of curlies
                    // was balanced, we can avoid full reparse and simply insert new sections in the right place. 
                    // Otherwise the safest thing to do is full reparse.
                    if (unbalanced) {
                        Log("Partial reparse failed because of the unbalanced curly count. Doing full reprase.");
                        reparseRangeStart = 0;
                        reparseRangeEnd = snapshot.Length;
                        _sections = ReparseRange(snapshot, reparseRangeStart, reparseRangeEnd, out unbalanced);
                    }
                    else {
                        if (currentSections.Count > 0) {
                            InsertNewSections(snapshot, reparseRangeStart, reparseRangeEnd, currentSections);
                        }
                    }
                }
#endif
            }
            else
            {
                if (currentSections.Count > 0)
                {
                    // We do depth-first traversal when we are collecting curlies, and we add them to the list as
                    // we return, so the outer-most section will be the last in the list.
                    int outerMostSectionStart = currentSections[currentSections.Count - 1].GetStartPoint(snapshot).Position;
                    int outerMostSectionEnd = currentSections[currentSections.Count - 1].GetEndPoint(snapshot).Position;

                    if (outerMostSectionStart == reparseRangeStart && outerMostSectionEnd == reparseRangeEnd)
                    {
                        ReplaceSectionRange(snapshot, containingSpanIndex, currentSections);
                    }
                    else
                    {
#if DEBUG
                        Util.Log("Partial reparse failed because (" + outerMostSectionStart + "," + outerMostSectionEnd + ") != (" + reparseRangeStart + "," + (reparseRangeEnd - 1) + ")");
#endif
                        reparseRangeStart = 0;
                        reparseRangeEnd = snapshot.Length;
                        _sections = ReparseRange(snapshot, reparseRangeStart, reparseRangeEnd, out unbalanced);
                    }
                }
            }

            if (TagsChanged != null)
                TagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshot, reparseRangeStart, reparseRangeEnd - reparseRangeStart)));

            long elapsedTime = DateTime.Now.Ticks - startTime;
#if DEBUG
            Util.Log("Exited ReparseFile(" + reparseRangeStart + "," + reparseRangeEnd + "," + containingSpanIndex + ") with elapsed time  of " + elapsedTime);
#endif
        }

        List<ITrackingSpan> ReparseRange(ITextSnapshot snapshot, int reparseRangeStart, int reparseRangeEnd, out bool unbalanced)
        {
            var currentSections = new List<ITrackingSpan>();
            var openCurlies = new Stack<int>();
            bool isInJavaScript = IsInJavaScriptBlock(reparseRangeStart);
            unbalanced = false;

            for (int i = reparseRangeStart; i < reparseRangeEnd; i++)
            {
                var point = new SnapshotSpan(snapshot, i, 1).Start;

                // If current point is not in a JS language block, return
                if (!isInJavaScript)
                {
                    if (_javaScriptBlockStarts.Contains(i))
                    {
                        // Our scan just barely entered a JavaScript block
                        isInJavaScript = true;

                        // We don't want opening curly from open JavaScript block to match with 
                        // a closing curly from another block, so purge all unmatched (dangling)
                        // opening curlies from the previous block.
                        openCurlies.Clear();
                    }
                    else
                        continue;
                }

                if (_javaScriptBlockEnds.Contains(i))
                    isInJavaScript = false;

                var ch = snapshot[i];
                if (ch == '{' || ch == '}')
                {
                    if (JScriptEditorUtil.IsClassifiedAs(_classifier, point,
                            JScriptClassifications.Comment,
                            JScriptClassifications.String,
                            JScriptClassifications.Operator))
                    {
                        continue;
                    }
                }
                switch (ch)
                {
                    case '{':
                        openCurlies.Push(i);
                        break;
                    case '}':
                        if (openCurlies.Count > 0)
                        {
                            int start = openCurlies.Pop();
                            var line = _buffer.CurrentSnapshot.GetLineFromPosition(i);
                            if (start < line.Start.Position)
                            {
                                currentSections.Add(snapshot.CreateTrackingSpan(start, i - start + 1, SpanTrackingMode.EdgeExclusive));
                            }
                        }
                        else
                        {
                            unbalanced = true;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (openCurlies.Count > 0)
            {
                unbalanced = true;
            }

            return currentSections;
        }

#if TopLevelEditIncrementalParse

        // NOTE - this is currently unused. There are too many special cases with top-level edits, e.g. an edit could've started inside
        // of a top-level section, then deleted one of the curlies, and became a top-level edit with no containing section. In this case
        // we need to remove existing section from the section list, and then insert new ones. Requires quite a bit of extra logic.
        // For now we will keep code reasonably simple and safe, and will only support incremental parsing for edits that occur inside of an 
        // existing section.

        /// <summary>
        /// This method gets called if there was an edit outside of any current section that added new sections
        /// In this case we simply want to insert new sections at the appropriate positition. This would be the case
        /// if a new top-level function got added.
        /// </summary>
        /// <param name="reparseRangeStart"></param>
        /// <param name="currentSections"></param>

        void InsertNewSections(ITextSnapshot snapshot, int reparseRangeStart, int reparseRangeEnd, List<ITrackingSpan> currentSections) {

            Log("Inserting new sections in range (" + reparseRangeStart + "," + reparseRangeEnd + ") ", snapshot.GetText(reparseRangeStart, reparseRangeEnd - reparseRangeStart));

            // First, find where to start inserting new sections
            int sectionIndex = 0;
            for (; sectionIndex < _sections.Count; sectionIndex++) {
                int currentSectionStart = _sections[sectionIndex].GetStartPoint(snapshot).Position;
                int currentSectionEnd = _sections[sectionIndex].GetEndPoint(snapshot).Position;
                if (currentSectionStart > reparseRangeStart) {
                    Log("Stopped on section (" + currentSectionStart + "," + currentSectionEnd + ")", snapshot.GetText(currentSectionStart, currentSectionEnd - currentSectionStart));
                    break;
                }
                else {
                    Log("Skipping section (" + currentSectionStart + "," + currentSectionEnd + ")", snapshot.GetText(currentSectionStart, currentSectionEnd - currentSectionStart));
                }
            }

            _sections.InsertRange(sectionIndex, currentSections);
        }
#endif

        void ReplaceSectionRange(ITextSnapshot snapshot, int containingSpanIndex, List<ITrackingSpan> replacementSections)
        {

#if DEBUG
            Util.Log("Entered ReplaceSectionRange(snapshot, " + containingSpanIndex + "," + replacementSections.Count);
#endif

            Debug.Assert(containingSpanIndex >= 0, "Negative containing index " + containingSpanIndex);
            if (containingSpanIndex < 0)
            {
                return;
            }

            // Avoid replacing as many sections as possible. First, walk both current section list
            // and replacement section list, and do nothing as long as sections at both lists are the
            // same. Start walking currentl list at containingSpanIndex and replacement list at 0.

            // We want to use loop counter outside of the loop. After the loop it is going to be the 
            // first mismatcing section index in the replacement section (or point beyond last section
            // in case if all replacement sections ended up being equal to current sections)
            var i = 0;

            for (i = 0; (i < replacementSections.Count) && (i <= containingSpanIndex); i++)
            {
                var currentSectionStart = _sections[containingSpanIndex - i].GetStartPoint(snapshot).Position;
                var currentSectionEnd = _sections[containingSpanIndex - i].GetEndPoint(snapshot).Position;
                var replacementSectionStart = replacementSections[replacementSections.Count - 1 - i].GetStartPoint(snapshot).Position;
                var replacementSectionEnd = replacementSections[replacementSections.Count - 1 - i].GetEndPoint(snapshot).Position;

                if (currentSectionStart != replacementSectionStart || currentSectionEnd != replacementSectionEnd)
                {
#if DEBUG
                    Util.Log("ReplaceSectionRange() - Stopping on unequal current range (" + currentSectionStart + "," + currentSectionEnd + ") ",
                        snapshot.GetText().Substring(currentSectionStart, currentSectionEnd - currentSectionStart));
                    Util.Log("ReplaceSectionRange() - Stopping on unequal replacement range (" + replacementSectionStart + "," + replacementSectionEnd + ") ",
                        snapshot.GetText().Substring(replacementSectionStart, replacementSectionEnd - replacementSectionStart));
#endif
                    break;
                }

#if DEBUG
                Util.Log("ReplaceSectionRange() - Skipping equal current range (" + currentSectionStart + "," + currentSectionEnd + ") ",
                    snapshot.GetText().Substring(currentSectionStart, currentSectionEnd - currentSectionStart));
                Util.Log("ReplaceSectionRange() - Skipping equal replacement range (" + replacementSectionStart + "," + replacementSectionEnd + ") ",
                    snapshot.GetText().Substring(replacementSectionStart, replacementSectionEnd - replacementSectionStart));
#endif
            }

            var j = i;

            var containingSectionStart = _sections[containingSpanIndex].GetStartPoint(snapshot).Position;
            var containingSectionEnd = _sections[containingSpanIndex].GetEndPoint(snapshot).Position;

            // Now go find all remaining sections in the current section list that are contained withing our edit-containing span
            // We will need to remove them and replace them with the remaining sections from the replacement list (if any)
            while (j <= containingSpanIndex)
            {
                var currentSectionStart = _sections[containingSpanIndex - j].GetStartPoint(snapshot).Position;
                var currentSectionEnd = _sections[containingSpanIndex - j].GetEndPoint(snapshot).Position;

                if (currentSectionEnd < containingSectionStart)
                {
#if DEBUG
                    Util.Log("Not removing current range (" + currentSectionStart + "," + currentSectionEnd + ")");
#endif
                    break;
                }
#if DEBUG
                Util.Log("Removing current range (" + currentSectionStart + "," + currentSectionEnd + ")");
#endif
                j++;
            }

            if (j > i)
            {
                // After the loop, if j changed, containingSpanIndex - j it will be pointing at the last
                // element that should *not* be removed. So we need to use formula containingSpanIndex - j + 1 
                // to find the first element to be removed.
                _sections.RemoveRange(containingSpanIndex - j + 1, j - i);
            }

            // Now we can finally insert the remaining sections from replacement sections
            if (i < replacementSections.Count)
            {
                if (i > 0)
                {
                    replacementSections.RemoveRange(replacementSections.Count - i, i);
                }

                // Insert in the same place that we removed items from
                _sections.InsertRange(containingSpanIndex - j + 1, replacementSections);
            }
        }

        ~OutliningTagger() { }
    }
}