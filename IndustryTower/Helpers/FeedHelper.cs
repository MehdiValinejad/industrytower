using IndustryTower.DAL;
using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IndustryTower.Helpers
{
    public class FeedHelper
    {
        public static void FeedInsert(FeedType typ, int elemId, int adjId ,string data = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork();

            unitOfWork.ReaderRepository.GetSPDataReader(
                "FeedInsert",
                new SqlParameter("typ", typ),
                new SqlParameter("adjId", adjId),
                new SqlParameter("elemId", elemId),
                new SqlParameter("data", data));
        }
    }
}