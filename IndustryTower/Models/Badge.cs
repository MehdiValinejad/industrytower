using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public enum BadgeColor
    {
        Gold,
        Silver,
        Bronze
    }
    public class Badge
    {
        [Key]
        public int badgeId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "badgeColor", ResourceType = typeof(ModelDisplayName))]
        public BadgeColor color { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(30, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "badgeName", ResourceType = typeof(ModelDisplayName))]
        public string name { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(30, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "badgeNameEN", ResourceType = typeof(ModelDisplayName))]
        public string nameEN { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "badgeDesc", ResourceType = typeof(ModelDisplayName))]
        public string desc { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "badgeDescEN", ResourceType = typeof(ModelDisplayName))]
        public string descEN { get; set; }


        public string CultureBgName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return name;
                else return nameEN;
            }
        }


        public virtual ICollection<BadgeUser> Users { get; set; }
    }
}