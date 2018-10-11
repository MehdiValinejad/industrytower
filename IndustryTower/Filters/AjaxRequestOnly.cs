using System.Web.Mvc;

namespace IndustryTower.Filters
{
    public class AjaxRequestOnly : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext actionContext)
        {
            //base.OnActionExecuting(actionContext);
            if (!actionContext.HttpContext.Request.IsAjaxRequest())
            {
                actionContext.Result = new RedirectResult("~/Views/Error/NotFound.cshtml");

            }

        }
    }
}