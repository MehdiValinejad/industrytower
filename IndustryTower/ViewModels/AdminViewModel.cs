using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class AdminViewModel
    {
        public int Users { get; set; }
        public int Posts { get; set; }
        public int ExpiredCompanies { get; set; }
        public int NotExpiredCompanies { get; set; }
        public int ExpiredStores { get; set; }
        public int NotExpiredStores { get; set; }
        public int Products { get; set; }
        public int Services { get; set; }
        public int UpcomingEvents { get; set; }
        public int ExpiredEvents { get; set; }
        public int Questions { get; set; }
    }
}