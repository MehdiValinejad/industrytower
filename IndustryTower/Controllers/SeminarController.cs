using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class SeminarController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult Create([Bind(Include = "title,desc,date,Time,maxAudiences,duration,price,isPublic")] Seminar smr, SemType semtype, string Cats, DateTime Time, string professionTags, string UserTag, string UserTags, string[] fileInsert)
        {
            if (String.IsNullOrWhiteSpace(UserTag))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorSemAdmin);
            }
            if (ModelState.IsValid)
            {
               
                smr.date = smr.date.ToUniversalTime();
                smr.files = String.Join(",", new HashSet<string>(fileInsert.Where(f => !String.IsNullOrWhiteSpace(f))));
                
                smr.moderatorId = (int)EncryptionHelper.Unprotect(UserTag);
                if (!String.IsNullOrWhiteSpace(UserTags)) UpSertSeminarBroadcasters(UserTags, smr);
                UpSertSeminarCats(Cats, smr);
                UpSertSeminarProffs(professionTags, smr);
                smr.token = Guid.NewGuid();
                switch(semtype)
                {
                     case SemType.Workshop:
                        unitOfWork.WorkshopRepository.Insert(smr as Workshop);
                        break;
                     case SemType.Webinar:
                        unitOfWork.WebinarRepository.Insert(smr as Webinar);
                        break;
                    case SemType.VideoConference:
                        unitOfWork.VideoConferenceRepository.Insert(smr as VideoConference);
                        break;
                    default:
                        throw new JsonCustomException(ControllerError.ajaxErrorSemAdmin);
                }
                   

                unitOfWork.Save();
                return Json(new { Success = true, Url = Url.Action("Detail", new { SnId = smr.seminarId, SnName = StringHelper.URLName(smr.title) }) });
            }
            throw new ModelStateException(this.ModelState);
        }


        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult Edit(string SnId)
        {
            NullChecker.NullCheck(new object[] { SnId });

            var semToEdit = unitOfWork.SeminarRepository.GetByID(EncryptionHelper.Unprotect(SnId));
            
            var catss = semToEdit.Categories.Select(u => u.catID);
            ViewData["Cats"] = String.Join(",", catss);
            return View(semToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        [SiteMapCacheRelease]
        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult Edit(string SnId, string S, SemType semtype, DateTime Time, string Cats, string professionTags, string UserTag, string UserTags, string[] fileInsert)
        {
            NullChecker.NullCheck(new object[] { SnId, S });
            if (String.IsNullOrWhiteSpace(UserTag))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorSemAdmin);
            }
            var snid = EncryptionHelper.Unprotect(SnId);
            var s = EncryptionHelper.Unprotect(S);
            var semToEdit = unitOfWork.SeminarRepository.GetByID(snid);
            if (s == snid)
            {
                if (TryUpdateModel(semToEdit, "", new string[] { "title", "desc", "date", "maxAudiences", "duration", "price", "isPublic" }))
                {
                    semToEdit.date = semToEdit.date + new TimeSpan(Time.Hour, Time.Minute, Time.Second);
                    
                    var currentType = semToEdit.GetType().BaseType.Name;
                    if (currentType != semtype.ToString())
                    {
                        unitOfWork.seminarTypeChange(semToEdit.seminarId, semtype.ToString());
                    }

                    semToEdit.date = semToEdit.date.ToUniversalTime();
                    semToEdit.files = String.Join(",", new HashSet<string>(fileInsert.Where(f => !String.IsNullOrWhiteSpace(f))));
                    semToEdit.moderatorId = (int)EncryptionHelper.Unprotect(UserTag);
                    if (!String.IsNullOrWhiteSpace(UserTags)) UpSertSeminarBroadcasters(UserTags, semToEdit);
                    UpSertSeminarCats(Cats, semToEdit);
                    UpSertSeminarProffs(professionTags, semToEdit);
                    unitOfWork.SeminarRepository.Update(semToEdit);
                    unitOfWork.Save();
                    return Json(new { Success = true, Url = Url.Action("Detail", new { SnId = semToEdit.seminarId, SName = StringHelper.URLName(semToEdit.title) }) });
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        public ActionResult AddAudience(string SnId)
        {
            NullChecker.NullCheck(new object[] { SnId });
            var sem = unitOfWork.SeminarRepository.GetByID(EncryptionHelper.Unprotect(SnId));
            if (!AuthorizationHelper.isRelevant(sem.moderatorId)) return new RedirectToError();
            ViewData["SnId"] = SnId;
            return View(sem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAudience(string SnId, string UserTags)
        {
            var snid = EncryptionHelper.Unprotect(SnId);
            var sem = unitOfWork.SeminarRepository.GetByID(snid);
            if (!AuthorizationHelper.isRelevant(sem.moderatorId)) return new RedirectToError();
            UpSertSeminarAudiences(UserTags, sem);
            unitOfWork.Save();
            return Json(new { Success = true, Url = Url.Action("Detail", "Seminar", new { SnId = snid, SnName = StringHelper.URLName(sem.title) }) });
        }

        [AllowAnonymous]
        public ActionResult Audiences(int SnId)
        {
            var members = unitOfWork.SeminarRepository.GetByID(SnId).Audiences;
            return PartialView("~/Views/UserProfile/_PartialUsers.cshtml", members);
        }

        [AllowAnonymous]

        public ActionResult Detail(int SnId, string message)
        {
            var sem = unitOfWork.SeminarRepository.GetByID(SnId);
            ViewData["message"] = message;
            return View(sem);
        }

        public ActionResult Attendance(string SnId)
        {
            NullChecker.NullCheck(new object[] { SnId });
            var snid = EncryptionHelper.Unprotect(SnId);

            var currentUser = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
            if (currentUser.melliCode == null)
            {
                return RedirectToAction("EditAdvInfo", "UserProfile", new { UId = EncryptionHelper.Protect(currentUser.UserId) });
            }
            var sem = unitOfWork.SeminarRepository.GetByID(snid);
            if (!sem.isPublic) return new RedirectToNotFound();
            return View(sem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Attendance(string SnId,string WN)
        {
            NullChecker.NullCheck(new object[] { SnId, WN });

            var snid = EncryptionHelper.Unprotect(SnId);
            var wn = EncryptionHelper.Unprotect(WN);
            var sem = unitOfWork.SeminarRepository.GetByID(snid);
            if (!sem.isPublic) return new RedirectToNotFound();

            var currentUser = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
            if (currentUser.melliCode == null)
            {
                return RedirectToAction("EditAdvInfo", "UserProfile", new { UId = EncryptionHelper.Protect(currentUser.UserId) });
            }
            var message = "";
            if (wn == snid && currentUser.melliCode != null)
            {
                if (sem.Audiences.Any(a => a.UserId == WebSecurity.CurrentUserId))
                {
                    sem.Audiences.Remove(currentUser);
                    message = Resource.Resource.semAttendcancelSuccess;
                }
                else
                {
                    sem.Audiences.Add(currentUser);
                    message = Resource.Resource.semAttendSuccess;
                }
                unitOfWork.Save();
                return RedirectToAction("Detail", "Seminar", new { SnId = sem.seminarId, message = message  });
            }
            return new RedirectToError();
        }

        [HttpPost]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult GetTime(string SnId)
        {
            NullChecker.NullCheck(new object[] { SnId });
            var sem = unitOfWork.SeminarRepository.GetByID(EncryptionHelper.Unprotect(SnId));
            return Json(new { time = sem.date.AddMinutes(sem.duration).Subtract(DateTime.UtcNow).Minutes });
        }
        




        [HostControl]
        [AjaxRequestOnly]
        [HttpPost]
        public bool UserCheck(string SnId, string UId, string role)
        {
            NullChecker.NullCheck(new object[] { SnId, UId, role });
            var sem = unitOfWork.SeminarRepository.GetByID(EncryptionHelper.Unprotect(SnId));
            var uid = EncryptionHelper.Unprotect(UId);

            switch (role) { 
                case "audience":
                    return sem.Audiences.Any(f => f.UserId == uid);
                case "broadcaster":
                    return sem.Broadcasters.Any(f => f.UserId == uid);
                default:
                    return false;
            }
        }


        //public FileResult Download(string SnId)
        //{
        //    NullChecker.NullCheck(new object[] { SnId});
        //    var sem = unitOfWork.SeminarRepository.GetByID(EncryptionHelper.Unprotect(SnId));

        //    return File("~/Uploads/", System.Net.Mime.MediaTypeNames.Application.Octet);
        //}


        private void UpSertSeminarProffs(string selectedItems, Seminar smrToUpdate)
        {
            if (smrToUpdate.Professions == null)
            {
                smrToUpdate.Professions = new List<Profession>();
            }
            smrToUpdate.Professions = new List<Profession>();
            var selectedProfessionsHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' }).Take(ITTConfig.MaxProfessionTagsLimit).Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var groupProfessions = smrToUpdate.Professions != null
                                      ? new HashSet<int>(smrToUpdate.Professions.Select(c => c.profID))
                                      : new HashSet<int>();
            var proffsToDelet = groupProfessions.Except(selectedProfessionsHS).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();
            var proffsToInsert = selectedProfessionsHS.Except(groupProfessions).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();

            foreach (var proffToDel in proffsToDelet)
            {
                smrToUpdate.Professions.Remove(proffToDel);
            }
            foreach (var proffToInsert in proffsToInsert)
            {
                smrToUpdate.Professions.Add(proffToInsert);
            }

        }


        private void UpSertSeminarCats(string selectedItems, Seminar smrToUpdate)
        {
            if (smrToUpdate.Categories == null)
            {
                smrToUpdate.Categories = new List<Category>();
            }
            var selectedCategoriesHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Take(ITTConfig.MaxCategoryTagsLimit)
                                                   .Select(u => int.Parse(u)))
                                      : new HashSet<int>();
            var GroupCategories = smrToUpdate.Categories != null
                                  ? new HashSet<int>(smrToUpdate.Categories.Select(c => c.catID))
                                  : new HashSet<int>();
            var catsToDelet = GroupCategories.Except(selectedCategoriesHS).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();
            var catsToInsert = selectedCategoriesHS.Except(GroupCategories).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();
           
            foreach (var catToDel in catsToDelet)
            {
                smrToUpdate.Categories.Remove(catToDel);
            }
            foreach (var catToInsert in catsToInsert)
            {
                smrToUpdate.Categories.Add(catToInsert);
            }

        }

        private void UpSertSeminarBroadcasters(string selectedItems, Seminar smrToUpdate)
        {
            if (smrToUpdate.Broadcasters == null)
            {
                smrToUpdate.Broadcasters = new List<ActiveUser>();
            }
            var selectedUsersHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Take(ITTConfig.MaxAdminsLimit)
                                                   .Select(u => (int)EncryptionHelper.Unprotect(u)))
                                      : new HashSet<int>();
            var Broadcasters = smrToUpdate.Broadcasters != null
                                  ? new HashSet<int>(smrToUpdate.Broadcasters.Select(c => c.UserId))
                                  : new HashSet<int>();

            var Audiences = smrToUpdate.Audiences != null
                                  ? new HashSet<int>(smrToUpdate.Audiences.Select(c => c.UserId))
                                  : new HashSet<int>();
            var adminsToDelet = Broadcasters.Except(selectedUsersHS).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            var adminsToInsert = selectedUsersHS
                                        .Except(Broadcasters)
                                        .Except(Audiences)
                                        .Where(d=> d != smrToUpdate.moderatorId)
                                        .Select(t => unitOfWork.ActiveUserRepository.GetByID(t))
                                        .ToList();
            //if ((adminsToDelet.Count == Broadcasters.Count) && adminsToInsert.Count == 0)
            //{
            //    throw new JsonCustomException(ControllerError.ajaxErrorGroupAdminDelete);
            //}
            foreach (var adminToDel in adminsToDelet)
            {
                smrToUpdate.Broadcasters.Remove(adminToDel);
            }
            foreach (var adminToInsert in adminsToInsert)
            {
                smrToUpdate.Broadcasters.Add(adminToInsert);
            }
        }

        private void UpSertSeminarAudiences(string selectedItems, Seminar smrToUpdate)
        {
            if (smrToUpdate.Audiences == null)
            {
                smrToUpdate.Audiences = new List<ActiveUser>();
            }
            var excludedUsers = smrToUpdate.Broadcasters.Select(c=>c.UserId).ToList();
            excludedUsers.Add(smrToUpdate.moderatorId);
            var selectedUsersHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Take((int)smrToUpdate.maxAudiences)
                                                   .Select(u => (int)EncryptionHelper.Unprotect(u)).Except(excludedUsers))
                                      : new HashSet<int>();
            var GroupAudiences = smrToUpdate.Audiences != null
                                  ? new HashSet<int>(smrToUpdate.Audiences.Select(c => c.UserId))
                                  : new HashSet<int>();
            var audiencesToDelet = GroupAudiences.Except(selectedUsersHS).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            var udiencesToInsert = selectedUsersHS.Except(GroupAudiences).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            foreach (var audienceToDel in audiencesToDelet)
            {
                smrToUpdate.Audiences.Remove(audienceToDel);
            }
            foreach (var audienceToInsert in udiencesToInsert)
            {
                smrToUpdate.Audiences.Add(audienceToInsert);
            }
        }

    }
}