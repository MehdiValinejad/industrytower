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
    public class GroupSession
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int sessionId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "sessionSubject", ResourceType = typeof(ModelDisplayName))]
        public string sessionSubject { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(1500, MinimumLength = 250, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "sessionDesc", ResourceType = typeof(ModelDisplayName))]
        public string sessionDesc { get; set; }

        public string image { get; set; }
        public string document { get; set; }
        public string video { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "sessionStartDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime startDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "sessionEndDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime? endDate { get; set; }


        public int groupId { get; set; }


        [ForeignKey("groupId")]
        public virtual Group Group { get; set; }

        public virtual GroupSesssionResult Result { get; set; }

        public virtual ICollection<GroupSessionOffer> Offers { get; set; }
    }
}