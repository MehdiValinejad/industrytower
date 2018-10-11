using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class FollowingController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult Followers(int? CoId, int? StId)
        {
            IEnumerable<UserProfile> currentLikers = Enumerable.Empty<UserProfile>();

            if (CoId != null)
            {
                currentLikers = unitOfWork.FollowingRepository.Get(u => u.followedCoID == CoId)
                                                         .Select(l => l.FollowerUser);
            }
            else if (StId != null)
            {
                currentLikers = unitOfWork.FollowingRepository.Get(filter: u => u.followedStoreID == StId)
                                                         .Select(l => l.FollowerUser);
            }
            return PartialView("~/Views/UserProfile/_PartialUsers.cshtml", currentLikers);
        }

        [AllowAnonymous]
        public ActionResult Follow(string CoId, string StId)
        {
            FollowViewModel viewmodel = new FollowViewModel();
            var currentUser = WebSecurity.CurrentUserId;
            if (!String.IsNullOrEmpty(CoId))
            {
                var coid = EncryptionHelper.Unprotect(CoId);
                var followers = unitOfWork.FollowingRepository.Get(f => f.followedCoID == coid);
                viewmodel.followedByUser = followers.Any(f=> f.followerUserID == currentUser);
                viewmodel.Followers = followers.Count();
                viewmodel.CoId = coid;
            }
            else if (!String.IsNullOrEmpty(StId))
            {
                var stid = EncryptionHelper.Unprotect(StId);
                var followers = unitOfWork.FollowingRepository.Get(f => f.followedStoreID == stid);
                viewmodel.followedByUser = followers.Any(f => f.followerUserID == currentUser);
                viewmodel.Followers = followers.Count();
                viewmodel.storeId = stid;
            }
            return PartialView(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult FollowInsert(string company, string store)
        {
            var currentUser = WebSecurity.CurrentUserId;
            if (!String.IsNullOrEmpty(company))
            {
                var coid = EncryptionHelper.Unprotect(company);
                var following = unitOfWork.FollowingRepository.Get(f => f.followedCoID == coid
                                                                     && f.followerUserID == currentUser).SingleOrDefault();
                if (following != null)
                {
                    unitOfWork.FollowingRepository.Delete(following);
                    unitOfWork.Save();

                    FollowViewModel viewmodel = new FollowViewModel();
                    var followers = unitOfWork.FollowingRepository.Get(f => f.followedCoID == coid);
                    viewmodel.followedByUser = followers.Any(f => f.followerUserID == currentUser);
                    viewmodel.Followers = followers.Count();
                    viewmodel.CoId = coid;
                    return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "Follow", viewmodel) });
                }
                else
                {

                    var newFollow = new Following();
                    newFollow.followedCoID = coid;
                    newFollow.followerUserID = currentUser;
                    newFollow.followDate = DateTime.UtcNow;
                    unitOfWork.FollowingRepository.Insert(newFollow);
                    unitOfWork.Save();

                    FollowViewModel viewmodel = new FollowViewModel();
                    var followers = unitOfWork.FollowingRepository.Get(f => f.followedCoID == coid);
                    viewmodel.followedByUser = followers.Any(f => f.followerUserID == currentUser);
                    viewmodel.Followers = followers.Count();
                    viewmodel.CoId = coid;
                    return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "Follow", viewmodel) });

                }

            }
            else if (!String.IsNullOrEmpty(store))
            {
                var stid = EncryptionHelper.Unprotect(store);
                var following = unitOfWork.FollowingRepository.Get(f => f.followedStoreID == stid
                                                                     && f.followerUserID == currentUser).SingleOrDefault();
                if (following != null)
                {

                    unitOfWork.FollowingRepository.Delete(following);
                    unitOfWork.Save();

                    FollowViewModel viewmodel = new FollowViewModel();
                    var followers = unitOfWork.FollowingRepository.Get(f => f.followedStoreID == stid);
                    viewmodel.followedByUser = followers.Any(f => f.followerUserID == currentUser);
                    viewmodel.Followers = followers.Count();
                    viewmodel.storeId = stid;
                    return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "Follow", viewmodel) });
                }
                else
                {

                    var newFollow = new Following();
                    newFollow.followedStoreID = stid;
                    newFollow.followerUserID = currentUser;
                    newFollow.followDate = DateTime.UtcNow;
                    unitOfWork.FollowingRepository.Insert(newFollow);
                    unitOfWork.Save();

                    FollowViewModel viewmodel = new FollowViewModel();
                    var followers = unitOfWork.FollowingRepository.Get(f => f.followedStoreID == stid);
                    viewmodel.followedByUser = followers.Any(f => f.followerUserID == currentUser);
                    viewmodel.Followers = followers.Count();
                    viewmodel.storeId = stid;
                    return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "Follow", viewmodel) });
                }
            }
            throw new JsonCustomException(ControllerError.ajaxErrorFollowing);
        }
	}
}