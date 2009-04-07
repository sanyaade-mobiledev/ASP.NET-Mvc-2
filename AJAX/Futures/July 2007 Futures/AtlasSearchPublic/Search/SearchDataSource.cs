namespace Microsoft.Web.Preview.Search {
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [NonVisualControl]
    public class SearchDataSource : ObjectDataSource {
        public SearchDataSource() {
            TypeName = "Microsoft.Web.Preview.Search.SearchService";
            SelectMethod = "Search";
        }

        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)
        ]
        public override bool Visible {
            get {
                return true;
            }
            set {
                throw new NotImplementedException();
            }
        }
    }
}