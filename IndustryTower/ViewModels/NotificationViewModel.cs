using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
        
    public class NotificationViewModelNew
    {
        public NotificationType notifType { get; set; }
        public int elemId { get; set; }
        public string senderUser { get; set; }
        public int? recId { get; set; }
        public string data { get; set; }
        public string brief { get; set; }
        public string image { get; set; }
        public int read { get; set; }
        public int count { get; set; }
        public DateTime occurDate { get; set; }
    }


    public class PosterCoNotif
    {
        public int? coid { get; set; }
        public int? stid { get; set; }
    }

 

    //public class PostNotif
    //{
    //    public Post post { get; set; }
    //    public IEnumerable<Notification> postLikeNotifications { get; set; }
    //    public IEnumerable<Notification> postCommentNotifications { get; set; }
    //    public DateTime maxDate { get; set; }
    //}

    //public class QuestionNotif
    //{
    //    public Question question { get; set; }
    //    public IEnumerable<Notification> questionLikeNotifications { get; set; }
    //    public IEnumerable<Notification> questionCommentNotifications { get; set; }
    //    public IEnumerable<Notification> questionAnswerNotifications { get; set; }
    //    public DateTime maxDate { get; set; }
    //}

    //public class AnswerNotif
    //{
    //    public Answer answer { get; set; }
    //    public IEnumerable<Notification> answerLikeNotifications { get; set; }
    //    public IEnumerable<Notification> answerCommentNotifications { get; set; }
    //    public IEnumerable<Notification> answerAcceptAnswerNotifications { get; set; }
    //    public DateTime maxDate { get; set; }
    //}
    //public class CompanyNotif
    //{
    //    public Company company { get; set; }
    //    public IEnumerable<Notification> companyFollowNotifications { get; set; }
    //    public DateTime maxDate { get; set; }
    //}
    //public class StoreNotif
    //{
    //    public Store store { get; set; }
    //    public IEnumerable<Notification> storeFollowNotifications { get; set; }
    //    public DateTime maxDate { get; set; }
    //}
    //public class ProductNotif
    //{
    //    public Product product { get; set; }
    //    public IEnumerable<Notification> productLikeNotifications { get; set; }
    //    public IEnumerable<Notification> productCommentNotifications { get; set; }
    //    public DateTime maxDate { get; set; }
    //}
    //public class ServiceNotif
    //{
    //    public Service service { get; set; }
    //    public IEnumerable<Notification> serviceLikeNotifications { get; set; }
    //    public IEnumerable<Notification> serviceCommentNotifications { get; set; }
    //    public DateTime maxDate { get; set; }
    //}
}