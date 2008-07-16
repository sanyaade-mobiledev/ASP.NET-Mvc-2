using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Web.Mvc
{
    public interface IHtmlElement
    {
        IDictionary<string, string> Attributes { get; }
        string TagName { get; }
        string InnerHtml { get; set; }
    }
}
