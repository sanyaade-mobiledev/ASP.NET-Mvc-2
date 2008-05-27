namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {

        private static ViewDataDictionary GetHiddenViewData() {
            return new ViewDataDictionary { { "foo", "ViewDataFoo" } };
        }

        [TestMethod]
        public void HiddenWithEmptyNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.Hidden(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void HiddenWithExplicitValue() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute
            string html = helper.Hidden("foo", "DefaultFoo");

            // Verify
            Assert.AreEqual(@"<input type=""hidden"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithExplicitValueAndAttributesDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute
            string html = helper.Hidden("foo", "DefaultFoo", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""hidden"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithExplicitValueAndAttributesObject() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute
            string html = helper.Hidden("foo", "DefaultFoo", _attributesObjectDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazObjValue"" type=""hidden"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithExplicitValueNull() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute
            string html = helper.Hidden("foo", (string)null /* value */);

            // Verify
            Assert.AreEqual(@"<input type=""hidden"" name=""foo"" id=""foo"" value="""" />", html);
        }

        [TestMethod]
        public void HiddenWithImplicitValue() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute
            string html = helper.Hidden("foo");

            // Verify
            Assert.AreEqual(@"<input type=""hidden"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithImplicitValueAndAttributesDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute
            string html = helper.Hidden("foo", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""hidden"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithImplicitValueAndAttributesDictionaryReturnsEmptyValueIfNotFound() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute
            string html = helper.Hidden("keyNotFound", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""hidden"" name=""keyNotFound"" id=""keyNotFound"" value="""" />", html);
        }

        [TestMethod]
        public void HiddenWithImplicitValueAndAttributesObject() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute
            string html = helper.Hidden("foo", _attributesObjectDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazObjValue"" type=""hidden"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithNullNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.Hidden(null /* name */);
                },
                "name");
        }
    }
}
