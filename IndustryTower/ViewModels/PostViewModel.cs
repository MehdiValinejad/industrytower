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
    public class PostViewModel
    {
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(600, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "post", ResourceType = typeof(ModelDisplayName))]
        public string post { get; set; }
        public postVars prams { get; set; }
    }


    [Serializable]
    public class postVars : IValidatableObject
    {
        public int? UId { get; set; }
        public int? CoId { get; set; }
        public int? StId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int?[] receivers = { UId, CoId, StId };
            if (receivers.Count(x => x !=null) != 1)
            {
                yield return new ValidationResult(Resource.ControllerError.ajaxError, new[] { "" });
            }

        }
    }
}