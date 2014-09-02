using System.Web.Mvc;
using WebClient.Filters;

namespace WebClient
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NLogFilter());
        }
    }
}
