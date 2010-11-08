using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace JScriptIntelliSenseParaExtension
{
    internal class ScriptSignature : ISignature
    {
        internal ScriptSignature(ITextBuffer subjectBuffer, ISignature origSignature)
        {
            subjectBuffer.Changed += (sender, e) => ComputeCurrentParameter();
            CopyFrom(origSignature);
        }

        private void CopyFrom(ISignature source)
        {
            if (source == null)
                return;

            ApplicableToSpan = source.ApplicableToSpan;
            Content = source.Content;
            Documentation = DocCommentHelper.ProcessParaTags(source.Documentation);
            PrettyPrintedContent = source.PrettyPrintedContent;
            Parameters = new ReadOnlyCollection<IParameter>(
                source.Parameters.Select(p => new ScriptParameter(p, this) as IParameter)
                .ToList());
            _currentParameter = Parameters.FirstOrDefault();
        }

        public event EventHandler<CurrentParameterChangedEventArgs> CurrentParameterChanged;
        private void OnCurrentParamaterChanged(CurrentParameterChangedEventArgs e)
        {
            if (CurrentParameterChanged != null)
            {
                CurrentParameterChanged(this, e);
            }
        }

        public ITrackingSpan ApplicableToSpan { get; private set; }
        public string Content { get; private set; }
        public string Documentation { get; private set; }
        public ReadOnlyCollection<IParameter> Parameters { get; private set; }
        public string PrettyPrintedContent { get; private set; }

        private IParameter _currentParameter;
        public IParameter CurrentParameter
        {
            get { return _currentParameter; }
            private set
            {
                if (_currentParameter != value)
                {
                    var prevCurrentParameter = _currentParameter;
                    _currentParameter = value;
                    OnCurrentParamaterChanged(new CurrentParameterChangedEventArgs(prevCurrentParameter, _currentParameter));
                }
            }
        }

        private void ComputeCurrentParameter()
        {
            if (Parameters.Count == 0)
            {
                CurrentParameter = null;
                return;
            }

            // the number of commas in the string is the index of the current parameter
            var sigText = ApplicableToSpan.GetText(ApplicableToSpan.TextBuffer.CurrentSnapshot);

            var commaCount = GetCommaCount(sigText);
            if (commaCount < Parameters.Count)
            {
                CurrentParameter = Parameters[commaCount];
            }
            else
            {
                // too many commas
                CurrentParameter = null;
            }
        }

        private int GetCommaCount(string input)
        {
            var currentIndex = 0;
            var commaCount = 0;
            while (currentIndex < input.Length)
            {
                int commaIndex = input.IndexOf(',', currentIndex);
                if (commaIndex == -1)
                    break;

                commaCount++;
                currentIndex = commaIndex + 1;
            }
            return commaCount;
        }
    }
}