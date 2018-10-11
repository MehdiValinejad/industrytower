using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class Logins
    {
        [Key]
        public int loginId { get; set; }
        public int userId { get; set; }
        public string ip { get; set; }
        public DateTime date { get; set; }


        [ForeignKey("userId")]
        public ActiveUser User { get; set; }
    }
}