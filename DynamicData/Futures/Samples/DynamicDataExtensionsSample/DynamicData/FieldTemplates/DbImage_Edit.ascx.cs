using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.DynamicData;
using System.Collections.Specialized;
using Microsoft.Web.DynamicData.Extensions;

namespace DynamicDataExtensionsSample {
    public partial class DbImage_Edit : FieldTemplateUserControl {

        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);

            //check if image exists
            if (FieldValue == null) {
                return;
            }

            //metadata
            int width = 100;
            int height = 100;
            bool displayinedit = true;

            var metadata = MetadataAttributes.OfType<ImageFormatAttribute>().FirstOrDefault();
            if (metadata != null) {
                width = metadata.DisplayWidth;
                height = metadata.DisplayHeight;
                displayinedit = metadata.DisplayInEdit;
            }

            string tablename = Column.Table.Name;
            string primarykeyname = Column.Table.PrimaryKeyColumns[0].Name;
            string primarykeyvalue = DataBinder.GetPropertyValue(Row, primarykeyname).ToString();
            string cachekey = string.Format("{0}:{1}:{2}", tablename, Column.Name, primarykeyvalue);
            ViewState["CacheKey"] = cachekey;

            //display picture if in edit mode, 
            if (Mode == DataBoundControlMode.Edit && displayinedit) {
                //set image properties
                PlaceHolderImage.Visible = true;
                ImageEdit.ImageUrl = "~/ImageHandler.ashx?table=" + tablename + "&column=" + Column.Name + "&pkv=" + primarykeyvalue + "&width=" + width.ToString() + "&height=" + height.ToString();
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary) {
            string cachekey = (string)ViewState["CacheKey"];
            if (!string.IsNullOrEmpty(cachekey)) {
                Cache.Remove(cachekey);
            }
            if (FileUploadEdit.HasFile) {
                dictionary[Column.Name] = new System.Data.Linq.Binary(FileUploadEdit.FileBytes);
            } else {
                dictionary[Column.Name] = null;
            }
        }

    }
}