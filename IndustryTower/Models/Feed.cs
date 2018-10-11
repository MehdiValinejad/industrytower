using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public enum FeedType
    {
        Post = 0,
        Share = 1,
        Question = 2,
        Answer = 3,
        PostLike = 4,
        PostComment =5,
        QuestionComment = 6,
        QuestionLike = 7,
        AnswerLike = 8,
        AnswerComment = 9,
        AnswerAccept = 10,
        FriendRequestAccept = 11,
        SessionOfferLike = 12,
        SessionOfferComment = 13,
        SessionOfferAccept = 14,
        UserCertificate = 15,
        Experience = 16,
        ProductLike = 17,
        ProductComment = 18,
        ServiceLike = 19,
        ServiceComment = 20,
        Book = 21,
        ReviewBook = 22
    }
    public class Feed
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long feedId { get; set; }

        [Index]
        public FeedType feedType { get; set; }

        [Index]
        public int elemId { get; set; }
        public int? adjId { get; set; }
        public string data { get; set; }
        public DateTime date { get; set; }

        [ForeignKey("adjId")]
        public ActiveUser adjUser { get; set; }

    }
}