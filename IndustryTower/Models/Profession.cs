using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IndustryTower.Models
{
    public class Profession
    {
        [Key]
        [ScaffoldColumn(false)]
        public int profID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "professionName", ResourceType = typeof(ModelDisplayName))]
        public string professionName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "professionNameEN", ResourceType = typeof(ModelDisplayName))]
        public string professionNameEN { get; set; }

        [Display(Name = "cultureProfessionName", ResourceType = typeof(ModelDisplayName))]
        public string CultureProfessionName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return professionName;
                else return professionNameEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "professionDescription", ResourceType = typeof(ModelDisplayName))]
        public string professionDescription { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "professionDescriptionEN", ResourceType = typeof(ModelDisplayName))]
        public string professionDescriptionEN { get; set; }

        [Display(Name = "cultureProfessionDesc", ResourceType = typeof(ModelDisplayName))]
        public string CultureProfessionDesc
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return professionDescription;
                else return professionDescriptionEN;
            }
        }

        public virtual ICollection<Category> categories { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<ActiveUser> users { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<Seminar> Seminars { get; set; }
        public virtual ICollection<Dict> Dictionaries { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}