using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDataEFProject {
    public partial class ManyToManyField : System.Web.DynamicData.FieldTemplateUserControl {
        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);
    
            // Get the real entity from the wrapper
            object entity = ((ICustomTypeDescriptor)Row).GetPropertyOwner(null);
    
            // Get the collection and make sure it's loaded
            RelatedEnd entityCollection = (RelatedEnd)Column.EntityTypeProperty.GetValue(entity, null);
            if (!entityCollection.IsLoaded)
                entityCollection.Load();
    
            // Bind the repeater to the list of children entities
            Repeater1.DataSource = entityCollection;
            Repeater1.DataBind();
        }
    
        public override Control DataControl {
            get {
                return Repeater1;
            }
        }
    
    }
}
