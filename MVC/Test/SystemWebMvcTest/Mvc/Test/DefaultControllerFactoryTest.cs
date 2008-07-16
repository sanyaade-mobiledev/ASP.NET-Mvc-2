namespace System.Web.Mvc.Test {
    using System;
    using System.Reflection;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DefaultControllerFactoryTest {
        static DefaultControllerFactoryTest() {
            MvcTestHelper.CreateMvcAssemblies();
        }

        [TestMethod]
        public void CreateControllerWithNullContextThrows() {
            // Setup
            DefaultControllerFactory factory = new DefaultControllerFactory();

            // Execute
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    ((IControllerFactory)factory).CreateController(
                        null,
                        "foo");
                },
                "requestContext");
        }

        [TestMethod]
        public void CreateControllerWithEmptyControllerNameThrows() {
            // Setup
            DefaultControllerFactory factory = new DefaultControllerFactory();

            // Execute
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    ((IControllerFactory)factory).CreateController(
                        new RequestContext(new Mock<HttpContextBase>().Object, new RouteData()),
                        String.Empty);
                },
                "Value cannot be null or empty.\r\nParameter name: controllerName");
        }

        [TestMethod]
        public void CreateControllerReturnsControllerInstance() {
            // Setup
            Mock<DefaultControllerFactory> factoryMock = new Mock<DefaultControllerFactory>();
            factoryMock.Expect(o => o.GetControllerType("moo")).Returns(typeof(DummyController));

            // Execute
            IController controller = ((IControllerFactory)factoryMock.Object).CreateController(
                new RequestContext(new Mock<HttpContextBase>().Object, new RouteData()),
                "moo");

            // Verify
            Assert.IsInstanceOfType(controller, typeof(DummyController));
        }

        [TestMethod]
        public void CreateControllerCanReturnNull() {
            // Setup
            Mock<DefaultControllerFactory> factoryMock = new Mock<DefaultControllerFactory>();
            factoryMock.Expect(o => o.GetControllerType("moo")).Returns(typeof(DummyController));
            factoryMock.Expect(o => o.GetControllerInstance(typeof(DummyController))).Returns((IController)null);

            // Execute
            IController controller = ((IControllerFactory)factoryMock.Object).CreateController(
                new RequestContext(new Mock<HttpContextBase>().Object, new RouteData()),
                "moo");

            // Verify
            Assert.IsNull(controller, "It should be OK for CreateController to return null");
        }

        [TestMethod]
        public void DisposeControllerFactoryWithDisposableController() {
            // Setup
            IControllerFactory factory = new DefaultControllerFactory();
            Mock<IDisposableController> mockController = new Mock<IDisposableController>();
            mockController.Expect(o => ((IDisposable)o).Dispose());

            // Execute
            factory.DisposeController(mockController.Object);

            // Verify
            mockController.Verify();
        }

        [TestMethod]
        public void DisposeControllerFactoryWithNonDisposableController() {
            // Setup
            IControllerFactory factory = new DefaultControllerFactory();
            IController controller = new Mock<IController>().Object;

            // Execute
            factory.DisposeController(controller);

            // Verify - If we got this far, no exception was thrown, so success.
        }

        [TestMethod]
        public void GetControllerInstanceWithNullTypeThrows() {
            // Setup
            Mock<DefaultControllerFactory> factoryMock = new Mock<DefaultControllerFactory>();
            factoryMock.Expect(o => o.GetControllerType("moo")).Returns((Type)null);
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> requestMock = new Mock<HttpRequestBase>();
            contextMock.Expect(o => o.Request).Returns(requestMock.Object);
            requestMock.Expect(o => o.Path).Returns("somepath");

            // Execute
            ArgumentNullException ex = ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    ((IControllerFactory)factoryMock.Object).CreateController(
                        new RequestContext(contextMock.Object, new RouteData()),
                        "moo");
                },
                "controllerType");
            Assert.AreEqual<string>("The controller for path 'somepath' could not be found or it does not implement the IController interface.\r\nParameter name: controllerType", ex.Message);
        }

        [TestMethod]
        public void GetControllerInstanceNonIControllerTypeThrows() {
            // Setup
            Mock<DefaultControllerFactory> factoryMock = new Mock<DefaultControllerFactory>();
            factoryMock.Expect(o => o.GetControllerType("moo")).Returns(typeof(int));
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();

            // Execute
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    ((IControllerFactory)factoryMock.Object).CreateController(
                        new RequestContext(contextMock.Object, new RouteData()),
                        "moo");
                },
                "The controller type 'System.Int32' must implement the IController interface.\r\nParameter name: controllerType");
        }

        [TestMethod]
        public void GetControllerInstanceWithBadConstructorThrows() {
            // Setup
            Mock<DefaultControllerFactory> factoryMock = new Mock<DefaultControllerFactory>();
            factoryMock.Expect(o => o.GetControllerType("moo")).Returns(typeof(DummyControllerThrows));
            Mock<HttpContextBase> contextMock = new Mock<HttpContextBase>();

            // Execute
            Exception ex = ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    ((IControllerFactory)factoryMock.Object).CreateController(
                        new RequestContext(contextMock.Object, new RouteData()),
                        "moo");
                },
                "An error occurred while creating a controller of type 'System.Web.Mvc.Test.DefaultControllerFactoryTest+DummyControllerThrows'. If the controller doesn't have a controller factory, ensure that it has a parameterless public constructor.");

            Assert.AreEqual<string>("constructor", ex.InnerException.InnerException.Message);
        }

        [TestMethod]
        public void GetControllerTypeWithEmptyControllerNameThrows() {
            // Setup
            DefaultControllerFactory factory = new DefaultControllerFactory();

            // Execute
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    factory.GetControllerType(
                        String.Empty);
                },
                "Value cannot be null or empty.\r\nParameter name: controllerName");
        }

        [TestMethod]
        public void GetControllerTypeForNoAssemblies() {
            // Setup
            DefaultControllerFactory factory = new DefaultControllerFactory();
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;

            // Execute
            Type controllerType = factory.GetControllerType("sometype");

            // Verify
            Assert.IsNull(controllerType, "Shouldn't have found a controller type.");
            Assert.AreEqual<int>(0, controllerTypeCache.Count, "Cache should be empty.");
        }

        [TestMethod]
        public void GetControllerTypeForOneAssembly() {
            // Setup
            DefaultControllerFactory factory = GetDefaultControllerFactory("ns1a.ns1b", "ns2a.ns2b");
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { Assembly.Load("MvcAssembly1") });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;

            // Execute
            Type c1Type = factory.GetControllerType("C1");
            Type c2Type = factory.GetControllerType("c2");

            // Verify
            Assembly asm1 = Assembly.Load("MvcAssembly1");
            Type verifiedC1 = asm1.GetType("NS1a.NS1b.C1Controller");
            Type verifiedC2 = asm1.GetType("NS2a.NS2b.C2Controller");
            Assert.AreEqual<Type>(verifiedC1, c1Type, "Should have found C1Controller type.");
            Assert.AreEqual<Type>(verifiedC2, c2Type, "Should have found C2Controller type.");
            Assert.AreEqual<int>(2, controllerTypeCache.Count, "Cache should have 2 controller types.");
        }

        [TestMethod]
        public void GetControllerTypeForManyAssemblies() {
            // Setup
            DefaultControllerFactory factory = GetDefaultControllerFactory("ns1a.ns1b", "ns2a.ns2b", "ns3a.ns3b", "ns4a.ns4b");
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { Assembly.Load("MvcAssembly1"), Assembly.Load("MvcAssembly2") });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;

            // Execute
            Type c1Type = factory.GetControllerType("C1");
            Type c2Type = factory.GetControllerType("C2");
            Type c3Type = factory.GetControllerType("c3"); // lower case
            Type c4Type = factory.GetControllerType("c4"); // lower case

            // Verify
            Assembly asm1 = Assembly.Load("MvcAssembly1");
            Type verifiedC1 = asm1.GetType("NS1a.NS1b.C1Controller");
            Type verifiedC2 = asm1.GetType("NS2a.NS2b.C2Controller");
            Assembly asm2 = Assembly.Load("MvcAssembly2");
            Type verifiedC3 = asm2.GetType("NS3a.NS3b.C3Controller");
            Type verifiedC4 = asm2.GetType("NS4a.NS4b.C4Controller");
            Assert.IsNotNull(verifiedC1, "Couldn't find real C1 type");
            Assert.IsNotNull(verifiedC2, "Couldn't find real C2 type");
            Assert.IsNotNull(verifiedC3, "Couldn't find real C3 type");
            Assert.IsNotNull(verifiedC4, "Couldn't find real C4 type");
            Assert.AreEqual<Type>(verifiedC1, c1Type, "Should have found C1Controller type.");
            Assert.AreEqual<Type>(verifiedC2, c2Type, "Should have found C2Controller type.");
            Assert.AreEqual<Type>(verifiedC3, c3Type, "Should have found C3Controller type.");
            Assert.AreEqual<Type>(verifiedC4, c4Type, "Should have found C4Controller type.");
            Assert.AreEqual<int>(4, controllerTypeCache.Count, "Cache should have 4 controller types.");
        }

        [TestMethod]
        public void GetControllerTypeForAssembliesWithSameTypeNamesInDifferentNamespaces() {
            // Setup
            DefaultControllerFactory factory = GetDefaultControllerFactory("ns1a.ns1b", "ns2a.ns2b");
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { Assembly.Load("MvcAssembly1"), Assembly.Load("MvcAssembly3") });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;

            // Execute
            Type c1Type = factory.GetControllerType("C1");
            Type c2Type = factory.GetControllerType("C2");

            // Verify
            Assembly asm1 = Assembly.Load("MvcAssembly1");
            Type verifiedC1 = asm1.GetType("NS1a.NS1b.C1Controller");
            Type verifiedC2 = asm1.GetType("NS2a.NS2b.C2Controller");
            Assert.IsNotNull(verifiedC1, "Couldn't find real C1 type");
            Assert.IsNotNull(verifiedC2, "Couldn't find real C2 type");
            Assert.AreEqual<Type>(verifiedC1, c1Type, "Should have found C1Controller type.");
            Assert.AreEqual<Type>(verifiedC2, c2Type, "Should have found C2Controller type.");
            Assert.AreEqual<int>(4, controllerTypeCache.Count, "Cache should have 4 controller types.");
        }

        [TestMethod]
        public void GetControllerTypeForAssembliesWithSameTypeNamesInDifferentNamespacesThrowsIfAmbiguous() {
            // Setup
            DefaultControllerFactory factory = GetDefaultControllerFactory("ns1a.ns1b", "ns3a.ns3b");
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { Assembly.Load("MvcAssembly1"), Assembly.Load("MvcAssembly3") });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    factory.GetControllerType("C1");
                },
                "The controller name 'C1' is ambiguous between the following types: NS1a.NS1b.C1Controller, NS3a.NS3b.C1Controller. "
                    + "To disambiguate the controller set the controller's namespace in the route.");

            // Verify
            Assert.AreEqual<int>(4, controllerTypeCache.Count, "Cache should have 4 controller types.");
        }

        [TestMethod]
        public void GetControllerTypeForAssembliesWithSameTypeNamesInSameNamespaceThrows() {
            // Setup
            DefaultControllerFactory factory = GetDefaultControllerFactory("ns1a.ns1b");
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { Assembly.Load("MvcAssembly1"), Assembly.Load("MvcAssembly4") });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    factory.GetControllerType("C1");
                },
                "The controller name 'C1' is ambiguous between the following types: NS1a.NS1b.C1Controller, NS1a.NS1b.C1Controller. "
                    + "To disambiguate the controller set the controller's namespace in the route.");

            // Verify
            Assert.AreEqual<int>(4, controllerTypeCache.Count, "Cache should have 4 controller types.");
        }

        [TestMethod]
        public void GetControllerTypeSearchesAllNamespacesAsLastResort() {
            // Setup
            RequestContext requestContext = GetRequestContextWithNamespaces("ns3a.ns3b");
            DefaultControllerFactory factory = GetDefaultControllerFactory("ns1a.ns1b");
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { Assembly.Load("MvcAssembly1") });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;
            factory.RequestContext = requestContext;

            // Execute
            Type c2Type = factory.GetControllerType("C2");

            // Verify
            Assembly asm1 = Assembly.Load("MvcAssembly1");
            Type verifiedC2 = asm1.GetType("NS2a.NS2b.C2Controller");
            Assert.IsNotNull(verifiedC2, "Couldn't find real C2 type");
            Assert.AreEqual<Type>(verifiedC2, c2Type, "Should have found C2Controller type.");
            Assert.AreEqual<int>(2, controllerTypeCache.Count, "Cache should have 2 controller types.");
        }

        [TestMethod]
        public void GetControllerTypeSearchesRouteDefinedNamespacesBeforeApplicationDefinedNamespaces() {
            // Setup
            RequestContext requestContext = GetRequestContextWithNamespaces("ns3a.ns3b");
            DefaultControllerFactory factory = GetDefaultControllerFactory("ns1a.ns1b", "ns2a.ns2b");
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { Assembly.Load("MvcAssembly1"), Assembly.Load("MvcAssembly3") });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;
            factory.RequestContext = requestContext;

            // Execute
            Type c1Type = factory.GetControllerType("C1");
            Type c2Type = factory.GetControllerType("C2");

            // Verify
            Assembly asm1 = Assembly.Load("MvcAssembly1");
            Type verifiedC2 = asm1.GetType("NS2a.NS2b.C2Controller");
            Assembly asm3 = Assembly.Load("MvcAssembly3");
            Type verifiedC1 = asm3.GetType("NS3a.NS3b.C1Controller");
            Assert.IsNotNull(verifiedC1, "Couldn't find real C1 type");
            Assert.IsNotNull(verifiedC2, "Couldn't find real C2 type");
            Assert.AreEqual<Type>(verifiedC1, c1Type, "Should have found C1Controller type.");
            Assert.AreEqual<Type>(verifiedC2, c2Type, "Should have found C2Controller type.");
            Assert.AreEqual<int>(4, controllerTypeCache.Count, "Cache should have 4 controller types.");
        }

        [TestMethod]
        public void GetControllerTypeThatDoesntExist() {
            // Setup
            DefaultControllerFactory factory = GetDefaultControllerFactory("ns1a.ns1b", "ns2a.ns2b", "ns3a.ns3b", "ns4a.ns4b");
            MockBuildManager buildManagerMock = new MockBuildManager(new Assembly[] { Assembly.Load("MvcAssembly1"), Assembly.Load("MvcAssembly2"), Assembly.Load("MvcAssembly3"), Assembly.Load("MvcAssembly4") });
            ControllerTypeCache controllerTypeCache = new ControllerTypeCache();

            factory.BuildManager = buildManagerMock;
            factory.ControllerTypeCache = controllerTypeCache;

            // Execute
            Type randomType1 = factory.GetControllerType("Cx");
            Type randomType2 = factory.GetControllerType("Cy");
            Type randomType3 = factory.GetControllerType("Foo.Bar");
            Type randomType4 = factory.GetControllerType("C1Controller");

            // Verify
            Assert.IsNull(randomType1, "Controller type should not have been found.");
            Assert.IsNull(randomType2, "Controller type should not have been found.");
            Assert.IsNull(randomType3, "Controller type should not have been found.");
            Assert.IsNull(randomType4, "Controller type should not have been found.");
            Assert.AreEqual<int>(8, controllerTypeCache.Count, "Cache should have 8 controller types.");
        }

        [TestMethod]
        public void IsControllerType() {
            // Execute
            bool isController1 = ControllerTypeCache.IsControllerType(null);
            bool isController2 = ControllerTypeCache.IsControllerType(typeof(NonPublicController));
            bool isController3 = ControllerTypeCache.IsControllerType(typeof(MisspelledKontroller));
            bool isController4 = ControllerTypeCache.IsControllerType(typeof(AbstractController));
            bool isController5 = ControllerTypeCache.IsControllerType(typeof(NonIControllerController));
            bool isController6 = ControllerTypeCache.IsControllerType(typeof(Goodcontroller));

            // Verify
            Assert.IsFalse(isController1, "IsControllerType(null) should return false.");
            Assert.IsFalse(isController2, "Non-public types should not be considered controller types.");
            Assert.IsFalse(isController3, "Types not ending in 'Controller' should not be considered controller types.");
            Assert.IsFalse(isController4, "Abstract types should not be considered controller types.");
            Assert.IsFalse(isController5, "Types not implementing IController should not be considered controller types.");
            Assert.IsTrue(isController6, "The 'Controller' suffix on controller types is not required to be case-sensitive.");
        }

        private static DefaultControllerFactory GetDefaultControllerFactory(params string[] namespaces) {
            ControllerBuilder builder = new ControllerBuilder();
            builder.DefaultNamespaces.UnionWith(namespaces);
            return new DefaultControllerFactory() { ControllerBuilder = builder };
        }

        private static RequestContext GetRequestContextWithNamespaces(params string[] namespaces) {
            RouteData routeData = new RouteData();
            routeData.DataTokens["namespaces"] = namespaces;
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            RequestContext requestContext = new RequestContext(mockHttpContext.Object, routeData);
            return requestContext;
        }

        private sealed class DummyController : IController {
            #region IController Members
            void IController.Execute(ControllerContext controllerContext) {
                throw new NotImplementedException();
            }
            #endregion
        }

        private sealed class DummyControllerThrows : IController {
            public DummyControllerThrows() {
                throw new Exception("constructor");
            }

            #region IController Members
            void IController.Execute(ControllerContext controllerContext) {
                throw new NotImplementedException();
            }
            #endregion
        }

        public interface IDisposableController : IController, IDisposable {
        }
    }

    // BAD: type isn't public
    internal class NonPublicController : Controller {
    }

    // BAD: type doesn't end with 'Controller'
    public class MisspelledKontroller : Controller {
    }

    // BAD: type is abstract
    public abstract class AbstractController : Controller {
    }

    // BAD: type doesn't implement IController
    public class NonIControllerController {
    }

    // GOOD: 'Controller' suffix should be case-insensitive
    public class Goodcontroller : Controller {
    }
}
