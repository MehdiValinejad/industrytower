using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class BadgeUser
    {
        [Key]
        public int bgUsId {get;set;}

        public int bdgId { get; set; }

        public int usrId { get; set; }

        public DateTime date { get; set; }

        [ForeignKey("usrId")]
        public virtual ActiveUser User { get; set; }

        [ForeignKey("bdgId")]
        public virtual Badge Badge { get; set; }
    }
}