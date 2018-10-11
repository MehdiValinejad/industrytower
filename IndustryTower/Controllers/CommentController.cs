using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Microsoft.Web.Mvc;
using PagedList;
using Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class CommentController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        [AjaxRequestOnly]
        public ActionResult Comments([Deserialize]commentVars model, int? commentPage)
        {
            int TotalRows = 0;
            SqlCommand outputCommand;

            int pageNumber = commentPage ?? 1;
            int pageSize = 20;

            List<SqlParameter> prams = new List<SqlParameter>();
            //var totlRes = new SqlParameter("TotalRows", SqlDbType.Int, 50);
            //totlRes.Direction = ParameterDirection.Output;
            //prams.Add(totlRes);

            if (commentPage == null)
            {
                prams.Add(new SqlParameter("pageSize", 2));
            }
            else
            {
                prams.Add(new SqlParameter("PagNum", pageNumber));
            }
            prams.Add(new SqlParameter("elemId", model.elemId));
            prams.Add(new SqlParameter("typ", model.typ));
            prams.Add(new SqlParameter("TBL", model.typ.ToString()));

            var reader = unitOfWork.ReaderRepository.GetSPDataReader("Comments", prams, out outputCommand);
            List<CommentEach> comms = new List<CommentEach>();
            while (reader.Read())
            {
                CommentEach comm = new CommentEach();
                comm.cmtID = reader.GetInt32(0);
                comm.comment = reader[1] as string;
                comm.date = reader.GetDateTime(2);
                comm.likes = reader.GetInt32(9);

                ActiveUser commenter = new ActiveUser();
                commenter.UserId = reader.GetInt32(3);
                commenter.firstName = reader[4] as string;
                commenter.firstNameEN = reader[5] as string;
                commenter.lastName = reader[6] as string;
                commenter.lastNameEN = reader[7] as string;
                commenter.image = reader[8] as string;

                comm.CommenterUser = commenter;

                comms.Add(comm);
            }
            ViewData["finalPage"] = !reader.HasRows;
            reader.Close();

            //TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;
            
            ViewData["pageNum"] = pageNumber;

            if (commentPage == null) return PartialView(new CommentViewModel
            {
                Comments = comms,
                prams = model
            });
            else return PartialView("NextComments", new CommentViewModel
            {
                Comments = comms,
                prams = model
            });

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AjaxRequestOnly]
        //[HostControl]
        public ActionResult CommentInsert([Deserialize]commentVars model, string comment)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(this.ModelState);
            }
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "CommentInsert",
                new SqlParameter("U", WebSecurity.CurrentUserId),
                new SqlParameter("elemId", model.elemId),
                new SqlParameter("TBL", model.typ.ToString()),
                new SqlParameter("text", comment)
                );

            CommentEach cmt = new CommentEach();
            if (reader.Read())
            {
                cmt.cmtID = reader.GetInt32(0);
                cmt.comment = reader[1] as string;
                cmt.date = reader.GetDateTime(2);
                ActiveUser us = new ActiveUser();
                us.UserId = reader.GetInt32(3);
                us.firstName = reader[4] as string;
                us.firstNameEN = reader[5] as string;
                us.lastName = reader[6] as string;
                us.lastNameEN = reader[7] as string;
                cmt.CommenterUser = us;

                ViewData["typ"] = model.typ;
            }

            NotificationType notiftyp = NotificationType.PostLike;
            FeedType feedtyp = FeedType.PostLike;
            switch (model.typ)
            {
                case CommentType.CommentPost:
                    notiftyp = NotificationType.PostComment;
                    feedtyp = FeedType.PostComment;
                    break;
                case CommentType.CommentQuestion:
                    notiftyp = NotificationType.QuestionComment;
                    feedtyp = FeedType.QuestionComment;
                    break;
                case CommentType.CommentAnswer:
                    notiftyp = NotificationType.AnswerComment;
                    feedtyp = FeedType.AnswerComment;
                    break;
                case CommentType.CommentProduct:
                    notiftyp = NotificationType.ProductComment;
                    feedtyp = FeedType.ProductComment;
                    break;
                case CommentType.CommentService:
                    notiftyp = NotificationType.ServiceComment;
                    feedtyp = FeedType.ServiceComment;
                    break;
                case CommentType.CommentGSO:
                    notiftyp = NotificationType.SessionOfferComment;
                    feedtyp = FeedType.SessionOfferComment;
                    break;
            }
            NotificationHelper.NotificationInsert(notiftyp, elemId: model.elemId);
            FeedHelper.FeedInsert(feedtyp, model.elemId, WebSecurity.CurrentUserId);


            return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "NewInsertedComment", cmt) });     
        }


        public ActionResult Delete(string CmtId, CommentType typ)
        {
            NullChecker.NullCheck(new object[] { CmtId });
            //var commentToDelete = unitOfWork.CommentRepository.GetByID(EncryptionHelper.Unprotect(CmtId));
            //if (!AuthorizationHelper.isRelevant(commentToDelete.commenterUserID))
            //{
            //    throw new JsonCustomException(ControllerError.ajaxError);
            //}
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Delete(string CmtId, string CTD)
        {
            NullChecker.NullCheck(new object[] { CTD });
            //var cmtid = EncryptionHelper.Unprotect(CmtId);
            var ctd = EncryptionHelper.Unprotect(CTD);
            if (ModelState.IsValid)
            {

                //var commentToDel = unitOfWork.CommentRepository.GetByID(ctd);
                //if (AuthorizationHelper.isRelevant(commentToDel.commenterUserID))
                //{
                //    commentToDel.Likes.Select(l => l.likeID as object).ToList().ForEach(unitOfWork.LikeRepository.Delete);//CommentLikesDelete
                //    unitOfWork.CommentRepository.Delete(ctd);
                //    unitOfWork.Save();
                //    return Json(new { Success = true, Message = Resource.Resource.deletedSuccessfully });
                //}
                //throw new JsonCustomException(ControllerError.ajaxError);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }
    }
}
