using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndustryTower.Models;
using System.ComponentModel.DataAnnotations;
using IndustryTower.Helpers;
using Resource;


namespace IndustryTower.ViewModels
{


    public enum CommentType
    {
        CommentQuestion,
        CommentAnswer,
        CommentGSO,
        CommentProduct,
        CommentService,
        CommentPost
    }
    public class CommentViewModel
    {
        public IEnumerable<CommentEach> Comments { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(400, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "comment", ResourceType = typeof(ModelDisplayName))]
        public string comment { get; set; }


        public commentVars prams { get; set; }
    }

    public class CommentEach
    {
        public int cmtID { get; set; }
        public int UID { get; set; }
        public int elemID { get; set; }
        public string comment { get; set; }
        public DateTime date { get; set; }
        public ActiveUser CommenterUser { get; set; }
        public int likes { get; set; }

    }

    [Serializable]
    public class commentVars 
    {
        public int elemId { get; set; }

        public CommentType typ { get; set; }
    }

    
    //public class commentVars : IValidatableObject
    //{
    //    public int? PId { get; set; }
    //    public int? CmtId { get; set; }
    //    public int? QId { get; set; }
    //    public int? AId { get; set; }
    //    public int? PrId { get; set; }
    //    public int? SrId { get; set; }
    //    public int? GSOId { get; set; }

    //    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //    {
    //        int?[] check = { PId, CmtId, QId, AId, PrId, SrId };
    //        var hasval = check.Count(x => x.HasValue);
    //        if (hasval != 1)
    //        {
    //            yield return new ValidationResult(Resource.ControllerError.ajaxError, new[] { "" });
    //        }
    //    }
    //}

}