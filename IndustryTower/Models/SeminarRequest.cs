using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class SeminarRequest
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int requestId { get; set; }
        public DateTime reqDate { get; set; }
        public int seminarId { get; set; }

        public virtual ICollection<ActiveUser> user { get; set; }
        [ForeignKey("seminarId")]
        public virtual Seminar Seminar { get; set; }
    }
}