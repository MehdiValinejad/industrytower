using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class LikeReviewBook
    {
        [Key]
        public int likeID { get; set; }

        public int UID { get; set; }

        public int elemID { get; set; }

        public DateTime date { get; set; }

        [ForeignKey("UID")]
        public virtual ActiveUser LikerUser { get; set; }

        [ForeignKey("elemID")]
        public virtual ReviewBook Review { get; set; }
    }
}