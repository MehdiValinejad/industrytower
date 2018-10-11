using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public class WordEdit
    {
        [Key]
        public int editId { get; set; }

        public int wordId { get; set; }
        public int? sugWordId { get; set; }
        public int dicId { get; set; }
        public int editorId { get; set; }

        public string text { get; set; }

        public DateTime date { get; set; }

        [ForeignKey("wordId")]
        [InverseProperty("Edits")]
        public virtual Word Word {get;set;}
        [ForeignKey("sugWordId")]
        [InverseProperty("Editsuggested")]
        public virtual Word SuggestedWord { get; set; }


        [ForeignKey("dicId")]
        public virtual Dict Dictionary { get; set; }

        [ForeignKey("editorId")]
        public virtual ActiveUser Editor { get; set; }
    }
}