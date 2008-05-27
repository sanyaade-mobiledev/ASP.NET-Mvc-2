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
            routes.Map("").To("~/Default.aspx");
            
            //We are intentionally creating this backdoor as a demonstration of 
            //bad security practices.
            routes.Map("Secret", "BackDoor").To("~/Admin/SecretPage.aspx", false);
            routes.Map("Blocked", "FrontDoor").To("~/Admin/SecretPage.aspx");

            //Even though we are not checking physical url access in this route, it should still block because the incoming 
            //request url would start with /Admin.
            routes.Map("Admin", "Admin/{*anything}").To("~/Admin/SecretPage.aspx", false);

            routes.Map("Forms", "forms/{whatever}").To("~/forms/{whatever}.aspx");
            routes.Map("Named", "foo/bar").To("~/forms/blech.aspx");
            routes.Map("Numbers", "one/two/three").To("~/forms/haha.aspx");
            
            //Maps any requests for /haha/*.aspx to /forms/hahah.aspx
            routes.Map("General", "haha/{filename}.aspx").To("~/forms/haha.aspx");
        }
    }
    
}