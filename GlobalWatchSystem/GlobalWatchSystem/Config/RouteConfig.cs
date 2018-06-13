using System.Web.Mvc;
using System.Web.Routing;

namespace GlobalWatchSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("DeviceDataExport", "devices/{deviceImei}/export/{from}/{to}/", new {controller = "Device", action="Export"});
            routes.MapRoute("Login", "login", new {controller = "Login", action = "Login"});
            routes.MapRoute("Logout", "logout", new {controller = "Login", action = "Logout"});
            routes.MapRoute("Default", "{controller}/{action}/{id}", new {controller = "Home", action = "Index", id = UrlParameter.Optional});
        }
    }
}