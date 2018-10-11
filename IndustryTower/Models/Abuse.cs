using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public enum abuseType
    { 
        User,Company,Store,Product,Service,Certificate,Event,Question,Answer,Job,JobOffer,Project,ProjectOffer,Post,Comment,Message
    }
    public class Abuse
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int abuseID { get; set; }

        public abuseType abuseType { get; set; }

        public int reporterUserID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecifyAbuse", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "abuseCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "abuseDesc", ResourceType = typeof(ModelDisplayName))]
        public string abuseDescription { get; set; }

        //public int? postID { get; set; }
        //public int? commentID { get; set; }
        //public int? questionID { get; set; }
        //public int? answerID { get; set; }
        //public int? eventID { get; set; }
        //public int? abuserUserID { get; set; }
        //public int? abuserCoID { get; set; }
        //public int? abuserStoreID { get; set; }
        //public int? productID { get; set; }
        //public int? serviceID { get; set; }
        //public int? projectID { get; set; }
        //public int? projectOfferID { get; set; }
        //public int? jobID { get; set; }
        //public int? jobOfferID { get; set; }
        //public int? messageID { get; set; }

        [DefaultValue(false)]
        public bool Audit { get; set; }

        public DateTime reportDate { get; set; }

        //[InverseProperty("ReportedAbuses")]
        [ForeignKey("reporterUserID")]
        public virtual ActiveUser reporterUser { get; set; }
        //[InverseProperty("AbusesByMe")]
        //[ForeignKey("abuserUserID")]
        //public virtual UserProfile abuserUser { get; set; }


        //[ForeignKey("postID")]
        //public virtual Post Post { get; set; }
        //[ForeignKey("commentID")]
        //public virtual Comment Comment { get; set; }
        //[ForeignKey("questionID")]
        //public virtual Question Question { get; set; }
        //[ForeignKey("answerID")]
        //public virtual Answer Answer { get; set; }
        //[ForeignKey("eventID")]
        //public virtual Event Event { get; set; }
        //[ForeignKey("abuserCoID")]
        //public virtual Company Company { get; set; }
        //[ForeignKey("abuserStoreID")]
        //public virtual Store Store { get; set; }
        //[ForeignKey("productID")]
        //public virtual Product Product { get; set; }
        //[ForeignKey("serviceID")]
        //public virtual Service Service { get; set; }
        //[ForeignKey("projectID")]
        //public virtual Project Project { get; set; }
        //[ForeignKey("projectOfferID")]
        //public virtual ProjectOffer ProjectOffer { get; set; }
        //[ForeignKey("jobID")]
        //public virtual Job Job { get; set; }
        //[ForeignKey("jobOfferID")]
        //public virtual JobOffer JobOffer { get; set; }
        //[ForeignKey("messageID")]
        //public virtual Message Message { get; set; }
    }
}