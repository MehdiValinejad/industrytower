using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Event : IValidatableObject
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int eventID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "eventSubj", ResourceType = typeof(ModelDisplayName))]
        public string eventSubj { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "eventSubjEN", ResourceType = typeof(ModelDisplayName))]
        public string eventSubjEN { get; set; }

        [Display(Name = "cultureEventSubj", ResourceType = typeof(ModelDisplayName))]
        public string CultureEventSubj
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return eventSubj;
                else return eventSubjEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "description", ResourceType = typeof(ModelDisplayName))]
        public string description { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "descriptionEN", ResourceType = typeof(ModelDisplayName))]
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
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "address", ResourceType = typeof(ModelDisplayName))]
        public string address { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "addressEN", ResourceType = typeof(ModelDisplayName))]
        public string addressEN { get; set; }


        [Display(Name = "cultureAddress", ResourceType = typeof(ModelDisplayName))]
        public string CultureAddress
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return address;
                else return addressEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "eventDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime eventDate { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "eventUntilDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime untilDate { get; set; }

        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "createDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime createDate { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "state", ResourceType = typeof(ModelDisplayName))]
        public int stateID { get; set; }
        public int creatorUserID { get; set; }


        [ForeignKey("stateID")]
        public virtual CountState State { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

        [InverseProperty("EventsAttended")]
        public virtual ICollection<ActiveUser> Attendors { get; set; }
        [InverseProperty("EventsCreated")]
        [ForeignKey("creatorUserID")]
        public virtual ActiveUser Creator { get; set; }

        public virtual ICollection<Company> AttendorCompanies { get; set; }
        public virtual ICollection<Store> AttendorStores { get; set; }
        //public virtual ICollection<Abuse> Abuses { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (eventDate > untilDate)
            {
                yield return new ValidationResult(Resource.ModelValidation.secondDateMustbeLater, new[] { "eventDate" });
            }

        }

    }
}