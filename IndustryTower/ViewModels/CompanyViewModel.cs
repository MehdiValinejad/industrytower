using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class CompanyViewModel
    {
    }

    public class RelatedCompanyViewModel
    {
        public Company company { get; set; }
        public int relevance { get; set; }
    }
}