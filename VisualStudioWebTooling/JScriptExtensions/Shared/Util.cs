using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.VisualStudio.Text;

namespace JScriptPowerTools.Shared
{
    public static class Util
    {
        // TODO: Refactor this to use System.Diagnostics.Trace

        private static void Log(string message, string code)
        {
            Log(String.Format("{0} \"{1}\"", message, code.Replace("\r\n", String.Empty)));
        }

        public static void Log(string msg)
        {
            Debug.WriteLine(msg);
        }

        public static void Log(string msg, params object[] args)
        {
            Debug.WriteLine(msg, args);
        }

        public static void LogTextChanges(INormalizedTextChangeCollection textChanges)
        {
            foreach (ITextChange textChange in textChanges)
            {
                Log("Text change: (" + textChange.OldPosition + "," + textChange.OldEnd + ") \"" + textChange.OldText + "\" =>(" +
                    textChange.NewPosition + "," + textChange.NewEnd + ") \"" + textChange.NewText + "\"");
            }
        }
    }
}
