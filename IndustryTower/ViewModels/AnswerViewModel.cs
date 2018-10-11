using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class AnswerViewModel
    {
        public int answerID { get; set; }
        public int answererID { get; set; }
        public string answerBody { get; set; }
        public int questionID { get; set; }
        public bool accept { get; set; }
        public DateTime answerDate { get; set; }
        public int questionerID { get; set; }
        public int? Scores { get; set; }
        public string senderName { get; set; }
        public string senderImage { get; set; }
    }

    public class UserAnswerViewModel
    {
        public int qId { get; set; }
        public string answer { get; set; }
        public string qsubj { get; set; }
    }
}