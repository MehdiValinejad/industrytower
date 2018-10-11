using IndustryTower.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IndustryTower.Filters
{
    public class ITTAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Request.RequestContext.RouteData.Values["culture"] = ITTConfig.CurrentCultureIsNotEN ? "fa":"en" ;
            RouteValueDictionary route = new RouteValueDictionary();
            route.Add("controller","Account");
            route.Add("action", "Login");
            route.Add("culture", ITTConfig.CurrentCultureIsNotEN ? "fa" : "en");
            route.Add("returnUrl", filterContext.HttpContext.Request.Url.PathAndQuery);
            filterContext.Result = new RedirectToRouteResult(route);
        }
    }
}