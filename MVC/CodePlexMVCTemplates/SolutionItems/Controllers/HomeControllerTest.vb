Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Web.Mvc
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class HomeControllerTest

    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = Value
        End Set
    End Property

    <TestMethod()> Public Sub Index()
        ' Setup
        Dim controller As HomeController = New HomeController()

        ' Execute
        Dim result As RenderViewResult = controller.Index()

        ' Verify
        Dim viewData As IDictionary(Of String, Object) = result.ViewData
        Assert.AreEqual("Home Page", viewData("Title"))
        Assert.AreEqual("Welcome to ASP.NET MVC!", viewData("Message"))
    End Sub

    <TestMethod()> Public Sub About()
        ' Setup
        Dim controller As HomeController = New HomeController()

        ' Execute
        Dim result As RenderViewResult = controller.About()

        ' Verify
        Dim viewData As IDictionary(Of String, Object) = result.ViewData
        Assert.AreEqual("About Page", viewData("Title"))
    End Sub
End Class
