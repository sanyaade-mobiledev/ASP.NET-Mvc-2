﻿namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TagBuilderTest {

        [TestMethod]
        public void AttributesProperty() {
            // Setup
            TagBuilder builder = new TagBuilder("SomeTag");

            // Execute
            Dictionary<string, string> attributes = builder.Attributes as Dictionary<string, string>;

            // Verify
            Assert.IsNotNull(attributes);
            Assert.AreEqual(StringComparer.OrdinalIgnoreCase, attributes.Comparer);
        }

        [TestMethod]
        public void ConstructorSetsTagNameProperty() {
            // Setup
            TagBuilder builder = new TagBuilder("SomeTag");

            // Execute
            string tagName = builder.TagName;

            // Verify
            Assert.AreEqual("SomeTag", tagName);
        }

        [TestMethod]
        public void ConstructorWithEmptyTagNameThrows() {
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new TagBuilder(String.Empty);
                }, "tagName");
        }

        [TestMethod]
        public void ConstructorWithNullTagNameThrows() {
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    new TagBuilder(null /* tagName */);
                }, "tagName");
        }

        [TestMethod]
        public void ToStringWithInnerHtmlAndNoAttributes() {
            // Setup
            TagBuilder builder = new TagBuilder("SomeTag") {
                InnerHtml = @"<br />"
            };

            // Execute
            string element = builder.ToString();

            // Verify
            Assert.AreEqual(@"<SomeTag><br /></SomeTag>", element);
        }

        [TestMethod]
        public void ToStringWithInnerHtmlAndSomeAttributes() {
            // Setup
            TagBuilder builder = new TagBuilder("SomeTag") {
                Attributes = GetAttributesDictionary(),
                InnerHtml = @"<br />"
            };

            // Execute
            string element = builder.ToString();

            // Verify
            Assert.AreEqual(@"<SomeTag a=""Foo"" b=""Bar&amp;Baz"" c=""&lt;&quot;Quux&quot;>""><br /></SomeTag>", element);
        }

        [TestMethod]
        public void ToStringWithNoInnerHtmlAndNoAttributes() {
            // Setup
            TagBuilder builder = new TagBuilder("SomeTag");
            builder.TagRenderMode = TagRenderMode.SelfClosing;

            // Execute
            string element = builder.ToString();

            // Verify
            Assert.AreEqual("<SomeTag />", element);
        }

        [TestMethod]
        public void ToStringWithNoInnerHtmlAndSomeAttributes() {
            // Setup
            TagBuilder builder = new TagBuilder("SomeTag") {
                Attributes = GetAttributesDictionary(),
                TagRenderMode = TagRenderMode.SelfClosing
            };

            // Execute
            string element = builder.ToString();

            // Verify
            Assert.AreEqual(@"<SomeTag a=""Foo"" b=""Bar&amp;Baz"" c=""&lt;&quot;Quux&quot;>"" />", element);
        }

        [TestMethod]
        public void TryAddValueReturnsFalseIfKeyAlreadyExists() {
            // Setup
            Dictionary<string, string> dictionary = new Dictionary<string, string> { { "foo", "OldValue" } };
            TagBuilder builder = new TagBuilder("SomeTag") {
                Attributes = dictionary
            };

            // Execute
            bool wasAdded = builder.TryAddValue("foo", "NewValue");

            // Verify
            Assert.IsFalse(wasAdded);
            Assert.AreEqual("OldValue", dictionary["foo"]);
        }

        [TestMethod]
        public void TryAddValueReturnsTrueIfKeyIsAdded() {
            // Setup
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            TagBuilder builder = new TagBuilder("SomeTag") {
                Attributes = dictionary
            };

            // Execute
            bool wasAdded = builder.TryAddValue("foo", "NewValue");

            // Verify
            Assert.IsTrue(wasAdded);
            Assert.AreEqual("NewValue", dictionary["foo"]);
        }

        private static IDictionary<string, string> GetAttributesDictionary() {
            return new SortedDictionary<string, string> {
                { "a", "Foo" },
                { "b", "Bar&Baz" },
                { "c", @"<""Quux"">" }
            };
        }

    }
}
