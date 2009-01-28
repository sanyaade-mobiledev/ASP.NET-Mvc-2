namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AntiForgeryTokenTest {

        [TestMethod]
        public void CopyConstructor() {
            // Arrange
            AntiForgeryToken originalToken = new AntiForgeryToken() {
                CreationDate = DateTime.Now,
                Salt = "some salt",
                Value = "some value"
            };

            // Act
            AntiForgeryToken newToken = new AntiForgeryToken(originalToken);

            // Assert
            Assert.AreEqual(originalToken.CreationDate, newToken.CreationDate);
            Assert.AreEqual(originalToken.Salt, newToken.Salt);
            Assert.AreEqual(originalToken.Value, newToken.Value);
        }

        [TestMethod]
        public void CopyConstructorThrowsIfTokenIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new AntiForgeryToken(null);
                }, "token");
        }

        [TestMethod]
        public void CreationDateProperty() {
            // Arrange
            AntiForgeryToken token = new AntiForgeryToken();

            // Act & Assert
            MemberHelper.TestPropertyValue(token, "CreationDate", DateTime.Now);
        }

        [TestMethod]
        public void NewToken() {
            // Act
            AntiForgeryToken token = AntiForgeryToken.NewToken();

            // Assert
            int valueLength = Convert.FromBase64String(token.Value).Length;
            Assert.AreEqual(16, valueLength, "Value was not of the correct length.");
            Assert.AreNotEqual(default(DateTime), token.CreationDate, "Creation date should have been initialized.");
        }

        [TestMethod]
        public void SaltProperty() {
            // Arrange
            AntiForgeryToken token = new AntiForgeryToken();

            // Act & Assert
            MemberHelper.TestStringProperty(token, "Salt", String.Empty, false /* testDefaultValue */, true /* allowNullAndEmpty */);
        }

        [TestMethod]
        public void ValueProperty() {
            // Arrange
            AntiForgeryToken token = new AntiForgeryToken();

            // Act & Assert
            MemberHelper.TestStringProperty(token, "Value", String.Empty, false /* testDefaultValue */, true /* allowNullAndEmpty */);
        }

    }
}
