using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace Microsoft.Web.DynamicData.Extensions {
    public class DynamicLabel : Label {
        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            var dynCtrl = FindControl(AssociatedControlID) as DynamicControl;
            var ftuc = dynCtrl.FieldTemplate as FieldTemplateUserControl;
            AssociatedControlID = ftuc.ID + "$" + ftuc.DataControl.ID;
        }
    }
}
