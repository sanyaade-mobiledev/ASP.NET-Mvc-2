namespace Microsoft.Web.Mvc.Test {
    using System.Globalization;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class ExpressionInputExtensionsTest {
        [TestMethod]
        public void TextboxForWithExpressionRendersInputTagUsingExpression() {
            // Arrange
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
            ViewDataDictionary viewData = new ViewDataDictionary(new Product { ProductName = "ASP.NET MVC" });
            Mock<IViewDataContainer> mockIViewDataContainer = new Mock<IViewDataContainer>();
            mockIViewDataContainer.Expect(c => c.ViewData).Returns(viewData);
            HtmlHelper<Product> htmlHelper = new HtmlHelper<Product>(mockViewContext.Object, mockIViewDataContainer.Object);

            // Act
            string result = htmlHelper.TextBoxFor(p => p.ProductName);

            // Assert
            Assert.AreEqual("<input id=\"ProductName\" name=\"ProductName\" type=\"text\" value=\"ASP.NET MVC\" />", result);
        }

        [TestMethod]
        public void TextboxForWithExpressionRendersInputTagUsingExpressionUsingValueFromModelState() {
            // Arrange
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
            ViewDataDictionary viewData = new ViewDataDictionary(new Product { ProductName = "ASP.NET MVC" });
            viewData.ModelState.SetModelValue("ProductName", new ValueProviderResult("Something Else", "Something Else", CultureInfo.InvariantCulture));
            Mock<IViewDataContainer> mockIViewDataContainer = new Mock<IViewDataContainer>();
            mockIViewDataContainer.Expect(c => c.ViewData).Returns(viewData);
            HtmlHelper<Product> htmlHelper = new HtmlHelper<Product>(mockViewContext.Object, mockIViewDataContainer.Object);

            // Act
            string result = htmlHelper.TextBoxFor(p => p.ProductName);

            // Assert
            Assert.AreEqual("<input id=\"ProductName\" name=\"ProductName\" type=\"text\" value=\"Something Else\" />", result);
        }

        [TestMethod]
        public void TextboxForWithExpressionRendersInputTagUsingExpressionUsingValueFromViewState() {
            // Arrange
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
            ViewDataDictionary viewData = new ViewDataDictionary();
            Product productInViewData = new Product { ProductName = "Something Else" };
            viewData["ProductName"] = "Not Something Else";
            Mock<IViewDataContainer> mockIViewDataContainer = new Mock<IViewDataContainer>();
            mockIViewDataContainer.Expect(c => c.ViewData).Returns(viewData);
            HtmlHelper<Product> htmlHelper = new HtmlHelper<Product>(mockViewContext.Object, mockIViewDataContainer.Object);

            // Act
            string result = htmlHelper.TextBoxFor(p => p.ProductName);

            // Assert
            Assert.AreEqual("<input id=\"ProductName\" name=\"ProductName\" type=\"text\" value=\"Not Something Else\" />", result);
        }

        [TestMethod]
        public void TextboxForWithExpressionRendersInputTagUsingExpressionWithIntProperty() {
            // Arrange
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
            ViewDataDictionary viewData = new ViewDataDictionary(new Product { Id = 123 });
            Mock<IViewDataContainer> mockIViewDataContainer = new Mock<IViewDataContainer>();
            mockIViewDataContainer.Expect(c => c.ViewData).Returns(viewData);
            HtmlHelper<Product> htmlHelper = new HtmlHelper<Product>(mockViewContext.Object, mockIViewDataContainer.Object);

            // Act
            string result = htmlHelper.TextBoxFor(p => p.Id);

            // Assert
            Assert.AreEqual("<input id=\"Id\" name=\"Id\" type=\"text\" value=\"123\" />", result);
        }

        [TestMethod]
        public void TextboxForWithExpressionContainingMethodCallRendersInputTagUsingExpressionProperty() {
            // Arrange
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
            ViewDataDictionary viewData = new ViewDataDictionary(new Product { Id = 123 });
            Mock<IViewDataContainer> mockIViewDataContainer = new Mock<IViewDataContainer>();
            mockIViewDataContainer.Expect(c => c.ViewData).Returns(viewData);
            HtmlHelper<Product> htmlHelper = new HtmlHelper<Product>(mockViewContext.Object, mockIViewDataContainer.Object);

            // Act
            string result = htmlHelper.TextBoxFor(p => p.Id.ToString());

            // Assert
            Assert.AreEqual("<input id=\"Id\" name=\"Id\" type=\"text\" value=\"123\" />", result);
        }

        [TestMethod]
        public void TextboxForWithAttributesAndExpressionRendersAttributes() {
            // Arrange
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>();
            ViewDataDictionary viewData = new ViewDataDictionary(new Product { ProductName = "ASP.NET MVC" });
            Mock<IViewDataContainer> mockIViewDataContainer = new Mock<IViewDataContainer>();
            mockIViewDataContainer.Expect(c => c.ViewData).Returns(viewData);
            HtmlHelper<Product> htmlHelper = new HtmlHelper<Product>(mockViewContext.Object, mockIViewDataContainer.Object);

            // Act
            string result = htmlHelper.TextBoxFor(p => p.ProductName, new { width = 123, maxlength = 99 });

            // Assert
            Assert.AreEqual("<input id=\"ProductName\" maxlength=\"99\" name=\"ProductName\" type=\"text\" value=\"ASP.NET MVC\" width=\"123\" />", result);
        }

        internal class Product {
            public string ProductName {
                get;
                set;
            }

            public int Id {
                get;
                set;
            }
        }
    }
}
