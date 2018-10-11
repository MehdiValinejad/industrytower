using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public enum  PatentStatus
    {
        Issued, Pending
    }
    public class Patent
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int patentID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "patentOfficeCountry", ResourceType = typeof(ModelDisplayName))]
        public int officeStateID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "patentNo", ResourceType = typeof(ModelDisplayName))]
        public long patentNo { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "patentTitle", ResourceType = typeof(ModelDisplayName))]
        public string patentTitle { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "patentTitleEN", ResourceType = typeof(ModelDisplayName))]
        public string patentTitleEN { get; set; }

        [Display(Name = "culturePatentTitle", ResourceType = typeof(ModelDisplayName))]
        public string CulturePatentTitle
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return patentTitle;
                else return patentTitleEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "PatentStatus", ResourceType = typeof(ModelDisplayName))]
        public PatentStatus status { get; set; }

        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "patentissueDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime issueDate { get; set; }

        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.URL), "URLCheck")]
        [RegularExpression(@"^(https?|ftp)://(-\.)?([^\s/?\.#-]+\.?)+(/[^\s]*)?$", ErrorMessageResourceName = "urlField", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "patentURL", ResourceType = typeof(ModelDisplayName))]
        public string patentURL { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(1500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "patentDescription", ResourceType = typeof(ModelDisplayName))]
        public string description { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(1500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "patentDescriptionEN", ResourceType = typeof(ModelDisplayName))]
        public string descriptionEN { get; set; }

        [Display(Name = "cultureDescription", ResourceType = typeof(ModelDisplayName))]
        public string CultureDescription
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return description;
                else return descriptionEN;
            }
        }


        public virtual ICollection<ActiveUser> Inventors { get; set; }
        [ForeignKey("officeStateID")]
        public virtual CountState OfficeState { get; set; }

    }
}