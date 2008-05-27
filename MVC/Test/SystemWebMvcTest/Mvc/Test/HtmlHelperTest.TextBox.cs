namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {

        private static readonly RouteValueDictionary _attributesDictionary = new RouteValueDictionary(new { baz = "BazValue" });
        private static readonly object _attributesObjectDictionary = new { baz = "BazObjValue" };

        private static ViewDataDictionary GetTextBoxViewData() {
            ViewDataDictionary viewData = new ViewDataDictionary { { "foo", "ViewDataFoo" } };
            viewData.Model = new { foo = "ViewItemFoo", bar = "ViewItemBar" };
            return viewData;
        }

        [TestMethod]
        public void TextBoxWithEmptyNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.TextBox(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void TextBoxWithExplicitValue() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute
            string html = helper.TextBox("foo", "DefaultFoo");

            // Verify
            Assert.AreEqual(@"<input type=""text"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithExplicitValueAndAttributesDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute
            string html = helper.TextBox("foo", "DefaultFoo", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""text"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithExplicitValueAndAttributesObject() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute
            string html = helper.TextBox("foo", "DefaultFoo", _attributesObjectDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazObjValue"" type=""text"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithExplicitValueNull() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute
            string html = helper.TextBox("foo", (string)null /* value */);

            // Verify
            Assert.AreEqual(@"<input type=""text"" name=""foo"" id=""foo"" value="""" />", html);
        }

        [TestMethod]
        public void TextBoxWithImplicitValue() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute
            string html = helper.TextBox("foo");

            // Verify
            Assert.AreEqual(@"<input type=""text"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithImplicitValueAndAttributesDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute
            string html = helper.TextBox("foo", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""text"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithImplicitValueAndAttributesDictionaryReturnsEmptyValueIfNotFound() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute
            string html = helper.TextBox("keyNotFound", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""text"" name=""keyNotFound"" id=""keyNotFound"" value="""" />", html);
        }

        [TestMethod]
        public void TextBoxWithImplicitValueAndAttributesObject() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute
            string html = helper.TextBox("foo", _attributesObjectDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazObjValue"" type=""text"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void TextBoxWithNullNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetTextBoxViewData());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.TextBox(null /* name */);
                },
                "name");
        }
    }
}
