namespace System.Web.Mvc.Html.Test {
    using System;
    using System.Web.Mvc.Test;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HtmlValidationExtensionsTest {
        [TestMethod]
        public void ValidationMessageWithEmptyNameThrows() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper();

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    htmlHelper.ValidationMessage(String.Empty);
                },
                "modelName");
        }

        [TestMethod]
        public void ValidationMessageReturnsFirstError() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act 
            string html = htmlHelper.ValidationMessage("foo");

            // Assert
            Assert.AreEqual(@"<span class=""field-validation-error"">foo error &lt;1&gt;</span>", html);
        }

        [TestMethod]
        public void ValidationMessageReturnsNullForInvalidName() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("boo");

            // Assert
            Assert.IsNull(html, "html should be null if name is invalid.");
        }

        [TestMethod]
        public void ValidationMessageReturnsWithObjectAttributes() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("foo", new { bar = "bar" });

            // Assert
            Assert.AreEqual(@"<span bar=""bar"" class=""field-validation-error"">foo error &lt;1&gt;</span>", html);
        }

        [TestMethod]
        public void ValidationMessageReturnsWithCustomClassOverridesDefault() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("foo", new { @class = "my-custom-css-class" });

            // Assert
            Assert.AreEqual(@"<span class=""my-custom-css-class"">foo error &lt;1&gt;</span>", html);
        }

        [TestMethod]
        public void ValidationMessageReturnsWithCustomMessage() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("foo", "bar error");

            // Assert
            Assert.AreEqual(@"<span class=""field-validation-error"">bar error</span>", html);
        }

        [TestMethod]
        public void ValidationMessageReturnsWithCustomMessageAndObjectAttributes() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("foo", "bar error", new { baz = "baz" });

            // Assert
            Assert.AreEqual(@"<span baz=""baz"" class=""field-validation-error"">bar error</span>", html);
        }

        [TestMethod]
        public void ValidationMessageWithModelStateAndNoErrors() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationMessage("baz");

            // Assert
            Assert.IsNull(html, "html should be null if there are no errors");
        }

        [TestMethod]
        public void ValidationSummary() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationSummary();

            // Assert
            Assert.AreEqual(@"<ul class=""validation-summary-errors""><li>foo error &lt;1&gt;</li>
<li>foo error 2</li>
<li>bar error &lt;1&gt;</li>
<li>bar error 2</li>
</ul>"
                , html);
        }

        [TestMethod]
        public void ValidationSummaryWithDictionary() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());
            RouteValueDictionary htmlAttributes = new RouteValueDictionary();
            htmlAttributes["class"] = "my-class";

            // Act
            string html = htmlHelper.ValidationSummary(htmlAttributes);

            // Assert
            Assert.AreEqual(@"<ul class=""my-class""><li>foo error &lt;1&gt;</li>
<li>foo error 2</li>
<li>bar error &lt;1&gt;</li>
<li>bar error 2</li>
</ul>"
                , html);
        }

        [TestMethod]
        public void ValidationSummaryWithNoErrorsReturnsNull() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(new ViewDataDictionary());

            // Act
            string html = htmlHelper.ValidationSummary();

            // Assert
            Assert.IsNull(html, "html should be null if there are no errors to report.");
        }

        [TestMethod]
        public void ValidationSummaryWithObjectAttributes() {
            // Arrange
            HtmlHelper htmlHelper = HtmlHelperTest.GetHtmlHelper(GetViewDataWithModelErrors());

            // Act
            string html = htmlHelper.ValidationSummary(new { baz = "baz" });

            // Assert
            Assert.AreEqual(@"<ul baz=""baz"" class=""validation-summary-errors""><li>foo error &lt;1&gt;</li>
<li>foo error 2</li>
<li>bar error &lt;1&gt;</li>
<li>bar error 2</li>
</ul>"
                , html);
        }

        private static ViewDataDictionary GetViewDataWithModelErrors() {
            ViewDataDictionary viewData = new ViewDataDictionary();
            ModelState modelStateFoo = new ModelState();
            ModelState modelStateBar = new ModelState();
            ModelState modelStateBaz = new ModelState();
            modelStateFoo.Errors.Add(new ModelError("foo error <1>"));
            modelStateFoo.Errors.Add(new ModelError("foo error 2"));
            modelStateBar.Errors.Add(new ModelError("bar error <1>"));
            modelStateBar.Errors.Add(new ModelError("bar error 2"));
            viewData.ModelState["foo"] = modelStateFoo;
            viewData.ModelState["bar"] = modelStateBar;
            viewData.ModelState["baz"] = modelStateBaz;
            return viewData;
        }
    }
}
