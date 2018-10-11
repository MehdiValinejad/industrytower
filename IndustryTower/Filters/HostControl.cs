using System.Web.Mvc;

namespace IndustryTower.Filters
{
    public class HostControl : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        { 
            var req = context.HttpContext.Request;
            if (!(req.Url.Host == req.UrlReferrer.Host))
            {
                if (req.IsAjaxRequest())
                {
                    context.Result = new JavaScriptResult()
                    {
                        Script = "window.location = '~/Views/Error/NotFound.cshtml';"
                    };
                }
                else
                {
                    context.Result = new RedirectResult("~/Views/Error/NotFound.cshtml");
                }
            }

        }
    }
}