﻿using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class StoreViewModel
    {

    }

    public class RelatedStoreViewModel
    {
        public Store store { get; set; }
        public int relevance { get; set; }
    }
}