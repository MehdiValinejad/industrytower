using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class GroupViewModel
    {
    }
    public class GSDetailViewModel
    {
        public int groupid { get; set; }
        public string groupName { get; set; }
        public int sessionid { get; set; }
        public string sessionSubject { get; set; }
        public string sessionDesc { get; set; }
        public string image { get; set; }
        public string doc { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? resultId { get; set; }
        public bool isAdmin { get; set; }
        public bool isMember { get; set; }
        public int Offers { get; set; }
        public int AcceptedOffers { get; set; }
    }

    public class GSOViewModel 
    {
        public int offerid { get; set; }
        public string offer { get; set; }
        public DateTime offerdate { get; set;  }
        public bool isaccepted { get; set; }
        public bool isadmin { get; set; }
        public ActiveUser offerer { get; set; }
        public string result { get; set; }

        public int? Scores { get; set; }
    }
}