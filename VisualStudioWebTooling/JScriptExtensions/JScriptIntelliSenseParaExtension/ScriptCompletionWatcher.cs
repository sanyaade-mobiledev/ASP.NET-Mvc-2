﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using JScriptPowerTools.Shared;

namespace JScriptIntelliSenseParaExtension
{
    sealed class ScriptCompletionWatcher : IOleCommandTarget
    {
        IWpfTextView _textView;
        ICompletionBroker _broker;
        ILanguageBlockManager _lbm;

        private bool IsHtmlFile { get { return _lbm != null; } }

        internal IOleCommandTarget NextCmdTarget { get; set; }

        public ScriptCompletionWatcher(IWpfTextView view, ICompletionBroker broker)
        {
            _textView = view;
            _broker = broker;

            var htmlBuffers = view.BufferGraph.GetTextBuffers(tb => tb.ContentType.TypeName.Equals("HTML", StringComparison.OrdinalIgnoreCase));

            if (htmlBuffers.Any())
                _lbm = VsServiceManager.GetLanguageBlockManager(htmlBuffers.First());
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            //_broker.GetSessions(_textView).Any()
            //_broker.IsCompletionActive(_textView)
            if (_broker.IsCompletionActive(_textView))
            {
                if (!IsHtmlFile ||
                    (_broker.GetSessions(_textView).Any(cs =>
                          cs.IsStarted && !cs.IsDismissed && cs.CompletionSets.Any(set =>
                            JScriptEditorUtil.IsInJScriptLanguageBlock(_lbm, set.ApplicableTo.GetStartPoint(_textView.TextSnapshot))))))
                {
                    var userTypedPeriod = UserTypedPeriod(pguidCmdGroup, nCmdID, pvaIn);
                    var userTypedParens = UserTypedParens(pguidCmdGroup, nCmdID, pvaIn);
                    if (UserPerfomedCommitAction(pguidCmdGroup, nCmdID, pvaIn) || userTypedPeriod || userTypedParens)
                    {
                        ForActiveCompletionSessions(s =>
                            {
                                if (nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB // Always complete on TAB
                                    || s.SelectedCompletionSet.SelectionStatus.IsSelected) // If completion is selected (not just highlighted)
                                    s.Commit();
                                else
                                    s.Dismiss();
                            });
                        if (!userTypedPeriod && !userTypedParens)
                        {
                            return VSConstants.S_OK; // Don't let anybody else handle this command
                        }
                    }
                    if (UserTypedOperator(pguidCmdGroup, nCmdID, pvaIn) && !userTypedPeriod)
                    {
                        ForActiveCompletionSessions(s => s.Dismiss());
                        Debug.WriteLine("Dismissed all active completion sessions because user typed an operator");
                    }
                }
            }
            return NextCmdTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        static bool UserTypedPeriod(Guid pguidCmdGroup, uint nCmdID, IntPtr pvaIn)
        {
            return UserTyped(pguidCmdGroup, nCmdID, pvaIn, c => c == '.');
        }

        static bool UserTypedParens(Guid pguidCmdGroup, uint nCmdID, IntPtr pvaIn)
        {
            return UserTyped(pguidCmdGroup, nCmdID, pvaIn, c => c == '(');
        }

        static bool UserTypedOperator(Guid pguidCmdGroup, uint nCmdID, IntPtr pvaIn)
        {
            return UserTyped(pguidCmdGroup, nCmdID, pvaIn,
                c => !char.IsLetterOrDigit(c) && c != '_' && c != '$'); // letters, digits, '_' & '$' are valid in an identifier
        }

        static bool UserTyped(Guid pguidCmdGroup, uint nCmdID, IntPtr pvaIn, Func<char, bool> predicate)
        {
            return pguidCmdGroup == VSConstants.VSStd2K &&
                   nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR &&
                   predicate((char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn));
        }

        static bool UserPerfomedCommitAction(Guid pguidCmdGroup, uint nCmdID, IntPtr pvaIn)
        {
            return pguidCmdGroup == VSConstants.VSStd2K &&
                    (nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB ||
                     nCmdID == (uint)VSConstants.VSStd2KCmdID.OPENLINEABOVE ||
                     nCmdID == (uint)VSConstants.VSStd2KCmdID.RETURN);
        }

        void ForActiveCompletionSessions(Action<ICompletionSession> action)
        {
            foreach (var session in _broker.GetSessions(_textView))
            {
                if (!session.IsDismissed)
                    action(session);
            }
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return NextCmdTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }
}