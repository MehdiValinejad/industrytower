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
    public class ShareController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public ActionResult Share(string PId)
        {
            NullChecker.NullCheck(new object[] { PId });

            ShareViewModel viewmodel = new ShareViewModel();
            var pid = EncryptionHelper.Unprotect(PId);
            viewmodel.ToShare = unitOfWork.PostRepository.GetByID(pid);
            viewmodel.Share = new Share {
                                        sharedPostID = pid
                                        };
            return PartialView(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Share(ShareViewModel newShareToInsert, string PId)
        {
            NullChecker.NullCheck(new object[] { PId });
            int user = WebSecurity.CurrentUserId;
            int pid = (int)EncryptionHelper.Unprotect(PId);
            var postToshare = unitOfWork.PostRepository.GetByID(pid);
            if (postToshare.Shares.Any(ts => ts.SharerUserID == user))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorPatentUser);
            }
            if (ModelState.IsValid)
            {
                Share finalshareToInsrt = newShareToInsert.Share;
                finalshareToInsrt.SharerUserID = user;
                finalshareToInsrt.sharedPostID = postToshare.postID;
                finalshareToInsrt.shareNote = newShareToInsert.Share.shareNote;
                finalshareToInsrt.insertdate = DateTime.UtcNow;
                unitOfWork.ShareRepository.Insert(finalshareToInsrt);
                unitOfWork.Save();
                FeedHelper.FeedInsert(FeedType.SessionOfferLike, postToshare.postID, WebSecurity.CurrentUserId);
                return Json(new { Success = true, Message = Resource.Resource.sharedSuccessfully });
            }
            throw new ModelStateException(this.ModelState);
        }

        public ActionResult Edit(string ShId)
        {
            
            var shareToEdit = unitOfWork.ShareRepository.GetByID(EncryptionHelper.Unprotect(ShId));
            if (!AuthorizationHelper.isRelevant(shareToEdit.shareID))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            ShareViewModel viewmodel = new ShareViewModel();
            viewmodel.ToShare = shareToEdit.sharedPost;
            viewmodel.Share = shareToEdit;
            return PartialView(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection formCollection, string STE, string S)
        {
            NullChecker.NullCheck(new object[] { STE, S });

            var ste = EncryptionHelper.Unprotect(STE);
            var s = EncryptionHelper.Unprotect(S);
            Share shareEntryToEdit = unitOfWork.ShareRepository.GetByID(ste);

            if (AuthorizationHelper.isRelevant((int)shareEntryToEdit.SharerUserID)
                && ste == s)
            {
                if (TryUpdateModel(shareEntryToEdit, "", new string[] { "shareNote" }))
                {
                    unitOfWork.ShareRepository.Update(shareEntryToEdit);
                    unitOfWork.Save();

                    return Json(new
                    {
                        Message = Resource.Resource.editedSuccessfully,
                        Note = Resource.Resource.shareNote + ": " + shareEntryToEdit.shareNote
                    });
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxErrorPostEdit);
        }

        public ActionResult Delete(string ShId)
        {
            NullChecker.NullCheck(new object[] { ShId });

            var shareToDelete = unitOfWork.ShareRepository.GetByID(EncryptionHelper.Unprotect(ShId));
            if (!AuthorizationHelper.isRelevant((int)shareToDelete.SharerUserID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorPostEdit);
            }
            return PartialView(shareToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string STD, string S)
        {
            NullChecker.NullCheck(new object[] { STD, S });
            var std = EncryptionHelper.Unprotect(STD);
            var s = EncryptionHelper.Unprotect(S);
            if (std == s)
            {
                var shareToDelete = unitOfWork.ShareRepository.GetByID(std);
                if (!AuthorizationHelper.isRelevant((int)shareToDelete.SharerUserID))
                {
                    throw new JsonCustomException(ControllerError.ajaxErrorPostEdit);
                }
                unitOfWork.ShareRepository.Delete(std);
                unitOfWork.Save();
                return Json(new {Message = Resource.Resource.deletedSuccessfully });

            }
            throw new JsonCustomException(ControllerError.ajaxErrorPostEdit);

        }
    }
}
