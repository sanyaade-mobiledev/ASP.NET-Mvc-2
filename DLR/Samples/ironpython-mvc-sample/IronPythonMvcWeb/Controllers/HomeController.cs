using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace IronPythonMvcWeb.Controllers {
    [HandleError]
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewData["Title"] = "Home Page";
            ViewData["Message"] = "Welcome to ASP.NET MVC using Dynamic Language Views!";

            return View("Index", "Site");
        }

        public ActionResult About() {
            ViewData["Title"] = "About Page";

            return View();
        }
    }
}
