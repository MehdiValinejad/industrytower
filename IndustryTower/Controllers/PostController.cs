using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Microsoft.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{

    [ITTAuthorizeAttribute]
    public class PostController : Controller
    {
        private UnitOfWork unitOfWOrk = new UnitOfWork();

        public ActionResult PostPartial(int PId)
        { 
            var post = unitOfWOrk.PostRepository.GetByID(PId);
            return PartialView(post);
        }
        public ActionResult Create(postVars model)
        {
            return PartialView(new PostViewModel { prams = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Deserialize]postVars model, string PrC, string PrS, string post, string filesToUpload)
        {
            //var up = EncryptionHelper.Unprotect(UP);
            //var cp = EncryptionHelper.Unprotect(CP);
            //var sp = EncryptionHelper.Unprotect(SP);
            var prc = EncryptionHelper.Unprotect(PrC);
            var prs = EncryptionHelper.Unprotect(PrS);

            //int?[] receivers = { UP, CP, SP };
            //int?[] senders = { PrC, PrS };

            int?[] senders = { prc, prs };

            if (ModelState.IsValid && senders.Count(x => x != null) <=1)
            {
                Post postEntry = new Post();
                int senderUser = WebSecurity.CurrentUserId;
                int? sndr = null;
                int recvr = 0;
                string notifData = null;

                postEntry.insertDate = DateTime.UtcNow;
                postEntry.posterUserID = senderUser;
                postEntry.post = post;

                var notiftype = NotificationType.Post;

                //send to userProfile
                if (model.UId != null)
                {
                    var receiverUser = unitOfWOrk.ActiveUserRepository.GetByID(model.UId);
                    if (receiverUser.Friends.Any(a => a.friendID == senderUser)
                        || receiverUser.FriendsImIn.Any(a => a.userID == senderUser)
                        || receiverUser.UserId == senderUser)
                    {
                        postEntry.postedUserID = model.UId;
                        recvr = (int)model.UId;
                    }
                    else throw new JsonCustomException(Resource.ControllerError.ajaxErrorPostInsert);
                }
                else if (model.CoId != null)
                {
                    if (unitOfWOrk.NotExpiredCompanyRepository.GetByID(model.CoId).Followers.Any(a => a.followerUserID == senderUser))
                    {
                        postEntry.postedCoID = model.CoId;
                        notiftype = NotificationType.PostCo;
                        recvr = (int)model.CoId;
                    }
                    else throw new JsonCustomException(Resource.ControllerError.ajaxErrorPostInsert);
                }
                else if (model.StId != null)
                {
                    if (unitOfWOrk.StoreNotExpiredRepository.GetByID(model.StId).Followers.Any(a => a.followerUserID == senderUser))
                    {
                        postEntry.postedStoreID = model.StId;
                        notiftype = NotificationType.PostSt;
                        recvr = (int)model.StId;
                    }
                    else throw new JsonCustomException(Resource.ControllerError.ajaxErrorPostInsert);
                }
                else throw new JsonCustomException(Resource.ControllerError.ajaxErrorPostInsert);

                //send as company
                if (prc != null)
                {
                    if (unitOfWOrk.NotExpiredCompanyRepository.GetByID(prc).Admins.Any(a => a.UserId == senderUser))
                    {
                        postEntry.posterCoID = prc;
                        sndr = prc;
                        notifData = JsonConvert.SerializeObject( new PosterCoNotif { coid = prc });
                    }
                    else throw new JsonCustomException(Resource.ControllerError.ajaxErrorPostInsert);
                }
                //send as Store
                else if (prs != null)
                {
                    if (unitOfWOrk.StoreNotExpiredRepository.GetByID(prs).Admins.Any(a => a.UserId == senderUser))
                    {
                        postEntry.posterStoreID = prs;
                        sndr = prs;
                        notifData = JsonConvert.SerializeObject(new PosterCoNotif { stid = prs });
                    }
                    else throw new JsonCustomException(Resource.ControllerError.ajaxErrorPostInsert);
                }
                
                
                unitOfWOrk.PostRepository.Insert(postEntry);
                unitOfWOrk.Save();

                NotificationHelper.NotificationInsert(notiftype,
                                                      elemId: postEntry.postID,
                                                      recId: recvr,
                                                      senderCoStoreId: sndr,
                                                      data: notifData
                                                      );
                FeedHelper.FeedInsert(FeedType.Post,
                                      postEntry.postID,
                                      WebSecurity.CurrentUserId);

                if (!String.IsNullOrWhiteSpace(filesToUpload))
                {
                    var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, String.Empty, "Post");
                    postEntry.image = fileUploadResult.ImagesToUpload;
                    postEntry.document = fileUploadResult.DocsToUpload;
                    unitOfWOrk.PostRepository.Update(postEntry);
                    
                }
                unitOfWOrk.Save();
                UnitOfWork newpost = new UnitOfWork();
                var npost = newpost.PostRepository.GetByID(postEntry.postID);
                return Json(new { Success = true, Result = RenderPartialViewHelper.RenderPartialView(this, "PostPartial", npost) });

            }
            return Json(new { Success = false, Message = ControllerError.ajaxErrorPostInsert });
        }


        public ActionResult Edit(string PId)
        {
            NullChecker.NullCheck(new object[] { PId });

            Post postToEdit =  unitOfWOrk.PostRepository.GetByID(EncryptionHelper.Unprotect(PId));
            if (postToEdit.posterUserID != WebSecurity.CurrentUserId)
            {
                throw new JsonCustomException(ControllerError.ajaxErrorPostEdit);
            }
            return PartialView(postToEdit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection formCollection, string filesToUpload, string PIN, string P)
        {
            NullChecker.NullCheck(new object[] { PIN, P });

            var pin = EncryptionHelper.Unprotect(PIN);
            var p = EncryptionHelper.Unprotect(P);
            Post postEntryToEdit = unitOfWOrk.PostRepository.GetByID(pin);

            if (AuthorizationHelper.isRelevant((int)postEntryToEdit.posterUserID)
                && pin == p)
            {
                if (TryUpdateModel(postEntryToEdit,"", new string[] {"post"}))
                {
                    if (!String.IsNullOrWhiteSpace(postEntryToEdit.post))
                    {
                        unitOfWOrk.PostRepository.Update(postEntryToEdit);
                    }
                    if (!String.IsNullOrWhiteSpace(filesToUpload))
                    {
                        var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, postEntryToEdit.image + postEntryToEdit.document, "Post");
                        postEntryToEdit.image = fileUploadResult.ImagesToUpload;
                        postEntryToEdit.document = fileUploadResult.DocsToUpload;
                    }
                    unitOfWOrk.PostRepository.Update(postEntryToEdit);
                    unitOfWOrk.Save();
                    UnitOfWork editContext = new UnitOfWork();
                    var post = editContext.PostRepository.GetByID(postEntryToEdit.postID);
                    return Json(new
                    {
                        Success = true,
                        Result = RenderPartialViewHelper.RenderPartialView(this, "PostPartial", post),
                        Message = Resource.Resource.editedSuccessfully
                    });
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxErrorPostEdit);
        }


        public ActionResult Delete(string PId)
        {
            NullChecker.NullCheck(new object[] { PId });

            var postToDelete = unitOfWOrk.PostRepository.GetByID(EncryptionHelper.Unprotect(PId));
            if (!AuthorizationHelper.isRelevant((int)postToDelete.posterUserID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorPostEdit);
            }
            return PartialView(postToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string PTD, string P)
        {
            NullChecker.NullCheck(new object[] { PTD, P });
            var ptd = EncryptionHelper.Unprotect(PTD);
            var p = EncryptionHelper.Unprotect(P);

            if (ptd == p)
            {
                var postToDelete = unitOfWOrk.PostRepository.GetByID(ptd);
                if (!AuthorizationHelper.isRelevant((int)postToDelete.posterUserID))
                {
                    throw new JsonCustomException(ControllerError.ajaxErrorPostEdit);
                }
                unitOfWOrk.LikeCommentRepository.Get(d => postToDelete.Comments.Select(f => f.cmtID).Contains(d.elemID) && d.type == LikeCommentType.Post).ToList().ForEach(unitOfWOrk.LikeCommentRepository.Delete);
                //postToDelete.Comments.SelectMany(t => t.Likes.Select(l => l.likeID as object)).ToList().ForEach(unitOfWOrk.LikeRepository.Delete); //PostCommentsLikesDelete
                postToDelete.Comments.Select(t => t.cmtID as object).ToList().ForEach(unitOfWOrk.CommentPostRepository.Delete);//PostCommentsDelete
                postToDelete.Likes.Select(l => l.likeID as object).ToList().ForEach(unitOfWOrk.LikePostRepository.Delete);//PostLikesDelete
                postToDelete.Shares.Select(l => l.shareID as object).ToList().ForEach(unitOfWOrk.ShareRepository.Delete);//PostSharesDelete
                //postToDelete.Notifications.Select(n => n.notificationID as object).ToList().ForEach(unitOfWOrk.NotificationRepository.Delete);//PostNotificationsDelete
                var fileUploadResult = UploadHelper.UpdateUploadedFiles(String.Empty, postToDelete.image + postToDelete.document, "Question");
                if (String.IsNullOrEmpty(fileUploadResult.ImagesToUpload) && String.IsNullOrEmpty(fileUploadResult.DocsToUpload))
                {
                    unitOfWOrk.PostRepository.Delete(ptd);
                    unitOfWOrk.Save();
                }
                return Json(new { Message = Resource.Resource.deletedSuccessfully });
            }
            else return Json(new { Message = ControllerError.ajaxErrorPostDelete });
            
        }


        [AllowAnonymous]
        public ActionResult ProfilePostFeed(int? UId, int? CoId, int? StId, int? page)
        {
            IEnumerable<ProfileFeed> feedList = Enumerable.Empty<ProfileFeed>();
            if (UId != null)
            { 
                var userPosts = unitOfWOrk.PostRepository.Get(filter: p => p.postedUserID == UId);
                var userShars = unitOfWOrk.ShareRepository.Get(filter: p => p.SharerUserID == UId);
                var posts = from p in userPosts
                         select new ProfileFeed
                         {
                             date = p.insertDate,
                             Posts = p
                         };
                var shares = from s in userShars
                         select new ProfileFeed
                         {
                             date = s.insertdate,
                             Shares = s
                         };
                feedList = posts.Concat(shares).OrderByDescending(dt => dt.date);
                ViewData["UId"] = UId;
            }
            else if (CoId != null)
            {
                var companyPosts = unitOfWOrk.PostRepository.Get(filter: p => p.postedCoID == CoId);
                var posts = from p in companyPosts
                            select new ProfileFeed
                            {
                                date = p.insertDate,
                                Posts = p
                            };
                feedList = posts.OrderByDescending(dt => dt.date);
                ViewData["CoId"] = CoId;
            }
            else if (StId != null)
            {
                var storePosts = unitOfWOrk.PostRepository.Get(filter: p => p.postedStoreID == StId);
                var posts = from p in storePosts
                            select new ProfileFeed
                            {
                                date = p.insertDate,
                                Posts = p
                            };
                feedList = posts.OrderByDescending(dt => dt.date);
                ViewData["StId"] = StId;
            }

                int pageSize = 6;
                int pageNumber = (page ?? 1);
            if (page != null) {
                return PartialView(feedList.ToPagedList(pageNumber, pageSize));
            }
            else return PartialView(feedList.ToPagedList(1, pageSize));
        }


        public ActionResult SinglePost(int PId)
        {
            var post = unitOfWOrk.PostRepository.GetByID(PId);
            return View(post);
        }


    }
}
