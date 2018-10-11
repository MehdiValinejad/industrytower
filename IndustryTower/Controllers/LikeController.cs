using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Microsoft.Web.Mvc;
using Newtonsoft.Json;
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
    public class LikeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult Likes(likeVars model)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                    "LikerUsers",
                    new SqlParameter("elemId", model.elemId),
                    new SqlParameter("TBL", model.typ.ToString()));

            IList<ActiveUser> users = new List<ActiveUser>();
            while (reader.Read())
            {
                ActiveUser user = new ActiveUser();
                user.UserId = reader.GetInt32(0);
                user.firstName = reader[1] as string;
                user.firstNameEN = reader[2] as string;
                user.lastName = reader[3] as string;
                user.lastNameEN = reader[4] as string;
                user.image = reader[5] as string;

                users.Add(user);
            }
            reader.Close();

            return PartialView("~/Views/UserProfile/_PartialUsers.cshtml", users);
        }

        [AllowAnonymous]
        public ActionResult Like(likeVars model)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                   "Likes",
                   new SqlParameter("elemId", model.elemId),
                   new SqlParameter("TBL", model.typ.ToString())
                   );
            IList<int> likersIds = new List<int>();
            while (reader.Read())
            {
                likersIds.Add(reader.GetInt32(0));
            }
            reader.Close();

            var finaleModel = new LikeViewModel
            {
                likerIds = likersIds,
                prams = model
            };
            return PartialView(finaleModel);
        }

        [AllowAnonymous]
        public ActionResult LikeComment(likeVars model)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                   "LikesComment",
                   new SqlParameter("elemId", model.elemId),
                   new SqlParameter("typ", model.typ)
                   );
            IList<int> likersIds = new List<int>();
            while (reader.Read())
            {
                likersIds.Add(reader.GetInt32(0));
            }
            reader.Close();

            var finaleModel = new LikeViewModel
            {
                likerIds = likersIds,
                prams = model
            };
            return PartialView("~/Views/Like/Like.cshtml", finaleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LikeInsert([Deserialize(SerializationMode.Encrypted)] likeVars model)
        {

            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    throw new JsonCustomException(ControllerError.ajaxErrorLike);
                }

                List<SqlParameter> prams =new List<SqlParameter>();
                SqlCommand outputCommand;
                var isAddedP = new SqlParameter("isAdded", SqlDbType.Int, 50);
                isAddedP.Direction = ParameterDirection.Output;
                prams.Add(isAddedP);
                prams.Add(new SqlParameter("U", WebSecurity.CurrentUserId));
                prams.Add(new SqlParameter("elemId", model.elemId));
                prams.Add(new SqlParameter("TBL", model.typ.ToString()));
                var reader = unitOfWork.ReaderRepository.GetSPDataReader("LikeUpdate", prams, out outputCommand);

                IList<int> likersIds = new List<int>();
                while (reader.Read())
                {
                    likersIds.Add(reader.GetInt32(0));
                }
                reader.Close();
                //if ((int)outputCommand.Parameters["isAdded"].Value == 1 && model.typ != LikeType.LikeComment)
                //{
                //    NotificationType notiftyp = NotificationType.PostLike;
                //    FeedType feedtyp = FeedType.PostLike;
                //    switch (model.typ)
                //    { 
                //        case LikeType.LikePost:
                //            notiftyp = NotificationType.PostLike;
                //            feedtyp = FeedType.PostLike;
                //            break;
                //        case LikeType.LikeQuestion:
                //            notiftyp = NotificationType.QuestionLike;
                //            feedtyp = FeedType.QuestionLike;
                //            break;
                //        case LikeType.LikeAnswer:
                //            notiftyp = NotificationType.AnswerLike;
                //            feedtyp = FeedType.AnswerLike;
                //            break;
                //        case LikeType.LikeProduct:
                //            notiftyp = NotificationType.ProductLike;
                //            feedtyp = FeedType.ProductLike;
                //            break;
                //        case LikeType.LikeService:
                //            notiftyp = NotificationType.ServiceLike;
                //            feedtyp = FeedType.ServiceLike;
                //            break;
                //        case LikeType.LikeGSO:
                //            notiftyp = NotificationType.SessionOfferLike;
                //            feedtyp = FeedType.SessionOfferLike;
                //            break;
                //    }
                //    NotificationHelper.NotificationInsert(notiftyp, elemId: model.elemId);
                //    FeedHelper.FeedInsert(feedtyp, model.elemId, WebSecurity.CurrentUserId);
                //}

                LikeViewModel finalModel = new LikeViewModel();
                finalModel.prams = model;
                finalModel.likerIds = likersIds; 
                //finalModel.Likes = likes;

                return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "Like", finalModel) });
            }
            throw new ModelStateException(this.ModelState);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LikeCmtInsert([Deserialize(SerializationMode.Encrypted)] likeVars model)
        {

            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    throw new JsonCustomException(ControllerError.ajaxErrorLike);
                }

                List<SqlParameter> prams = new List<SqlParameter>();
                SqlCommand outputCommand;
                var isAddedP = new SqlParameter("isAdded", SqlDbType.Int, 50);
                isAddedP.Direction = ParameterDirection.Output;
                prams.Add(isAddedP);
                prams.Add(new SqlParameter("U", WebSecurity.CurrentUserId));
                prams.Add(new SqlParameter("elemId", model.elemId));
                prams.Add(new SqlParameter("typ", model.typ));
                var reader = unitOfWork.ReaderRepository.GetSPDataReader("LikeCommentUpdate", prams, out outputCommand);

                IList<int> likersIds = new List<int>();
                while (reader.Read())
                {
                    likersIds.Add(reader.GetInt32(0));
                }
                reader.Close();
                //if ((int)outputCommand.Parameters["isAdded"].Value == 1 && model.typ != LikeType.LikeComment)
                //{
                //    NotificationType notiftyp = NotificationType.PostLike;
                //    FeedType feedtyp = FeedType.PostLike;
                //    switch (model.typ)
                //    { 
                //        case LikeType.LikePost:
                //            notiftyp = NotificationType.PostLike;
                //            feedtyp = FeedType.PostLike;
                //            break;
                //        case LikeType.LikeQuestion:
                //            notiftyp = NotificationType.QuestionLike;
                //            feedtyp = FeedType.QuestionLike;
                //            break;
                //        case LikeType.LikeAnswer:
                //            notiftyp = NotificationType.AnswerLike;
                //            feedtyp = FeedType.AnswerLike;
                //            break;
                //        case LikeType.LikeProduct:
                //            notiftyp = NotificationType.ProductLike;
                //            feedtyp = FeedType.ProductLike;
                //            break;
                //        case LikeType.LikeService:
                //            notiftyp = NotificationType.ServiceLike;
                //            feedtyp = FeedType.ServiceLike;
                //            break;
                //        case LikeType.LikeGSO:
                //            notiftyp = NotificationType.SessionOfferLike;
                //            feedtyp = FeedType.SessionOfferLike;
                //            break;
                //    }
                //    NotificationHelper.NotificationInsert(notiftyp, elemId: model.elemId);
                //    FeedHelper.FeedInsert(feedtyp, model.elemId, WebSecurity.CurrentUserId);
                //}

                LikeViewModel finalModel = new LikeViewModel();
                finalModel.prams = model;
                finalModel.likerIds = likersIds;
                //finalModel.Likes = likes;

                return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "Like", finalModel) });
            }
            throw new ModelStateException(this.ModelState);
        }
    }
}
