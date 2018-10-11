using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class Word
    {
        [Key]
        public int wordId { get; set; }
        public lang lang { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "word", ResourceType = typeof(ModelDisplayName))]
        public string word { get; set; }

        public int creatorId { get; set; }
        public DateTime date { get; set; }

        //public virtual ICollection<Dict> Dictionaries { get; set; }

        public virtual ICollection<Word> EnglisheList { get; set; }
        public virtual ICollection<Word> NotEnglishList { get; set; }

        public virtual ICollection<WordDesc> Descs { get; set; }


        [ForeignKey("creatorId")]
        public virtual ActiveUser Creator { get; set; }

        [InverseProperty("Word")]
        public virtual ICollection<WordEdit> Edits { get; set; }
        [InverseProperty("SuggestedWord")]
        public virtual ICollection<WordEdit> Editsuggested { get; set; }
    }
}