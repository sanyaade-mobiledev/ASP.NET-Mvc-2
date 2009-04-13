using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

namespace DynamicDataProject.DynamicData.EntityTemplates {
    public partial class Products_Edit : System.Web.DynamicData.EntityTemplateUserControl {
        protected void DynamicControl_Load(object sender, EventArgs e) {
            ((DynamicControl)sender).ValidationGroup = ValidationGroup;
        }
    }
}