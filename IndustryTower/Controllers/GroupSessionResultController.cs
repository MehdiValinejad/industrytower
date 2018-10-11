using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using Resource;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorize]
    public class GroupSessionResultController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult Create(string SsId)
        {
            NullChecker.NullCheck(new object[] { SsId });
            var ssid = EncryptionHelper.Unprotect(SsId);
            if (!AuthorizationHelper.isRelevant(WebSecurity.CurrentUserId, "UserPerGS", new SqlParameter("GS", ssid), new SqlParameter("U", WebSecurity.CurrentUserId)))
            {
                return new RedirectToError();
            }
            var reader = unitOfWork.ReaderRepository.GetSPDataReader("GSResult", new SqlParameter("GS", ssid));

            if (reader.HasRows) return new RedirectToError();

            ViewData["SsId"] = SsId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SessionResult")] GroupSesssionResult res, string S)
        {
            NullChecker.NullCheck(new object[] { S });
            var ssid = EncryptionHelper.Unprotect(S);
            if (!AuthorizationHelper.isRelevant(WebSecurity.CurrentUserId, "UserPerGS", new SqlParameter("GS", ssid), new SqlParameter("U", WebSecurity.CurrentUserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            if (ModelState.IsValid)
            {
                res.sessionId = (int)ssid;
                res.creationDate = DateTime.UtcNow;
                unitOfWork.GroupSessionResultRepository.Insert(res);
                unitOfWork.Save();
                return RedirectToAction("Detail", "GroupSession", new { SsId = S });
            }
            throw new ModelStateException(this.ModelState);
        }

        public ActionResult Edit(string SsId)
        {
            NullChecker.NullCheck(new object[] { SsId });
            var ssid = EncryptionHelper.Unprotect(SsId);
            if (!AuthorizationHelper.isRelevant(WebSecurity.CurrentUserId, "UserPerGS", new SqlParameter("GS", ssid), new SqlParameter("U", WebSecurity.CurrentUserId)))
            {
                return new RedirectToError();
            }
            var reader = unitOfWork.ReaderRepository.GetSPDataReader("GSResult", new SqlParameter("GS", ssid));
            GroupSesssionResult gsr = new GroupSesssionResult();
            while (reader.Read())
            {
                gsr.sessionId = reader.GetInt32(0);
                gsr.SessionResult = reader[1] as string;
                gsr.creationDate = reader.GetDateTime(2);
            }
            ViewData["SsId"] = SsId;
            return View(gsr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string S,string ff)
        {
            NullChecker.NullCheck(new object[] { S });
            var ssid = EncryptionHelper.Unprotect(S);
            if (!AuthorizationHelper.isRelevant(WebSecurity.CurrentUserId, "UserPerGS", new SqlParameter("GS", ssid), new SqlParameter("U", WebSecurity.CurrentUserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            var res = unitOfWork.GroupSessionResultRepository.GetByID(ssid);
            if (TryUpdateModel(res, "", new string[] { "SessionResult" }))
            {
                unitOfWork.GroupSessionResultRepository.Update(res);
                unitOfWork.Save();
                return RedirectToAction("Detail", "GroupSession", new { SsId = S });
            }
            throw new ModelStateException(this.ModelState);
        }
    }
}