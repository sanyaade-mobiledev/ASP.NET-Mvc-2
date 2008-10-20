namespace System.Web.Mvc.Test {
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AutoViewEngineTest {

        [TestMethod]
        public void FindPartialViewAggregatesAllSearchedLocationsIfAllEnginesFail() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine1 = new Mock<IViewEngine>();
            var engine1Result = new ViewEngineResult(new[] { "location1", "location2" });
            engine1.Expect(e => e.FindPartialView(context, "partial")).Returns(engine1Result);
            var engine2 = new Mock<IViewEngine>();
            var engine2Result = new ViewEngineResult(new[] { "location3", "location4" });
            engine2.Expect(e => e.FindPartialView(context, "partial")).Returns(engine2Result);
            collection.Add(engine1.Object);
            collection.Add(engine2.Object);

            // Act
            var result = collectionEngine.FindPartialView(context, "partial");

            // Assert
            Assert.IsNull(result.View);
            Assert.AreEqual(4, result.SearchedLocations.Count());
            Assert.IsTrue(result.SearchedLocations.Contains("location1"));
            Assert.IsTrue(result.SearchedLocations.Contains("location2"));
            Assert.IsTrue(result.SearchedLocations.Contains("location3"));
            Assert.IsTrue(result.SearchedLocations.Contains("location4"));
        }

        [TestMethod]
        public void FindPartialViewFailureWithOneEngine() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine = new Mock<IViewEngine>();
            var engineResult = new ViewEngineResult(new[] { "location1", "location2" });
            engine.Expect(e => e.FindPartialView(context, "partial")).Returns(engineResult);
            collection.Add(engine.Object);

            // Act
            var result = collectionEngine.FindPartialView(context, "partial");

            // Assert
            Assert.IsNull(result.View);
            Assert.AreEqual(2, result.SearchedLocations.Count());
            Assert.IsTrue(result.SearchedLocations.Contains("location1"));
            Assert.IsTrue(result.SearchedLocations.Contains("location2"));
        }

        [TestMethod]
        public void FindPartialViewIteratesThroughCollectionUntilFindsSuccessfulEngine() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine1 = new Mock<IViewEngine>();
            var engine1Result = new ViewEngineResult(new[] { "location1", "location2" });
            engine1.Expect(e => e.FindPartialView(context, "partial")).Returns(engine1Result);
            var engine2 = new Mock<IViewEngine>();
            var engine2Result = new ViewEngineResult(new Mock<IView>().Object, engine2.Object);
            engine2.Expect(e => e.FindPartialView(context, "partial")).Returns(engine2Result);
            collection.Add(engine1.Object);
            collection.Add(engine2.Object);

            // Act
            var result = collectionEngine.FindPartialView(context, "partial");

            // Assert
            Assert.AreSame(engine2Result, result);
        }

        [TestMethod]
        public void FindPartialViewReturnsNoViewAndEmptySearchedLocationsIfCollectionEmpty() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);

            // Act
            var result = collectionEngine.FindPartialView(context, "partial");

            // Assert
            Assert.IsNull(result.View);
            Assert.AreEqual(0, result.SearchedLocations.Count());
        }

        [TestMethod]
        public void FindPartialViewReturnsValueFromFirstSuccessfulEngine() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine1 = new Mock<IViewEngine>();
            var engine1Result = new ViewEngineResult(new Mock<IView>().Object, engine1.Object);
            engine1.Expect(e => e.FindPartialView(context, "partial")).Returns(engine1Result);
            var engine2 = new Mock<IViewEngine>();
            var engine2Result = new ViewEngineResult(new Mock<IView>().Object, engine2.Object);
            engine2.Expect(e => e.FindPartialView(context, "partial")).Returns(engine2Result);
            collection.Add(engine1.Object);
            collection.Add(engine2.Object);

            // Act
            var result = collectionEngine.FindPartialView(context, "partial");

            // Assert
            Assert.AreSame(engine1Result, result);
        }

        [TestMethod]
        public void FindPartialViewSuccessWithOneEngine() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine = new Mock<IViewEngine>();
            var engineResult = new ViewEngineResult(new Mock<IView>().Object, engine.Object);
            engine.Expect(e => e.FindPartialView(context, "partial")).Returns(engineResult);
            collection.Add(engine.Object);

            // Act
            var result = collectionEngine.FindPartialView(context, "partial");

            // Assert
            Assert.AreSame(engineResult, result);
        }

        [TestMethod]
        public void FindPartialViewThrowsIfPartialViewNameIsEmpty() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                () => collectionEngine.FindPartialView(context, ""),
                "partialViewName");
        }

        [TestMethod]
        public void FindPartialViewThrowsIfPartialViewNameIsNull() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                () => collectionEngine.FindPartialView(context, null),
                "partialViewName");
        }

        [TestMethod]
        public void FindPartialViewThrowsIfControllerContextIsNull() {
            // Arrange
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                () => collectionEngine.FindPartialView(null, "partial"),
                "controllerContext");
        }

        [TestMethod]
        public void FindViewAggregatesAllSearchedLocationsIfAllEnginesFail() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine1 = new Mock<IViewEngine>();
            var engine1Result = new ViewEngineResult(new[] { "location1", "location2" });
            engine1.Expect(e => e.FindView(context, "view", "master")).Returns(engine1Result);
            var engine2 = new Mock<IViewEngine>();
            var engine2Result = new ViewEngineResult(new[] { "location3", "location4" });
            engine2.Expect(e => e.FindView(context, "view", "master")).Returns(engine2Result);
            collection.Add(engine1.Object);
            collection.Add(engine2.Object);

            // Act
            var result = collectionEngine.FindView(context, "view", "master");

            // Assert
            Assert.IsNull(result.View);
            Assert.AreEqual(4, result.SearchedLocations.Count());
            Assert.IsTrue(result.SearchedLocations.Contains("location1"));
            Assert.IsTrue(result.SearchedLocations.Contains("location2"));
            Assert.IsTrue(result.SearchedLocations.Contains("location3"));
            Assert.IsTrue(result.SearchedLocations.Contains("location4"));
        }

        [TestMethod]
        public void FindViewFailureWithOneEngine() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine = new Mock<IViewEngine>();
            var engineResult = new ViewEngineResult(new[] { "location1", "location2" });
            engine.Expect(e => e.FindView(context, "view", "master")).Returns(engineResult);
            collection.Add(engine.Object);

            // Act
            var result = collectionEngine.FindView(context, "view", "master");

            // Assert
            Assert.IsNull(result.View);
            Assert.AreEqual(2, result.SearchedLocations.Count());
            Assert.IsTrue(result.SearchedLocations.Contains("location1"));
            Assert.IsTrue(result.SearchedLocations.Contains("location2"));
        }

        [TestMethod]
        public void FindViewIteratesThroughCollectionUntilFindsSuccessfulEngine() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine1 = new Mock<IViewEngine>();
            var engine1Result = new ViewEngineResult(new[] { "location1", "location2" });
            engine1.Expect(e => e.FindView(context, "view", "master")).Returns(engine1Result);
            var engine2 = new Mock<IViewEngine>();
            var engine2Result = new ViewEngineResult(new Mock<IView>().Object, engine2.Object);
            engine2.Expect(e => e.FindView(context, "view", "master")).Returns(engine2Result);
            collection.Add(engine1.Object);
            collection.Add(engine2.Object);

            // Act
            var result = collectionEngine.FindView(context, "view", "master");

            // Assert
            Assert.AreSame(engine2Result, result);
        }

        [TestMethod]
        public void FindViewReturnsNoViewAndEmptySearchedLocationsIfCollectionEmpty() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);

            // Act
            var result = collectionEngine.FindView(context, "view", null);

            // Assert
            Assert.IsNull(result.View);
            Assert.AreEqual(0, result.SearchedLocations.Count());
        }

        [TestMethod]
        public void FindViewReturnsValueFromFirstSuccessfulEngine() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine1 = new Mock<IViewEngine>();
            var engine1Result = new ViewEngineResult(new Mock<IView>().Object, engine1.Object);
            engine1.Expect(e => e.FindView(context, "view", "master")).Returns(engine1Result);
            var engine2 = new Mock<IViewEngine>();
            var engine2Result = new ViewEngineResult(new Mock<IView>().Object, engine2.Object);
            engine2.Expect(e => e.FindView(context, "view", "master")).Returns(engine2Result);
            collection.Add(engine1.Object);
            collection.Add(engine2.Object);

            // Act
            var result = collectionEngine.FindView(context, "view", "master");

            // Assert
            Assert.AreSame(engine1Result, result);
        }

        [TestMethod]
        public void FindViewSuccessWithOneEngine() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);
            var engine = new Mock<IViewEngine>();
            var engineResult = new ViewEngineResult(new Mock<IView>().Object, engine.Object);
            engine.Expect(e => e.FindView(context, "view", "master")).Returns(engineResult);
            collection.Add(engine.Object);

            // Act
            var result = collectionEngine.FindView(context, "view", "master");

            // Assert
            Assert.AreSame(engineResult, result);
        }

        [TestMethod]
        public void FindViewThrowsIfControllerContextIsNull() {
            // Arrange
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                () => collectionEngine.FindView(null, "view", null),
                "controllerContext"
            );
        }

        [TestMethod]
        public void FindViewThrowsIfViewNameIsEmpty() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                () => collectionEngine.FindView(context, "", null),
                "viewName"
            );
        }

        [TestMethod]
        public void FindViewThrowsIfViewNameIsNull() {
            // Arrange
            var context = GetControllerContext();
            var collection = new ViewEngineCollection();
            var collectionEngine = new AutoViewEngine(collection);

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                () => collectionEngine.FindView(context, null, null),
                "viewName"
            );
        }

        private static ControllerContext GetControllerContext() {
            RequestContext requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            return new ControllerContext(requestContext, new Mock<ControllerBase>().Object);
        }

    }
}
