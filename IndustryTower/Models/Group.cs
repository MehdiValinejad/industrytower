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
    public class Group
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int groupId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "groupName", ResourceType = typeof(ModelDisplayName))]
        public string groupName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(1500, MinimumLength = 250, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "groupDesc", ResourceType = typeof(ModelDisplayName))]
        public string groupDesc { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "regDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime registerDate { get; set; }

        [DefaultValue(true)]
        [Display(Name = "groupIsPublic", ResourceType = typeof(ModelDisplayName))]
        public bool isPublic { get; set; }

        public virtual ICollection<GroupSession> Sessions { get; set; }
        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

        [InverseProperty("GroupsAdmined")]
        public virtual ICollection<ActiveUser> Admins { get; set; }
        [InverseProperty("GroupsMembered")]
        public virtual ICollection<ActiveUser> Members { get; set; }
    }
}