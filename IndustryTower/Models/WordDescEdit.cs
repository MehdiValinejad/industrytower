using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class WordDescEdit
    {
        [Key]
        public int editId { get; set; }

        public int wdescId { get; set; }
        public int editorId { get; set; }

        public string text { get; set; }
        public DateTime date { get; set; }

        [ForeignKey("wdescId")]
        public virtual WordDesc WordDesc { get; set; }

        [ForeignKey("editorId")]
        public virtual ActiveUser Editor { get; set; }

    }
}