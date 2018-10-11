using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class Dict
    {
        [Key]
        public int dicId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(60, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "dicName", ResourceType = typeof(ModelDisplayName))]
        public string name { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "dicDesc", ResourceType = typeof(ModelDisplayName))]
        public string desc { get; set; }
        public DateTime date { get; set; }

        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<WordEdit> WordEdits { get; set; }
        public virtual ICollection<WordDesc> WordDescs { get; set; }
        public virtual ICollection<ActiveUser> Admins { get; set; }
        
    }
}