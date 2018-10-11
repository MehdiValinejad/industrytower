using IndustryTower.Filters;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult OldBrowser()
        {
            return View();
        }

	}
}