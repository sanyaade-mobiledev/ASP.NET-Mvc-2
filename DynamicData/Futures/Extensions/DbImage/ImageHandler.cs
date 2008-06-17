using System;
using System.Web;

namespace Microsoft.Web.DynamicData.Extensions {

    public class ImageHandler : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            string tablename = context.Request["table"];
            string column = context.Request["column"];
            string primarykeyvalue = context.Request["pkv"];
            int width = Convert.ToInt32(context.Request["width"]);
            int height = Convert.ToInt32(context.Request["height"]);

            string cachekey = string.Format("{0}:{1}:{2}", tablename, column, primarykeyvalue);
            CachedImages images = (CachedImages)context.Cache[cachekey];
            if (images == null) {
                //get image bytes
                byte[] bytes;
                object picture = LinqImageHelper.LoadImage(tablename, primarykeyvalue, column);
                if (picture is byte[]) {
                    bytes = (byte[])picture;
                } else if (picture is System.Data.Linq.Binary) {
                    bytes = ((System.Data.Linq.Binary)picture).ToArray();
                } else {
                    bytes = null;
                }

                if (bytes != null) {
                    if (bytes.Length > 78 && bytes[0] == 0x15 && bytes[1] == 0x1c && bytes[2] == 0x2f) {
                        //hack to strip off OLE header for Northwind categories table
                        byte[] newbytes = new byte[bytes.Length - 78];
                        Array.Copy(bytes, 78, newbytes, 0, bytes.Length - 78);
                        bytes = newbytes;
                    }

                    images = new CachedImages(bytes);

                    //cache image
                    context.Cache.Add(cachekey, images, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(1), System.Web.Caching.CacheItemPriority.Low, null);
                }
            }

            if (images != null) {
                context.Response.ContentType = images.ContentType;
                context.Response.BinaryWrite(images.GetImage(width, height));
                context.Response.Cache.SetNoStore();
            } else {
                context.Response.ContentType = "image/gif";
                context.Response.WriteFile(context.Server.MapPath("~/DynamicData/Content/Images/Blank.gif"));
            }
        }

        public bool IsReusable {
            get { return true; }
        }

    }

}

