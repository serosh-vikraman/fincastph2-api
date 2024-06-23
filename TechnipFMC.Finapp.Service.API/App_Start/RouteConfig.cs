using System.Web.Mvc;
using System.Web.Routing;

namespace TechnipFMC.Finapp.Service.API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Values", action = "Get", id = UrlParameter.Optional }
            );
        }
    }
}
