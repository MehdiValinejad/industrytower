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
    public class ReviewBook
    {
        [Key]
        public int revId { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(2000, MinimumLength = 250, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "bookAbstract", ResourceType = typeof(ModelDisplayName))]
        public string review { get; set; }

        public int userId { get; set; }
        public int bookId { get; set; }

        public DateTime date { get; set; }

        [ForeignKey("userId")]
        public virtual ActiveUser ActiveUser { get; set; }

        [ForeignKey("bookId")]
        public virtual Book Book { get; set; }

        public virtual ICollection<LikeReviewBook> Likes { get; set; }
    }
}