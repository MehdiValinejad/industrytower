using IndustryTower.DAL;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class ScoreController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Main(ScoreVars model)
        {
            return View(model);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        [AjaxRequestOnly]
        public ActionResult Update([Deserialize]ScoreVars model)
        {
            //string spName = null, 
            //       elemparm = null;
            //switch(model.type)
            //{
            //    case ScoreType.Qvote:
            //        spName = "ScoreQUpdate";
            //        elemparm = "Q";
            //        break;
            //    case ScoreType.Avote:
            //        spName = "ScoreAUpdate";
            //        elemparm = "A";
            //        break;
            //    case ScoreType.GSOvote:
            //        spName = "ScoreGSOUpdate";
            //        elemparm = "GSO";
            //        break;
            //}
            //var reader = unitOfWork.ReaderRepository.GetSPDataReader(
            //                        spName,
            //                        new SqlParameter(elemparm, model.elemId),
            //                        new SqlParameter("sig", model.sign),
            //                        new SqlParameter("granterId", WebSecurity.CurrentUserId));

            //int res = 0;
            //while (reader.Read())
            //{
            //    res = reader.GetInt32(0);
            //}

            var res = ScoreHelper.Update(model);
            return Json(new { Result = res });
        }
	}
}