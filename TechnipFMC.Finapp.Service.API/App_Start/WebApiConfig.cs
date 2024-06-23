using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;
using System.Configuration;

namespace TechnipFMC.Finapp.Service.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //string OrginURL_prod = ConfigurationManager.AppSettings["URL_Production"].ToString();
            //var cors = new EnableCorsAttribute(OrginURL_prod, "*", "*");
            //config.EnableCors(cors);

            // Web API configuration and services    
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new TokenValidationHandler());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

          

            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
        }
    }
}
