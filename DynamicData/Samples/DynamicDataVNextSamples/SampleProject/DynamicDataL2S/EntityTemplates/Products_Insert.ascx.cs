﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

namespace DynamicDataProject.DynamicDataL2S.EntityTemplates {
    public partial class Products_Insert : System.Web.DynamicData.EntityTemplateUserControl {
        protected void DynamicControl_Load(object sender, EventArgs e) {
            ((DynamicControl)sender).ValidationGroup = ValidationGroup;
        }
    }
}