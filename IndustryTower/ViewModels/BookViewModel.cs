using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class BookViewModel
    {
    }

    public class BDetailViewModel
    {
        public int BookId { get; set; }
        public string title { get; set; }
        public string writer { get; set; }

        public string image { get; set; }
        public string file { get; set; }
        public string print { get; set; }

        public string translator { get; set; }
        public string abtrct { get; set; }

        public string UserIds { get; set; }

        public int? Scores { get; set; }
    }

}