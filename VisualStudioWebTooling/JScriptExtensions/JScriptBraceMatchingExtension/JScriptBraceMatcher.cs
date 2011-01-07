using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JScriptPowerTools.Shared;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace JScriptBraceMatchingExtension
{
    internal class JScriptBraceMatcher
    {
        internal static bool FindMatchingBrace(ILanguageBlockManager lbm, IClassifier classifier, SnapshotPoint startPoint, bool findClosing, char open, char close, int maxLines, ref bool maxLinesReached, out SnapshotSpan pairSpan)
        {
            // Get script block we're working in
            // lbm will be null if this is not in an HTML file
            var scriptBlock = lbm != null
                ? JScriptEditorUtil.GetJavaScriptBlockSpans(lbm)
                    .Single(b => b.Start <= startPoint.Position && b.End >= startPoint.Position)
                : new BlockSpan(0, startPoint.Snapshot.TextBuffer.CurrentSnapshot.Length, true);

            return findClosing
                ? FindMatchingCloseChar(scriptBlock, classifier, startPoint, open, close, maxLines, ref maxLinesReached, out pairSpan)
                : FindMatchingOpenChar(scriptBlock, classifier, startPoint, close, open, maxLines, ref maxLinesReached, out pairSpan);
        }

        // TODO: Can we refactor FindMatchingCloseChar and FindMatchingOpenChar into a single method?

        static bool FindMatchingCloseChar(BlockSpan scriptBlock, IClassifier classifier, SnapshotPoint startPoint, char open, char close, int maxLines, ref bool maxLinesReached, out SnapshotSpan pairSpan)
        {
            var isHTML = startPoint.Snapshot.ContentType.TypeName.Equals("HTML", StringComparison.OrdinalIgnoreCase);
            pairSpan = new SnapshotSpan(startPoint.Snapshot, 1, 1);
            var line = startPoint.GetContainingLine();
            var lineText = line.GetText();
            var lineNumber = line.LineNumber;
            var offset = startPoint.Position - line.Start.Position + 1;
            var endOfScriptBlockReached = false;

            var stopLineNumber = startPoint.Snapshot.LineCount - 1;
            if (maxLines > 0)
                stopLineNumber = Math.Min(stopLineNumber, lineNumber + maxLines);
#if DEBUG
            var startTicks = DateTime.Now.Ticks;
            Util.Log("Looking for matching closing brace '{0}'", close);
#endif
            var openCount = 0;

            while (true)
            {
                // walk the entire line
                while (offset < line.Length)
                {
                    // Check if we've hit the end of the script block
                    if (scriptBlock.End < line.Start + offset)
                    {
                        // We've hit the end of a script block
                        endOfScriptBlockReached = true;
                        break;
                    }

                    char currentChar = lineText[offset];
                    var currentCharPoint = line.Start + offset;

                    if (currentChar == close) // found the close character
                    {
                        if (JScriptEditorUtil.IsClassifiedAs(classifier, currentCharPoint,
                                JScriptClassifications.Comment,
                                JScriptClassifications.String,
                                JScriptClassifications.Operator))
                        {
#if DEBUG
                            Util.Log("Candidate closing brace found but ignored because it was a comment, string or operator");
#endif
                        }
                        else if (openCount > 0)
                        {
                            openCount--;
                        }
                        else // found the matching close
                        {
#if DEBUG
                            Util.Log("Matching closing brace '{0}' found after {1}ms", close, TimeSpan.FromTicks(DateTime.Now.Ticks - startTicks).TotalMilliseconds);
#endif
                            pairSpan = new SnapshotSpan(startPoint.Snapshot, currentCharPoint, 1);
                            return true;
                        }
                    }
                    else if (currentChar == open) // this is another open
                    {
                        if (JScriptEditorUtil.IsClassifiedAs(classifier, currentCharPoint,
                                JScriptClassifications.Comment,
                                JScriptClassifications.String,
                                JScriptClassifications.Operator))
                        {
#if DEBUG
                            Util.Log("New opening brace found but ignored because it was a comment, string or operator");
#endif
                        }
                        else
                        {
                            openCount++;
                        }
                    }
                    offset++;
                }

                if (endOfScriptBlockReached)
                    break;

                // move on to the next line
                if (++lineNumber > stopLineNumber)
                {
#if DEBUG
                    Util.Log("Reached max lines ({0}), stopped looking at line {1} after {2}ms", maxLines, stopLineNumber, TimeSpan.FromTicks(DateTime.Now.Ticks - startTicks).TotalMilliseconds);
#endif
                    // Set maxLinesReached flag only if we actually hit max lines limit, not just end of buffer
                    maxLinesReached = stopLineNumber >= maxLines;
                    break;
                }

                // Move to next line
                line = line.Snapshot.GetLineFromLineNumber(lineNumber);
                lineText = line.GetText();
                offset = 0;
            }
#if DEBUG
            Util.Log("Matching closing brace '{0}' was not found. Open count: {1} after {2}ms", close, openCount, TimeSpan.FromTicks(DateTime.Now.Ticks - startTicks).TotalMilliseconds);
#endif
            return false;
        }

        static bool FindMatchingOpenChar(BlockSpan scriptBlock, IClassifier classifier, SnapshotPoint startPoint, char open, char close, int maxLines, ref bool maxLinesReached, out SnapshotSpan pairSpan)
        {
            var isHTML = startPoint.Snapshot.ContentType.TypeName.Equals("HTML", StringComparison.OrdinalIgnoreCase);
            pairSpan = new SnapshotSpan(startPoint, startPoint);
            ITextSnapshotLine line = startPoint.GetContainingLine();
            var lineNumber = line.LineNumber;
            var offset = startPoint - line.Start - 1; // move the offset to the character before this one
            var startOfScriptBlockReached = false;

            // if the offset is negative, move to the previous line
            if (offset < 0)
            {
                line = line.Snapshot.GetLineFromLineNumber(--lineNumber);
                offset = line.Length - 1;
            }

            var lineText = line.GetText();

            var stopLineNumber = 0;
            var nominalStopLineNumber = lineNumber - maxLines;
            if (maxLines > 0)
                stopLineNumber = Math.Max(stopLineNumber, nominalStopLineNumber);
#if DEBUG
            var startTicks = DateTime.Now.Ticks;
            Util.Log("Looking for matching opening brace '{0}'", open);
#endif
            var closeCount = 0;

            Debug.Assert(scriptBlock != null);

            while (true)
            {
                // Walk the entire line
                while (offset >= 0)
                {
                    // Check if we've hit the start of the script block
                    if (scriptBlock.Start > line.Start + offset)
                    {
                        // We've hit the end of a script block
                        startOfScriptBlockReached = true;
                        break;
                    }

                    char currentChar = lineText[offset];
                    var currentCharPoint = line.Start + offset;

                    if (currentChar == open) // found the open character
                    {
                        if (JScriptEditorUtil.IsClassifiedAs(classifier, currentCharPoint,
                                JScriptClassifications.Comment,
                                JScriptClassifications.String,
                                JScriptClassifications.Operator))
                        {
#if DEBUG
                            Util.Log("Candidate opening brace found but ignored because it was a comment, string or operator");
#endif
                        }
                        else if (closeCount > 0)
                        {
                            closeCount--;
                        }
                        else // We've found the matching open character
                        {
#if DEBUG
                            Util.Log("Matching opening brace '{0}' found in {1}ms", open, TimeSpan.FromTicks(DateTime.Now.Ticks - startTicks).TotalMilliseconds);
#endif
                            pairSpan = new SnapshotSpan(currentCharPoint, 1); // we just want the character itself
                            return true;
                        }
                    }
                    else if (currentChar == close)
                    {
                        if (JScriptEditorUtil.IsClassifiedAs(classifier, currentCharPoint,
                                JScriptClassifications.Comment,
                                JScriptClassifications.String,
                                JScriptClassifications.Operator))
                        {
#if DEBUG
                            Util.Log("New closing brace found but ignored because it was a comment, string or operator");
#endif
                        }
                        else
                        {
                            closeCount++;
                        }
                    }
                    offset--;
                }

                if (startOfScriptBlockReached)
                    break;

                // Move to the previous line
                if (--lineNumber < stopLineNumber)
                {
                    // Set maxLinesReached flag only if we actually hit max lines limit, not just start of buffer
                    maxLinesReached = stopLineNumber == nominalStopLineNumber;
#if DEBUG
                    Util.Log("Reached max lines ({0}, stopped looking at line {1} after {2}ms", maxLines, stopLineNumber, TimeSpan.FromTicks(DateTime.Now.Ticks - startTicks).TotalMilliseconds);
#endif
                    break;
                }

                line = line.Snapshot.GetLineFromLineNumber(lineNumber);
                lineText = line.GetText();
                offset = line.Length - 1;
            }
#if DEBUG
            Util.Log("Matching opening brace '{0}' was not found. Open count: {1} after {2}ms", open, closeCount, TimeSpan.FromTicks(DateTime.Now.Ticks - startTicks).TotalMilliseconds);
#endif
            return false;
        }
    }
}