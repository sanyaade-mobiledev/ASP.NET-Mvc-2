using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace JScriptIntelliSenseParaExtension
{
    public class ScriptSignatureHelpReplacementSource : ISignatureHelpSource
    {
        ITextBuffer _textBuffer;

        public ScriptSignatureHelpReplacementSource(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
        }

        public void AugmentSignatureHelpSession(ISignatureHelpSession session, IList<ISignature> signatures)
        {
            var newSigs = signatures.Select(s => new ScriptSignature(_textBuffer, s)).ToList();
            signatures.Clear();
            newSigs.ForEach(s => signatures.Add(s));
        }

        public ISignature GetBestMatch(ISignatureHelpSession session)
        {
            // Returning null will default to the next signature help source's best match (the built-in JS signature help source)
            return null;
        }

        private bool _isDisposed;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }
    }
}