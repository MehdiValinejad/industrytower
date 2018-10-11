using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.Helpers
{
    public class WbinarSecret
    {
        public int secretId(int id)
        {
            int secret = (id * 180 + 246) * 7;
            
            return secret;
        }
    }
}