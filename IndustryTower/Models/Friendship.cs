using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Friendship
    {
        [Key]
        public int frendshipId { get; set; }

        public int userID { get; set; }
        public int friendID { get; set; }

        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        public DateTime friendshipDate { get; set; }

        [InverseProperty("Friends")]
        [ForeignKey("userID")]
        public virtual ActiveUser User { get; set; }

        [InverseProperty("FriendsImIn")]
        [ForeignKey("friendID")]
        public virtual ActiveUser Friend { get; set; }

    }
}