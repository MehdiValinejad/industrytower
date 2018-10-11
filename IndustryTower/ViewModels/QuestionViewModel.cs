using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class RelatedQuestionViewModel
    {
        public Question question { get; set; }
        public int relevance { get; set; }
    }

    
    public class QDetailViewModel
    {
        public int questionID { get; set; }
        public string questionSubject { get; set; }
        public string questionBody { get; set; }
        public lang language { get; set; }
        public string image { get; set; }
        public string docuoment { get; set; }
        public DateTime questionDate { get; set; }

        public int questionerID { get; set; }
        public string senderName { get; set; }
        public string senderImage { get; set; }

        public int? Answers { get; set; }
        public int? Scores { get; set; }
    }

}