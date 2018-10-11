using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public enum lang
    { 
        en, fa
    }
    public class Question
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int questionID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(120, MinimumLength = 20, ErrorMessageResourceName = "numberBeetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "questionSubject", ResourceType = typeof(ModelDisplayName))]
        public string questionSubject { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(600, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "questionBody", ResourceType = typeof(ModelDisplayName))]
        public string questionBody { get; set; }

        [Required(ErrorMessageResourceName = "YouMustChoose", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "language", ResourceType = typeof(ModelDisplayName))]
        public lang language { get; set; }

        public string image { get; set; }
        public string video { get; set; }
        public string docuoment { get; set; }

        public int questionerID { get; set; }

        public DateTime questionDate { get; set; }

        [ForeignKey("questionerID")]
        public virtual ActiveUser Questioner { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        
        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<CommentQuestion> Comments { get; set; }
        public virtual ICollection<LikeQuestion> Likes { get; set; }
        //public virtual ICollection<Abuse> Abuses { get; set; }

    }
}