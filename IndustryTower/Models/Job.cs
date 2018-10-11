using IndustryTower.Helpers;
using Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Job
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int jobID { get; set; }

        public int jobCoID { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "jobTitle", ResourceType = typeof(ModelDisplayName))]
        public string jobTitle { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "jobDescription", ResourceType = typeof(ModelDisplayName))]
        public string jobDescription { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "state", ResourceType = typeof(ModelDisplayName))]
        public int stateID { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        public long? salary { get; set; }


        [ForeignKey("jobCoID")]
        [Display(Name = "jobCompany", ResourceType = typeof(ModelDisplayName))]
        public virtual Company JobCo { get; set; }

        [ForeignKey("stateID")]
        public virtual CountState State { get; set; }

        [Display(Name = "Jobprofesstions", ResourceType = typeof(ModelDisplayName))]
        public virtual ICollection<Profession> Professtions { get; set; }

        [Display(Name = "JobOffers", ResourceType = typeof(ModelDisplayName))]
        public virtual ICollection<JobOffer> Offers { get; set; }

        //public virtual ICollection<Abuse> Abuses { get; set; }

    }
}