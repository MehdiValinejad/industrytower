using IndustryTower.Helpers;
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
    public class GroupSessionOffer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int offerId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 100, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "sessionOffer", ResourceType = typeof(ModelDisplayName))]
        public string offer { get; set; }
        

        [DefaultValue(false)]
        [Display(Name = "offerIsAccepted", ResourceType = typeof(ModelDisplayName))]
        public bool isAccepted { get; set; }


        [Display(Name = "offerDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime offerDate { get; set; }

        public int sessionId { get; set; }
        public int offererId { get; set; }

        [ForeignKey("sessionId")]
        public virtual GroupSession GroupSession { get; set; }

        [ForeignKey("offererId")]
        public virtual ActiveUser offerer { get; set; }

        public virtual ICollection<LikeGSO> Likes { get; set; }
        public virtual ICollection<CommentGSO> Comments { get; set; }
    }
}