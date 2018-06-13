using System.Web.Mvc;
using GlobalWatchSystem.Filters;

namespace GlobalWatchSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new CustomExceptionFiliterAttribute());
        }
    }
}