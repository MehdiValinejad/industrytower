using IndustryTower.DAL;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace IndustryTower.Helpers
{
    public static class ScoreHelper
    {
        static public int Update(ScoreVars model)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            string spName = null,
                   elemparm = null;
            switch (model.type)
            {
                case ScoreType.Qvote:
                    spName = "ScoreQUpdate";
                    elemparm = "Q";
                    break;
                case ScoreType.Avote:
                    spName = "ScoreAUpdate";
                    elemparm = "A";
                    break;
                case ScoreType.GSOvote:
                    spName = "ScoreGSOUpdate";
                    elemparm = "GSO";
                    break;
                case ScoreType.Aacc:
                    spName = "ScoreAaccUpdate";
                    elemparm = "A";
                    break;
                case ScoreType.GSOacc:
                    spName = "ScoreGSOaccUpdate";
                    elemparm = "GSO";
                    break;
                case ScoreType.WEditvote:
                    spName = "ScoreWEditUpdate";
                    elemparm = "WTE";
                    break;
                case ScoreType.WDEditvote:
                    spName = "ScoreWDEditUpdate";
                    elemparm = "WDTE";
                    break;
                case ScoreType.BCreate:
                    spName = "ScoreBCreateUpdate";
                    elemparm = "BTE";
                    break;
                case ScoreType.BReview:
                    spName = "ScoreBReviewUpdate";
                    elemparm = "BRVTE";
                    break;
            }
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                                    spName,
                                    new SqlParameter(elemparm, model.elemId),
                                    new SqlParameter("sig", model.sign),
                                    new SqlParameter("granterId", WebSecurity.CurrentUserId));

            int res = 0;
            while (reader.Read())
            {
                res = reader.GetInt32(0);
            }
            return res;
        }
    }
}