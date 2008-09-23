namespace DlrMvcLib {
    using System.Web.Mvc;

    public class DlrViewEngine : WebFormViewEngine {
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath) {
            return new DlrView(partialPath, null);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath) {
            return new DlrView(viewPath, masterPath);
        }
    }
}
