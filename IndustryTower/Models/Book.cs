using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "bookTitle", ResourceType = typeof(ModelDisplayName))]
        public string title { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(7000, MinimumLength = 250, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "bookAbstract", ResourceType = typeof(ModelDisplayName))]
        public string abtrct { get; set; }

        public string file { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "bookPrint", ResourceType = typeof(ModelDisplayName))]
        public string print { get; set; }

        public string image { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "bookWriter", ResourceType = typeof(ModelDisplayName))]
        public string writer { get; set; }


        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "bookTranslator", ResourceType = typeof(ModelDisplayName))]
        public string translator { get; set; }

        public DateTime date { get; set; }


        public virtual ICollection<ActiveUser> Users { get; set; }
        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<ReviewBook> Reviews { get; set; }
        public virtual ICollection<LikeBook> Likes { get; set; }
    }
}