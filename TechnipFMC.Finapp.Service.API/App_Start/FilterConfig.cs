using System.Web;
using System.Web.Mvc;

namespace TechnipFMC.Finapp.Service.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
