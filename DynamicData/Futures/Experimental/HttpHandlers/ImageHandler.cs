using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.DynamicData;

namespace Microsoft.Web.DynamicData {

    public class ImageHandler : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            // get request parameters
            string tableName = context.Request.QueryString["table"];
            string column = context.Request.QueryString["column"];
            int width = Convert.ToInt32(context.Request.QueryString["width"]);
            int height = Convert.ToInt32(context.Request.QueryString["height"]);

            StringBuilder sb = new StringBuilder();
            List<string> primaryKeyValues = new List<string>();
            int i = 0;
            string primaryKeyValue = context.Request.QueryString["pk" + i.ToString()];
            while (primaryKeyValue != null) {
                if (sb.Length > 0) {
                    sb.Append(',');
                }
                sb.Append(primaryKeyValue);
                primaryKeyValues.Add(primaryKeyValue);
                i++;
                primaryKeyValue = context.Request["pk" + i.ToString()];
            }

            string cachekey = string.Format("{0}:{1}:{2}", tableName, column, sb.ToString());
            ImageCacheEntry images = (ImageCacheEntry)context.Cache[cachekey];
            if (images == null) {
                //get image bytes
                byte[] bytes;
                object picture = LoadImage(tableName, primaryKeyValues.ToArray(), column);
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

                    images = new ImageCacheEntry(bytes);

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

        private static object LoadImage(string tablename, string[] primaryKeyValues, string imageColumn) {
            //get iqueryable for table
            MetaTable metaTable = MetaModel.Default.GetTable(tablename);
            IQueryable query = metaTable.GetQuery();

            // build query
            // Items.Where(row => row.ID == 1).Single()
            //var singleWhereCall = LinqExpressionHelper.BuildSingleItemQuery(query, metaTable, primaryKeyValues);
            var whereCall = LinqExpressionHelper.BuildItemsQuery(query, metaTable, metaTable.PrimaryKeyColumns, primaryKeyValues);
            var q = query.Provider.CreateQuery(whereCall);
            foreach (var o in q) {
                return System.Web.UI.DataBinder.GetPropertyValue(o, imageColumn);
            }
            return null;

            // Items.Where(row => row.ID == 1).Single().Picture
            //var imageBytes = Expression.Property(singleWhereCall, imageColumn);

            //return query.Provider.Execute(imageBytes);
        }

        internal class ImageCacheEntry {
            private Dictionary<Size, byte[]> Images = new Dictionary<Size, byte[]>();
            private byte[] OriginalImage;

            public string ContentType {
                get;
                private set;
            }

            public ImageCacheEntry(byte[] imageBytes) {
                using (Image image = Imaging.ImageFromBytes(imageBytes)) {
                    ContentType = Imaging.GetContentTypeByImageFormat(image.RawFormat);
                    OriginalImage = imageBytes;
                    Images.Add(new Size(image.Width, image.Height), imageBytes);
                }
            }

            public byte[] GetImage(int width, int height) {
                lock (this) {
                    Size size = new Size(width, height);
                    byte[] result;
                    if (!Images.TryGetValue(size, out result)) {
                        result = Imaging.ScaleImage(OriginalImage, width, height, null);
                        Images.Add(size, result);
                    }
                    return result;
                }
            }
        }

        internal class Imaging {
            public static string GetContentTypeByImageFormat(ImageFormat format) {
                string ctype = "image/x-unknown";

                if (format.Equals(ImageFormat.Gif)) {
                    ctype = "image/gif";
                } else if (format.Equals(ImageFormat.Jpeg)) {
                    ctype = "image/jpeg";
                } else if (format.Equals(ImageFormat.Png)) {
                    ctype = "image/png";
                } else if (format.Equals(ImageFormat.Bmp) || format.Equals(ImageFormat.MemoryBmp)) {
                    ctype = "image/bmp";
                } else if (format.Equals(ImageFormat.Icon)) {
                    ctype = "image/x-icon";
                } else if (format.Equals(ImageFormat.Tiff)) {
                    ctype = "image/tiff";
                }

                return ctype;
            }

            public static ImageFormat GetImageFormatByContentType(string contentType) {
                ImageFormat format = null;

                if (contentType != null) {
                    if (contentType.Equals("image/gif")) {
                        format = ImageFormat.Gif;
                    } else if (contentType.Equals("image/jpeg") || contentType.Equals("image/pjpeg")) {
                        format = ImageFormat.Jpeg;
                    } else if (contentType.Equals("image/png")) {
                        format = ImageFormat.Png;
                    } else if (contentType.Equals("image/bmp")) {
                        format = ImageFormat.Bmp;
                    } else if (contentType.Equals("image/x-icon")) {
                        format = ImageFormat.Icon;
                    } else if (contentType.Equals("image/tiff")) {
                        format = ImageFormat.Tiff;
                    }
                }

                return format;
            }

            public static string GetFileExtensionByContentType(string contentType) {
                string ext = "bin";

                if (contentType.Equals("image/gif")) {
                    ext = "gif";
                } else if (contentType.Equals("image/jpeg") || contentType.Equals("image/pjpeg")) {
                    ext = "jpg";
                } else if (contentType.Equals("image/png")) {
                    ext = "png";
                } else if (contentType.Equals("image/bmp")) {
                    ext = "bmp";
                } else if (contentType.Equals("image/x-icon")) {
                    ext = "ico";
                } else if (contentType.Equals("image/tiff")) {
                    ext = "tif";
                }

                return ext;
            }

            public static byte[] BytesFromImage(Image image) {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] imageBytes = new byte[ms.Length];
                ms.Read(imageBytes, 0, (int)ms.Length);
                return imageBytes;
            }

            public static Image ImageFromBytes(byte[] bytes) {
                if (bytes == null || bytes.Length == 0) {
                    return null;
                } else {
                    MemoryStream ms = new MemoryStream(bytes);
                    return Image.FromStream(ms);
                }
            }

            public static byte[] ScaleImage(byte[] bytes, int maxWidth, int maxHeight) {
                return ScaleImage(bytes, maxWidth, maxHeight, ImageFormat.Jpeg);
            }

            public static byte[] ScaleImage(byte[] bytes, int maxWidth, int maxHeight, ImageFormat format) {
                try {
                    using (Image img = Image.FromStream(new MemoryStream(bytes))) {
                        if (format == null) {
                            format = img.RawFormat;
                        }
                        if (img.Size.Width > maxWidth || img.Size.Height > maxHeight) {
                            //resize the image to fit our website's required size
                            int newWidth = img.Size.Width;
                            int newHeight = img.Size.Height;
                            if (newWidth > maxWidth) {
                                newWidth = maxWidth;
                                newHeight = (int)(newHeight * ((float)newWidth / img.Size.Width));
                            }
                            if (newHeight > maxHeight) {
                                newHeight = maxHeight;
                                newWidth = img.Size.Width;
                                newWidth = (int)(newWidth * ((float)newHeight / img.Size.Height));
                            }

                            //resize the image to fit in the allowed image size
                            bool indexed;
                            Bitmap newImage;
                            if (img.PixelFormat == PixelFormat.Format1bppIndexed || img.PixelFormat == PixelFormat.Format4bppIndexed || img.PixelFormat == PixelFormat.Format8bppIndexed || img.PixelFormat == PixelFormat.Indexed) {
                                indexed = true;
                                newImage = new Bitmap(newWidth, newHeight);
                            } else {
                                indexed = false;
                                newImage = new Bitmap(newWidth, newHeight, img.PixelFormat);
                            }
                            using (newImage) {
                                using (Graphics g = Graphics.FromImage(newImage)) {
                                    if (indexed) {
                                        g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                                    }
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight));
                                }
                                using (MemoryStream ms = new MemoryStream()) {
                                    newImage.Save(ms, format);
                                    bytes = ms.ToArray();
                                }
                            }
                        } else if (img.RawFormat != format) {
                            using (MemoryStream ms = new MemoryStream()) {
                                img.Save(ms, format);
                                bytes = ms.ToArray();
                            }
                        }
                        return bytes;
                    }
                } catch (ArgumentException) {
                    return null;
                }
            }
        }
    }
}

