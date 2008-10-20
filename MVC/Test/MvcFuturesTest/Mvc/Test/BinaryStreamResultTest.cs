namespace MvcFuturesTest.Mvc.Test {
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class BinaryStreamResultTest {

        private static readonly Random _random = new Random();

        [TestMethod]
        public void ConstructorSetsStreamProperty() {
            // Arrange
            Stream stream = new MemoryStream();

            // Act
            BinaryStreamResult result = new BinaryStreamResult(stream);

            // Assert
            Assert.AreSame(stream, result.Stream);
        }

        [TestMethod]
        public void ConstructorThrowsIfStreamIsNull() {
            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new BinaryStreamResult(null);
                }, "stream");
        }

        [TestMethod]
        public void ExecuteResultCopiesToOutputStream() {
            // Arrange
            int byteLen = 0x1234;
            byte[] originalBytes = GetRandomByteArray(byteLen);
            MemoryStream originalStream = new MemoryStream(originalBytes);
            MemoryStream outStream = new MemoryStream();

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.ExpectSetProperty(r => r.ContentType, "application/octet-stream").Verifiable();
            mockResponse.Expect(r => r.OutputStream).Returns(outStream);

            BinaryStreamResult result = new BinaryStreamResult(originalStream);
            ControllerContext context = BinaryResultTest.GetControllerContext(mockResponse.Object);

            // Act
            result.ExecuteResult(context);

            // Assert
            byte[] outBytes = outStream.ToArray();
            Assert.IsTrue(originalBytes.SequenceEqual(outBytes), "Output stream does not match input stream.");
            mockResponse.Verify();
        }

        private static byte[] GetRandomByteArray(int length) {
            byte[] bytes = new byte[length];
            _random.NextBytes(bytes);
            return bytes;
        }

    }
}
