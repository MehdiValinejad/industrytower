using Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class FriendRequest
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int requestID { get;set; }

        [StringLength(200, MinimumLength = 0, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "message", ResourceType = typeof(ModelDisplayName))]
        public string message { get; set; }

        public bool ignore { get; set; }

        public int requestSenderID { get; set; }
        public int requestReceiverID { get; set; }

        public DateTime requestDate { get; set; }

        [InverseProperty("SentFriendRequests")]
        [ForeignKey("requestSenderID")]
        public virtual ActiveUser RequesterUser { get; set; }
        [InverseProperty("ReceivedFriendRequests")]
        [ForeignKey("requestReceiverID")]
        public virtual ActiveUser RequestReceiverUser { get; set; }

    }
}