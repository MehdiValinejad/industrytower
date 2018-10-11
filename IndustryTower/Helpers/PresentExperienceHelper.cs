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
    static public class PresentExperienceHelper
    {

       
       
        public static string PresentExperience(this HtmlHelper helper, int UId)
        {
            UnitOfWork uni = new UnitOfWork();
            List<SqlParameter> prams = new List<SqlParameter>();
            prams.Add(new SqlParameter("UId", UId));
            var reader = uni.ReaderRepository.GetSPDataReader("UserExperience", prams);

            while (reader.Read())
            {
                if (ITTConfig.CurrentCultureIsNotEN)
                {
                    return reader.GetString(1) + Resource.Resource.at + reader.GetString(3);
                }
                else return reader.GetString(2) + Resource.Resource.at + reader.GetString(4);
            }
            return String.Empty;
        }
    }
}