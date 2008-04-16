Public Class $globalclassname$
    Inherits System.Web.HttpApplication

    Shared Sub RegisterRoutes(routes As RouteCollection)
        ' Note: Change the URL to "{controller}.mvc/{action}/{id}" to enable
        '       automatic support on IIS6 and IIS7 classic mode

        ' MapRoute takes the following parameters, in order:
        ' (1) Route name
        ' (2) URL with parameters
        ' (3) Parameter defaults
        ' (4) Parameter constraints
        routes.MapRoute( _
            "Default", _
            "{controller}/{action}/{id}", _
            New With {.controller = "Home", .action = "Index", .id = ""}, _
            New With {.controller = "[^\.]*"} _
        )

    End Sub

    Sub Application_Start()
        RegisterRoutes(RouteTable.Routes)
    End Sub
End Class
