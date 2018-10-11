using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Post
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int postID { get; set; }

        public int? posterUserID { get; set; }
        public int? postEditorUserID { get; set; }
        public int? postedUserID { get; set; }
        public int? posterCoID { get; set; }
        public int? postedCoID { get; set; }
        public int? posterStoreID { get; set; }
        public int? postedStoreID { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(600, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "post", ResourceType = typeof(ModelDisplayName))]
        public string post { get; set; }

        public string image { get; set; }
        public string document { get; set; }
        public string video { get; set; }
        

        public DateTime insertDate { get; set; }

        public DateTime? editDate { get; set; }

        [InverseProperty("SentPosts")]
        [ForeignKey("posterUserID")]
        public virtual ActiveUser PosterUser { get; set; }
        [InverseProperty("EditedPosts")]
        [ForeignKey("postEditorUserID")]
        public virtual ActiveUser PostEditorUser { get; set; }
        [InverseProperty("ReceivedPosts")]
        [ForeignKey("postedUserID")]
        public virtual ActiveUser PostedUser { get; set; }

        [InverseProperty("Sentposts")]
        [ForeignKey("posterCoID")]
        public virtual Company PosterCompany { get; set; }
        [InverseProperty("Receivedposts")]
        [ForeignKey("postedCoID")]
        public virtual Company PostedCompany { get; set; }

        [InverseProperty("Sentposts")]
        [ForeignKey("posterStoreID")]
        public virtual Store PosterStore { get; set; }
        [InverseProperty("Receivedposts")]
        [ForeignKey("postedStoreID")]
        public virtual Store PostedStore { get; set; }

        public virtual ICollection<Share> Shares { get; set; }
        public virtual ICollection<CommentPost> Comments { get; set; }
        public virtual ICollection<LikePost> Likes { get; set; }
        //public virtual ICollection<Abuse> Abuses { get; set; }
    }
}