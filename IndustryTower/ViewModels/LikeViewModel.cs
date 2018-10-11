using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public enum LikeType
    {
        LikeQuestion,
        LikeAnswer,
        LikeGSO,
        LikeProduct,
        LikeService,
        LikePost,
        LikeComment,
        LikeBook,
        LikeReviewBook
    }
    [Serializable]
    public class LikeViewModel
    {
        public IList<int> likerIds { get; set; }

        public likeVars prams { get; set; }
    }

    //[Serializable]
    //public class likeVars : IValidatableObject
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
    //         int?[] check = { PId, CmtId, QId, AId, PrId, SrId };
    //        var hasval = check.Count(x=>x.HasValue);
    //        if (hasval != 1 )
    //        {
    //            yield return new ValidationResult(Resource.ControllerError.ajaxError, new[] { "" });
    //        }
    //    }
    //}



    [Serializable]
    public class likeVars 
    {
        public int elemId { get; set; }
        public LikeType typ { get; set; }
    }
}