using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class FollowViewModel
    {
        public int Followers { get; set; }

        public int? CoId { get; set; }

        public int? storeId { get; set; }

        public bool followedByUser { get; set; }
    }
}