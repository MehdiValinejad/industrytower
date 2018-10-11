using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Experience : IValidatableObject
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int experienceID { get; set; }

        public int userID { get; set; }

        public int? CoId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "coName", ResourceType = typeof(ModelDisplayName))]
        public string coName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "coNameEN", ResourceType = typeof(ModelDisplayName))]
        public string coNameEN { get; set; }

        [Display(Name = "cultureCoName", ResourceType = typeof(ModelDisplayName))]
        public string CultureCoName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return coName;
                else return coNameEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "experienceTitle", ResourceType = typeof(ModelDisplayName))]
        public string title { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "experienceTitleEN", ResourceType = typeof(ModelDisplayName))]
        public string titleEN { get; set; }

        [Display(Name = "cultureTitle", ResourceType = typeof(ModelDisplayName))]
        public string CultureTitle
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return title;
                else return titleEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(350, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "experienceDescription", ResourceType = typeof(ModelDisplayName))]
        public string description { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(350, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "experienceDescriptionEN", ResourceType = typeof(ModelDisplayName))]
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

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "expStartDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime attendDate { get; set; }

        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "expUntilDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime? untilDate { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "experienceState", ResourceType = typeof(ModelDisplayName))]
        public int stateID { get; set; }


        [ForeignKey("CoId")]
        public virtual Company Company { get; set; }

        [ForeignKey("stateID")]
        public virtual CountState State { get; set; }

        [ForeignKey("userID")]
        public virtual ActiveUser User { get; set; }




        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (attendDate > untilDate)
            {
                yield return new ValidationResult(Resource.ModelValidation.secondDateMustbeLater, new[] { "attendDate" });
            }
        }
    }
}