using IndustryTower.DAL;
using IndustryTower.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using WebMatrix.WebData;

namespace IndustryTower.Helpers
{
    public class NotificationHelper
    {
        
        public static void NotificationInsert(NotificationType NotifType,
                                                int elemId,
                                                int? senderId = null,
                                                int? senderCoStoreId = null,
                                                int? recId = null,
                                                string data = null)
        {
            if (AuthorizationHelper.isAdmin())
            {
                return;
            }
            senderId = WebSecurity.CurrentUserId;

            UnitOfWork unitOfWork = new UnitOfWork();
            unitOfWork.ReaderRepository.SPExecuteNonQuery(
                "NotifInserter",
                new SqlParameter("U", WebSecurity.CurrentUserId),
                new SqlParameter("typ", NotifType),
                new SqlParameter("elemId", elemId),
                new SqlParameter("senderId", senderId),
                //new SqlParameter("senderCoStId", senderCoStoreId),
                new SqlParameter("recId", recId),
                new SqlParameter("data", data)
                );
            //Notification notif = new Notification();
            //notif.notifType = NotifType;
            //notif.occurDate = DateTime.UtcNow;
            //notif.senderUserID = senderUserId != null ? senderUserId : WebSecurity.CurrentUserId;
            //notif.senderCoID = senderCoId;
            //notif.senderStID = senderStId;
            //notif.receiverUserID = receiverUserId;
            //notif.receiverCoID = receiverCoId;
            //notif.receiverStID = receiverStId;

            //var currentNotifs = unitOfWork.NotificationRepository
            //                    .Get(n => n.senderUserID == WebSecurity.CurrentUserId
            //                         && n.notifType == NotifType
            //                         && n.postID == PId
            //                         && n.questionID == QId
            //                         && n.answerID == AId
            //                         && n.coID == CoId
            //                         && n.storeID == StId
            //                         && n.productID == PrId
            //                         && n.serviceID == SrId
            //                         && n.friendShipID == FsId
            //                         && n.GSofferID == GSOId);
            //if (currentNotifs.Count() > 0)
            //{
            //    foreach (var item in currentNotifs)
            //    {
            //        item.occurDate = DateTime.UtcNow;
            //        item.read = false;
            //        unitOfWork.NotificationRepository.Update(item);
            //    }
            //    return;
            //}

            //switch(NotifType)
            //{
            //    case NotificationType.Post:
            //            notif.postID = PId;
            //            if (receiverCoId != null)
            //            {
            //                var co = unitOfWork.NotExpiredCompanyRepository.GetByID(receiverCoId);
            //                foreach (var i in co.Admins)
            //                {
            //                    if (i.UserId != WebSecurity.CurrentUserId)
            //                    {
            //                        notif.receiverUserID = i.UserId;
            //                        unitOfWork.NotificationRepository.Insert(notif);
            //                    }
            //                }
            //            }
            //            else if (receiverStId != null)
            //            {
            //                var st = unitOfWork.StoreNotExpiredRepository.GetByID(receiverStId);
            //                foreach (var i in st.Admins)
            //                {
            //                    if (i.UserId != WebSecurity.CurrentUserId)
            //                    {
            //                        notif.receiverUserID = i.UserId;
            //                        unitOfWork.NotificationRepository.Insert(notif);
            //                    }
            //                }
            //            }
            //            else 
            //            {
            //                if (receiverUserId != WebSecurity.CurrentUserId)
            //                {
            //                    unitOfWork.NotificationRepository.Insert(notif);
            //                }
            //            }
            //        break;
            //    case NotificationType.PostComment:
            //    case NotificationType.PostLike:
            //        notif.postID = PId;
            //        var post = unitOfWork.PostRepository.GetByID(PId);
            //        if (post.postedCoID != null)
            //        { 
            //            foreach(var admin in post.PostedCompany.Admins)
            //            {
            //                if (admin.UserId != WebSecurity.CurrentUserId)
            //                {
            //                    notif.receiverUserID = admin.UserId;
            //                    unitOfWork.NotificationRepository.Insert(notif);
            //                }
            //            }
            //        }
            //        else if (post.postedStoreID != null)
            //        {
            //            foreach (var admin in post.PostedStore.Admins)
            //            {
            //                if (admin.UserId != WebSecurity.CurrentUserId)
            //                {
            //                    notif.receiverUserID = admin.UserId;
            //                    unitOfWork.NotificationRepository.Insert(notif);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (post.postedUserID != WebSecurity.CurrentUserId)
            //            {
            //                notif.receiverUserID = post.postedUserID;
            //                unitOfWork.NotificationRepository.Insert(notif);
            //            }
            //        }

            //        if (post.posterCoID != null)
            //        {
            //            foreach (var admin in post.PosterCompany.Admins)
            //            {
            //                if (admin.UserId != WebSecurity.CurrentUserId)
            //                {
            //                    notif.receiverUserID = admin.UserId;
            //                    unitOfWork.NotificationRepository.Insert(notif);
            //                }
            //            }
            //        }
            //        else if (post.posterStoreID != null)
            //        {
            //            foreach (var admin in post.PosterStore.Admins)
            //            {
            //                if (admin.UserId != WebSecurity.CurrentUserId)
            //                {
            //                    notif.receiverUserID = admin.UserId;
            //                    unitOfWork.NotificationRepository.Insert(notif);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (post.postedUserID != WebSecurity.CurrentUserId)
            //            {
            //                notif.receiverUserID = post.posterUserID;
            //                unitOfWork.NotificationRepository.Insert(notif);
            //            }
            //        }
            //        //notif.receiverUserID = post.postedUserID;
            //        //notif.receiverCoID = post.postedCoID;
            //        //notif.receiverStID = post.postedStoreID;
            //        //unitOfWork.NotificationRepository.Insert(notif);
            //        break;
            //    case NotificationType.QuestionAnswer:
            //    case NotificationType.QuestionComment:
            //    case NotificationType.QuestionLike:
            //        notif.questionID = QId;
            //        var question = unitOfWork.QuestionRepository.GetByID(QId);
            //        if (question.questionerID != WebSecurity.CurrentUserId)
            //        {
            //            notif.receiverUserID = question.questionerID;
            //            unitOfWork.NotificationRepository.Insert(notif);
            //        }
            //        break;
            //    case NotificationType.AnswerAccept:
            //        if (receiverUserId != WebSecurity.CurrentUserId)
            //        {
            //            notif.answerID = AId;
            //            unitOfWork.NotificationRepository.Insert(notif);
            //        }
            //        break;
            //    case NotificationType.AnswerComment:
            //    case NotificationType.AnswerLike:
            //        notif.answerID = AId;
            //        var answer = unitOfWork.AnswerRepository.GetByID(AId);
            //        if (answer.answererID != WebSecurity.CurrentUserId)
            //        {
            //            notif.receiverUserID = answer.answererID;
            //            unitOfWork.NotificationRepository.Insert(notif);
            //        }
            //        break;
            //    case NotificationType.CompanyFollow:
            //        notif.coID = CoId;
            //        var company = unitOfWork.NotExpiredCompanyRepository.GetByID(CoId);
            //        foreach (var admin in company.Admins)
            //        {
            //            if (admin.UserId != WebSecurity.CurrentUserId)
            //            {
            //                notif.receiverUserID = admin.UserId;
            //                unitOfWork.NotificationRepository.Insert(notif);
            //            }
            //        }
            //        break;
            //    case NotificationType.StoreFollow:
            //        notif.storeID = StId;
            //        var store = unitOfWork.StoreNotExpiredRepository.GetByID(StId);
            //        foreach (var admin in store.Admins)
            //        {
            //            if (admin.UserId != WebSecurity.CurrentUserId)
            //            {
            //                notif.receiverUserID = admin.UserId;
            //                unitOfWork.NotificationRepository.Insert(notif);
            //            }
            //        }
            //        break;
            //    case NotificationType.ProductLike:
            //    case NotificationType.ProductComment:
            //        notif.productID = PrId;
            //        var product = unitOfWork.ProductRepository.GetByID(PrId);
            //        foreach (var admin in product.company.Admins)
            //        {
            //            if (admin.UserId != WebSecurity.CurrentUserId)
            //            {
            //                notif.receiverUserID = admin.UserId;
            //                unitOfWork.NotificationRepository.Insert(notif);
            //            }
            //        }
            //        break;
            //    case NotificationType.ServiceLike:
            //    case NotificationType.ServiceComment:
            //        notif.serviceID = SrId;
            //        var service = unitOfWork.ServiceRepository.GetByID(SrId);
            //        foreach (var admin in service.company.Admins)
            //        {
            //            if (admin.UserId != WebSecurity.CurrentUserId)
            //            {
            //                notif.receiverUserID = admin.UserId;
            //                unitOfWork.NotificationRepository.Insert(notif);
            //            }
            //        }
            //        break;
            //    case NotificationType.FriendRequestAccept:
            //        if (receiverUserId != WebSecurity.CurrentUserId)
            //        {
            //            notif.receiverUserID = receiverUserId;
            //            notif.friendShipID = FsId;
            //            unitOfWork.NotificationRepository.Insert(notif);
            //        }
            //        break;
            //    case NotificationType.SessionOfferComment:
            //    case NotificationType.SessionOfferLike:
            //        notif.GSofferID = GSOId;
            //        var groupSessionoffer = unitOfWork.GroupSessionOfferRepository.GetByID(GSOId);
            //        if (groupSessionoffer.offererId != WebSecurity.CurrentUserId)
            //        {
            //            notif.receiverUserID = groupSessionoffer.offererId;
            //            unitOfWork.NotificationRepository.Insert(notif);
            //        }
            //        break;

            //}
        }
        
    }
}