using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    public class NotificationController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        //
        // GET: /Notification/

        public ActionResult Index()
        {
            return View();
        }

        [ITTAuthorizeAttribute]
        public ActionResult NotifCounter()
        {
            if(Request.IsAuthenticated)
            {
                SqlCommand outputCommand;
                List<SqlParameter> prams = new List<SqlParameter>();
                var notifCount = new SqlParameter("NF",SqlDbType.Int,50);
                notifCount.Direction = ParameterDirection.Output;

                var frnotifCount = new SqlParameter("FR", SqlDbType.Int, 50);
                frnotifCount.Direction = ParameterDirection.Output;

                prams.Add(new SqlParameter("U", WebSecurity.CurrentUserId));
                prams.Add(notifCount);
                prams.Add(frnotifCount);
                

                var reader = unitOfWork.ReaderRepository.GetSPDataReader("NotifCount",prams,out outputCommand);
                reader.Close();

                return Json(new { FR = outputCommand.Parameters["FR"].Value, NF = outputCommand.Parameters["NF"].Value }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { FR = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NotificationRead(int elem, NotificationType nType)
        {
            var user = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);

            unitOfWork.ReaderRepository.SPExecuteNonQuery(
                "NotifRead",
                new SqlParameter("elemId", elem),
                new SqlParameter("typ",nType),
                new SqlParameter("U",WebSecurity.CurrentUserId)
                );
            //if (!String.IsNullOrEmpty(PId))
            //{
            //    var pid = EncryptionHelper.Unprotect(PId);
            //    user.ReceivedNotifications.Where(n => n.postID == pid)
            //                              .ToList().ForEach(i => i.read = true);
            //}
            //else if (!String.IsNullOrEmpty(CoId))
            //{
            //    var coid = EncryptionHelper.Unprotect(CoId);
            //    user.ReceivedNotifications.Where(n => n.coID == coid)
            //                              .ToList().ForEach(i => i.read = true);
            //}
            //else if (!String.IsNullOrEmpty(StId))
            //{
            //    var stid = EncryptionHelper.Unprotect(StId);
            //    user.ReceivedNotifications.Where(n => n.storeID == stid)
            //                              .ToList().ForEach(i => i.read = true);
            //}
            //else if (!String.IsNullOrEmpty(QId))
            //{
            //    var qid = EncryptionHelper.Unprotect(QId);
            //    user.ReceivedNotifications.Where(n => n.questionID == qid)
            //                              .ToList().ForEach(i => i.read = true);
            //}
            //else if (!String.IsNullOrEmpty(AId))
            //{
            //    var aid = EncryptionHelper.Unprotect(AId);
            //    user.ReceivedNotifications.Where(n => n.answerID == aid)
            //                              .ToList().ForEach(i => i.read = true);
            //}
            //else if (!String.IsNullOrEmpty(PrId))
            //{
            //    var prid = EncryptionHelper.Unprotect(PrId);
            //    user.ReceivedNotifications.Where(n => n.productID == prid)
            //                              .ToList().ForEach(i => i.read = true);
            //}
            //else if (!String.IsNullOrEmpty(SrId))
            //{
            //    var srid = EncryptionHelper.Unprotect(SrId);
            //    user.ReceivedNotifications.Where(n => n.serviceID == srid)
            //                              .ToList().ForEach(i => i.read = true);
            //}
            //else if (!String.IsNullOrEmpty(FsId))
            //{
            //    var fsid = EncryptionHelper.Unprotect(FsId);
            //    user.ReceivedNotifications.Where(n => n.friendShipID == fsid)
            //                              .ToList().ForEach(i => i.read = true);
            //}
            //else if (!String.IsNullOrEmpty(GSOId))
            //{
            //    var gsoid = EncryptionHelper.Unprotect(GSOId);
            //    user.ReceivedNotifications.Where(n => n.GSofferID == gsoid)
            //                              .ToList().ForEach(i => i.read = true);
            //}



            unitOfWork.Save(); 

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AllRead()
        {
            //var notifs = unitOfWork.NotificationRepository.Get(n => n.receiverUserID == WebSecurity.CurrentUserId);
            //foreach (var item in notifs)
            //{
            //    item.read = true;
            //    unitOfWork.NotificationRepository.Update(item);
            //}
            //unitOfWork.Save();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Notifications()
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "Notifs",
                new SqlParameter("U", WebSecurity.CurrentUserId),
                new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));

            IList<NotificationViewModelNew> notifs = new List<NotificationViewModelNew>();
            while (reader.Read())
            {
                notifs.Add(
                    new NotificationViewModelNew
                    {
                        notifType = (NotificationType)reader.GetInt32(0),
                        elemId = reader.GetInt32(1),
                        data = reader[2] as string,
                        senderUser = reader[3] as string,
                        image = reader[4] as string,
                        brief = reader[5] as string,
                        occurDate = reader.GetDateTime(6),
                        count = reader.GetInt32(7),
                        read = reader.GetInt32(8)
                        
                    });
            }

            return PartialView(notifs);






            //var notifRec = currentUser.ReceivedNotifications;

            ////Delete more than MaxNotifications
            //var morethan = notifRec.OrderByDescending(d => d.occurDate).Skip(ITTConfig.MaxNotifications);
            //if (morethan.Count() > 0)
            //{
            //    morethan.Select(f => f.notificationID as object).ToList().ForEach(unitOfWork.NotificationRepository.Delete);
            //    unitOfWork.Save();
            //}
            

            //var post = notifRec.Where(n => n.notifType == NotificationType.Post).GroupBy(g => g.Post)
            //                   .Select(g => new NotificationViewModel
            //                   {
            //                       read = g.Any(r => r.read == true),
            //                       post = g.Key,
            //                       PostNotification = g,
            //                       notifType = NotificationType.Post,
            //                       date = g.Max(d => d.occurDate)
            //                   });
            //var postLikes = notifRec.Where(n => n.notifType == NotificationType.PostLike).GroupBy(g => g.Post)
            //                   .Select(g => new NotificationViewModel
            //                   {
            //                       read = g.Any(r => r.read == true),
            //                       LikeNotifications = g,
            //                       post = g.Key,
            //                       notifType = NotificationType.PostLike,
            //                       date = g.Max(d => d.occurDate)
            //                   });
            //var postComments = notifRec.Where(n => n.notifType == NotificationType.PostComment).GroupBy(g => g.Post)
            //                   .Select(g => new NotificationViewModel
            //                   {
            //                       read = g.Any(r => r.read == true),
            //                       CommentNotifications = g,
            //                       post = g.Key,
            //                       notifType = NotificationType.PostComment,
            //                       date = g.Max(d => d.occurDate)
            //                   });
            //var questionLikes = notifRec.Where(n => n.notifType == NotificationType.QuestionLike).GroupBy(g => g.Question)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      LikeNotifications = g,
            //                      question = g.Key,
            //                      notifType = NotificationType.QuestionLike,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var questionComments = notifRec.Where(n => n.notifType == NotificationType.QuestionComment).GroupBy(g => g.Question)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      CommentNotifications = g,
            //                      question = g.Key,
            //                      notifType = NotificationType.QuestionComment,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var questionAnswers = notifRec.Where(n => n.notifType == NotificationType.QuestionAnswer).GroupBy(g => g.Question)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      questionAnswerNotifications = g,
            //                      question = g.Key,
            //                      notifType = NotificationType.QuestionAnswer,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var answerLike = notifRec.Where(n => n.notifType == NotificationType.AnswerLike).GroupBy(g => g.Answer)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      LikeNotifications = g,
            //                      answer = g.Key,
            //                      notifType = NotificationType.AnswerLike,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var answerComment = notifRec.Where(n => n.notifType == NotificationType.AnswerComment).GroupBy(g => g.Answer)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      CommentNotifications = g,
            //                      answer = g.Key,
            //                      notifType = NotificationType.AnswerComment,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var answerAccept = notifRec.Where(n => n.notifType == NotificationType.AnswerAccept).GroupBy(g => g.Answer)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      answerAcceptAnswerNotifications = g,
            //                      answer = g.Key,
            //                      notifType = NotificationType.AnswerAccept,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var companyFollow = notifRec.Where(n => n.notifType == NotificationType.CompanyFollow).GroupBy(g => g.Company)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      followNotifications = g,
            //                      company = g.Key,
            //                      notifType = NotificationType.CompanyFollow,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var storwFollow = notifRec.Where(n => n.notifType == NotificationType.StoreFollow).GroupBy(g => g.Store)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      followNotifications = g,
            //                      store = g.Key,
            //                      notifType = NotificationType.StoreFollow,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var productLike = notifRec.Where(n => n.notifType == NotificationType.ProductLike).GroupBy(g => g.Product)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      LikeNotifications = g,
            //                      product = g.Key,
            //                      notifType = NotificationType.ProductLike,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var productComment = notifRec.Where(n => n.notifType == NotificationType.ProductComment).GroupBy(g => g.Product)
            //                  .Select(g => new NotificationViewModel
            //                  {
            //                      read = g.Any(r => r.read == true),
            //                      CommentNotifications = g,
            //                      product = g.Key,
            //                      notifType = NotificationType.ProductComment,
            //                      date = g.Max(d => d.occurDate)
            //                  });
            //var serviceLike = notifRec.Where(n => n.notifType == NotificationType.ServiceLike).GroupBy(g => g.Service)
            //                 .Select(g => new NotificationViewModel
            //                 {
            //                     read = g.Any(r => r.read == true),
            //                     LikeNotifications = g,
            //                     service = g.Key,
            //                     notifType = NotificationType.ServiceLike,
            //                     date = g.Max(d => d.occurDate)
            //                 });
            //var serviceComment = notifRec.Where(n => n.notifType == NotificationType.ServiceComment).GroupBy(g => g.Service)
            //                 .Select(g => new NotificationViewModel
            //                 {
            //                     read = g.Any(r => r.read == true),
            //                     CommentNotifications = g,
            //                     service = g.Key,
            //                     notifType = NotificationType.ServiceComment,
            //                     date = g.Max(d => d.occurDate)
            //                 });
            //var friendRequests = notifRec.Where(n => n.notifType == NotificationType.FriendRequestAccept).GroupBy(g => g.FriendShip)
            //                 .Select(g => new NotificationViewModel
            //                 {
            //                     read = g.Any(r => r.read == true),
            //                     friendShip = g.Key,
            //                     friendShipAcceptNotifications = g,
            //                     notifType = NotificationType.FriendRequestAccept,
            //                     date = g.Max(d => d.occurDate)
            //                 });
            //var gsOffersLikes = notifRec.Where(n => n.notifType == NotificationType.SessionOfferLike).GroupBy(g => g.GroupSessionOffer)
            //                 .Select(g => new NotificationViewModel
            //                 {
            //                     read = g.Any(r => r.read == true),
            //                     gsOffer = g.Key,
            //                     LikeNotifications = g,
            //                     notifType = NotificationType.SessionOfferLike,
            //                     date = g.Max(d => d.occurDate)
            //                 });
            //var gsOffersComments = notifRec.Where(n => n.notifType == NotificationType.SessionOfferComment).GroupBy(g => g.GroupSessionOffer)
            //                 .Select(g => new NotificationViewModel
            //                 {
            //                     read = g.Any(r => r.read == true),
            //                     gsOffer = g.Key,
            //                     CommentNotifications = g,
            //                     notifType = NotificationType.SessionOfferComment,
            //                     date = g.Max(d => d.occurDate)
            //                 });
            
            //viewmodel = post
            //           .Concat(postLikes)
            //           .Concat(postComments)
            //           .Concat(questionLikes)
            //           .Concat(questionComments)
            //           .Concat(questionAnswers)
            //           .Concat(answerLike)
            //           .Concat(answerComment)
            //           .Concat(answerAccept)
            //           .Concat(companyFollow)
            //           .Concat(storwFollow)
            //           .Concat(productLike)
            //           .Concat(productComment)
            //           .Concat(serviceLike)
            //           .Concat(serviceComment)
            //           .Concat(friendRequests)
            //           .Concat(gsOffersLikes)
            //           .Concat(gsOffersComments)
            //           .OrderByDescending(o => o.date);
        }

    }
}
