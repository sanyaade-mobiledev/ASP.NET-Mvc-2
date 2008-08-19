using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Web.Test.GeneratedImage {
    [TestClass]
    public class ImageResizeTransformTest {
        [TestMethod]
        public void Height_Property() {
            var transform = new ImageResizeTransform();
            Assert.AreEqual(0, transform.Height);

            transform.Height = 200;
            Assert.AreEqual(200, transform.Height);

            ExceptionHelper.ExpectArgumentOutOfRangeException(delegate() {
                transform.Height = -10;
            }, "value", "Specified argument was out of the range of valid values.\r\nParameter name: value");
        }

        [TestMethod]
        public void Width_Property() {
            var transform = new ImageResizeTransform();
            Assert.AreEqual(0, transform.Width);

            transform.Width = 200;
            Assert.AreEqual(200, transform.Width);

            ExceptionHelper.ExpectArgumentOutOfRangeException(delegate() {
                transform.Width = -10;
            }, "value", "Specified argument was out of the range of valid values.\r\nParameter name: value");
        }

        [TestMethod]
        public void Mode_Property() {
            var transform = new ImageResizeTransform();
            Assert.AreEqual(ImageResizeMode.Fit, transform.Mode);

            transform.Mode = ImageResizeMode.Crop;
            Assert.AreEqual(ImageResizeMode.Crop, transform.Mode);
        }

        [TestMethod]
        public void InterpolationMode_Property() {
            var transform = new ImageResizeTransform();
            Assert.AreEqual(InterpolationMode.HighQualityBicubic, transform.InterpolationMode);

            transform.InterpolationMode = InterpolationMode.High;
            Assert.AreEqual(InterpolationMode.High, transform.InterpolationMode);
        }

        [TestMethod]
        public void ProcessImage_Crop() {
            ProcessImageHelper(5, 10, CreateTransform(5, 10, ImageResizeMode.Crop), new Bitmap(20, 20));
            ProcessImageHelper(50, 100, CreateTransform(50, 100, ImageResizeMode.Crop), new Bitmap(20, 20));
        }

        [TestMethod]
        public void ProcessImage_Fit() {
            ProcessImageHelper(5, 5, CreateTransform(5, 10, ImageResizeMode.Fit), new Bitmap(20, 20));
            ProcessImageHelper(50, 50, CreateTransform(50, 100, ImageResizeMode.Fit), new Bitmap(20, 20));
        }

        private void ProcessImageHelper(int eWidth, int eHeight, ImageResizeTransform transform, Image image) {
            Image result = transform.ProcessImage(image);
            Assert.AreEqual(eWidth, result.Width);
            Assert.AreEqual(eHeight, result.Height);
        }

        private ImageResizeTransform CreateTransform(int width, int height, ImageResizeMode mode) {
            return new ImageResizeTransform() {
                Width = width,
                Height = height,
                Mode = mode
            };
        }
    }
}
