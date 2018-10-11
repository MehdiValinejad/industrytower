using IndustryTower.App_Start;
using IndustryTower.Controllers;
using IndustryTower.DAL;
using MvcSiteMapProvider.Web.Mvc;
using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace IndustryTower
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // QUARTZ
            Quartz.Quartz.ConfigureQuartzJobs();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Server.ClearError();
            var httpException = exception as HttpException;
            //Logging goes here
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";
            if (httpException != null)
            {
                if (httpException.GetHttpCode() == 404)
                {
                    routeData.Values["action"] = "NotFound";
                }
                Response.StatusCode = httpException.GetHttpCode();
            }
            else
            {
                Response.StatusCode = 500;
            }
            // Avoid IIS7 getting involved
            Response.TrySkipIisCustomErrors = true;
            // Execute the error controller
            IController errorsController = new ErrorController();
            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var rc = new RequestContext(wrapper, routeData);
            Response.ContentType = "text/html";
            errorsController.Execute(rc);
        }

        protected void Session_Start()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext context = HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                string ip = null;
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        ip =  addresses[0];
                    }
                    else ip = context.Request.ServerVariables["REMOTE_ADDR"];
                }
                else ip =  context.Request.ServerVariables["REMOTE_ADDR"];

                UnitOfWork uniOfWork = new UnitOfWork();
                uniOfWork.ReaderRepository.SPExecuteNonQuery("LoginUpdate",
                    new SqlParameter("U", User.Identity.Name),
                    new SqlParameter("IP", ip));
            }
        }
    }
}
