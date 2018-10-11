using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Helpers;
using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Resource;
using IndustryTower.Filters;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Data.SqlClient;
using WebMatrix.WebData;
using IndustryTower.ViewModels;
using System.Data;

namespace IndustryTower.Controllers
{

    public enum SessionType 
    {
        Current = 1,
        Fisnished = 2,
        NoResult =3
    }

    [ITTAuthorizeAttribute]
    public class GroupSessionController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        [AllowAnonymous]
        public ActionResult PartialList(int GId, SessionType sessionType, int? page)
        {
            if (!AuthorizationHelper.isRelevant(WebSecurity.CurrentUserId, "UserPerGSView", new SqlParameter("GID", GId), new SqlParameter("U", WebSecurity.CurrentUserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }


            IList<GroupSession> groupSessions = new List<GroupSession>();
            int TotalRows = 0;
            int pageSize = 12;
            int pageNumber = (page ?? 1); 


            List<SqlParameter> prams = new List<SqlParameter>();
            var totalRows = new SqlParameter("TotalRows", SqlDbType.Int, 50);
            totalRows.Direction = ParameterDirection.Output;
            prams.Add(totalRows);
            prams.Add(new SqlParameter("GID", GId));
            prams.Add(new SqlParameter("pageSize", pageSize));
            prams.Add(new SqlParameter("pagNum", pageNumber));

            SqlCommand outputCommand;
            
            switch (sessionType)
            {
                case SessionType.Current:
                    prams.Add(new SqlParameter("TYP", 1));
                    var curReader = unitOfWork.ReaderRepository.GetSPDataReader("GSList", prams, out outputCommand);
                    while (curReader.Read())
                    {
                        groupSessions.Add(new GroupSession
                        {
                            sessionId = curReader.GetInt32(1),
                            sessionSubject = curReader[2] as string,
                            sessionDesc = curReader[3] as string,
                            startDate = curReader.GetDateTime(4),
                            endDate = curReader[5] as DateTime?
                        });
                    }
                    curReader.Close();
                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;
                    //groupSessions = unitOfWork.GroupSessionRepository.Get(s => s.groupId == gid && !s.Offers.Any())
                    //                                        .OrderByDescending(dt => dt.startDate);
                    break;
                case SessionType.Fisnished:
                    prams.Add(new SqlParameter("TYP", 2));
                    var finReader = unitOfWork.ReaderRepository.GetSPDataReader("GSList", prams, out outputCommand);
                    while (finReader.Read())
                    {
                        groupSessions.Add(new GroupSession
                        {
                            sessionId = finReader.GetInt32(1),
                            sessionSubject = finReader[2] as string,
                            sessionDesc = finReader[3] as string,
                            startDate = finReader.GetDateTime(4),
                            endDate = finReader[5] as DateTime?
                        });
                    }
                    finReader.Close();
                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;
                    //groupSessions = unitOfWork.GroupSessionRepository.Get(s => s.groupId == gid && s.Offers.Any())
                    //                                        .OrderByDescending(dt => dt.startDate);
                    break;
                case SessionType.NoResult:
                    prams.Add(new SqlParameter("TYP", 3));
                    var noReader = unitOfWork.ReaderRepository.GetSPDataReader("GSList", prams, out outputCommand);
                    while (noReader.Read())
                    {
                        groupSessions.Add(new GroupSession
                        {
                            sessionId = noReader.GetInt32(1),
                            sessionSubject = noReader[2] as string,
                            sessionDesc = noReader[3] as string,
                            startDate = noReader.GetDateTime(4),
                            endDate = noReader[5] as DateTime?
                        });
                    }
                    noReader.Close();
                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;
                    //groupSessions = unitOfWork.GroupSessionRepository.Get(s => s.groupId == gid && s.Offers.Any(o => !o.isAccepted))
                    //                                        .OrderByDescending(dt => dt.startDate);
                    break;
            }

            ViewData["sessionType"] = sessionType;
            ViewData["GId"] = GId;

            ViewData["finalPage"] = TotalRows < pageSize * pageNumber;
            ViewData["pageNum"] = pageNumber;

            return PartialView(groupSessions);

            //if (page != null)
            //{
            //    return PartialView(groupSessions.ToPagedList(pageNumber, pageSize));
            //}
            //else return PartialView(groupSessions.ToPagedList(1, pageSize));
        }

        [AllowAnonymous]
        public ActionResult Detail(int SsId)
        {
            //var session = unitOfWork.GroupSessionRepository.GetByID(EncryptionHelper.Unprotect(SsId));
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "GSDetail",
                new SqlParameter("U", WebSecurity.CurrentUserId),
                new SqlParameter("GS", SsId));
            GSDetailViewModel sess = new GSDetailViewModel();
            while(reader.Read())
            {
                sess = new GSDetailViewModel
                {
                    groupid = reader.GetInt32(0),
                    groupName = reader[1] as string,
                    sessionid = reader.GetInt32(2),
                    sessionSubject = reader[3] as string,
                    sessionDesc = reader[4] as string,
                    image = reader[5] as string,
                    doc = reader[6] as string,
                    startDate = reader.GetDateTime(7),
                    endDate = reader[8] as DateTime?,
                    isAdmin = reader.GetBoolean(9),
                    isMember = reader.GetBoolean(10),
                    Offers = reader.GetInt32(11),
                    AcceptedOffers = reader.GetInt32(12)
                };
            }
            reader.NextResult();
            while (reader.Read())
            {
                ViewData["res"] = reader[0] as string;
            }

            return View(sess);
        }


        public ActionResult Create(string GId)
        {
            NullChecker.NullCheck(new object[] { GId });
            ViewData["GId"] = GId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        public ActionResult Create([Bind(Include = "sessionSubject,sessionDesc,endDate")] GroupSession groupSessionToCreate, string GId, string filesToUpload)
        {
            NullChecker.NullCheck(new object[] { GId });
            if (ModelState.IsValid)
            {
                var gid = EncryptionHelper.Unprotect(GId);
                var group = unitOfWork.GroupRepository.GetByID(gid);
                if (group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
                {
                    

                    var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, null, "GSession");
                    groupSessionToCreate.image = fileUploadResult.ImagesToUpload;
                    groupSessionToCreate.document = fileUploadResult.DocsToUpload;

                    groupSessionToCreate.groupId = (int)gid;
                    groupSessionToCreate.startDate = DateTime.UtcNow;
                    unitOfWork.GroupSessionRepository.Insert(groupSessionToCreate);
                    unitOfWork.Save();
                    return Json(new { Success = true, Url = Url.Action("Detail", new { SsId = groupSessionToCreate.sessionId, GSName = StringHelper.URLName(groupSessionToCreate.sessionSubject) }) });
                }
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            throw new ModelStateException(this.ModelState);
        }

        public ActionResult Edit(string SsId)
        {
            NullChecker.NullCheck(new object[] { SsId });
            var session = unitOfWork.GroupSessionRepository.GetByID(EncryptionHelper.Unprotect(SsId));
            if(!session.Group.Admins.Any(a=> AuthorizationHelper.isRelevant(a.UserId)))
            {
                return new RedirectToError();
            }
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        [SiteMapCacheRelease]
        public ActionResult Edit(string SsId, string filesToUpload)
        {
            NullChecker.NullCheck(new object[] { SsId });
            var session  = unitOfWork.GroupSessionRepository.GetByID(EncryptionHelper.Unprotect(SsId));
            if (TryUpdateModel(session, "", new string[] { "sessionSubject", "sessionDesc", "endDate" }))
            {
                if (session.Group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
                {
                    if (ITTConfig.CurrentCultureIsNotEN)
                    {
                        

                        var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, session.image + session.document, "GSession");

                        session.image = fileUploadResult.ImagesToUpload;
                        session.document = fileUploadResult.DocsToUpload;

                    }
                    unitOfWork.GroupSessionRepository.Update(session);
                    unitOfWork.Save();
                    return Json(new { Success = true, Url = Url.Action("Detail", new { SsId = session.sessionId, GSName = StringHelper.URLName(session.sessionSubject) }) });
                }
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            throw new ModelStateException(this.ModelState);
        }



        public ActionResult Delete(string SsId)
        {
            NullChecker.NullCheck(new object[] { SsId });
            var sessionToDel = unitOfWork.GroupSessionRepository.GetByID(EncryptionHelper.Unprotect(SsId));
            if (!sessionToDel.Group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                return new RedirectToError();
            }
            return PartialView(sessionToDel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        [SiteMapCacheRelease]
        public ActionResult Delete(string SsId, string S)
        {
            NullChecker.NullCheck(new object[] { SsId, S });
            var ssid = EncryptionHelper.Unprotect(SsId);
            var s = EncryptionHelper.Unprotect(S);
            var sessionToDel = unitOfWork.GroupSessionRepository.GetByID(ssid);
            if (sessionToDel.Group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)) && ssid == s)
            {
                var urlparm = sessionToDel.Group.groupName;
                //sessionToDel.Offers.SelectMany(o => o.Likes.Select(l=>l.likeID as object)).ToList().ForEach(unitOfWork.LikeRepository.Delete);
                //sessionToDel.Offers.SelectMany(c => c.Comments.SelectMany(l => l.Likes.Select(f => f.likeID as object))).ToList().ForEach(unitOfWork.LikeRepository.Delete);
                //sessionToDel.Offers.SelectMany(c => c.Comments.Select(f => f.commentID as object)).ToList().ForEach(unitOfWork.CommentRepository.Delete);
                sessionToDel.Offers.Select(o => o.offerId as object).ToList().ForEach(unitOfWork.GroupSessionOfferRepository.Delete);
                var fileUploadResult = UploadHelper.UpdateUploadedFiles(String.Empty, sessionToDel.image + sessionToDel.document, "GSession");
                if (String.IsNullOrEmpty(fileUploadResult.ImagesToUpload) && String.IsNullOrEmpty(fileUploadResult.DocsToUpload))
                {
                    unitOfWork.GroupSessionRepository.Delete(sessionToDel);
                    unitOfWork.Save();
                }
                return Json(new { Success = true, Url = Url.Action("GroupPage", "Group", new { GId = sessionToDel.groupId, GName = StringHelper.URLName(urlparm) }) });
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        

        public ActionResult ExportSessionToXL(string SsId)
        {
            NullChecker.NullCheck(new object[] { SsId });
            var ssid = EncryptionHelper.Unprotect(SsId);
            if (!AuthorizationHelper.isRelevant(WebSecurity.CurrentUserId, "UserPerGS", new SqlParameter("GS", ssid), new SqlParameter("U", WebSecurity.CurrentUserId)))
            {
                return new RedirectToError();
            }
            var reader = unitOfWork.ReaderRepository.GetSPDataReader("GSOffers", new SqlParameter("GS", ssid));

            var offers = new System.Data.DataTable("Group Sessions");
            offers.Columns.Add("پیشنهاد دهنده", typeof(string));
            offers.Columns.Add("پیشنهاد", typeof(string));
            offers.Columns.Add("وضعیت پذیرش", typeof(string));
            while (reader.Read())
            {
                var name = ITTConfig.CurrentCultureIsNotEN ? reader[0] as string : reader[1] as string;
                offers.Rows.Add(
                    name,
                    reader[2] as string,
                    reader[3] as bool? == true ? Resource.Resource.yes: Resource.Resource.no);
            }



            var grid = new GridView();
            grid.DataSource = offers;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=IT-SessionSummary.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            //return File(sw.ToString(),"application/ms-excel");
            return Content(String.Empty);
        }
    }
};