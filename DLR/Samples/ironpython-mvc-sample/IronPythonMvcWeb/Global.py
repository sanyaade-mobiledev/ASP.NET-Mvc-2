import clr
clr.AddReference("DlrMvcLib")
import DlrMvcLib

def Application_Start():
    DlrMvcLib.Routes.AddDefaultNamespace("IronPythonMvcWeb");
    DlrMvcLib.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
    DlrMvcLib.Routes.MapRoute("default", "{controller}/{action}/{id}", {'controller':'home', 'action':'index', 'id':''})
    DlrMvcLib.RouteInitializer.InitializeViewEngines()
    pass

def Application_End():
    ' Code that runs on application shutdown'
    pass

def Application_Error(app, e): 
    ' Code that runs when an unhandled error occurs'
    pass

def Application_BeginRequest(app, e):
    ' Code that runs at the beginning of each request'
    pass

def Application_EndRequest(app, e):
    ' Code that runs at the end of each request'
    pass 
