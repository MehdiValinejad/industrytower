using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndustryTower.Models;


namespace IndustryTower.ViewModels
{
    public class ShareViewModel
    {
        public Post ToShare { get; set; }
        public Share Share { get; set; }
    }
}