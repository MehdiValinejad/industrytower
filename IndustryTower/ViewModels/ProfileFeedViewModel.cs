using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndustryTower.Models;
using PagedList;

namespace IndustryTower.ViewModels
{
    public class ProfileFeedViewModel
    {
        public ActiveUser User { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Profession> Professions { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public IEnumerable<Certificate> Certificates { get; set; }
        public IEnumerable<Event> EventsAttended { get; set; }
    }
    public class ProfileFeed
    {
        public DateTime? date { get; set; }

        public Post Posts { get; set; }
        public Share Shares { get; set; }


    }
}
