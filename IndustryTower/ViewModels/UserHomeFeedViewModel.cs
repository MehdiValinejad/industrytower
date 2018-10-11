using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndustryTower.Models;

namespace IndustryTower.ViewModels
{
    public class UserHomeFeedViewModel
    {
        public DateTime Date { get; set; }

        public Post post { get; set; }
        public Share share { get; set; }
        //public Like like { get; set; }
        //public Comment comment { get; set; }
        public Question question { get; set; }
        public Answer answer { get; set; }
        public Experience experience { get; set; }
        public Certificate certificate { get; set; }

        public Product CoProduct { get; set; }
        public Service CoService { get; set; }

    }

    public class HomeFeedWebinarsClassified
    {
        public IEnumerable<Seminar> attending { get; set; }
        public IEnumerable<Seminar> Now { get; set; }
    }

    public class HomeFeedEvents 
    {
        public Event Event { get; set; }
        public int relevance { get; set; }
    }


    public class HomeFeedQuestionsClassified
    {
        public IEnumerable<HomeFeedQuestions> answered { get; set; }
        public IEnumerable<HomeFeedQuestions> unAnswered { get; set; }
        public IEnumerable<HomeFeedQuestions> old { get; set; }
    }

    public class HomeFeedQuestions
    {
        public Question Question { get; set; }
        public int relevance { get; set; }
    }
}