using IndustryTower.Helpers;
using Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    
    public class Project
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int projectID { get; set; }

        public int outSourcerID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(200, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "projectTitle", ResourceType = typeof(ModelDisplayName))]
        public string projectTitle { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "projectDescription", ResourceType = typeof(ModelDisplayName))]
        public string projectDescription { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "state", ResourceType = typeof(ModelDisplayName))]
        public int stateID { get; set; }


        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "projectValue", ResourceType = typeof(ModelDisplayName))]
        public long? Value { get; set; }


        [ForeignKey("outSourcerID")]
        public virtual Company OutSourcer { get; set; }

        [ForeignKey("stateID")]
        public virtual CountState State { get; set; }

        public virtual ICollection<Profession> Proffessions { get; set; }
        public virtual ICollection<ProjectOffer> Offers { get; set; }
        //public virtual ICollection<Abuse> Abuses { get; set; }
    }
}