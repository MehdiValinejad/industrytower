using IndustryTower.App_Start;
using IndustryTower.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Filters
{
    public class BrowserCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            //this will be true when it's their first visit to the site (will happen again if they clear cookies)
            //if (request.UrlReferrer == null && request.Cookies["browserChecked"] == null)
            //{
                //give old IE users a warning the first time
                //SupportedBrowsers BSupport = new SupportedBrowsers(HttpContext.Current.Request.Browser);
                //if (BrowserVersionHelper.IsSupported() == false)
                //{
                //    filterContext.Controller.ViewData["RequestedUrl"] = request.Url.ToString();
                //    filterContext.Result = new ViewResult { ViewName = "~/Views/Error/OldBrowser.cshtml" };
                //}
            //    filterContext.HttpContext.Response.AppendCookie(new HttpCookie("browserChecked", "true"));
            //}

        }
    }
}