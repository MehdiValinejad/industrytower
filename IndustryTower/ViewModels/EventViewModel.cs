using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class RelatedEventViewModel
    {
        public Event Event { get; set; }
        public int relevance { get; set; }
    }
}