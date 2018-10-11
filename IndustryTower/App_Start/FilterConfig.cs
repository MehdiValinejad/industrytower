using IndustryTower.Filters;
using System.Web.Mvc;

namespace IndustryTower
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleCustomError());
            filters.Add(new InitializeSimpleMembershipAttribute());
            //filters.Add(new HandleErrorAttribute());
        }
    }
}