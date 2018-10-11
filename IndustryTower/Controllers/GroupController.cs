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
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class GroupController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult GroupPage(int GId)
        {
            var group = unitOfWork.GroupRepository.GetByID(GId);
            return View(group);
        }

        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        [HostControl]
        [AjaxRequestOnly]
        public ActionResult Create([Bind(Include = "groupName,groupDesc,isPublic")] Group group, string Cats, string professionTags, string UserTags)
        {
            
            if (ModelState.IsValid)
            {
                if (AuthorizationHelper.isAdmin())
                {
                    UpSertGroupCats(Cats, group);
                    UpSertGroupProffs(professionTags, group);
                    UpSertGroupAdmins(UserTags, group);
                    UpSertGroupMembers(UserTags, group);
                    group.registerDate = DateTime.UtcNow;

                    unitOfWork.GroupRepository.Insert(group);

                    
                    unitOfWork.Save();

                    return Json(new { Success = true, Url = Url.Action("GroupPage", new { GId = group.groupId, GName = StringHelper.URLName(group.groupName) }) });
                }
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            throw new ModelStateException(this.ModelState);
        }


        public ActionResult Edit(string GId)
        {
            NullChecker.NullCheck(new object[] { GId });

            var groupToEdit = unitOfWork.GroupRepository.GetByID(EncryptionHelper.Unprotect(GId));
            if (!groupToEdit.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                return new RedirectToError();
            }
            var catss = groupToEdit.Categories.Select(u => u.catID);
            ViewData["Cats"] = String.Join(",", catss);
            return View(groupToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        [SiteMapCacheRelease]
        public ActionResult Edit(string GId, string G, string Cats, string professionTags, string UserTags)
        {
            NullChecker.NullCheck(new object[] { GId,G });
            var gid = EncryptionHelper.Unprotect(GId);
            var g = EncryptionHelper.Unprotect(G);
            var groupToEdit = unitOfWork.GroupRepository.GetByID(gid);
            if (groupToEdit.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)) && g == gid)
            {
                if (TryUpdateModel(groupToEdit, "", new string[] { "groupDesc" }))
                {
                    UpSertGroupCats(Cats, groupToEdit);
                    UpSertGroupProffs(professionTags, groupToEdit);
                    UpSertGroupAdmins(UserTags, groupToEdit);
                    unitOfWork.GroupRepository.Update(groupToEdit);
                    unitOfWork.Save();
                    return Json(new { Success = true, Url = Url.Action("GroupPage", new { GId = groupToEdit.groupId, GName = StringHelper.URLName(groupToEdit.groupName) }) });
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        [AllowAnonymous]
        public ActionResult Members(int GId)
        {
            var members = unitOfWork.GroupRepository.GetByID(GId).Members;
            return PartialView("~/Views/UserProfile/_PartialUsers.cshtml", members.ToList());
        }

        [AllowAnonymous]
        public ActionResult Admins(int GId)
        {
            var admins = unitOfWork.GroupRepository.GetByID(GId).Admins;
            return PartialView("~/Views/UserProfile/_PartialUsers.cshtml", admins.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        public ActionResult Membership(int GId)
        {
            var group = unitOfWork.GroupRepository.GetByID(GId);
            if (!group.isPublic)
            {
                throw new JsonCustomException("The Group Is not Public");
            }
            String returnString = Resource.Resource.groupMembership;
            String returnMessage = Resource.Resource.groupLeaveMessage;
            var user = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
            if (group.Members.Any(f => f.UserId == WebSecurity.CurrentUserId))
            {
                group.Members.Remove(user);
            }
            else 
            {
                group.Members.Add(user);
                returnString = Resource.Resource.groupLeave;
                returnMessage = Resource.Resource.groupMembershipMessage;
            }
            unitOfWork.Save();

            return Json(new { Message = returnMessage, Result = returnString });
        }

        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult Delete(string GId)
        {
            NullChecker.NullCheck(new object[] { GId });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        [HostControl]
        [AjaxRequestOnly]
        [SiteMapCacheRelease]
        public ActionResult Delete(string GId, int i)
        {
            NullChecker.NullCheck(new object[] { GId });
            var groupTpDel = unitOfWork.GroupRepository.GetByID(EncryptionHelper.Unprotect(GId));
            //groupTpDel.Sessions.SelectMany(s => s.Offers.SelectMany(o => o.Comments.SelectMany(c => c.Likes.Select(l => l.likeID as object)))).ToList().ForEach(unitOfWork.LikeRepository.Delete);
            //groupTpDel.Sessions.SelectMany(s => s.Offers.SelectMany(o => o.Comments.Select(c => c.cmtID as object))).ToList().ForEach(unitOfWork.CommentRepository.Delete);
            //groupTpDel.Sessions.SelectMany(s => s.Offers.SelectMany(o => o.Likes.Select(l => l.likeID as object))).ToList().ForEach(unitOfWork.LikeRepository.Delete);
            //groupTpDel.Sessions.SelectMany(s => s.Offers.Select(o => o.offerId as object)).ToList().ForEach(unitOfWork.GroupSessionOfferRepository.Delete);
            //groupTpDel.Sessions.Select(s => s.sessionId as object).ToList().ForEach(unitOfWork.GroupSessionRepository.Delete);
            unitOfWork.GroupRepository.Delete(groupTpDel);
            unitOfWork.Save();
            return View();
        }

        public ActionResult AddUsers(string GId)
        {
            NullChecker.NullCheck(new object[] { GId });
            var group = unitOfWork.GroupRepository.GetByID(EncryptionHelper.Unprotect(GId));
            ViewData["GId"] = GId;
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        [AjaxRequestOnly]
        public ActionResult AddUsers(string GId, string UserTags)
        {
            var gid = EncryptionHelper.Unprotect(GId);
            var group = unitOfWork.GroupRepository.GetByID(gid);
            UpSertGroupMembers(UserTags, group);
            unitOfWork.Save();
            return Json(new { Success = true, Url = Url.Action("GroupPage", "Group", new { GId = gid, GName = StringHelper.URLName(group.groupName) }) });
        }

        public ActionResult UserGroups()
        {
            var userGroups = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
            return PartialView(userGroups.GroupsMembered);
        }

        [AllowAnonymous]
        public ActionResult UserInfoGroups(int UId)
        {
            var userGroups = unitOfWork.ActiveUserRepository.GetByID(UId);
            return PartialView("~/Views/Group/UserGroups.cshtml", userGroups.GroupsMembered);
        }

        private void UpSertGroupProffs(string selectedItems, Group groupToUpdate)
        {
            if (groupToUpdate.Professions == null)
            {
                groupToUpdate.Professions = new List<Profession>();
            }
            groupToUpdate.Professions = new List<Profession>();
            var selectedProfessionsHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' }).Take(ITTConfig.MaxProfessionTagsLimit).Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var groupProfessions = groupToUpdate.Professions != null
                                      ? new HashSet<int>(groupToUpdate.Professions.Select(c => c.profID))
                                      : new HashSet<int>();
            var proffsToDelet = groupProfessions.Except(selectedProfessionsHS).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();
            var proffsToInsert = selectedProfessionsHS.Except(groupProfessions).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();

            foreach (var proffToDel in proffsToDelet)
            {
                groupToUpdate.Professions.Remove(proffToDel);
            }
            foreach (var proffToInsert in proffsToInsert)
            {
                groupToUpdate.Professions.Add(proffToInsert);
            }
        }


        private void UpSertGroupCats(string selectedItems, Group groupToUpdate)
        {
            if (groupToUpdate.Categories == null)
            {
                groupToUpdate.Categories = new List<Category>();
            }
            var selectedCategoriesHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Take(ITTConfig.MaxCategoryTagsLimit)
                                                   .Select(u => int.Parse(u)))
                                      : new HashSet<int>();
            var GroupCategories = groupToUpdate.Categories != null
                                  ? new HashSet<int>(groupToUpdate.Categories.Select(c => c.catID))
                                  : new HashSet<int>();
            var catsToDelet = GroupCategories.Except(selectedCategoriesHS).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();
            var catsToInsert = selectedCategoriesHS.Except(GroupCategories).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();

            foreach (var catToDel in catsToDelet)
            {
                groupToUpdate.Categories.Remove(catToDel);
            }
            foreach (var catToInsert in catsToInsert)
            {
                groupToUpdate.Categories.Add(catToInsert);
            }

        }

        private void UpSertGroupAdmins(string selectedItems, Group groupToUpdate)
        {
            if (groupToUpdate.Admins == null)
            {
                groupToUpdate.Admins = new List<ActiveUser>();
            }
            var selectedUsersHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Take(ITTConfig.MaxAdminsLimit)
                                                   .Select(u => (int)EncryptionHelper.Unprotect(u)))
                                      : new HashSet<int>();
            var GroupAdmins = groupToUpdate.Admins != null
                                  ? new HashSet<int>(groupToUpdate.Admins.Select(c => c.UserId))
                                  : new HashSet<int>();
            var adminsToDelet = GroupAdmins.Except(selectedUsersHS).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            var adminsToInsert = selectedUsersHS.Except(GroupAdmins).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            if ((adminsToDelet.Count == GroupAdmins.Count) && adminsToInsert.Count == 0)
            {
                throw new JsonCustomException(ControllerError.ajaxErrorGroupAdminDelete);
            }
            foreach (var adminToDel in adminsToDelet)
            {
                groupToUpdate.Admins.Remove(adminToDel);
            }
            foreach (var adminToInsert in adminsToInsert)
            {
                groupToUpdate.Admins.Add(adminToInsert);
            }

        }

        private void UpSertGroupMembers(string selectedItems, Group groupToUpdate)
        {
            if (groupToUpdate.Members == null)
            {
                groupToUpdate.Members = new List<ActiveUser>();
            }
            var selectedUsersHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Select(u => (int)EncryptionHelper.Unprotect(u)))
                                      : new HashSet<int>();
            var GroupMembers = groupToUpdate.Members != null
                                  ? new HashSet<int>(groupToUpdate.Members.Select(c => c.UserId))
                                  : new HashSet<int>();
            var usersToDelet = GroupMembers.Except(selectedUsersHS).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            var usersToInsert = selectedUsersHS.Except(GroupMembers).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            var admins = groupToUpdate.Admins;
            foreach (var userToDel in usersToDelet)
            {
                if (!admins.Any(a => a.UserId == userToDel.UserId))
                {
                    groupToUpdate.Members.Remove(userToDel);
                }
            }
            foreach (var userToInsert in usersToInsert)
            {
                groupToUpdate.Members.Add(userToInsert);
            }

        }
    }
}