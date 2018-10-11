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
    public class GroupSesssionResult
    {
        [Key]
        public int sessionId { get; set; }

        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(3000, MinimumLength = 200, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "sessionResult", ResourceType = typeof(ModelDisplayName))]
        public string SessionResult { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "creationDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime creationDate { get; set; }

        public virtual GroupSession groupSession { get; set; }
    }
}