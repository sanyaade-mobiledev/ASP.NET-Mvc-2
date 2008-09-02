namespace System.Web.Mvc.Test {
    using System.Collections.Generic;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewEngineCollectionTest {

        [TestMethod]
        public void ListWrappingConstructor() {
            // Arrange
            List<IViewEngine> list = new List<IViewEngine>() { new Mock<IViewEngine>().Object, new Mock<IViewEngine>().Object };

            // Act
            ViewEngineCollection collection = new ViewEngineCollection(list);

            // Assert
            Assert.AreEqual(2, collection.Count);
            Assert.AreSame(list[0], collection[0]);
            Assert.AreSame(list[1], collection[1]);
        }

        [TestMethod]
        public void ListWrappingConstructorThrowsIfListIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ViewEngineCollection(null);
                }, "list");
        }

        [TestMethod]
        public void DefaultConstructor() {
            // Act
            ViewEngineCollection collection = new ViewEngineCollection();

            // Assert
            Assert.AreEqual(0, collection.Count);
        }

    }
}
