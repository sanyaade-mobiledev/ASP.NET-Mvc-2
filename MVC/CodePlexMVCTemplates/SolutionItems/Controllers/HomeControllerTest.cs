using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $safeprojectname$.Controllers
{
    /// <summary>
    /// Summary description for HomeControllerTest
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Setup
            HomeController controller = new HomeController();

            // Execute
            RenderViewResult result = controller.Index() as RenderViewResult;

            // Verify
            IDictionary<string, object> viewData = result.ViewData as IDictionary<string, object>;
            Assert.AreEqual("Home Page", viewData["Title"]);
            Assert.AreEqual("Welcome to ASP.NET MVC!", viewData["Message"]);
        }

        [TestMethod]
        public void About()
        {
            // Setup
            HomeController controller = new HomeController();

            // Execute
            RenderViewResult result = controller.About() as RenderViewResult;

            // Verify
            IDictionary<string, object> viewData = result.ViewData as IDictionary<string, object>;
            Assert.AreEqual("About Page", viewData["Title"]);
        }
    }
}
