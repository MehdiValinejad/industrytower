using IndustryTower.App_Start;
using IndustryTower.Exceptions;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Filters
{
    public class HandleCustomError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //Don't bother if custom errors are turned off or if the exception is already handled
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
                return;

            //Get currently logged in user (null if anonymous)
            //int loggedInUserId = WebSecurity.CurrentUserId;

            //Log the exception and get a correlation id
            Elmah.ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);

            var httpException = filterContext.Exception as HttpException;

            //Set the view correctly depending if it's an AJAX request or not
            if (filterContext.HttpContext.Request.IsAjaxRequest())//.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errorMessage = Resource.ControllerError.ajaxError;
                if (typeof(JsonCustomException).IsInstanceOfType(filterContext.Exception))
                { 
                    errorMessage = (filterContext.Exception as JsonCustomException).Message;

                }
                else if (typeof(ModelStateException).IsInstanceOfType(filterContext.Exception))
                {
                    var dict = (filterContext.Exception as ModelStateException).Errors;
                    var entries = dict.Select(d => string.Format("{0}%%{1}", d.Key, d.Value));
                    var final = string.Join("||", entries);
                    errorMessage = final;
                }
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        errorMessage = errorMessage
                    }
                };

            }
            else
            {
                string view = "~/Views/Error/Index.cshtml";
                if (httpException != null)
                {
                    if (httpException.GetHttpCode() == 404)
                    {
                        view = "~/Views/Error/NotFound.cshtml";
                    }
                }

                filterContext.Result = new ViewResult
                {
                    ViewName = view
                };
            }
            
            //If it's not a httpException, just set the status code as 500
            if (httpException != null)
            {
                filterContext.HttpContext.Response.StatusCode = httpException.GetHttpCode();
            }
            else if (typeof(ModelStateException).IsInstanceOfType(filterContext.Exception))
            {
                filterContext.HttpContext.Response.StatusCode = 400;
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = 500;
            }

            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.ContentEncoding = Encoding.UTF8;
            filterContext.HttpContext.Response.HeaderEncoding = Encoding.UTF8;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }

    public class RedirectToNotFound : ActionResult
    {
        public RedirectToNotFound()
        { 

        }
        public override void ExecuteResult(ControllerContext context)
        {
            var req = context.HttpContext.Request;
            if (req.IsAjaxRequest())
                {
                    var lang = ITTConfig.CurrentCultureIsNotEN ? "fa" : "en";
                    new JavaScriptResult()
                    {
                        Script = "window.location = '~/" + lang + "/Error/NotFound';"
                    }.ExecuteResult(context);
                }
                else
                {
                    var lang = ITTConfig.CurrentCultureIsNotEN ? "fa" : "en";
                    new RedirectResult("~/" + lang + "/Error/NotFound").ExecuteResult(context);
                }

        }

    }

    public class RedirectToError: ActionResult
    {
        public RedirectToError()
        {

        }
        public override void ExecuteResult(ControllerContext context)
        {
            var req = context.HttpContext.Request;
            if (req.IsAjaxRequest())
            {
                var lang = ITTConfig.CurrentCultureIsNotEN ? "fa" : "en";
                new JavaScriptResult()
                {
                    Script = "window.location = '~/" + lang + "/Error/Error';"
                }.ExecuteResult(context);
            }
            else
            {
                var lang = ITTConfig.CurrentCultureIsNotEN ? "fa" : "en";
                new RedirectResult("~/" + lang + "/Error/Index").ExecuteResult(context);
            }

        }

    }
}