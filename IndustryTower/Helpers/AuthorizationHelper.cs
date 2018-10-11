using IndustryTower.DAL;
using System.Data.SqlClient;
using System.Web.Security;
using WebMatrix.WebData;

namespace IndustryTower.Helpers
{
    public static class AuthorizationHelper
    {
        public static bool isAdmin()
        {
            if (WebSecurity.IsAuthenticated && Roles.IsUserInRole(WebSecurity.CurrentUserName, "ITAdmin"))
            {
                return true;
            }
            return false;
        }

        public static bool isRelevant(int currentUserID)
        {
            if (WebSecurity.IsAuthenticated && Roles.IsUserInRole(WebSecurity.CurrentUserName, "ITAdmin") || currentUserID == WebSecurity.CurrentUserId)
            {
                return true;
            }
            return false;
        }

        public static bool isRelevant(int currentUserID,string proc,params SqlParameter [] prams)
        {
            if (WebSecurity.IsAuthenticated && Roles.IsUserInRole(WebSecurity.CurrentUserName, "ITAdmin") || currentUserID == WebSecurity.CurrentUserId)
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                var perReader = unitOfWork.ReaderRepository.GetSPDataReader(proc, prams);
                bool isRel = false;
                while (perReader.Read())
                {
                    isRel = perReader.GetBoolean(0);
                }
                return isRel;
            }
            return false;
        }
    }
}