using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Service
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int serviceID { get; set; }
        
        
        public int coID { get; set; }

        [Required]
        public int catID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "serviceName", ResourceType = typeof(ModelDisplayName))]
        public string serviceName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "serviceNameEN", ResourceType = typeof(ModelDisplayName))]
        public string serviceNameEN { get; set; }

        [Display(Name = "cultureServiceName", ResourceType = typeof(ModelDisplayName))]
        public string CultureServiceName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return serviceName;
                else return serviceNameEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "servicebrandName", ResourceType = typeof(ModelDisplayName))]
        public string brandName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "servicebrandNameEN", ResourceType = typeof(ModelDisplayName))]
        public string brandNameEN { get; set; }

        [Display(Name = "cultureBrandName", ResourceType = typeof(ModelDisplayName))]
        public string CultureBrandName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return brandName;
                else return brandNameEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 100, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "aboutService", ResourceType = typeof(ModelDisplayName))]
        public string about { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 100, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "aboutServiceEN", ResourceType = typeof(ModelDisplayName))]
        public string aboutEN { get; set; }

        [Display(Name = "cultureAbout", ResourceType = typeof(ModelDisplayName))]
        public string CultureAbout
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return about;
                else return aboutEN;
            }
        }

        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "serviceCountry", ResourceType = typeof(ModelDisplayName))]
        public int serviceCountryID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [Range(0, 500, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "serviceEra", ResourceType = typeof(ModelDisplayName))]
        public int serviceEra { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "price", ResourceType = typeof(ModelDisplayName))]
        public long? price { get; set; }

        [Range(0, 500, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "afterSale", ResourceType = typeof(ModelDisplayName))]
        public int? afterSale { get; set; }

        public string image { get; set; }
        public string Video { get; set; }
        public string document { get; set; }

        [Display(Name = "regDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime regDate { get; set; }

        [ForeignKey("coID")]
        public virtual Company company { get; set; }

        [ForeignKey("serviceCountryID")]
        public virtual CountState serviceCountry { get; set; }
        [ForeignKey("catID")]
        public virtual ICollection<Category> categories { get; set; }

        public virtual ICollection<CommentService> Comment { get; set; }
        public virtual ICollection<LikeService> Likes { get; set; }
        //public virtual ICollection<Abuse> Abuses { get; set; }

    }
}