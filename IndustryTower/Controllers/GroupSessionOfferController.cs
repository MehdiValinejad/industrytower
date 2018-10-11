using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using PagedList;
using Resource;
using System.Data.SqlClient;
using IndustryTower.ViewModels;
using System.Data;
using Newtonsoft.Json;

namespace IndustryTower.Controllers
{
    [ITTAuthorize]
    public class GroupSessionOfferController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult PartialOffers(int SsId, int? page)
        {
            //var offers = unitOfWork.GroupSessionOfferRepository.Get(o => o.sessionId == ssid)
            //                                                .OrderByDescending(f=>f.offerDate);
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            int TotalRows = 0;
            SqlCommand outputCommand;

            List<SqlParameter> prams = new List<SqlParameter>();
            var totalRows = new SqlParameter("TotalRows", SqlDbType.Int, 50);
            totalRows.Direction = ParameterDirection.Output;
            var isadmin = new SqlParameter("IsAdmin", SqlDbType.Int, 50);
            isadmin.Direction = ParameterDirection.Output;
            prams.Add(totalRows);
            prams.Add(isadmin);
            prams.Add(new SqlParameter("U", WebSecurity.CurrentUserId));
            prams.Add(new SqlParameter("GS", SsId));
            prams.Add(new SqlParameter("pagNum", pageNumber));
            prams.Add(new SqlParameter("pageSize", pageSize));

            var reader = unitOfWork.ReaderRepository.GetSPDataReader("GSOffersList", prams, out outputCommand);

            IList<GSOViewModel> offers = new List<GSOViewModel>();
            while (reader.Read())
            {
                
                ActiveUser offerer = new ActiveUser {
                    UserId = reader.GetInt32(5),
                    firstName = reader[6] as string,
                    firstNameEN = reader[7] as string,
                    lastName = reader[8] as string,
                    lastNameEN = reader[9] as string,
                    image = reader[10] as string
                };
                GSOViewModel sess = new GSOViewModel();
                sess = new GSOViewModel
                {
                    offerid = reader.GetInt32(1),
                    offer = reader[2] as string,
                    offerdate = reader.GetDateTime(3),
                    isaccepted = reader.GetBoolean(4),
                    offerer = offerer,
                    Scores = reader[11] as int?
                };
                offers.Add(sess);
            }

            reader.Close();
            TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;
            ViewData["isAdmin"] = outputCommand.Parameters["IsAdmin"].Value;

            ViewData["SsId"] = SsId;

            ViewData["finalPage"] = TotalRows < pageSize * pageNumber;
            ViewData["pageNum"] = pageNumber;

            return PartialView(offers);
            //if (page != null)
            //{
            //    return PartialView(offers.ToPagedList(pageNumber, pageSize));
            //}
            //else return PartialView(offers.ToPagedList(1, pageSize));
        }

        public ActionResult PartialOffer(int GSOId)
        {
            var o = unitOfWork.GroupSessionOfferRepository.GetByID(GSOId);
            return PartialView(o);
        }

        public ActionResult PartialOfferFeed(int GSOId)
        {
            var o = unitOfWork.GroupSessionOfferRepository.GetByID(GSOId);
            return PartialView(o);
        }

