using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Resource;
using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class FriendRequestController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public ActionResult FriendsRequest(string UId)
        {
            NullChecker.NullCheck(new object[] { UId });
            var userid = EncryptionHelper.Unprotect(UId);
            if (WebSecurity.CurrentUserId == userid)
            {
                return new EmptyResult();
            }
            var onlineUserID = WebSecurity.CurrentUserId;
            bool areFriends = unitOfWork.FriendshipRepository.Get().Any(t => (t.userID == userid && t.friendID == onlineUserID)
                                                                           || (t.friendID == userid && t.userID == onlineUserID));

            if (areFriends)
            {
                ViewData["userToUnfriend"] = UId;
                return PartialView("AreFriends");
            }
            else
            {
                FriendRequestViewModel viewmodel = new FriendRequestViewModel();
                var requestIsSent = unitOfWork.FriendshipRequestRepository.Get(t => t.requestSenderID == userid && t.requestReceiverID == onlineUserID).SingleOrDefault();
                var requestISReceived = unitOfWork.FriendshipRequestRepository.Get(t => t.requestReceiverID == userid && t.requestSenderID == onlineUserID).SingleOrDefault();
                var ignored = unitOfWork.FriendshipRequestRepository.Get(t => t.requestReceiverID == userid && t.requestSenderID == onlineUserID && t.ignore).SingleOrDefault();
                if (ignored != null)
                {
                    return new EmptyResult();
                }
                if (requestIsSent != null)
                {
                    viewmodel.request = EncryptionHelper.Protect(requestIsSent.requestID);
                    viewmodel.user = UId;
                    return PartialView("FRRecievedBefore", viewmodel);
                }
                else if (requestISReceived != null)
                {
                    viewmodel.request = EncryptionHelper.Protect(requestISReceived.requestID);
                    viewmodel.user = UId;
                    return PartialView("FRSentBefore", viewmodel);
                }
                else
                {
                    viewmodel.user = UId;
                    return PartialView("NoFRBefore",viewmodel);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult AddFriend(FriendRequestViewModel friendRequest)
        {
            var onlineUser = WebSecurity.CurrentUserId;
            var requesteruserid = (int)EncryptionHelper.Unprotect(friendRequest.user);
            var formerRequests = unitOfWork.FriendshipRequestRepository.Get();
            bool isFormerRequest = formerRequests.Any(u => (u.requestReceiverID == requesteruserid && u.requestSenderID == onlineUser)
                                            || (u.requestSenderID == requesteruserid && u.requestReceiverID == onlineUser));
            if (ModelState.IsValid)
            {
                if (!isFormerRequest)
                {
                    FriendRequest newFriendRequest = new FriendRequest();
                    newFriendRequest.message = friendRequest.message;
                    newFriendRequest.requestSenderID = onlineUser;
                    newFriendRequest.requestReceiverID = requesteruserid;
                    newFriendRequest.requestDate = DateTime.UtcNow;

                    unitOfWork.FriendshipRequestRepository.Insert(newFriendRequest);
                    unitOfWork.Save();
                    
                    friendRequest.request = EncryptionHelper.Protect(newFriendRequest.requestID);
                    return Json(new {
                                      Result = RenderPartialViewHelper.RenderPartialView(this, "FRSentBefore", friendRequest),
                                      Message = Resource.Resource.friendRequestSent
                                    });
                }
            }
            throw new JsonCustomException(ControllerError.ajaxErrorFriendRequestAdd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult CancelFriendRequest(FriendRequestViewModel friendRequest)
        {
            var requestid = EncryptionHelper.Unprotect(friendRequest.request);
            var sentFriendRequest = unitOfWork.FriendshipRequestRepository.GetByID(requestid);
            if (sentFriendRequest.requestSenderID == WebSecurity.CurrentUserId)
            {
                unitOfWork.FriendshipRequestRepository.Delete(requestid);
                unitOfWork.Save();
                friendRequest.message = null;
                friendRequest.request = null;
                return Json(new {
                                  Result = RenderPartialViewHelper.RenderPartialView(this, "NoFRBefore", friendRequest),
                                  Message = Resource.Resource.friendRequestCanceled
                                });
            }
            throw new JsonCustomException(ControllerError.ajaxErrorFriendRequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult ConfirmFriendRequest(FriendRequestViewModel friendRequest)
        {
            var rrequestid = EncryptionHelper.Unprotect(friendRequest.request);
            var fRequest = unitOfWork.FriendshipRequestRepository.GetByID(rrequestid);
            if (fRequest.requestReceiverID == WebSecurity.CurrentUserId)
            {
                Friendship newFriendship = new Friendship();
                newFriendship.userID = fRequest.requestSenderID;
                newFriendship.friendID = fRequest.requestReceiverID;
                newFriendship.friendshipDate = DateTime.UtcNow;
                unitOfWork.FriendshipRepository.Insert(newFriendship);
                unitOfWork.FriendshipRequestRepository.Delete(fRequest.requestID);
                
                unitOfWork.Save();
                NotificationHelper.NotificationInsert(NotificationType.FriendRequestAccept,
                                                          recId: fRequest.requestSenderID,
                                                          elemId: newFriendship.frendshipId,
                                                          data: fRequest.requestReceiverID.ToString());
                unitOfWork.Save();
                return Json(new { 
                                  Result = RenderPartialViewHelper.RenderPartialView(this, "AreFriends", null),
                                  Message = Resource.Resource.friendRequestConfirmed
                                });
            }
            throw new JsonCustomException(ControllerError.ajaxErrorFriendRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult DenyFriendRequest(FriendRequestViewModel friendRequest)
        {
            var requestid = EncryptionHelper.Unprotect(friendRequest.request);
            unitOfWork.FriendshipRequestRepository.Delete(requestid);
            unitOfWork.Save();

            friendRequest.message = null;
            friendRequest.request = null;
            return Json(new
            {
                Success = true,
                Result = RenderPartialViewHelper.RenderPartialView(this, "NoFRBefore", friendRequest),
                Message = Resource.Resource.friendRequestDenied
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult IgnoreFriendRequest(FriendRequestViewModel friendRequest)
        {
            var requestid = EncryptionHelper.Unprotect(friendRequest.request);
            var requstToIgnore = unitOfWork.FriendshipRequestRepository.GetByID(requestid);
            requstToIgnore.ignore = true;
            unitOfWork.FriendshipRequestRepository.Update(requstToIgnore);
            unitOfWork.Save();
            return Json(new
            {
                Success = true,
                Result = RenderPartialViewHelper.RenderPartialView(this, "AreFriends", null),
                Message = Resource.Resource.friendRequestIgnore
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult UnFriend(string UTU)
        {
            var onlineUserID = WebSecurity.CurrentUserId;
            var utuid = EncryptionHelper.Unprotect(UTU);
            var frienshtipToDelete = unitOfWork.FriendshipRepository.Get(f => (f.userID == onlineUserID && f.friendID == utuid)
                                                                           || (f.friendID == onlineUserID && f.userID == utuid)).Single();
            //delete notifications
            //frienshtipToDelete.Notifications.Select(n => n.notificationID as object).ToList().ForEach(unitOfWork.NotificationRepository.Delete);
            unitOfWork.FriendshipRepository.Delete(frienshtipToDelete.frendshipId);
            unitOfWork.Save();

            FriendRequestViewModel viewmodel = new FriendRequestViewModel();
            viewmodel.user = UTU;
            return Json(new
            {
                Result = RenderPartialViewHelper.RenderPartialView(this, "NoFRBefore", viewmodel),
                Message = Resource.Resource.friendRemoved
            });
        }


        public ActionResult FRNotifications()
        {
            var currentUser = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
            var ReceivedRequests = currentUser.ReceivedFriendRequests;
            return PartialView(ReceivedRequests);
        }
    }
}
