namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ControllerBuilderTest {

        [TestMethod]
        public void ControllerBuilderReturnsDefaultControllerBuilderByDefault() {
            // Setup
            ControllerBuilder cb = new ControllerBuilder();
            
            // Execute
            IControllerFactory cf = cb.GetControllerFactory();

            // Verify
            Assert.IsInstanceOfType(cf, typeof(DefaultControllerFactory));
        }

        [TestMethod]
        public void ControllerIsDisposedIfExceptionThrown() {
            // Setup
            bool wasDisposed = false;
            ControllerBuilder cb = new ControllerBuilder();
            IController controller = new DisposableController(
                delegate() {
                    wasDisposed = true;
                },
                delegate() {
                    throw new Exception("Execute");
                });
            cb.SetControllerFactory(new SimpleControllerFactory(controller));
            RequestContext reqContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            reqContext.RouteData.Values["controller"] = "foo";
            MvcHandler handler = new MvcHandler(reqContext) {
                ControllerBuilder = cb
            };

            // Execute
            ExceptionHelper.ExpectException<Exception>(
                delegate() {
                    handler.ProcessRequest(reqContext.HttpContext);
                },
                "Execute");

            // Verify
            Assert.IsTrue(wasDisposed);
        }

        [TestMethod]
        public void ControllerIsDisposedIfNoExceptionThrown() {
            // Setup
            bool wasDisposed = false;
            ControllerBuilder cb = new ControllerBuilder();
            IController controller = new DisposableController(
                delegate() {
                    wasDisposed = true;
                },
                null /* executeCallback */);
            cb.SetControllerFactory(new SimpleControllerFactory(controller));
            RequestContext reqContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            reqContext.RouteData.Values["controller"] = "foo";
            MvcHandler handler = new MvcHandler(reqContext) {
                ControllerBuilder = cb
            };

            // Execute
            handler.ProcessRequest(reqContext.HttpContext);

            // Verify
            Assert.IsTrue(wasDisposed);
        }

        [TestMethod]
        public void CreateControllerWithFactoryThatCannotBeCreatedThrows() {
            // Setup
            ControllerBuilder cb = new ControllerBuilder();
            cb.SetControllerFactory(typeof(ControllerFactoryThrowsFromConstructor));

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    RequestContext reqContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
                    reqContext.RouteData.Values["controller"] = "foo";
                    MvcHandler handler = new MvcHandler(reqContext) {
                        ControllerBuilder = cb
                    };
                    handler.ProcessRequest(reqContext.HttpContext);
                },
                "There was an error creating the IControllerFactory 'System.Web.Mvc.Test.ControllerBuilderTest+ControllerFactoryThrowsFromConstructor'. Check that it has a public parameterless constructor.");
        }

        [TestMethod]
        public void CreateControllerWithFactoryThatReturnsNullThrows() {
            // Setup
            ControllerBuilder cb = new ControllerBuilder();
            cb.SetControllerFactory(typeof(ControllerFactoryReturnsNull));

            // Execute
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    RequestContext reqContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
                    reqContext.RouteData.Values["controller"] = "boo";
                    MvcHandler handler = new MvcHandler(reqContext) {
                        ControllerBuilder = cb
                    };
                    handler.ProcessRequest(reqContext.HttpContext);
                },
                "The IControllerFactory 'System.Web.Mvc.Test.ControllerBuilderTest+ControllerFactoryReturnsNull' did not return a controller for a controller named 'boo'.");
        }

        [TestMethod]
        public void CreateControllerWithFactoryThatThrowsDoesNothingSpecial() {
            // Setup
            ControllerBuilder cb = new ControllerBuilder();
            cb.SetControllerFactory(typeof(ControllerFactoryThrows));

            // Execute
            ExceptionHelper.ExpectException<Exception>(
                delegate {
                    RequestContext reqContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
                    reqContext.RouteData.Values["controller"] = "foo";
                    MvcHandler handler = new MvcHandler(reqContext) {
                        ControllerBuilder = cb
                    };
                    handler.ProcessRequest(reqContext.HttpContext);
                },
                "ControllerFactoryThrows");
        }

        [TestMethod]
        public void CreateControllerWithFactoryInstanceReturnsInstance() {
            // Setup
            ControllerBuilder cb = new ControllerBuilder();
            DefaultControllerFactory factory = new DefaultControllerFactory();
            cb.SetControllerFactory(factory);

            // Execute
            IControllerFactory cf = cb.GetControllerFactory();

            // Verify
            Assert.AreSame(factory, cf);
        }

        [TestMethod]
        public void CreateControllerWithFactoryTypeReturnsValidType() {
            // Setup
            ControllerBuilder cb = new ControllerBuilder();
            cb.SetControllerFactory(typeof(MockControllerFactory));

            // Execute
            IControllerFactory cf = cb.GetControllerFactory();
            
            // Verify
            Assert.IsInstanceOfType(cf, typeof(MockControllerFactory));
        }

        [TestMethod]
        public void SetControllerFactoryInstanceWithNullThrows() {
            ControllerBuilder cb = new ControllerBuilder();
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    cb.SetControllerFactory((IControllerFactory)null);
                },
                "controllerFactory");
        }

        [TestMethod]
        public void SetControllerFactoryTypeWithNullThrows() {
            ControllerBuilder cb = new ControllerBuilder();
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    cb.SetControllerFactory((Type)null);
                },
                "controllerFactoryType");
        }

        [TestMethod]
        public void SetControllerFactoryTypeWithNonFactoryTypeThrows() {
            ControllerBuilder cb = new ControllerBuilder();
            ExceptionHelper.ExpectArgumentException(
                delegate {
                    cb.SetControllerFactory(typeof(int));
                },
                "The controller factory type 'System.Int32' must implement the IControllerFactory interface.\r\nParameter name: controllerFactoryType");
        }

        public class ControllerFactoryThrowsFromConstructor : IControllerFactory {
            public ControllerFactoryThrowsFromConstructor() {
                throw new Exception("ControllerFactoryThrowsFromConstructor");
            }

            public IController CreateController(RequestContext context, string controllerName) {
                return null;
            }

            public void DisposeController(IController controller) {
            }
        }

        public class ControllerFactoryReturnsNull : IControllerFactory {
            public IController CreateController(RequestContext context, string controllerName) {
                return null;
            }

            public void DisposeController(IController controller) {
            }
        }

        public class ControllerFactoryThrows : IControllerFactory {
            public IController CreateController(RequestContext context, string controllerName) {
                throw new Exception("ControllerFactoryThrows");
            }

            public void DisposeController(IController controller) {
            }
        }

        public class MockControllerFactory : IControllerFactory {

            public IController CreateController(RequestContext context, string controllerName) {
                return new DummyController { ControllerName = controllerName };
            }

            public void DisposeController(IController controller) {
            }
        }

        public class DummyController : IController {
            public string ControllerName {
                get;
                set;
            }

            #region IController Members
            void IController.Execute(ControllerContext controllerContext) {
                throw new NotImplementedException();
            }
            #endregion
        }

        public class SimpleControllerFactory : IControllerFactory {

            private IController _instance;

            public SimpleControllerFactory(IController instance) {
                _instance = instance;
            }

            #region IControllerFactory Members
            public IController CreateController(RequestContext context, string controllerName) {
                return _instance;
            }
            public void DisposeController(IController controller) {
                IDisposable disposable = _instance as IDisposable;
                if (disposable != null) {
                    disposable.Dispose();
                }
            }
            #endregion
        }

        public class DisposableController : IController, IDisposable {

            private Action _disposeCallback;
            private Action _executeCallback;
            private static Action _defaultAction = delegate {
            };

            public DisposableController(Action disposeCallback, Action executeCallback) {
                _disposeCallback = disposeCallback ?? _defaultAction;
                _executeCallback = executeCallback ?? _defaultAction;
            }

            public void Execute(ControllerContext controllerContext) {
                _executeCallback();
            }
            public void Dispose() {
                _disposeCallback();
            }
        }

    }
}
