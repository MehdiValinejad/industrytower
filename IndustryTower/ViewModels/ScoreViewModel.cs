using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class ScoreViewModel
    {
    }


    [Serializable]
    public class ScoreVars
    {
        public ScoreType type { get; set; }
        public int elemId { get; set; }
        public Int16 sign { get; set; }
        public int count { get; set; }
    }
}