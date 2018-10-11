using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndustryTower.Models;


namespace IndustryTower.ViewModels
{
    public class UserQuestionsViewmodel
    {
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Question> QuestionsAnswered { get; set; }
    }
}