using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class BadgeViewModel
    {
    }

    public class UserBadgesViewModel
    {
        public int bdgId { get; set; }
        public string name { get; set; }
        public BadgeColor color { get; set; }
        public int count { get; set; }
    }
}