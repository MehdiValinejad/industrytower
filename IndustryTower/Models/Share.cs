using Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace IndustryTower.Models
{
    [Bind(Include = "shareNote,sharedPostID")]
    public class Share
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int shareID { get; set; }

        [StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "shareNote", ResourceType = typeof(ModelDisplayName))]
        public string shareNote { get; set; }

        public int? sharedPostID { get; set; }

        public int SharerUserID { get; set; }

        public DateTime insertdate { get; set; }

        [ForeignKey("sharedPostID")]
        public virtual Post sharedPost { get; set; }

        [ForeignKey("SharerUserID")]
        public virtual ActiveUser sharerUser { get; set; }
    }
}