namespace System.Web.Mvc.Test {
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MethodSelectionAttributeTest {

        [TestMethod]
        public void OnMethodSelectedReturnsContinue() {
            // Arrange
            MethodSelectionAttribute attr = new MySelectionAttribute();

            // Act
            MethodSelectionResult result = attr.OnMethodSelected(null, null, null);

            // Assert
            Assert.AreEqual(MethodSelectionResult.Continue, result);
        }

        [TestMethod]
        public void OnMethodSelectingReturnsContinue() {
            // Arrange
            MethodSelectionAttribute attr = new MySelectionAttribute();

            // Act
            MethodSelectionResult result = attr.OnMethodSelecting(null, null, null);

            // Assert
            Assert.AreEqual(MethodSelectionResult.Continue, result);
        }

        private class MySelectionAttribute : MethodSelectionAttribute {
        }

    }
}
