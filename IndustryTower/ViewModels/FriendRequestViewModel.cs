using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class FriendRequestViewModel
    {
        public string user { get; set; }

        public string request { get; set; }

        [StringLength(200, MinimumLength = 0, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "message", ResourceType = typeof(ModelDisplayName))]
        public string message { get; set; }
    }
}