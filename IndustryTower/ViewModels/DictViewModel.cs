using IndustryTower.Helpers;
using IndustryTower.Models;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class DictViewModel
    {
    }

    public class DictVars
    { 

    }


    public class WordViewModel
    {
        public Word word { get; set; }
        public IList<Word> Translates { get; set; }
        public IList<WordDesc> Descs { get; set; }
    }

    public class wordByLetterVM
    {
        public int id { get; set; }
        public lang lang { get; set; }
        public string translate { get; set; }
        public int descId { get; set; }
        public string desc { get; set; }
    }

    public class WordVars
    {
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        public lang lang { get; set; }
        public int? WId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "word", ResourceType = typeof(ModelDisplayName))]
        public string word { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "wordDesc", ResourceType = typeof(ModelDisplayName))]
        public string wordDesc { get; set; }


        public DicInfo dicInfo { get; set; }

    }

    [Serializable]
    public class WordEditVars
    {
        public lang lang { get; set; }
        public int? WId { get; set; }
        public int DId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "word", ResourceType = typeof(ModelDisplayName))]
        public string word { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "word", ResourceType = typeof(ModelDisplayName))]
        public string edited { get; set; }

        public int mainWId { get; set; }

        public DicInfo dicInfo { get; set; }

        public List<WordEditwithScore> wordEditgs { get; set; }


    }

    public class WordEditwithScore
    {
        public int editId { get; set; }
        public int editorId { get; set; }
        public int? sugWordId { get; set; }
        public string text { get; set; }
        public DateTime date { get; set; }


        public string editorName { get; set; }
        public string editorIMage { get; set; }
        public int? Score { get; set; }
    }




    public class WordTranslateVars
    {
        public lang lang { get; set; }
        public int? WId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "word", ResourceType = typeof(ModelDisplayName))]
        public string word { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(500, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "wordDesc", ResourceType = typeof(ModelDisplayName))]
        public string wordDesc { get; set; }

        public Word mainWord { get; set; }
        public DicInfo dicInfo { get; set; }

    }

    [Serializable]
    public class DicInfo
    {
        
        public string name { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        public int id { get; set; }

        public int mainWid { get;set;}
    }


    public class WordDescEditVM
    {
        public int wordId { get; set; }
        public string word { get; set; }

        public string desc { get; set; }
        public lang lang { get; set; }
        public DicInfo dicinfo { get; set; }

        public IList<WordDescEditWithScore> DescEdits { get; set; }
    }

    public class WordDescEditWithScore
    {
        public int editId { get; set; }
        public string text { get; set; }
        public int editorId { get; set; }
        public DateTime date { get; set; }

        public string senderName { get; set; }
        public string senderImage { get; set; }
        public int? Score { get; set; }
    }

    [Serializable]
    public class WordDescVars
    {
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        public int WdId { get; set; }
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        public int WId { get; set; }
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        public int DId { get; set; }
    }




    public class UserWordsViewModel
    {
        public int typ { get; set; }
        public int wordId { get; set; }
        public int? dicId { get; set; }
        public string text { get; set; }

    }



}