namespace System.Web.Mvc.Ajax.Test {
    using System.Web.Mvc;
    using System.Web.TestUtil;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AjaxHelperTest {
        [TestMethod]
        public void ConstructorWithNullViewContextThrows() {
            // Assert
            ExceptionHelper.ExpectArgumentNullException(
                delegate {
                    AjaxHelper ajaxHelper = new AjaxHelper(null);
                },
                "viewContext");
        }
    }
}
