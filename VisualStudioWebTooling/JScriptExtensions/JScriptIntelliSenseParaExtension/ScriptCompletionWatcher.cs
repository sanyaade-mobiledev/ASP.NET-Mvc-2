using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;

namespace JScriptIntelliSenseParaExtension
{
    sealed class ScriptCompletionWatcher : IOleCommandTarget
    {
        IWpfTextView _textView;
        ICompletionBroker _broker;

        internal IOleCommandTarget NextCmdTarget { get; set; }

        public ScriptCompletionWatcher(IWpfTextView view, ICompletionBroker broker)
        {
            _textView = view;
            _broker = broker;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (_broker.GetSessions(_textView).Any())
            {
                if (UserTypedOperator(pguidCmdGroup, nCmdID, pvaIn))
                {
                    ForActiveCompletionSessions(s => s.Dismiss());
                    Debug.WriteLine("Dismissed all active completion sessions because user typed an operator");
                }
                else if (UserPerfomedCommitAction(pguidCmdGroup, nCmdID))
                {
                    ForActiveCompletionSessions(s =>
                        {
                            if (nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB // Always complete on TAB
                                || s.SelectedCompletionSet.SelectionStatus.IsSelected) // If completion is selected (not just highlighted)
                                s.Commit();
                            else
                                s.Dismiss();
                        });
                    return VSConstants.S_OK; // Don't let anybody else handle this command
                }
            }
            return NextCmdTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        static bool UserTypedOperator(Guid pguidCmdGroup, uint nCmdID, IntPtr pvaIn)
        {
            var isOperator = new Func<ushort, bool>(u => 
                {
                    var c = (char)u;
                    // letters, digits, '_' & '$' are valid in an identifier
                    return !char.IsLetterOrDigit(c) && c != '_' && c != '$';
                }
            );

            return pguidCmdGroup == VSConstants.VSStd2K &&
                   nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR &&
                   isOperator((ushort)Marshal.GetObjectForNativeVariant(pvaIn));
        }

        static bool UserPerfomedCommitAction(Guid pguidCmdGroup, uint nCmdID)
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
