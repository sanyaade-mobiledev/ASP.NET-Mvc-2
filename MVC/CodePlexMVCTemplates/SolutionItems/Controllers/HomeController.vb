Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        ViewData("Title") = "Home Page"
        ViewData("Message") = "Welcome to ASP.NET MVC!"

        Return RenderView()
    End Function

    Function About() As ActionResult
        ViewData("Title") = "About Page"

        Return RenderView()
    End Function
End Class
