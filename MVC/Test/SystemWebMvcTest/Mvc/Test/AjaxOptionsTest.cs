namespace System.Web.Mvc.Test {
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AjaxOptionsTest {

        [TestMethod]
        public void InsertionModeProperty() {
            // Setup
            AjaxOptions options = new AjaxOptions();

            // Execute & verify
            MemberHelper.TestEnumProperty(options, "InsertionMode", InsertionMode.Replace, false);
        }

        [TestMethod]
        public void InsertionModePropertyExceptionText() {
            // Setup
            AjaxOptions options = new AjaxOptions();

            // Execute & verify
            ExceptionHelper.ExpectArgumentOutOfRangeException(
                delegate {
                    options.InsertionMode = (InsertionMode)4;
                },
                "value",
                "The value '4' is outside the valid range of the enumeration type 'System.Web.Mvc.InsertionMode'.\r\nParameter name: value");
        }

        [TestMethod]
        public void OnBeginProperty() {
            // Setup
            AjaxOptions options = new AjaxOptions();

            // Execute & verify
            MemberHelper.TestStringProperty(options, "OnBegin", String.Empty, false, true);
        }

        [TestMethod]
        public void OnFailureProperty() {
            // Setup
            AjaxOptions options = new AjaxOptions();

            // Execute & verify
            MemberHelper.TestStringProperty(options, "OnFailure", String.Empty, false, true);
        }

        [TestMethod]
        public void OnSuccessProperty() {
            // Setup
            AjaxOptions options = new AjaxOptions();

            // Execute & verify
            MemberHelper.TestStringProperty(options, "OnSuccess", String.Empty, false, true);
        }

        [TestMethod]
        public void ToJavascriptString() {
            // Setup
            AjaxOptions options = new AjaxOptions { 
                InsertionMode = InsertionMode.InsertBefore,
                OnBegin = "some_begin_code();",
                OnFailure = "some_failure_code();",
                OnSuccess = "some_success_code();",
                UpdateTargetId = "someId" 
            };

            // Execute
            string s = options.ToJavascriptString();

            // Verify
            Assert.AreEqual("{ insertionMode: 1, updateTargetId: 'someId', onBegin: function(request) { some_begin_code(); }, "
                + "onFailure: function(request) { some_failure_code(); }, onSuccess: function(request) { some_success_code(); } }", s);
        }

        [TestMethod]
        public void ToDictionaryWithOnlyUpdateTargetId() {
            // Setup
            AjaxOptions options = new AjaxOptions { UpdateTargetId = "someId" };

            // Execute
            string s = options.ToJavascriptString();

            // Verify
            Assert.AreEqual("{ insertionMode: 0, updateTargetId: 'someId' }", s);
        }

        [TestMethod]
        public void ToDictionaryWithUpdateTargetIdAndExplicitInsertionMode() {
            // Setup
            AjaxOptions options = new AjaxOptions { InsertionMode = InsertionMode.InsertAfter, UpdateTargetId = "someId" };

            // Execute
            string s = options.ToJavascriptString();

            // Verify
            Assert.AreEqual("{ insertionMode: 2, updateTargetId: 'someId' }", s);
        }

        [TestMethod]
        public void UpdateTargetIdProperty() {
            // Setup
            AjaxOptions options = new AjaxOptions();

            // Execute & verify
            MemberHelper.TestStringProperty(options, "UpdateTargetId", String.Empty, false, true);
        }

    }
}
