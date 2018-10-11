using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IndustryTower.Models
{
    public class Answer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int answerID { get; set; }

        public int answererID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(700, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "answerBody", ResourceType = typeof(ModelDisplayName))]
        public string answerBody { get; set; }

        public int questionID { get; set; }

        [DefaultValue(false)]
        [Display(Name = "markedAsAnswer", ResourceType = typeof(ModelDisplayName))]
        public bool accept { get; set; }

        public DateTime answerDate { get; set; }

        
        [ForeignKey("questionID")]
        public virtual Question Question { get; set; }

        [ForeignKey("answererID")]
        public virtual ActiveUser Answerer { get; set; }

        public virtual ICollection<CommentAnswer> Comments { get; set; }
        public virtual ICollection<LikeAnswer> Likes { get; set; }
        //public virtual ICollection<Abuse> Abuses { get; set; }

    }
}