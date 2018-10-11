using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public enum PorT 
    { 
        Production, Distribution
    }
    public class Product
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int productID { get; set; }

        public int coID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "productName", ResourceType = typeof(ModelDisplayName))]
        public string productName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "productNameEN", ResourceType = typeof(ModelDisplayName))]
        public string productNameEN { get; set; }

        [Display(Name = "cultureProductName", ResourceType = typeof(ModelDisplayName))]
        public string CultureProductName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return productName;
                else return productNameEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "brandName", ResourceType = typeof(ModelDisplayName))]
        public string brandName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "brandNameEN", ResourceType = typeof(ModelDisplayName))]
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
        [Display(Name = "aboutProduct", ResourceType = typeof(ModelDisplayName))]
        public string about { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 100, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "aboutProductEN", ResourceType = typeof(ModelDisplayName))]
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

        [Required(ErrorMessageResourceName = "YouMustChoose", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "PorT", ResourceType = typeof(ModelDisplayName))]
        public PorT PorT { get; set; }

        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "productionCountry", ResourceType = typeof(ModelDisplayName))]
        public int productionCountryID { get; set; }

        [Range(0, float.MaxValue, ErrorMessageResourceName = "tooLongFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression(@"[+]?(\d*[.])?\d{0,3}", ErrorMessageResourceName = "invalidFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "weight", ResourceType = typeof(ModelDisplayName))]
        public float? weight { get; set; }

        [Range(0, float.MaxValue, ErrorMessageResourceName = "tooLongFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression(@"[+]?(\d*[.])?\d{0,3}", ErrorMessageResourceName = "invalidFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "height", ResourceType = typeof(ModelDisplayName))]
        public float? height { get; set; }

        [Range(0, float.MaxValue, ErrorMessageResourceName = "tooLongFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression(@"[+]?(\d*[.])?\d{0,3}", ErrorMessageResourceName = "invalidFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "width", ResourceType = typeof(ModelDisplayName))]
        public float? width { get; set; }

        [Range(0, float.MaxValue, ErrorMessageResourceName = "tooLongFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression(@"[+]?(\d*[.])?\d{0,3}", ErrorMessageResourceName = "invalidFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "length", ResourceType = typeof(ModelDisplayName))]
        public float? length { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "power", ResourceType = typeof(ModelDisplayName))]
        public int? power { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "shabnam", ResourceType = typeof(ModelDisplayName))]
        public long? shabnam { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "iranCode", ResourceType = typeof(ModelDisplayName))]
        public long? iranCode { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "packCount", ResourceType = typeof(ModelDisplayName))]
        public int? packCount { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "price", ResourceType = typeof(ModelDisplayName))]
        public long? price { get; set; }

        [DefaultValue(false)]
        [Display(Name = "shipFree", ResourceType = typeof(ModelDisplayName))]
        public bool shipFree { get; set; }

        [Range(0, 200, ErrorMessageResourceName = "wuaranteeMonths", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "wuarantee", ResourceType = typeof(ModelDisplayName))]
        public int? wuarantee { get; set; }

        [Range(0, 200, ErrorMessageResourceName = "wuaranteeMonths", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "guarantee", ResourceType = typeof(ModelDisplayName))]
        public int? guarantee { get; set; }

        [Range(0, 500, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "afterSale", ResourceType = typeof(ModelDisplayName))]
        public int? afterSale { get; set; }

        public string image { get; set; }
        public string Video { get; set; }
        public string document { get; set; }

        [Display(Name = "regDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime regDate { get; set; }
        
        [ForeignKey("coID")]
        public virtual Company company { get; set; }
        [ForeignKey("productionCountryID")]
        public virtual CountState productionCountry { get; set; }

        public virtual ICollection<Category> categories { get; set; }
        
        public virtual ICollection<CommentProduct> Comment { get; set; }
        public virtual ICollection<LikeProduct> Likes { get; set; }
        //public virtual ICollection<Abuse> Abuses { get; set; }


    }

}