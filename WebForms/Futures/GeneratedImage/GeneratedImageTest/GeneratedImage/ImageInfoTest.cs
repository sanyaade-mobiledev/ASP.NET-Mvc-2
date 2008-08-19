using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Web.Test {
    [TestClass]
    public class ImageInfoTest {
        [TestMethod]
        public void Constructor_ByteArray() {
            ExceptionHelper.ExpectArgumentNullException(delegate() {
                new ImageInfo((byte[])null);
            }, "imageBuffer");

        }

        [TestMethod]
        public void Constructor_Image() {
            ExceptionHelper.ExpectArgumentNullException(delegate() {
                new ImageInfo((Image)null);
            }, "image");
        }

        [TestMethod]
        public void Image() {
            Bitmap bitmap = new Bitmap(1, 1);
            var ii = new ImageInfo(bitmap);
            Assert.AreSame(bitmap, ii.Image);
            Assert.IsNull(ii.ImageByteBuffer);
        }

        [TestMethod]
        public void ImageBytes() {
            byte[] buffer = new byte[] { 10, 20 };
            var ii = new ImageInfo(buffer);
            Assert.AreSame(buffer, ii.ImageByteBuffer);
            Assert.IsNull(ii.Image);
        }
    }
}
