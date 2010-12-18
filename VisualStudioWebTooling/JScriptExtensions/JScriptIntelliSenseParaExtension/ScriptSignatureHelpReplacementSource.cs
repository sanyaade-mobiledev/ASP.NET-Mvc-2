using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.Reflection;

namespace JScriptIntelliSenseParaExtension
{
    public class ScriptSignatureHelpReplacementSource : ISignatureHelpSource
    {
        static PropertyInfo _sigDocoProp;
        static PropertyInfo _paramDocoProp;

        public ScriptSignatureHelpReplacementSource() { }

        public void AugmentSignatureHelpSession(ISignatureHelpSession session, IList<ISignature> signatures)
        {
            if (signatures.Count == 0)
                return;

            // Update signature doco
            var sig = signatures.First();
            if (_sigDocoProp == null) _sigDocoProp = sig.GetType().GetProperty("Documentation");
            var newDoco = DocCommentHelper.ProcessParaTags(sig.Documentation);
            _sigDocoProp.SetValue(sig, newDoco, null);

            // Update parameter doco
            if (_paramDocoProp == null && sig.Parameters.Count > 0)
                _paramDocoProp = sig.Parameters.First().GetType().GetProperty("Documentation");
            foreach (var parameter in sig.Parameters)
            {
                newDoco = DocCommentHelper.ProcessParaTags(parameter.Documentation);
                _paramDocoProp.SetValue(parameter, newDoco, null);
            }
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