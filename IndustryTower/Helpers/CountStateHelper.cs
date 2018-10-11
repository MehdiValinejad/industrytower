using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    static public class CountStateHelper
    {
        static public string FullState(this HtmlHelper helper, int StateId)
        {
            UnitOfWork uni = new UnitOfWork();
            List<SqlParameter> prams = new List<SqlParameter>();
            prams.Add(new SqlParameter("StId", StateId));
            var reader = uni.ReaderRepository.GetSPDataReader("CountStateFull", prams);

            
            while (reader.Read())
            {
                if (ITTConfig.CurrentCultureIsNotEN)
                {
                    return reader.GetString(0) + Resource.Resource.camma + reader.GetString(3);
                }
                else return reader.GetString(1) + Resource.Resource.camma + reader.GetString(4);
            }
            return String.Empty;
        }

    }
}