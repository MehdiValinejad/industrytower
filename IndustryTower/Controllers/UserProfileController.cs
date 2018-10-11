using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using MvcSiteMapProvider.Web.Mvc.Filters;
using PagedList;
using Resource;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;




namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class UserProfileController : Controller
    {
               
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult _LoginPartial()
        {
            if (AuthorizationHelper.isAdmin())
            {
                return PartialView("~/Views/Admin/_LoginAdmin.cshtml"); ;
            }
            var user = WebSecurity.IsAuthenticated ?  unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId): null;
            return PartialView(user);
        }


        public ActionResult Home()
        {
            if (AuthorizationHelper.isAdmin())
            {
                return RedirectToAction("MainMenu", "Admin");
            }
            return View();
        }


        public ActionResult HomeFeed(int? page)
        {
            page = page ?? 1;
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "Feeds",
                new SqlParameter("@pageNum", page),
                new SqlParameter("@U",WebSecurity.CurrentUserId));
            bool isFinalPage = true;
            IList<Feed> feeds = new List<Feed>();
            while (reader.Read())
            {
                isFinalPage = false;
                
                ActiveUser u = new ActiveUser { 
                    UserId = reader.GetInt32(2),
                    firstName = reader[5] as string,
                    lastName = reader[6] as string,
                    firstNameEN = reader[7] as string,
                    lastNameEN = reader [8]  as string,
                    image = reader[9] as string
                };
                
                feeds.Add(new Feed
                {
                    feedType = (FeedType)reader.GetInt32(0),
                    elemId = reader.GetInt32(1),
                    adjId = reader.GetInt32(2),
                    data = reader[3] as string,
                    adjUser = u
                });
            }

            ViewData["finalPage"] = isFinalPage;
            ViewData["pageNum"] = page;

            return PartialView(feeds);
        }

        public ActionResult HomeWebinars()
        {
            HomeFeedWebinarsClassified viewmodel = new HomeFeedWebinarsClassified();
            var user = unitOfWork.ActiveUserRepository.Get(u => u.UserId == WebSecurity.CurrentUserId, includeProperties: "SeminarsAudience,SeminarsBroadcast,SeminarsModerate").First();
            IEnumerable<Seminar> seminars = user.SeminarsAudience.Concat(user.SeminarsBroadcast).Concat(user.SeminarsModerate);
            var now = DateTime.UtcNow;
            viewmodel.Now = seminars.Where(s => s.date < now.AddMinutes(5) && s.date.AddMinutes(s.duration) > now)
                                          .OrderByDescending(d => d.date);
            viewmodel.attending = seminars.Where(a => a.date > now).OrderByDescending(d => d.date);

            return PartialView(viewmodel);
        }

        public ActionResult HomeEvents() 
        {
            IEnumerable<HomeFeedEvents> Events = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId)
                        .Professions.SelectMany(f => f.categories.SelectMany(h => h.Events.Where(d => d.eventDate > DateTime.UtcNow)))
                        .GroupBy(p => p, p => p, (key, g) => new HomeFeedEvents { Event = key, relevance = g.Count() })
                        .OrderByDescending(o => o.relevance)
                        .ThenByDescending(d => d.Event.eventID)
                        .Take(15);

            return PartialView(Events);
        }

        public ActionResult HomeQuestions()
        {
            HomeFeedQuestionsClassified viewmodel = new HomeFeedQuestionsClassified();
            IEnumerable<Profession> professions = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId).Professions;
            viewmodel.answered = professions.SelectMany(f => f.Questions.Where(q => q.Answers.Any(a => a.accept == true)))
                                        .GroupBy(p => p, p => p, (key, g) => new HomeFeedQuestions { Question = key, relevance = g.Count() })
                                        .OrderByDescending(o => o.relevance)
                                        .ThenByDescending(o => o.Question.questionID)
                                        .Take(15);
            viewmodel.unAnswered = professions.SelectMany(f => f.Questions.Where(q => !q.Answers.Any(a => a.accept == true)))
                                        .GroupBy(p => p, p => p, (key, g) => new HomeFeedQuestions { Question = key, relevance = g.Count() })
                                        .OrderByDescending(o => o.relevance)
                                        .ThenByDescending(o => o.Question.questionID)
                                        .Take(15);
            viewmodel.old = professions.SelectMany(f => f.Questions.Where(q => !q.Answers.Any(a => a.accept == true)))
                                        .GroupBy(p => p, p => p, (key, g) => new HomeFeedQuestions { Question = key, relevance = g.Count() })
                                        .OrderByDescending(o => o.relevance)
                                        .ThenByDescending(o => o.Question.questionID)
                                        .Take(15);
            var tt = professions.SelectMany(f => f.Questions.Where(q => !q.Answers.Any(a => a.accept == true))).ToList();
            var hhh = tt.GroupBy(g => g);
            var ttt = tt.GroupBy(p => p, p => p, (key, g) => new HomeFeedQuestions { Question = key, relevance = g.Count() })
                        .OrderByDescending(o => o.relevance)
                        .ThenByDescending(o=>o.Question.questionID);
            var tttt = ttt.OrderByDescending(o => o.relevance).ToList();

            return PartialView(viewmodel);
        }

        [AllowAnonymous]
        public ActionResult UProfile(int UId)
        {
            var user = unitOfWork.ActiveUserRepository.GetByID(UId);

            return View(user);
        }

        [AllowAnonymous]
        public ActionResult UserInfo(int UId)
        {
            var user = unitOfWork.ActiveUserRepository.GetByID(UId);

            return View(user);
        }

        [AllowAnonymous]
        public ActionResult UserPartial(int UId)
        {
            UserProfile User = unitOfWork.ActiveUserRepository.Get(filter: qq => qq.UserId == UId, includeProperties: "State").SingleOrDefault();
            return PartialView(User);
        }

        [AllowAnonymous]
        public ActionResult AvatarPartial(int UId)
        {
            UserProfile User = unitOfWork.ActiveUserRepository.Get(filter: qq => qq.UserId == UId).SingleOrDefault();
            return PartialView(User);
        }

        [AllowAnonymous]
        public ActionResult UsersPartial(IEnumerable<UserProfile> users)
        {
            return PartialView(users);
        }


        public ActionResult EditInfo(string UId)
        {
            NullChecker.NullCheck(new object[] { UId });
            var uid = EncryptionHelper.Unprotect(UId);
            if (!AuthorizationHelper.isRelevant((int)uid))
            {
                return new RedirectToError();
            }
            var user = unitOfWork.ActiveUserRepository.GetByID(uid);
            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.counts(user.State);
            ViewBag.StateDropDown = countries.states(user.State);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [SiteMapCacheRelease]
        public ActionResult EditInfo(string UId, string professionTags)
        {
            NullChecker.NullCheck(new object[] { UId });
            var uid = EncryptionHelper.Unprotect(UId);
            if (!AuthorizationHelper.isRelevant((int)uid))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            var userToEdit = unitOfWork.ActiveUserRepository.GetByID(uid);
            if (TryUpdateModel(userToEdit, "", new string[] {
                    "firstName","firstNameEN","lastName","lastNameEN",
                    "about","aboutEN","stateID", "birthDay" }))
            {

                UpdSertUserProffs(professionTags, userToEdit);

                unitOfWork.ActiveUserRepository.Update(userToEdit);
                unitOfWork.Save();
                UrlHelper Url = new UrlHelper(Request.RequestContext);
                return Json(new { URL = Url.Action("UProfile", "UserProfile", new { UId = userToEdit.UserId, UName = StringHelper.URLName(userToEdit.CultureFullName) }) });
            }
            throw new ModelStateException(this.ModelState);
        }

        public ActionResult EditAdvInfo(string UId)
        {
            var user = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult EditAdvInfo(string UId, FormCollection form)
        {
            NullChecker.NullCheck(new object[] { UId });
            var uid = EncryptionHelper.Unprotect(UId);
            if (!AuthorizationHelper.isRelevant((int)uid))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            var userToEdit = unitOfWork.ActiveUserRepository.GetByID(uid);
            if (userToEdit.melliCode != null)
            {
                if (TryUpdateModel(userToEdit, "", new string[] { "mobile" }))
                {
                    unitOfWork.ActiveUserRepository.Update(userToEdit);
                    unitOfWork.Save();
                    UrlHelper Url = new UrlHelper(Request.RequestContext);
                    return Json(new { URL = Url.Action("UProfile", "UserProfile", new { UId = userToEdit.UserId, UName = StringHelper.URLName(userToEdit.CultureFullName) }) });
                }
            }
            else 
            {
                if (TryUpdateModel(userToEdit, "", new string[] { "mobile", "melliCode" }))
                {
                    unitOfWork.ActiveUserRepository.Update(userToEdit);
                    unitOfWork.Save();
                    UrlHelper Url = new UrlHelper(Request.RequestContext);
                    return Json(new { URL = Url.Action("UProfile", "UserProfile", new { UId = userToEdit.UserId, UName = StringHelper.URLName(userToEdit.CultureFullName) }) });
                }
            }
            throw new ModelStateException(this.ModelState);
        }


        public ActionResult ProfilePic(string UId)
        {
            NullChecker.NullCheck(new object[] { UId });
            var uid = EncryptionHelper.Unprotect(UId);
            if (!AuthorizationHelper.isRelevant((int)uid))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            ViewData["UId"] = UId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult ProfilePic(string UId, int x, int y, int w, int h, string picToUpload)
        {
            NullChecker.NullCheck(new object[] { UId });
            var uid = EncryptionHelper.Unprotect(UId);
            if (!AuthorizationHelper.isRelevant((int)uid))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            UploadHelper.CropImage(x, y, w, h, picToUpload);
            UploadHelper.moveFile(picToUpload, "Profile");
            var user = unitOfWork.ActiveUserRepository.GetByID(uid);
            if (user.image != null)
            {
                UploadHelper.deleteFile(user.image, "Profile");
            }
            user.image = picToUpload;
            unitOfWork.ActiveUserRepository.Update(user);
            unitOfWork.Save();
            
            return Json(new { Success = true });
        }

        public ActionResult ProfilePicDelete(string UId)
        {
            NullChecker.NullCheck(new object[] { UId });
            var uid = EncryptionHelper.Unprotect(UId);
            if (!AuthorizationHelper.isRelevant((int)uid))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            ViewData["UId"] = UId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult ProfilePicDeletePost(string UId)
        {
            NullChecker.NullCheck(new object[] { UId });
            var uid = EncryptionHelper.Unprotect(UId);
            if (!AuthorizationHelper.isRelevant((int)uid))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            var user = unitOfWork.ActiveUserRepository.GetByID(uid);
            UploadHelper.deleteFile(user.image, "Profile");
            user.image = null;
            unitOfWork.ActiveUserRepository.Update(user);
            unitOfWork.Save();
            return Json(new { Success = true });
        }



        public ActionResult UserCompanyStore()
        {
            var currentUser = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
            return PartialView(currentUser);
        }

        private void UpdSertUserProffs(string selectedItems, ActiveUser userToUpdate)
        {
            if (userToUpdate.Professions == null)
            {
                userToUpdate.Professions = new List<Profession>();
            }
            userToUpdate.Professions = new List<Profession>();
            var selectedProfessionsHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                .Take(ITTConfig.MaxProfessionTagsLimit)
                                                .Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var QuestionProfessions = userToUpdate.Professions != null
                                      ? new HashSet<int>(userToUpdate.Professions.Select(c => c.profID))
                                      : new HashSet<int>();
            var proffsToDelet = QuestionProfessions.Except(selectedProfessionsHS).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();
            var proffsToInsert = selectedProfessionsHS.Except(QuestionProfessions).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();

            foreach (var proffToDel in proffsToDelet)
            {
                userToUpdate.Professions.Remove(proffToDel);
            }
            foreach (var proffToInsert in proffsToInsert)
            {
                userToUpdate.Professions.Add(proffToInsert);
            }

        }

        [AllowAnonymous]
        public ActionResult _UserSearchPartial(string searchString)
        {
            var User = unitOfWork.ActiveUserRepository.Get(c => c.firstName.Contains(searchString)
                                                               || c.firstNameEN.Contains(searchString)
                                                               || c.lastName.Contains(searchString)
                                                               || c.lastNameEN.Contains(searchString)).Take(10);
            return PartialView(User);
        }
    }
}
