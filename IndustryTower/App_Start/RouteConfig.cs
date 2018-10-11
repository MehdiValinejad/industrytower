using IndustryTower.App_Start;
using IndustryTower.Helpers;
using IndustryTower.Hubs;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IndustryTower
{
   
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("office/{*pathInfo}");
           // routes.MapRoute(
           //    name: "Search",
           //    url: "TotalSearch/{searchType}",
           //    defaults: new { controller = "Home", action = "TotalSearch", searchType = "ALL", }
           //);

            routes.MapRoute(
                name: "User",
                url: "UserProfile/{UName}/{UId}",
                defaults: new { controller = "UserProfile", action = "UProfile" }
            );

            routes.MapRoute(
                name: "UserInfo",
                url: "UserInfo/{RType}/{UName}/{UId}",
                defaults: new { controller = "UserProfile", action = "UserInfo" }
            );

            routes.MapRoute(
                name: "Badge",
                url: "Badge/{BgName}/{BgId}",
                defaults: new { controller = "Badge", action = "Detail" }
            );

            routes.MapRoute(
                name: "Company",
                url: "Company/{CoName}/{CoId}",
                defaults: new { controller = "Company", action = "CProfile" }
            );

            routes.MapRoute(
                name: "Store",
                url: "Store/{StName}/{StId}",
                defaults: new { controller = "Store", action = "SProfile" }
            );

            routes.MapRoute(
                name: "Product",
                url: "Product/{PrName}/{PrId}",
                defaults: new { controller = "Product", action = "Detail" }
            );

            routes.MapRoute(
                name: "Service",
                url: "Service/{SrName}/{SrId}",
                defaults: new { controller = "Service", action = "Detail" }
            );

            routes.MapRoute(
                name: "Question",
                url: "Question/{QName}/{QId}",
                defaults: new { controller = "Question", action = "Detail" }
            );

            routes.MapRoute(
                name: "Event",
                url: "Event/{EvName}/{EvId}",
                defaults: new { controller = "Event", action = "Detail" }
            );

            //routes.MapRoute(
            //    name: "Job",
            //    url: "Job/{JName}/{JId}",
            //    defaults: new { controller = "Job", action = "Detail" }
            //);

            //routes.MapRoute(
            //    name: "Project",
            //    url: "Project/{PjName}/{PjId}",
            //    defaults: new { controller = "Project", action = "Detail" }
            //);

            routes.MapRoute(
                name: "Patent",
                url: "Patent/{PtName}/{patId}",
                defaults: new { controller = "Patent", action = "Detail" }
            );

            routes.MapRoute(
                name: "Group",
                url: "Group/{GName}/{GId}",
                defaults: new { controller = "Group", action = "GroupPage" }
            );

            routes.MapRoute(
                name: "GroupSession",
                url: "GroupSession/{GSName}/{SsId}",
                defaults: new { controller = "GroupSession", action = "Detail" }
            );

            routes.MapRoute(
                name: "Seminar",
                url: "Seminar/{SnName}/{SnId}",
                defaults: new { controller = "Seminar", action = "Detail" }
            );

            routes.MapRoute(
                name: "Dictionary",
                url: "Dict/{DName}/{DId}",
                defaults: new { controller = "Dict", action = "Dictionary" }
            );

            routes.MapRoute(
                name: "Word",
                url: "Word/{WName}/{WId}",
                defaults: new { controller = "Word", action = "Detail" }
            );

            routes.MapRoute(
                name: "Book",
                url: "Book/{BName}/{BId}",
                defaults: new { controller = "Book", action = "Detail" }
            );

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

            // Add our route registration for MvcSiteMapProvider sitemaps
            MvcSiteMapProvider.Web.Mvc.XmlSiteMapController.RegisterRoutes(routes);


            foreach (Route r in routes)
            {
                if (!(r.RouteHandler is CultureHelper.SingleCultureMvcRouteHandler))
                {
                    if (!r.Url.StartsWith("api"))
                    {
                        r.RouteHandler = new CultureHelper.MultiCultureMvcRouteHandler();
                        r.Url = "{culture}/" + r.Url;
                        //Adding default culture 
                        if (r.Defaults == null)
                        {
                            r.Defaults = new RouteValueDictionary();
                        }
                        r.Defaults.Add("culture", "fa");


                        //Adding constraint for culture param
                        if (r.Constraints == null)
                        {
                            r.Constraints = new RouteValueDictionary();
                        }
                        //r.Constraints.Add("culture", new CultureHelper.CultureConstraint(CultureHelper.Culture.en.ToString(), CultureHelper.Culture.fa.ToString()));
                        r.Constraints.Add("culture", "en|fa");
                    }
                }
            }

            //routes.MapRoute("NotFound", "{*url}",
            //    new { controller = "Error", action = "NotFound", culture = "fa" }
            //    ).RouteHandler = new CultureHelper.MultiCultureMvcRouteHandler();


        }
    }
}