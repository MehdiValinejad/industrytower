using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using Resource;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class DictController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        
        [AllowAnonymous]
        public ActionResult Dictionary(int DId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                "DicDetail",
                new SqlParameter("DId", DId),
                new SqlParameter("U", WebSecurity.CurrentUserId));
            Dict dic = new Dict();
            if (reader.Read())
            {
                dic.dicId = reader.GetInt32(0);
                dic.name = reader[1] as string;
                dic.desc = reader[2] as string;
                dic.date = reader.GetDateTime(3);
                ViewData["isAdmin"] = reader.GetBoolean(4);
            }
            return View(dic);
        }

        [AllowAnonymous]
        public ActionResult TopConts (int DId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                         "DicTopContributors",
                         new SqlParameter("DId", DId),
                         new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));
            IList<ActiveUser> users = new List<ActiveUser>();
            while (reader.Read())
            {
                users.Add(new ActiveUser
                {
                    UserId = reader.GetInt32(0),
                    firstName= reader[1] as string,
                    image = reader[2]as string
                });
            }
            return PartialView("~/Views/UserProfile/UsersPartial.cshtml",users);
        }

        [ITTAuthorizeAttribute(Roles="ITAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create([Bind(Include = "name,desc")] Dict dic, string professionTags, string UserTags)
        {
            if (ModelState.IsValid)
            {
                if (AuthorizationHelper.isAdmin())
                {
                    UpSertGroupProffs(professionTags, dic);
                    UpSertGroupAdmins(UserTags, dic);
                    dic.date = DateTime.UtcNow;

                    unitOfWork.DictionaryRepository.Insert(dic);
                    unitOfWork.Save();

                    return Json(new { Success = true, Url = Url.Action("Dictionary", new { DId = dic.dicId, DName = StringHelper.URLName(dic.name) }) });
                }
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            throw new ModelStateException(this.ModelState);
        }



        public ActionResult Edit(int DId)
        {
            var model = unitOfWork.DictionaryRepository.GetByID(DId);
            if (!model.Admins.Any(d => d.UserId == WebSecurity.CurrentUserId))
            {
                return new RedirectToError();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Edit(int DId, string professionTags, string UserTags)
        {
            var dic = unitOfWork.DictionaryRepository.GetByID(DId);
            if (dic.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                if (TryUpdateModel(dic, "", new string[] { "" }))
                {
                    UpSertGroupProffs(professionTags, dic);
                    UpSertGroupAdmins(UserTags, dic);
                    unitOfWork.DictionaryRepository.Update(dic);
                    unitOfWork.Save();
                    return Json(new { Url = Url.Action("Dictionary", "Dict", new { DId = dic.dicId }) });
                }
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            throw new ModelStateException(this.ModelState);
        }

        private void UpSertGroupProffs(string selectedItems, Dict dicToUpdate)
        {
            if (dicToUpdate.Professions == null)
            {
                dicToUpdate.Professions = new List<Profession>();
            }
            dicToUpdate.Professions = new List<Profession>();
            var selectedProfessionsHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' }).Take(ITTConfig.MaxProfessionTagsLimit).Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var groupProfessions = dicToUpdate.Professions != null
                                      ? new HashSet<int>(dicToUpdate.Professions.Select(c => c.profID))
                                      : new HashSet<int>();
            var proffsToDelet = groupProfessions.Except(selectedProfessionsHS).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();
            var proffsToInsert = selectedProfessionsHS.Except(groupProfessions).Select(t => unitOfWork.ProfessionRepository.GetByID(t)).ToList();

            foreach (var proffToDel in proffsToDelet)
            {
                dicToUpdate.Professions.Remove(proffToDel);
            }
            foreach (var proffToInsert in proffsToInsert)
            {
                dicToUpdate.Professions.Add(proffToInsert);
            }
        }

        private void UpSertGroupAdmins(string selectedItems, Dict dicToUpdate)
        {
            if (dicToUpdate.Admins == null)
            {
                dicToUpdate.Admins = new List<ActiveUser>();
            }
            var selectedUsersHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Take(ITTConfig.MaxAdminsLimit)
                                                   .Select(u => (int)EncryptionHelper.Unprotect(u)))
                                      : new HashSet<int>();
            var GroupAdmins = dicToUpdate.Admins != null
                                  ? new HashSet<int>(dicToUpdate.Admins.Select(c => c.UserId))
                                  : new HashSet<int>();
            var adminsToDelet = GroupAdmins.Except(selectedUsersHS).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            var adminsToInsert = selectedUsersHS.Except(GroupAdmins).Select(t => unitOfWork.ActiveUserRepository.GetByID(t)).ToList();
            if ((adminsToDelet.Count == GroupAdmins.Count) && adminsToInsert.Count == 0)
            {
                throw new JsonCustomException(ControllerError.ajaxErrorGroupAdminDelete);
            }
            foreach (var adminToDel in adminsToDelet)
            {
                dicToUpdate.Admins.Remove(adminToDel);
            }
            foreach (var adminToInsert in adminsToInsert)
            {
                dicToUpdate.Admins.Add(adminToInsert);
            }

        }

    }
}