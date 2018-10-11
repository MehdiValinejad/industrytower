using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IndustryTower.Models
{
    public enum ScoreType
    { 
        Qvote = 0,
        Avote = 1,
        GSOvote = 2,
        Aacc = 3,
        GSOacc= 4,
        WEditvote = 5,
        WDEditvote = 6,
        Bdg = 7,
        BCreate = 8,
        BReview = 9
    }
    public class Score
    {
        [Key]
        public long ScId { get; set; }

        [Index]
        public ScoreType type { get; set; }

        [Index]
        public int elemId { get; set; }

        public int userId { get; set; }

        public int granterId { get; set; }

        public Int16 sign { get; set; }

        [ForeignKey("userId")]
        [InverseProperty("Scores")]
        public ActiveUser User { get; set; }

        [ForeignKey("granterId")]
        [InverseProperty("ScoresSent")]
        public ActiveUser Granter { get; set; }
    }
}