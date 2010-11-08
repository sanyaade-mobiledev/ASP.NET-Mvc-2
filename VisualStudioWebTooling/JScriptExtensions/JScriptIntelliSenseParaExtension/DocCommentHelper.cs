using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JScriptIntelliSenseParaExtension
{
    internal static class DocCommentHelper
    {
        internal static string ProcessParaTags(string input)
        {
            return input == null ? null : input.Replace("<para>", Environment.NewLine).Replace("</para>", "");
        }
    }
}