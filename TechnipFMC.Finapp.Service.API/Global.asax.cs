using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.UI;
using TechnipFMC.Finapp.Service.API.App_Start;

namespace TechnipFMC.Finapp.Service.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapperConfig.Initialize();
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            var cors = new System.Web.Http.Cors.EnableCorsAttribute("https://fincast.app", "*", "*");
            GlobalConfiguration.Configuration.EnableCors(cors);

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            string OrginURL_prod = ConfigurationManager.AppSettings["URL_Production"].ToString();

            if (Context.Request.Path.Contains("api/") && (Context.Request.HttpMethod == "POST" || Context.Request.HttpMethod == "GET" || Context.Request.HttpMethod == "OPTIONS"))
            {
                Context.Response.AppendHeader("Access-Control-Allow-Origin", OrginURL_prod);
                Context.Response.AppendHeader("Access-Control-Allow-Headers", "Authorization,Origin, X-Requested-With, Content-Type, Accept"); 

                Context.Response.AppendHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                Context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
                if (Context.Request.HttpMethod == "OPTIONS")
                {
                    Context.Response.End();
                }
            }
            else if (Context.Request.Path.EndsWith("api/") || Context.Request.Path.EndsWith("api"))
            {
                Context.Response.Write("Application : RDocs Api");
                Context.Response.Write(" :) ");
                Context.Response.End();
            }


        }
        
        void Session_Start(object sender, EventArgs e)
        
        {
            try
            {
                HttpContext.Current.Session["dummy"] = "dummy";
                Response.Redirect("~/Index.html");
            }
            catch (Exception)
            {

            }
        }
        void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Index.html");
            }
            catch (Exception)
            {

            }
        }
    }
}
