namespace System.Web.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeHelpersTest {

        [TestMethod]
        public void TypeAllowsNullValueReturnsFalseForGenericDefinitionType() {
            Assert.IsFalse(TypeHelpers.TypeAllowsNullValue(typeof(Nullable<>)));
        }

        [TestMethod]
        public void TypeAllowsNullValueReturnsFalseForNonNullableGenericType() {
            Assert.IsFalse(TypeHelpers.TypeAllowsNullValue(typeof(KeyValuePair<int, string>)));
        }

        [TestMethod]
        public void TypeAllowsNullValueReturnsFalseForValueType() {
            Assert.IsFalse(TypeHelpers.TypeAllowsNullValue(typeof(int)));
        }

        [TestMethod]
        public void TypeAllowsNullValueReturnsTrueForNullableType() {
            Assert.IsTrue(TypeHelpers.TypeAllowsNullValue(typeof(int?)));
        }

        [TestMethod]
        public void TypeAllowsNullValueReturnsTrueForReferenceType() {
            Assert.IsTrue(TypeHelpers.TypeAllowsNullValue(typeof(object)));
        }
    }
}
