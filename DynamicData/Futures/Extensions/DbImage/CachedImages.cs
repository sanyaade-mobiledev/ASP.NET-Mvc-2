using System.Collections.Generic;
using System.Drawing;

namespace Microsoft.Web.DynamicData.Extensions {

    internal class CachedImages {
        private Dictionary<Size, byte[]> Images;
        private byte[] OriginalImage;
        private string _ContentType;

        public CachedImages(byte[] imagebytes) {
            using (Image image = Imaging.ImageFromBytes(imagebytes)) {
                _ContentType = Imaging.GetContentTypeByImageFormat(image.RawFormat);
                OriginalImage = imagebytes;
                Images = new Dictionary<Size, byte[]>();
                Images.Add(new Size(image.Width, image.Height), imagebytes);
            }
        }

        public string ContentType {
            get {
                return _ContentType;
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

}