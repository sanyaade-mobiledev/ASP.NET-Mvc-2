using System;
using System.Web.Routing;
using WebFormRouting;

namespace WebFormRoutingDemoWebApp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            //We are intentionally creating this backdoor as a demonstration of 
            //bad security practices.
            routes.MapWebFormRoute("Secret", "BackDoor", "~/Admin/SecretPage.aspx", false);
            routes.MapWebFormRoute("Blocked", "FrontDoor", "~/Admin/SecretPage.aspx", true);

            //Even though we are not checking physical url access in this route, it should still block because the incoming 
            //request url would start with /Admin.
            routes.MapWebFormRoute("Admin", "Admin/{*anything}", "~/Admin/SecretPage.aspx", false);

            routes.MapWebFormRoute("Named", "foo/bar", "~/forms/blech.aspx");
            routes.MapWebFormRoute("Numbers", "one/two/three", "~/forms/haha.aspx");
            
            //Maps any requests for /haha/*.aspx to /forms/hahah.aspx
            routes.MapWebFormRoute("Substitution", "haha/{filename}", "~/forms/haha.aspx");
        }
    }
    
}