using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class MessageViewModel
    {
        public Company company { get; set; }
        public Store store { get; set; }
        public ActiveUser user { get; set; }
        public IEnumerable<Message> message { get; set; }
    }
}