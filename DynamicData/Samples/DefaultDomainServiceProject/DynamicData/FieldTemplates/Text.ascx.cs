using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDataProject {
    public partial class TextField : System.Web.DynamicData.FieldTemplateUserControl {
        public override Control DataControl {
            get {
                return Literal1;
            }
        }
    
    }
}
