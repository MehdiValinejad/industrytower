﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace IndustryTower.Helpers
{
    public static class JsonHelper
    {
            public static string ToJSON(this object obj)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(obj);
            }

            public static string ToJSON(this object obj, int recursionDepth)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.RecursionLimit = recursionDepth;
                return serializer.Serialize(obj);
            }
    }
}