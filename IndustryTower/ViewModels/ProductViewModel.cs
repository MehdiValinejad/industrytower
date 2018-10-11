using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class ProductViewModel
    {
    }

    public class ProductSideBarViewModel
    {
        public IEnumerable<Product> RelatedProducts { get; set; }
        public IEnumerable<Product> SimilarProducts { get; set; }
    }


    public class RelatedProductViewModel
    {
        public Product product { get; set; }
        public int relevance { get; set; }
    }


    
}