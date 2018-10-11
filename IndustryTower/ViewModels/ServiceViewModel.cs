using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class ServiceViewModel
    {
    }

    public class ServiceSideBarViewModel
    {
        public IEnumerable<Service> RelatedServices { get; set; }
        public IEnumerable<Service> SimilarServices { get; set; }
    }

    public class RelatedServiceViewModel
    {
        public Service service { get; set; }
        public int relevance { get; set; }
    }
}