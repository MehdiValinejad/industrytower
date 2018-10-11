using IndustryTower.DAL;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using System.Linq;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class FriendshipController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult Friends(int UId)
        {
            var friends = unitOfWork.FriendshipRepository.Get(f => f.friendID == UId).Select(g => g.User);
            var friendsIMIN = unitOfWork.FriendshipRepository.Get(f => f.userID == UId).Select(g => g.Friend);
            var finalmodel = friends.Concat(friendsIMIN);
            return PartialView("~/Views/UserProfile/_PartialUsers.cshtml", finalmodel.ToList());
        }

    }
}
