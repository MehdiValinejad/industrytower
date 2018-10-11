using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Following
    {
        [Key]
        public int followID { get; set; }

        public int? followerUserID { get;set;}

        public int? followedCoID { get; set; }

        public int? followedStoreID { get; set; }

        public DateTime followDate { get; set; }

        [ForeignKey("followerUserID")]
        public virtual ActiveUser FollowerUser { get; set; }
        [ForeignKey("followedCoID")]
        public virtual Company FollowedCompany { get; set; }
        [ForeignKey("followedStoreID")]
        public virtual Store FollowedStore { get; set; }

    }
}