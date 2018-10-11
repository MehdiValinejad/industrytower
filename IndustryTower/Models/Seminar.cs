using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public enum SemType 
    {
        Webinar = 0,
        Workshop = 1,
        VideoConference = 2
    }
    

    public class Seminar
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int seminarId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "semTitle", ResourceType = typeof(ModelDisplayName))]
        public string title { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(1500, MinimumLength = 250, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "semDesc", ResourceType = typeof(ModelDisplayName))]
        public string desc { get; set; }
        public string files { get; set; }
        public Guid token { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "semDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime date { get; set; }

        [Range(0, 180, ErrorMessageResourceName = "semDuration", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "semDuraion", ResourceType = typeof(ModelDisplayName))]
        public Int16 duration { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, 250, ErrorMessageResourceName = "semMaxAudience", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "semMaxAudience", ResourceType = typeof(ModelDisplayName))]
        public Int16 maxAudiences { get; set; }

        [DefaultValue(true)]
        [Display(Name = "semIsPublic", ResourceType = typeof(ModelDisplayName))]
        public bool isPublic { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "semPrice", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "semPrice", ResourceType = typeof(ModelDisplayName))]
        public int price { get; set; }

        public int moderatorId { get; set; }


        [InverseProperty("SeminarsModerate")]
        [ForeignKey("moderatorId")]
        public virtual ActiveUser Moderator { get; set; }
        [InverseProperty("SeminarsBroadcast")]
        public virtual ICollection<ActiveUser> Broadcasters { get; set; }
        [InverseProperty("SeminarsAudience")]
        public virtual ICollection<ActiveUser> Audiences { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<SeminarRequest> Requests { get; set; }
    }

    public class Webinar : Seminar
    { 

    }

    public class Workshop : Seminar
    {

    }

    public class VideoConference : Seminar
    {

    }
}