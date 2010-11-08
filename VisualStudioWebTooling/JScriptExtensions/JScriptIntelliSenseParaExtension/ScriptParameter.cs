using System;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace JScriptIntelliSenseParaExtension
{
    public class ScriptParameter : IParameter
    {
        public ScriptParameter(IParameter origParameter, ISignature signature)
        {
            CopyFrom(origParameter);
            Signature = signature;
        }

        private void CopyFrom(IParameter source)
        {
            Name = source.Name;
            Locus = source.Locus;
            PrettyPrintedLocus = source.PrettyPrintedLocus;
            Documentation = DocCommentHelper.ProcessParaTags(source.Documentation);
        }

        public string Documentation { get; private set; }
        public Span Locus { get; private set; }
        public string Name { get; private set; }
        public Span PrettyPrintedLocus { get; private set; }
        public ISignature Signature { get; private set; }
    }
}