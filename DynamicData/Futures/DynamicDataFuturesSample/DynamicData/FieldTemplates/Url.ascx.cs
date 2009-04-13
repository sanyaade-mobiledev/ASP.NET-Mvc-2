using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

namespace DynamicDataProject
{
    public partial class UrlField : System.Web.DynamicData.FieldTemplateUserControl {
        protected override void OnDataBinding(EventArgs e) {
        HyperLinkUrl.NavigateUrl = ProcessUrl(FieldValueString);
    }

    private string ProcessUrl(string url) {
        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
            return url;    
        }

        return "http://" + url;
    }

    public override Control DataControl {
        get {
            return HyperLinkUrl;
        }
    }
    }
}
