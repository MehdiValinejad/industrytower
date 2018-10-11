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
    public class WordDesc
    {
        [Key]
        public int descId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "wordDesc", ResourceType = typeof(ModelDisplayName))]
        public string desc { get; set; }

        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "wordDesc", ResourceType = typeof(ModelDisplayName))]
        public string edited { get; set; }


        public int creatorId { get; set; }
        public int dicId { get; set; }
        public int wordId { get; set; }
        public DateTime date { get; set; }

        [ForeignKey("dicId")]
        public virtual Dict Dict { get; set; }

        [ForeignKey("wordId")]
        public virtual Word Word { get; set; }

        [ForeignKey("creatorId")]
        public virtual ActiveUser Creator { get; set; }

        public virtual ICollection<WordDescEdit> Edits { get; set; }

    }
}