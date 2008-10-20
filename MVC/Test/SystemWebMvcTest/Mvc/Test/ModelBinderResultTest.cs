namespace System.Web.Mvc.Test {
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ModelBinderResultTest {

        [TestMethod]
        public void ValueProperty() {
            // Arrange
            ModelBinderResult result = new ModelBinderResult(42);

            // Act & assert
            Assert.AreEqual(42, result.Value);
        }

    }
}
