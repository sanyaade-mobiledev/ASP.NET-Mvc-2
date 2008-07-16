using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Web.DynamicData;

namespace DynamicDataFuturesSample {
    public partial class DbImage_Edit : FieldTemplateUserControl {

        protected void Page_Load(object sender, EventArgs e) {
            CustomValidator1.ServerValidate += new ServerValidateEventHandler(CustomValidator1_ServerValidate);
        }

        void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args) {
            if (FileUploadEdit.HasFile) {
                var bytes = FileUploadEdit.FileBytes;
                try {
                    var a = System.Drawing.Image.FromStream(new MemoryStream(bytes));
                } catch {
                    args.IsValid = false;
                    CustomValidator1.ErrorMessage = "Invalid image file.";
                }
            } else {
                if (Column.IsRequired) {
                    var attribute = MetadataAttributes.OfType<RequiredAttribute>().DefaultIfEmpty(new RequiredAttribute()).First();
                    CustomValidator1.ErrorMessage = attribute.FormatErrorMessage(Column.GetDisplayName());
                    args.IsValid = false;
                }
            }
        }

        protected override void OnDataBinding(EventArgs e) {
            base.OnDataBinding(e);

            //check if image exists
            if (FieldValue == null) {
                return;
            }

            //get metadata
            int width = 100;
            int height = 100;
            bool displayinedit = true;
            var metadata = MetadataAttributes.OfType<ImageFormatAttribute>().FirstOrDefault();
            if (metadata != null) {
                width = metadata.DisplayWidth;
                height = metadata.DisplayHeight;
                displayinedit = metadata.DisplayInEdit;
            }

            //Build and store cache key
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Column.Table.PrimaryKeyColumns.Count; i++) {
                if (sb.Length > 0) {
                    sb.Append(',');
                }
                sb.Append(DataBinder.GetPropertyValue(Row, Column.Table.PrimaryKeyColumns[i].Name));
            }
            string cachekey = string.Format(
                "{0}:{1}:{2}",
                Column.Table.Name,
                Column.Name,
                sb.ToString()
            );
            ViewState["CacheKey"] = cachekey;

            //display picture if in edit mode, 
            if (Mode == DataBoundControlMode.Edit && displayinedit) {
                //set image properties
                PlaceHolderImage.Visible = true;
                sb = new StringBuilder();
                sb.AppendFormat(
                    "~/ImageHandler.ashx?table={0}&column={1}&width={2}&height={3}",
                    HttpUtility.UrlEncode(Column.Table.Name),
                    HttpUtility.UrlEncode(Column.Name),
                    width,
                    height
                );
                for (int i = 0; i < Column.Table.PrimaryKeyColumns.Count; i++) {
                    sb.AppendFormat("&pk{0}={1}", i, DataBinder.GetPropertyValue(Row, Column.Table.PrimaryKeyColumns[i].Name));
                }
                ImageEdit.ImageUrl = sb.ToString();
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary) {
            string cachekey = (string)ViewState["CacheKey"];
            if (!string.IsNullOrEmpty(cachekey)) {
                Cache.Remove(cachekey);
            }
            if (FileUploadEdit.HasFile) {
                if (Column.ColumnType == typeof(System.Data.Linq.Binary)) {
                    dictionary[Column.Name] = new System.Data.Linq.Binary(FileUploadEdit.FileBytes);
                } else {
                    dictionary[Column.Name] = FileUploadEdit.FileBytes;
                }
            } else {
                dictionary[Column.Name] = null;
            }
        }

    }
}