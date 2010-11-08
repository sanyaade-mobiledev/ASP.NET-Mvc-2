using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace JScriptIntelliSenseParaExtension
{
    [Export(typeof(IVsTextViewCreationListener))]
    [Name("JScript IntelliSense <para> Extension - Completion Helper")]
    [ContentType("jscript")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    sealed class ViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        public ICompletionBroker CompletionBroker { get; set; }

        [Import]
        IVsEditorAdaptersFactoryService AdaptersFactory { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var view = AdaptersFactory.GetWpfTextView(textViewAdapter);

            var watcher = new ScriptCompletionWatcher(view, CompletionBroker);
            IOleCommandTarget nextCmdTarget;
            textViewAdapter.AddCommandFilter(watcher, out nextCmdTarget);
            watcher.NextCmdTarget = nextCmdTarget;
        }
    }
}