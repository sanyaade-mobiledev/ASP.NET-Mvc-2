namespace System.Web.Mvc.Test {
    using System;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OutputCacheAttributeTest {

        [TestMethod]
        public void ExecuteReplacesMinusOneLocationWithAny() {
            // Arrange
            OutputCacheAttribute attr = new OutputCacheAttribute() {
                Duration = 20,
                Location = (OutputCacheLocation)(-1),
                NoStore = true,
                VaryByContentEncoding = " ContentEncoding3 ; ContentEncoding4 ",
                VaryByCustom = " Custom3 ; Custom4 ",
                VaryByHeader = " Header3 ; Header4 ",
                VaryByParam = " Param3 ; Param4 "
            };

            // Arrange
            Mock<HttpCachePolicyBase> mockCache = new Mock<HttpCachePolicyBase>();
            HttpCacheVaryByContentEncodings varyByContentEncodings = GetInternalType<HttpCacheVaryByContentEncodings>();
            HttpCacheVaryByHeaders varyByHeaders = GetInternalType<HttpCacheVaryByHeaders>();
            HttpCacheVaryByParams varyByParams = GetInternalType<HttpCacheVaryByParams>();

            // timestamp is midnight on Jan 1, 2001 and validity period is 20 sec
            mockCache.Expect(c => c.SetNoStore()).Verifiable();
            mockCache.Expect(c => c.SetCacheability(HttpCacheability.Public)).Verifiable();
            mockCache.Expect(c => c.SetExpires(new DateTime(2001, 1, 1, 0, 0, 20))).Verifiable();
            mockCache.Expect(c => c.SetMaxAge(new TimeSpan(0, 0, 20))).Verifiable();
            mockCache.Expect(c => c.SetValidUntilExpires(true)).Verifiable();
            mockCache.Expect(c => c.SetLastModified(new DateTime(2001, 1, 1))).Verifiable();
            mockCache.Expect(c => c.VaryByContentEncodings).Returns(varyByContentEncodings).Verifiable();
            mockCache.Expect(c => c.VaryByHeaders).Returns(varyByHeaders).Verifiable();
            mockCache.Expect(c => c.SetVaryByCustom(" Custom3 ; Custom4 ")).Verifiable();
            mockCache.Expect(c => c.VaryByParams).Returns(varyByParams).Verifiable();
            HttpCachePolicyBase cache = mockCache.Object;
            ResultExecutingContext filterContext = GetFilterContext(cache);

            // Act
            attr.OnResultExecuting(filterContext);

            // Assert
            Assert.IsTrue(varyByContentEncodings["ContentEncoding3"]);
            Assert.IsTrue(varyByContentEncodings["ContentEncoding4"]);
            Assert.IsTrue(varyByHeaders["Header3"]);
            Assert.IsTrue(varyByHeaders["Header4"]);
            Assert.IsTrue(varyByParams["Param3"]);
            Assert.IsTrue(varyByParams["Param4"]);
            Assert.IsFalse(varyByParams.IgnoreParams);
            mockCache.Verify();
        }

        //[TestMethod]
        // See DevDiv bug 207591 and 207634
        public void ExecuteWithDisabledOutputCacheSectionThrows() {
            // Arrange
            ResultExecutingContext filterContext = GetFilterContext(new Mock<HttpCachePolicyBase>().Object);
            OutputCacheAttribute attr = new OutputCacheAttribute();
            OutputCacheSection newSection = new OutputCacheSection() { EnableOutputCache = false };

            // Act & Assert
            using (ReplaceOutputCacheSection(newSection)) {
                attr.OnResultExecuting(filterContext);
            }
        }

        [TestMethod]
        public void ExecuteWithInvalidOutputCacheProfileThrows() {
            // Arrange
            ResultExecutingContext filterContext = GetFilterContext(new Mock<HttpCachePolicyBase>().Object);
            OutputCacheAttribute attr = new OutputCacheAttribute() { CacheProfile = "InvalidProfile" };

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    attr.OnResultExecuting(filterContext);
                },
                "The configuration file does not contain a cache profile with the name 'InvalidProfile'.");
        }

        [TestMethod]
        public void ExecuteWithProfileDisabled() {
            // Arrange
            ResultExecutingContext filterContext = GetFilterContext(new Mock<HttpCachePolicyBase>().Object);
            OutputCacheAttribute attr = new OutputCacheAttribute() { CacheProfile = "MyProfile" };
            OutputCacheProfile profile = new OutputCacheProfile("MyProfile") { Enabled = false };

            // Act & Assert
            // Should do nothing - if the code tries to call into the HttpCachePolicyBase object, it will
            // throw a NotImplementedException.
            using (AddOutputCacheProfile(profile)) {
                attr.OnResultExecuting(filterContext);
            }
        }

        [TestMethod]
        public void ExecuteWithProfileEnabled() {
            // Arrange
            OutputCacheProfile profile = GetProfile("MyProfile");
            OutputCacheAttribute attr = new OutputCacheAttribute() { CacheProfile = "MyProfile" };

            // Arrange
            Mock<HttpCachePolicyBase> mockCache = new Mock<HttpCachePolicyBase>();
            HttpCacheVaryByContentEncodings varyByContentEncodings = GetInternalType<HttpCacheVaryByContentEncodings>();
            HttpCacheVaryByHeaders varyByHeaders = GetInternalType<HttpCacheVaryByHeaders>();
            HttpCacheVaryByParams varyByParams = GetInternalType<HttpCacheVaryByParams>();

            // by default, timestamp is midnight on Jan 1, 2001 and validity period is 10 sec
            mockCache.Expect(c => c.SetCacheability(HttpCacheability.ServerAndPrivate)).Verifiable();
            mockCache.Expect(c => c.SetExpires(new DateTime(2001, 1, 1, 0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetMaxAge(new TimeSpan(0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetValidUntilExpires(true)).Verifiable();
            mockCache.Expect(c => c.SetLastModified(new DateTime(2001, 1, 1))).Verifiable();
            mockCache.Expect(c => c.VaryByContentEncodings).Returns(varyByContentEncodings).Verifiable();
            mockCache.Expect(c => c.VaryByHeaders).Returns(varyByHeaders).Verifiable();
            mockCache.Expect(c => c.SetVaryByCustom(" Custom1 ; Custom2 ")).Verifiable();
            mockCache.Expect(c => c.VaryByParams).Returns(varyByParams).Verifiable();
            HttpCachePolicyBase cache = mockCache.Object;
            ResultExecutingContext filterContext = GetFilterContext(cache);

            // Act
            using (AddOutputCacheProfile(profile)) {
                attr.OnResultExecuting(filterContext);
            }

            // Assert
            Assert.IsTrue(varyByContentEncodings["ContentEncoding1"]);
            Assert.IsTrue(varyByContentEncodings["ContentEncoding2"]);
            Assert.IsTrue(varyByHeaders["Header1"]);
            Assert.IsTrue(varyByHeaders["Header2"]);
            Assert.IsTrue(varyByParams["Param1"]);
            Assert.IsTrue(varyByParams["Param2"]);
            Assert.IsFalse(varyByParams.IgnoreParams);
            mockCache.Verify();
        }

        [TestMethod]
        public void ExecuteWithProfileGetsOverriddenWithExplicitValues() {
            // Arrange
            OutputCacheProfile profile = GetProfile("MyProfile");
            OutputCacheAttribute attr = new OutputCacheAttribute() {
                CacheProfile = "MyProfile",
                Duration = 20,
                Location = OutputCacheLocation.Server,
                NoStore = true,
                VaryByContentEncoding = " ContentEncoding3 ; ContentEncoding4 ",
                VaryByCustom = " Custom3 ; Custom4 ",
                VaryByHeader = " Header3 ; Header4 ",
                VaryByParam = " Param3 ; Param4 "
            };

            // Arrange
            Mock<HttpCachePolicyBase> mockCache = new Mock<HttpCachePolicyBase>();
            HttpCacheVaryByContentEncodings varyByContentEncodings = GetInternalType<HttpCacheVaryByContentEncodings>();
            HttpCacheVaryByHeaders varyByHeaders = GetInternalType<HttpCacheVaryByHeaders>();
            HttpCacheVaryByParams varyByParams = GetInternalType<HttpCacheVaryByParams>();

            // timestamp is midnight on Jan 1, 2001 and validity period is 20 sec
            mockCache.Expect(c => c.SetNoStore()).Verifiable();
            mockCache.Expect(c => c.SetCacheability(HttpCacheability.ServerAndNoCache)).Verifiable();
            mockCache.Expect(c => c.SetExpires(new DateTime(2001, 1, 1, 0, 0, 20))).Verifiable();
            mockCache.Expect(c => c.SetMaxAge(new TimeSpan(0, 0, 20))).Verifiable();
            mockCache.Expect(c => c.SetValidUntilExpires(true)).Verifiable();
            mockCache.Expect(c => c.SetLastModified(new DateTime(2001, 1, 1))).Verifiable();
            mockCache.Expect(c => c.VaryByContentEncodings).Returns(varyByContentEncodings).Verifiable();
            mockCache.Expect(c => c.VaryByHeaders).Returns(varyByHeaders).Verifiable();
            mockCache.Expect(c => c.SetVaryByCustom(" Custom3 ; Custom4 ")).Verifiable();
            mockCache.Expect(c => c.VaryByParams).Returns(varyByParams).Verifiable();
            HttpCachePolicyBase cache = mockCache.Object;
            ResultExecutingContext filterContext = GetFilterContext(cache);

            // Act
            using (AddOutputCacheProfile(profile)) {
                attr.OnResultExecuting(filterContext);
            }

            // Assert
            Assert.IsTrue(varyByContentEncodings["ContentEncoding3"]);
            Assert.IsTrue(varyByContentEncodings["ContentEncoding4"]);
            Assert.IsTrue(varyByHeaders["Header3"]);
            Assert.IsTrue(varyByHeaders["Header4"]);
            Assert.IsTrue(varyByParams["Param3"]);
            Assert.IsTrue(varyByParams["Param4"]);
            Assert.IsFalse(varyByParams.IgnoreParams);
            mockCache.Verify();
        }

        [TestMethod]
        public void InitializeCacheClient() {
            // Arrange
            Mock<HttpCachePolicyBase> mockCache = new Mock<HttpCachePolicyBase>();
            // by default, timestamp is midnight on Jan 1, 2001 and validity period is 10 sec
            mockCache.Expect(c => c.SetCacheability(HttpCacheability.Private)).Verifiable();
            mockCache.Expect(c => c.SetExpires(new DateTime(2001, 1, 1, 0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetMaxAge(new TimeSpan(0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetValidUntilExpires(true)).Verifiable();
            mockCache.Expect(c => c.SetLastModified(new DateTime(2001, 1, 1))).Verifiable();
            HttpCachePolicyBase cache = mockCache.Object;

            var initializer = GetInitializer();
            initializer.Location = OutputCacheLocation.Client;

            // Act
            initializer.InitializeCache(cache);

            // Assert
            mockCache.Verify();
        }

        [TestMethod]
        public void InitializeCacheDownstream() {
            // Arrange
            Mock<HttpCachePolicyBase> mockCache = new Mock<HttpCachePolicyBase>();
            HttpCacheVaryByContentEncodings varyByContentEncodings = GetInternalType<HttpCacheVaryByContentEncodings>();
            HttpCacheVaryByHeaders varyByHeaders = GetInternalType<HttpCacheVaryByHeaders>();

            // by default, timestamp is midnight on Jan 1, 2001 and validity period is 10 sec
            mockCache.Expect(c => c.SetNoServerCaching()).Verifiable();
            mockCache.Expect(c => c.SetCacheability(HttpCacheability.Public)).Verifiable();
            mockCache.Expect(c => c.SetExpires(new DateTime(2001, 1, 1, 0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetMaxAge(new TimeSpan(0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetValidUntilExpires(true)).Verifiable();
            mockCache.Expect(c => c.SetLastModified(new DateTime(2001, 1, 1))).Verifiable();
            mockCache.Expect(c => c.VaryByContentEncodings).Returns(varyByContentEncodings).Verifiable();
            mockCache.Expect(c => c.VaryByHeaders).Returns(varyByHeaders).Verifiable();
            HttpCachePolicyBase cache = mockCache.Object;

            var initializer = GetInitializer();
            initializer.Location = OutputCacheLocation.Downstream;

            // Act
            initializer.InitializeCache(cache);

            // Assert
            Assert.IsTrue(varyByContentEncodings["ContentEncoding1"]);
            Assert.IsTrue(varyByContentEncodings["ContentEncoding2"]);
            Assert.IsTrue(varyByHeaders["Header1"]);
            Assert.IsTrue(varyByHeaders["Header2"]);
            mockCache.Verify();
        }

        [TestMethod]
        public void InitializeCacheNoStore() {
            // Arrange
            Mock<HttpCachePolicyBase> mockCache = new Mock<HttpCachePolicyBase>();
            mockCache.Expect(c => c.SetNoStore()).Verifiable();
            mockCache.Expect(c => c.SetCacheability(HttpCacheability.NoCache)).Verifiable();
            HttpCachePolicyBase cache = mockCache.Object;

            var initializer = GetInitializer();
            initializer.Location = OutputCacheLocation.None;
            initializer.NoStore = true;

            // Act
            initializer.InitializeCache(cache);

            // Assert
            mockCache.Verify();
        }

        [TestMethod]
        public void InitializeCacheServerAndClient() {
            // Arrange
            Mock<HttpCachePolicyBase> mockCache = new Mock<HttpCachePolicyBase>();
            HttpCacheVaryByContentEncodings varyByContentEncodings = GetInternalType<HttpCacheVaryByContentEncodings>();
            HttpCacheVaryByHeaders varyByHeaders = GetInternalType<HttpCacheVaryByHeaders>();
            HttpCacheVaryByParams varyByParams = GetInternalType<HttpCacheVaryByParams>();

            // by default, timestamp is midnight on Jan 1, 2001 and validity period is 10 sec
            mockCache.Expect(c => c.SetCacheability(HttpCacheability.ServerAndPrivate)).Verifiable();
            mockCache.Expect(c => c.SetExpires(new DateTime(2001, 1, 1, 0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetMaxAge(new TimeSpan(0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetValidUntilExpires(true)).Verifiable();
            mockCache.Expect(c => c.SetLastModified(new DateTime(2001, 1, 1))).Verifiable();
            mockCache.Expect(c => c.VaryByContentEncodings).Returns(varyByContentEncodings).Verifiable();
            mockCache.Expect(c => c.VaryByHeaders).Returns(varyByHeaders).Verifiable();
            mockCache.Expect(c => c.SetVaryByCustom(" Custom1 ; Custom2 ")).Verifiable();
            mockCache.Expect(c => c.VaryByParams).Returns(varyByParams).Verifiable();
            HttpCachePolicyBase cache = mockCache.Object;

            var initializer = GetInitializer();
            initializer.Location = OutputCacheLocation.ServerAndClient;

            // Act
            initializer.InitializeCache(cache);

            // Assert
            Assert.IsTrue(varyByContentEncodings["ContentEncoding1"]);
            Assert.IsTrue(varyByContentEncodings["ContentEncoding2"]);
            Assert.IsTrue(varyByHeaders["Header1"]);
            Assert.IsTrue(varyByHeaders["Header2"]);
            Assert.IsTrue(varyByParams["Param1"]);
            Assert.IsTrue(varyByParams["Param2"]);
            Assert.IsFalse(varyByParams.IgnoreParams);
            mockCache.Verify();
        }

        [TestMethod]
        public void InitializeCacheServerAndClientWithoutVaryByParam() {
            // Arrange
            Mock<HttpCachePolicyBase> mockCache = new Mock<HttpCachePolicyBase>();
            HttpCacheVaryByContentEncodings varyByContentEncodings = GetInternalType<HttpCacheVaryByContentEncodings>();
            HttpCacheVaryByHeaders varyByHeaders = GetInternalType<HttpCacheVaryByHeaders>();
            HttpCacheVaryByParams varyByParams = GetInternalType<HttpCacheVaryByParams>();

            // by default, timestamp is midnight on Jan 1, 2001 and validity period is 10 sec
            mockCache.Expect(c => c.SetCacheability(HttpCacheability.ServerAndPrivate)).Verifiable();
            mockCache.Expect(c => c.SetExpires(new DateTime(2001, 1, 1, 0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetMaxAge(new TimeSpan(0, 0, 10))).Verifiable();
            mockCache.Expect(c => c.SetValidUntilExpires(true)).Verifiable();
            mockCache.Expect(c => c.SetLastModified(new DateTime(2001, 1, 1))).Verifiable();
            mockCache.Expect(c => c.VaryByContentEncodings).Returns(varyByContentEncodings).Verifiable();
            mockCache.Expect(c => c.VaryByHeaders).Returns(varyByHeaders).Verifiable();
            mockCache.Expect(c => c.SetVaryByCustom(" Custom1 ; Custom2 ")).Verifiable();
            mockCache.Expect(c => c.VaryByParams).Returns(varyByParams).Verifiable();
            HttpCachePolicyBase cache = mockCache.Object;

            var initializer = GetInitializer();
            initializer.VaryByParam = String.Empty;
            initializer.Location = OutputCacheLocation.ServerAndClient;

            // Act
            initializer.InitializeCache(cache);

            // Assert
            Assert.IsTrue(varyByContentEncodings["ContentEncoding1"]);
            Assert.IsTrue(varyByContentEncodings["ContentEncoding2"]);
            Assert.IsTrue(varyByHeaders["Header1"]);
            Assert.IsTrue(varyByHeaders["Header2"]);
            Assert.IsTrue(varyByParams.IgnoreParams);
            mockCache.Verify();
        }

        [TestMethod]
        public void InitializeCacheWithInvalidDurationThrows() {
            // Arrange
            HttpCachePolicyBase cache = new Mock<HttpCachePolicyBase>().Object;
            var initializer = GetInitializer();
            initializer.Duration = 0;

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    initializer.InitializeCache(cache);
                },
                "A valid value for 'Duration' must be specified on the OutputCacheAttribute declaration or in a cache profile in the configuration file.");
        }

        [TestMethod]
        public void SetCacheabilityInvalidThrows() {
            // Arrange
            HttpCachePolicyBase cache = new Mock<HttpCachePolicyBase>().Object;
            var initializer = new OutputCacheAttribute.OutputCacheInitializer() { Duration = 10, Location = (OutputCacheLocation)(-1) };

            // Act & Assert
            ExceptionHelper.ExpectException<InvalidOperationException>(
                delegate {
                    initializer.InitializeCache(cache);
                },
                "A valid value for 'Location' must be specified on the OutputCacheAttribute declaration or in a cache profile in the configuration file.");
        }

        [TestMethod]
        public void StringPropertiesReturnEmptyInsteadOfNull() {
            // Arrange
            OutputCacheAttribute attr = new OutputCacheAttribute();

            // Act & Assert
            MemberHelper.TestStringProperty(attr, "CacheProfile", String.Empty, false /* testDefaultValue */);
            MemberHelper.TestStringProperty(attr, "VaryByContentEncoding", String.Empty, false /* testDefaultValue */);
            MemberHelper.TestStringProperty(attr, "VaryByCustom", String.Empty, false /* testDefaultValue */);
            MemberHelper.TestStringProperty(attr, "VaryByHeader", String.Empty, false /* testDefaultValue */);
            MemberHelper.TestStringProperty(attr, "VaryByParam", String.Empty, false /* testDefaultValue */);
        }

        private static T GetInternalType<T>() {
            return (T)Activator.CreateInstance(typeof(T), true /* nonPublic */);
        }

        private static ResultExecutingContext GetFilterContext(HttpCachePolicyBase cache) {
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Expect(r => r.Cache).Returns(cache);
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Response).Returns(mockResponse.Object);
            mockHttpContext.Expect(c => c.Timestamp).Returns(new DateTime(2001, 1, 1));
            return new ResultExecutingContext(
                new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object),
                EmptyResult.Instance);
        }

        private static OutputCacheAttribute.OutputCacheInitializer GetInitializer() {
            // initialize with known-good values so that we can selectively make them invalid
            return new OutputCacheAttribute.OutputCacheInitializer() {
                Duration = 10,
                Location = OutputCacheLocation.Any,
                NoStore = false,
                Timestamp = new DateTime(2001, 1, 1),
                VaryByContentEncoding = " ContentEncoding1 ; ContentEncoding2 ",
                VaryByCustom = " Custom1 ; Custom2 ",
                VaryByHeader = " Header1 ; Header2 ",
                VaryByParam = " Param1 ; Param2 "
            };
        }

        private static OutputCacheProfile GetProfile(string name) {
            // initialize with known-good values so that we can selectively make them invalid
            return new OutputCacheProfile(name) {
                Duration = 10,
                Location = OutputCacheLocation.ServerAndClient,
                NoStore = false,
                VaryByContentEncoding = " ContentEncoding1 ; ContentEncoding2 ",
                VaryByCustom = " Custom1 ; Custom2 ",
                VaryByHeader = " Header1 ; Header2 ",
                VaryByParam = " Param1 ; Param2 "
            };
        }

        private static IDisposable AddOutputCacheProfile(OutputCacheProfile profile) {
            OutputCacheSettingsSection settings = new OutputCacheSettingsSection();
            settings.OutputCacheProfiles.Add(profile);
            ConfigHelper.OverrideSection("system.web/caching/outputCacheSettings", settings);
            return new MyDisposable(ConfigHelper.Revert);
        }

        private static IDisposable ReplaceOutputCacheSection(OutputCacheSection replacement) {
            ConfigHelper.OverrideSection("system.web/caching/outputCache", replacement);
            return new MyDisposable(ConfigHelper.Revert);
        }

        private sealed class MyDisposable : IDisposable {
            Action _dispose;
            public MyDisposable(Action dispose) {
                _dispose = dispose;
            }
            public void Dispose() {
                _dispose();
            }
        }
    }
}
