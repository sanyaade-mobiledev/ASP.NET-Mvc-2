namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AntiForgeryTokenSerializerTest {

        [TestMethod]
        public void DeserializeReturnsDeserializedToken() {
            // Arrange
            Mock<IStateFormatter> mockFormatter = new Mock<IStateFormatter>();
            mockFormatter.Expect(f => f.Deserialize("serialized value")).Returns(new Triplet("the salt", "the value", new DateTime(2001, 1, 1)));

            AntiForgeryTokenSerializer serializer = new AntiForgeryTokenSerializer() {
                Formatter = mockFormatter.Object
            };

            // Act
            AntiForgeryToken token = serializer.Deserialize("serialized value");

            // Assert
            Assert.IsNotNull(token);
            Assert.AreEqual(new DateTime(2001, 1, 1), token.CreationDate);
            Assert.AreEqual("the salt", token.Salt);
            Assert.AreEqual("the value", token.Value);
        }

        [TestMethod]
        public void DeserializeThrowsIfFormatterThrows() {
            // Arrange
            Exception innerException = new Exception();

            Mock<IStateFormatter> mockFormatter = new Mock<IStateFormatter>();
            mockFormatter.Expect(f => f.Deserialize("bad value")).Throws(innerException);

            AntiForgeryTokenSerializer serializer = new AntiForgeryTokenSerializer() {
                Formatter = mockFormatter.Object
            };

            // Act
            HttpAntiForgeryException ex = ExceptionHelper.ExpectException<HttpAntiForgeryException>(
                delegate {
                    serializer.Deserialize("bad value");
                }, "A required anti-forgery token was not supplied or was invalid.");

            // Assert
            Assert.AreEqual(innerException, ex.InnerException);
        }

        [TestMethod]
        public void DeserializeThrowsIfSerializedTokenIsEmpty() {
            // Arrange
            AntiForgeryTokenSerializer serializer = new AntiForgeryTokenSerializer();

            // Act & assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    serializer.Deserialize(String.Empty);
                }, "serializedToken");
        }

        [TestMethod]
        public void DeserializeThrowsIfSerializedTokenIsNull() {
            // Arrange
            AntiForgeryTokenSerializer serializer = new AntiForgeryTokenSerializer();

            // Act & assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    serializer.Deserialize(null);
                }, "serializedToken");
        }

        [TestMethod]
        public void SerializeReturnsSerializedString() {
            // Arrange
            AntiForgeryToken token = new AntiForgeryToken() {
                CreationDate = new DateTime(2001, 1, 1),
                Salt = "the salt",
                Value = "the value"
            };

            Mock<IStateFormatter> mockFormatter = new Mock<IStateFormatter>();
            mockFormatter
                .Expect(f => f.Serialize(It.IsAny<object>()))
                .Returns(
                    delegate(object state) {
                        Triplet t = state as Triplet;
                        Assert.IsNotNull(t);
                        Assert.AreEqual("the salt", t.First);
                        Assert.AreEqual("the value", t.Second);
                        Assert.AreEqual(new DateTime(2001, 1, 1), t.Third);
                        return "serialized value";
                    }
                );

            AntiForgeryTokenSerializer serializer = new AntiForgeryTokenSerializer() {
                Formatter = mockFormatter.Object
            };

            // Act
            string serializedValue = serializer.Serialize(token);

            // Assert
            Assert.AreEqual("serialized value", serializedValue);
        }

        [TestMethod]
        public void SerializeThrowsIfTokenIsNull() {
            // Arrange
            AntiForgeryTokenSerializer serializer = new AntiForgeryTokenSerializer();

            // Act & assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    serializer.Serialize(null);
                }, "token");
        }

    }
}
