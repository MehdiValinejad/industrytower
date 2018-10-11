using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public enum NotificationType
    {
        Post = 0,
        PostCo = 1,
        PostSt = 2,
        PostLike = 3,
        PostComment = 4,
        QuestionAnswer = 5,
        QuestionComment = 6,
        QuestionLike = 7,
        AnswerLike = 8,
        AnswerComment = 9,
        AnswerAccept =10,
        ProductLike = 11,
        ProductComment = 12,
        ServiceLike = 13,
        ServiceComment = 14,
        FriendRequestAccept = 15,
        SessionOfferLike = 16,
        SessionOfferComment = 17,
        SessionOfferAccept = 18
    }
    public class Notification
    {
        [Key]
        public long notificationID { get; set; }

        [Index]
        public NotificationType notifType { get; set; }

        [Index]
        public int elemId { get; set; }

        [Index]
        public int senderId { get; set; }

        [Index]
        public int recId { get; set; }

        [StringLength(100)]
        public string brief { get; set; }

        public string data { get; set; }

        [DefaultValue(false)]
        public bool read { get; set; }

        public DateTime occurDate { get; set; }


    }
}