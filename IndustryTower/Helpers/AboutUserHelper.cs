using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    static public class AboutUserHelper
    {
        public static string AboutUser(this HtmlHelper helper, ActiveUser user )
        {
            var reader = new UnitOfWork().ReaderRepository.GetSPDataReader(
                            "AboutUser",
                            new SqlParameter("UId", user.UserId),
                            new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));
            if (reader.Read())
            {
                return String.Concat(reader[0] as string,
                                    Resource.Resource.at,
                                    reader[1] as string);
            }
            return String.Empty;
        }
    }
}