        public ActionResult Create(string SsId)
        {
            NullChecker.NullCheck(new object[] { SsId });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        public ActionResult Create([Bind(Include = "offer")] GroupSessionOffer offerToInsert, int SsId)
        {
            var group = unitOfWork.GroupSessionRepository.GetByID(SsId).Group;
            if (group.Members.Any(a => a.UserId == WebSecurity.CurrentUserId))
            {
                if (ModelState.IsValid)
                {
                    offerToInsert.offerDate = DateTime.UtcNow;
                    offerToInsert.sessionId = SsId;
                    offerToInsert.offererId = WebSecurity.CurrentUserId;

                    unitOfWork.GroupSessionOfferRepository.Insert(offerToInsert);
                    unitOfWork.Save();
                    
                    return Json(new { Message = Resource.Resource.createdSuccessfully, Result = RenderPartialViewHelper.RenderPartialView(this, "PartialOffer", offerToInsert) });
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        

        public ActionResult Edit(string GSOId)
        {
            NullChecker.NullCheck(new object[] { GSOId });
            var offer = unitOfWork.GroupSessionOfferRepository.GetByID(EncryptionHelper.Unprotect(GSOId));
            if (offer.offererId == WebSecurity.CurrentUserId || offer.GroupSession.Group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                return PartialView(offer);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        public ActionResult Edit(string GSOId, FormCollection formcoll)
        {
            NullChecker.NullCheck(new object[] { GSOId });
            var offer = unitOfWork.GroupSessionOfferRepository.GetByID(EncryptionHelper.Unprotect(GSOId));
            if (offer.offererId == WebSecurity.CurrentUserId || offer.GroupSession.Group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                if (TryUpdateModel(offer, "", new string[] { "offer" }))
                {
                    unitOfWork.GroupSessionOfferRepository.Update(offer);
                    unitOfWork.Save();
                    return Json(new { Message = Resource.Resource.editedSuccessfully, Result = offer.offer });
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }


        public ActionResult Delete(string GSOId)
        {
            NullChecker.NullCheck(new object[] { GSOId });
            var offerToDel = unitOfWork.GroupSessionOfferRepository.GetByID(EncryptionHelper.Unprotect(GSOId));
            if (offerToDel.offererId == WebSecurity.CurrentUserId || offerToDel.GroupSession.Group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                return PartialView(offerToDel);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        public ActionResult Delete(string GSOId, string O)
        {
            NullChecker.NullCheck(new object[] { GSOId, O });
            var gsoid = EncryptionHelper.Unprotect(GSOId);
            var o = EncryptionHelper.Unprotect(O);
            var offerToDel = unitOfWork.GroupSessionOfferRepository.GetByID(EncryptionHelper.Unprotect(GSOId));
            if (offerToDel.offererId == WebSecurity.CurrentUserId || offerToDel.GroupSession.Group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                if (o == gsoid)
                {
                    offerToDel.Likes.Select(l => l.likeID as object).ToList().ForEach(unitOfWork.LikeGSORepository.Delete);
                    unitOfWork.LikeCommentRepository.Get(d => offerToDel.Comments.Select(f => f.cmtID).Contains(d.elemID) && d.type == LikeCommentType.GSO).ToList().ForEach(unitOfWork.LikeCommentRepository.Delete);
                    //offerToDel.Comments.SelectMany(l => l.Likes.Select(d => d.likeID as object)).ToList().ForEach(unitOfWork.LikeRepository.Delete);
                    offerToDel.Comments.Select(c => c.cmtID as object).ToList().ForEach(unitOfWork.CommentGSORepository.Delete);
                    unitOfWork.GroupSessionOfferRepository.Delete(offerToDel.offerId);
                    unitOfWork.Save();
                    return Json(new { Success = true, Message = Resource.Resource.deletedSuccessfully });
                }
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        public ActionResult AcceptOffer(string GSOId)
        {
            NullChecker.NullCheck(new object[] { GSOId });

            var offer = unitOfWork.GroupSessionOfferRepository.GetByID(EncryptionHelper.Unprotect(GSOId));
            if (offer.GroupSession.Group.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                string text = Resource.Resource.acceptedNotSessionOffer;
                string message = Resource.Resource.acceptedNotSessionOfferMessage;
                if (offer.isAccepted)
                {
                    offer.isAccepted = false;
                }
                else
                {
                    offer.isAccepted = true;
                    text = Resource.Resource.acceptedSessionOffer;
                    message = Resource.Resource.acceptedSessionOfferMessage;
                }
                unitOfWork.GroupSessionOfferRepository.Update(offer);
                unitOfWork.Save();
                if (offer.isAccepted)
                {
                    FeedHelper.FeedInsert(FeedType.SessionOfferAccept,
                                           offer.sessionId,
                                          offer.offererId
                                          );
                    NotificationHelper.NotificationInsert(
                                                NotificationType.SessionOfferAccept,
                                                elemId: offer.offerId
                                             );
                    ScoreHelper.Update(new ScoreVars
                    {
                        type = ScoreType.GSOacc,
                        elemId = offer.offerId,
                        sign = 1
                    });
                }
                
                return Json(new { Accepted = offer.isAccepted, Message = message, Result = text });
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }
    }
}