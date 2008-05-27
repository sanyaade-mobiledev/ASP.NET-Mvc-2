namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {

        private static ViewDataDictionary GetPasswordViewData() {
            return new ViewDataDictionary { { "foo", "ViewDataFoo" } };
        }

        [TestMethod]
        public void PasswordWithEmptyNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.Password(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void PasswordWithExplicitValue() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute
            string html = helper.Password("foo", "DefaultFoo");

            // Verify
            Assert.AreEqual(@"<input type=""password"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithExplicitValueAndAttributesDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute
            string html = helper.Password("foo", "DefaultFoo", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""password"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithExplicitValueAndAttributesObject() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute
            string html = helper.Password("foo", "DefaultFoo", _attributesObjectDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazObjValue"" type=""password"" name=""foo"" id=""foo"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithExplicitValueNull() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute
            string html = helper.Password("foo", (string)null /* value */);

            // Verify
            Assert.AreEqual(@"<input type=""password"" name=""foo"" id=""foo"" value="""" />", html);
        }

        [TestMethod]
        public void PasswordWithImplicitValue() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute
            string html = helper.Password("foo");

            // Verify
            Assert.AreEqual(@"<input type=""password"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithImplicitValueAndAttributesDictionary() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute
            string html = helper.Password("foo", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""password"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithImplicitValueAndAttributesDictionaryReturnsEmptyValueIfNotFound() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute
            string html = helper.Password("keyNotFound", _attributesDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazValue"" type=""password"" name=""keyNotFound"" id=""keyNotFound"" value="""" />", html);
        }

        [TestMethod]
        public void PasswordWithImplicitValueAndAttributesObject() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute
            string html = helper.Password("foo", _attributesObjectDictionary);

            // Verify
            Assert.AreEqual(@"<input baz=""BazObjValue"" type=""password"" name=""foo"" id=""foo"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void PasswordWithNullNameThrows() {
            // Setup
            HtmlHelper helper = GetHtmlHelper(GetPasswordViewData());

            // Execute & verify
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.Password(null /* name */);
                },
                "name");
        }
    }
}
