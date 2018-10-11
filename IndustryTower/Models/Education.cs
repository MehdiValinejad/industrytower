using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public enum EducationDegree
    {
        Associate, Bachelor, Master, Doctorate
    }
    public class Education : IValidatableObject
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int educationID { get; set; }

        public int userID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "school", ResourceType = typeof(ModelDisplayName))]
        public string school { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "schoolEN", ResourceType = typeof(ModelDisplayName))]
        public string schoolEN { get; set; }

        [Display(Name = "cultureSchool", ResourceType = typeof(ModelDisplayName))]
        public string CultureSchool
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return school;
                else return schoolEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "educationDegree", ResourceType = typeof(ModelDisplayName))]
        public EducationDegree degree { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "fieldOfStudy", ResourceType = typeof(ModelDisplayName))]
        public string fieldOfStudy { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "fieldOfStudyEN", ResourceType = typeof(ModelDisplayName))]
        public string fieldOfStudyEN { get; set; }

        [Display(Name = "culturefieldOfStudy", ResourceType = typeof(ModelDisplayName))]
        public string CulturefieldOfStudy
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return fieldOfStudy;
                else return fieldOfStudy;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "eduStartDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime attendDate { get; set; }

        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "eduUntilDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime? untilDate { get; set; }
        

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