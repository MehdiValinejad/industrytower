using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class CommentAnswer
    {
        [Key]
        public int cmtID { get; set; }

        public int UID { get; set; }

        public int elemID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(400, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "comment", ResourceType = typeof(ModelDisplayName))]
        public string comment { get; set; }
        public DateTime date { get; set; }

        [ForeignKey("UID")]
        public virtual ActiveUser CommenterUser { get; set; }

        [ForeignKey("elemID")]
        public virtual Answer Answer { get; set; }

    }
}