namespace System.Web.Mvc.Test {
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewEngineResultTest {

        [TestMethod]
        public void ConstructorThrowsIfSearchedLocationsIsNull() {
            // Arrange
            string[] searchedLocations = null;

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ViewEngineResult(searchedLocations);
                }, "searchedLocations");
        }

        [TestMethod]
        public void ConstructorThrowsIfViewIsNull() {
            // Arrange
            IView view = null;

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    new ViewEngineResult(view);
                }, "view");
        }

        [TestMethod]
        public void SearchedLocationsProperty() {
            // Arrange
            string[] searchedLocations = new string[0];
            ViewEngineResult result = new ViewEngineResult(searchedLocations);

            // Act & Assert
            Assert.AreSame(searchedLocations, result.SearchedLocations);
            Assert.IsNull(result.View);
        }

        [TestMethod]
        public void ViewProperty() {
            // Arrange
            IView view = new Mock<IView>().Object;
            ViewEngineResult result = new ViewEngineResult(view);

            // Act & Assert
            Assert.AreSame(view, result.View);
            Assert.IsNull(result.SearchedLocations);
        }

    }
}
