﻿namespace System.Web.Mvc.Test {
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RouteCollectionExtensionsTest {

        [TestMethod]
        public void MapRoute3() {
            // Setup
            RouteCollection routes = new RouteCollection();

            // Execute
            routes.MapRoute("RouteName", "SomeUrl");

            // Verify
            Assert.AreEqual(1, routes.Count);
            Route route = routes[0] as Route;
            Assert.IsNotNull(route);
            Assert.AreSame(route, routes["RouteName"]);
            Assert.AreEqual("SomeUrl", route.Url);
            Assert.IsInstanceOfType(route.RouteHandler, typeof(MvcRouteHandler));
            Assert.AreEqual(0, route.Defaults.Count);
            Assert.AreEqual(0, route.Constraints.Count);
        }

        [TestMethod]
        public void MapRoute4() {
            // Setup
            RouteCollection routes = new RouteCollection();
            var defaults = new { Foo = "DefaultFoo" };

            // Execute
            routes.MapRoute("RouteName", "SomeUrl", defaults);

            // Verify
            Assert.AreEqual(1, routes.Count);
            Route route = routes[0] as Route;
            Assert.IsNotNull(route);
            Assert.AreSame(route, routes["RouteName"]);
            Assert.AreEqual("SomeUrl", route.Url);
            Assert.IsInstanceOfType(route.RouteHandler, typeof(MvcRouteHandler));
            Assert.AreEqual("DefaultFoo", route.Defaults["Foo"]);
            Assert.AreEqual(0, route.Constraints.Count);
        }

        [TestMethod]
        public void MapRoute5() {
            // Setup
            RouteCollection routes = new RouteCollection();
            var defaults = new { Foo = "DefaultFoo" };
            var constraints = new { Foo = "ConstraintFoo" };

            // Execute
            routes.MapRoute("RouteName", "SomeUrl", defaults, constraints);

            // Verify
            Assert.AreEqual(1, routes.Count);
            Route route = routes[0] as Route;
            Assert.IsNotNull(route);
            Assert.AreSame(route, routes["RouteName"]);
            Assert.AreEqual("SomeUrl", route.Url);
            Assert.IsInstanceOfType(route.RouteHandler, typeof(MvcRouteHandler));
            Assert.AreEqual("DefaultFoo", route.Defaults["Foo"]);
            Assert.AreEqual("ConstraintFoo", route.Constraints["Foo"]);
        }

        [TestMethod]
        public void MapRoute5TreatsEmptyNameAsUnnamedRoute() {
            // Setup
            RouteCollection routes = new RouteCollection();

            // Execute
            routes.MapRoute("", "", null, null);

            // Verify
            Assert.AreEqual(1, routes.Count);
        }

        [TestMethod]
        public void MapRoute5TreatsNullNameAsUnnamedRoute() {
            // Setup
            RouteCollection routes = new RouteCollection();

            // Execute
            routes.MapRoute(null, "", null, null);

            // Verify
            Assert.AreEqual(1, routes.Count);
        }

        [TestMethod]
        public void MapRoute5WithNullRouteCollectionThrows() {
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    RouteCollectionExtensions.MapRoute(null, null, null, null, null);
                },
                "routes");
        }

        [TestMethod]
        public void MapRoute5WithNullUrlThrows() {
            // Setup
            RouteCollection routes = new RouteCollection();

            // Execute & verify
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    routes.MapRoute(null, null /* url */, null, null);
                },
                "url");
        }
    }
}