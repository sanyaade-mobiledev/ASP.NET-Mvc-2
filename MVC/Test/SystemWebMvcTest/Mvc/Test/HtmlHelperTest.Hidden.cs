namespace System.Web.Mvc.Test {
    using System;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class HtmlHelperTest {

        private static ViewDataDictionary GetHiddenViewData() {
            return new ViewDataDictionary { { "foo", "ViewDataFoo" } };
        }

        private static ViewDataDictionary GetHiddenViewDataWithErrors() {
            ViewDataDictionary viewData = new ViewDataDictionary { { "foo", "ViewDataFoo" } };

            ModelState modelStateFoo = new ModelState();
            modelStateFoo.Errors.Add(new ModelError("foo error 1"));
            modelStateFoo.Errors.Add(new ModelError("foo error 2"));
            viewData.ModelState["foo"] = modelStateFoo;
            modelStateFoo.AttemptedValue = "AttemptedValueFoo";

            return viewData;
        }

        [TestMethod]
        public void HiddenWithEmptyNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.Hidden(String.Empty);
                },
                "name");
        }

        [TestMethod]
        public void HiddenWithExplicitValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act
            string html = helper.Hidden("foo", "DefaultFoo");

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""hidden"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithExplicitValueAndAttributesDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act
            string html = helper.Hidden("foo", "DefaultFoo", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""hidden"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithExplicitValueAndAttributesObject() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act
            string html = helper.Hidden("foo", "DefaultFoo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""hidden"" value=""DefaultFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithExplicitValueNull() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act
            string html = helper.Hidden("foo", (string)null /* value */);

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""hidden"" value="""" />", html);
        }

        [TestMethod]
        public void HiddenWithImplicitValue() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act
            string html = helper.Hidden("foo");

            // Assert
            Assert.AreEqual(@"<input id=""foo"" name=""foo"" type=""hidden"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithImplicitValueAndAttributesDictionary() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act
            string html = helper.Hidden("foo", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""foo"" name=""foo"" type=""hidden"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithImplicitValueAndAttributesDictionaryReturnsEmptyValueIfNotFound() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act
            string html = helper.Hidden("keyNotFound", _attributesDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazValue"" id=""keyNotFound"" name=""keyNotFound"" type=""hidden"" value="""" />", html);
        }

        [TestMethod]
        public void HiddenWithImplicitValueAndAttributesObject() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act
            string html = helper.Hidden("foo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" id=""foo"" name=""foo"" type=""hidden"" value=""ViewDataFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithNullNameThrows() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewData());

            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    helper.Hidden(null /* name */);
                },
                "name");
        }

        [TestMethod]
        public void HiddenWithViewDataErrors() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewDataWithErrors());

            // Act
            string html = helper.Hidden("foo", _attributesObjectDictionary);

            // Assert
            Assert.AreEqual(@"<input baz=""BazObjValue"" class=""input-validation-error"" id=""foo"" name=""foo"" type=""hidden"" value=""AttemptedValueFoo"" />", html);
        }

        [TestMethod]
        public void HiddenWithViewDataErrorsAndCustomClass() {
            // Arrange
            HtmlHelper helper = GetHtmlHelper(GetHiddenViewDataWithErrors());

            // Act
            string html = helper.Hidden("foo", new { @class = "foo-class" });

            // Assert
            Assert.AreEqual(@"<input class=""input-validation-error foo-class"" id=""foo"" name=""foo"" type=""hidden"" value=""AttemptedValueFoo"" />", html);
        }
    }
}
