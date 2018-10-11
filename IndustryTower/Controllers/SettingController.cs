using IndustryTower.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class SettingController : Controller
    {
        //
        // GET: /Setting/

        public ActionResult Index()
        {
            return View();
        }

    }
}
