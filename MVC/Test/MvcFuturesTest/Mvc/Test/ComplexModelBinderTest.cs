namespace MvcFuturesTest.Mvc.Test {
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.Mvc;
    using Moq;

    [TestClass]
    public class ComplexModelBinderTest {

        private static readonly IModelBinder _defaultConverter = new NestedComplexModelBinder();

        [TestMethod]
        public void GetValuePopulatesNestedComplexTypes() {
            // Arrange
            Dictionary<string, object> values = new Dictionary<string,object>(StringComparer.OrdinalIgnoreCase) {
                { "employee.firstname", "John" },
                { "employee.lastname", "Doe" },
                { "employee.employeeid", "123456" },
                { "employee.salary", "45000.00" },
                { "employee.homeaddress.street", "123 Nowhere Lane" },
                { "employee.homeaddress.city", "Nowhere" },
                { "employee.homeaddress.state", "AK" },
                { "employee.homeaddress.zip", "99999" },
                { "employee.workaddress.street", "1 Microsoft Way" },
                { "employee.workaddress.city", "Redmond" },
                { "employee.workaddress.state", "WA" },
                { "employee.workaddress.zip", "98052" }
            };
            ControllerContext controllerContext = GetControllerContext(values);

            // Act
            object o = _defaultConverter.GetValue(controllerContext, "employee", typeof(Employee), null);
            Employee e = o as Employee;

            // Assert
            Assert.IsNotNull(e, "GetValue() should have returned an employee.");
            Assert.AreEqual("John", e.FirstName);
            Assert.AreEqual("Doe", e.LastName);
            Assert.AreEqual(123456, e.EmployeeId);
            Assert.AreEqual(45000m, e.Salary);

            Address homeAddress = e.HomeAddress;
            Assert.IsNotNull(homeAddress, "Home address should not be null.");
            Assert.AreEqual("123 Nowhere Lane", homeAddress.Street);
            Assert.AreEqual("Nowhere", homeAddress.City);
            Assert.AreEqual("AK", homeAddress.State);
            Assert.AreEqual(99999, homeAddress.Zip);

            Address workAddress = e.WorkAddress;
            Assert.IsNotNull(homeAddress, "Work address should not be null.");
            Assert.AreEqual("1 Microsoft Way", workAddress.Street);
            Assert.AreEqual("Redmond", workAddress.City);
            Assert.AreEqual("WA", workAddress.State);
            Assert.AreEqual(98052, workAddress.Zip);
        }

        [TestMethod]
        public void GetValueThrowsIfControllerContextIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    _defaultConverter.GetValue(null, "some name", typeof(object), null);
                }, "controllerContext");
        }

        [TestMethod]
        public void GetValueThrowsIfModelNameIsEmpty() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    _defaultConverter.GetValue(GetControllerContext(), String.Empty, typeof(object), null);
                }, "modelName");
        }

        [TestMethod]
        public void GetValueThrowsIfModelNameIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentExceptionNullOrEmpty(
                delegate {
                    _defaultConverter.GetValue(GetControllerContext(), null, typeof(object), null);
                }, "modelName");
        }

        [TestMethod]
        public void GetValueThrowsIfModelTypeIsNull() {
            // Act & Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    _defaultConverter.GetValue(GetControllerContext(), "some name", null, null);
                }, "modelType");
        }

        [TestMethod]
        public void GetValueWithDateTime() {
            // Arrange
            DateTime originalDateTime = DateTime.Now;
            IDictionary<string, object> values = GetValuesDict(originalDateTime); // integer is primitive type
            ControllerContext controllerContext = GetControllerContext(values);

            // Act
            object o = _defaultConverter.GetValue(controllerContext, "foo", typeof(DateTime), null);

            // Assert
            Assert.AreEqual(originalDateTime, o);
        }

        [TestMethod]
        public void GetValueWithDecimal() {
            // Arrange
            IDictionary<string, object> values = GetValuesDict(5m);
            ControllerContext controllerContext = GetControllerContext(values);

            // Act
            object o = _defaultConverter.GetValue(controllerContext, "foo", typeof(decimal), null);

            // Assert
            Assert.AreEqual(5m, o);
        }

        [TestMethod]
        public void GetValueWithEnumType() {
            // Arrange
            IDictionary<string, object> values = GetValuesDict(MyEnum.Delta);
            ControllerContext controllerContext = GetControllerContext(values);

            // Act
            object o = _defaultConverter.GetValue(controllerContext, "foo", typeof(MyEnum), null);

            // Assert
            Assert.AreEqual(MyEnum.Delta, o);
        }

        [TestMethod]
        public void GetValueWithGuid() {
            // Arrange
            Guid originalGuid = Guid.NewGuid();
            IDictionary<string, object> values = GetValuesDict(originalGuid);
            ControllerContext controllerContext = GetControllerContext(values);

            // Act
            object o = _defaultConverter.GetValue(controllerContext, "foo", typeof(Guid), null);

            // Assert
            Assert.AreEqual(originalGuid, o);
        }

        [TestMethod]
        public void GetValueWithPrimitiveType() {
            // Arrange
            IDictionary<string, object> values = GetValuesDict(5); // integer is primitive type
            ControllerContext controllerContext = GetControllerContext(values);

            // Act
            object o = _defaultConverter.GetValue(controllerContext, "foo", typeof(int), null);

            // Assert
            Assert.AreEqual(5, o);
        }

        [TestMethod]
        public void GetValueWithString() {
            // Arrange
            IDictionary<string, object> values = GetValuesDict("some string");
            ControllerContext controllerContext = GetControllerContext(values);

            // Act
            object o = _defaultConverter.GetValue(controllerContext, "foo", typeof(string), null);

            // Assert
            Assert.AreEqual("some string", o);
        }

        private static ControllerContext GetControllerContext() {
            return GetControllerContext(null);
        }

        private static ControllerContext GetControllerContext(IDictionary<string, object> values) {
            RouteData routeData = new RouteData();
            if (values != null) {
                foreach (var entry in values) {
                    routeData.Values[entry.Key] = entry.Value;
                }
            }
            return new ControllerContext(new Mock<HttpContextBase>().Object, routeData, new Mock<ControllerBase>().Object);
        }

        private static IDictionary<string, object> GetValuesDict(object o) {
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) {
                { "foo", o }
            };
        }

        private class Employee {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Address HomeAddress { get; set; }
            public Address WorkAddress { get; set; }

            public int EmployeeId { get; set; }
            public decimal Salary { get; set; }
        }

        private class Address {
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public int Zip { get; set; }
        }

        private enum MyEnum {
            Alpha,
            Bravo,
            Charlie,
            Delta,
            Echo,
            Foxtrot
        }

        private class NestedComplexModelBinder : ComplexModelBinder {
            protected override IModelBinder GetBinder(Type parameterType) {
                return this;
            }
        }

    }
}
