using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class ReviewBookViewModel
    {
        public Book book { get; set; }
        public ReviewBook review { get; set; }
        public int? Scores { get; set; }
    }

    public class ReviewBookUpsertModel
    {
        public Book book { get; set; }
        public string rev { get; set; }

        public revBookVars vars { get; set; }
    }

    [Serializable]
    public class revBookVars
    {
        public int bid { get; set; }

    }
}