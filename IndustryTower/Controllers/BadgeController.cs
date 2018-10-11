using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{
    [ITTAuthorize(Roles="ITAdmin")]
    public class BadgeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var badges = unitOfWork.BadgeRepository.Get();
            return View(badges);
        }

        [AllowAnonymous]
        public ActionResult Detail(int BgId)
        {
            var badge = unitOfWork.BadgeRepository.GetByID(BgId);
            return View(badge);
        }

        [AllowAnonymous]
        public ActionResult BadgeUsers(int BgId)
        {
            var users = unitOfWork.ActiveUserRepository.Get(d => d.Badges.Any(g => g.Badge.badgeId == BgId));
            return PartialView("~/Views/UserProfile/AvatarsPartial.cshtml",users);
        }

        [AllowAnonymous]
        public ActionResult UserBadges(int UId)
        {
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                            "UInfoBadges",
                            new SqlParameter("UId", UId),
                            new SqlParameter("isNotEN", ITTConfig.CurrentCultureIsNotEN));
            IList<UserBadgesViewModel> badges = new List<UserBadgesViewModel>();
            while (reader.Read())
            {
                badges.Add(new UserBadgesViewModel
                {
                    bdgId = reader.GetInt32(0),
                    name = reader[1] as string,
                    color = (BadgeColor)reader.GetInt32(2),
                    count = reader.GetInt32(3)
                });
            }
            return PartialView(badges);
        }

        public ActionResult Create()
        {

            var badges = from BadgeColor e in Enum.GetValues(typeof(BadgeColor))
                           select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewData["badgeColSL"] = new SelectList(badges, "Id", "Name");
          
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "color,name,nameEN,desc,descEN")]Badge badge)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.BadgeRepository.Insert(badge);
                unitOfWork.Save();
                return RedirectToAction("Detail", new { BgId = badge.badgeId });
            }

            var badges = from BadgeColor e in Enum.GetValues(typeof(BadgeColor))
                         select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewData["badgeColSL"] = new SelectList(badges, "Id", "Name", badge.color);
            return View();
        }


        public ActionResult Edit(int BgId)
        {
            
            var bdg = unitOfWork.BadgeRepository.GetByID(BgId);

            var badges = from BadgeColor e in Enum.GetValues(typeof(BadgeColor))
                         select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewData["badgeColSL"] = new SelectList(badges, "Id", "Name", bdg.color);

            
            return View(bdg);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int BgId, FormCollection formcollection)
        {
            var bdg = unitOfWork.BadgeRepository.GetByID(BgId);
            if (TryUpdateModel(bdg, "", new string[] { "color", "name", "nameEN", "desc", "descEN" }))
            {
                unitOfWork.BadgeRepository.Update(bdg);
                unitOfWork.Save();
                return RedirectToAction("Detail", new { BgId = bdg.badgeId });
            }

            var badges = from BadgeColor e in Enum.GetValues(typeof(BadgeColor))
                         select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewData["badgeColSL"] = new SelectList(badges, "Id", "Name", bdg.color);
            return View();
        }


        public ActionResult AddUsers(int BgId)
        {
            var badge = unitOfWork.BadgeRepository.Get(d=>d.badgeId == BgId,includeProperties:"Users").Single();
            return View(badge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult AddUsers(string B, string UserTags)
        {
            NullChecker.NullCheck(new object[] { B, UserTags });
            var bgid = EncryptionHelper.Unprotect(B);
            var bdg = unitOfWork.BadgeRepository.GetByID(bgid);
            UpSertBadgeUsers(unitOfWork,UserTags, bdg);
            unitOfWork.Save();
            return Json(new { Success = true, Url = Url.Action("Detail", "Badge", new { BgId = bgid }) });
        }


        private void UpSertBadgeUsers(UnitOfWork unitOfWork,string selectedItems, Badge badgeToUpdate)
        {
            if (badgeToUpdate.Users == null)
            {
                badgeToUpdate.Users = new List<BadgeUser>();
            }
            var selectedUsersHS = !String.IsNullOrWhiteSpace(selectedItems)
                                      ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                   .Select(u => (int)EncryptionHelper.Unprotect(u)))
                                      : new HashSet<int>();
            var badgeUsers = badgeToUpdate.Users != null
                                  ? new HashSet<int>(badgeToUpdate.Users.Select(c => c.User.UserId))
                                  : new HashSet<int>();
            var usersToDelet = badgeUsers.Except(selectedUsersHS).Select(t => unitOfWork.BadgeUserRepository.Get(f => f.usrId == t && f.bdgId == badgeToUpdate.badgeId).Single().bdgId).ToList();
            var usersToInsert = selectedUsersHS.Except(badgeUsers).Select(t => new BadgeUser 
            {
                bdgId = badgeToUpdate.badgeId,
                usrId = t,
                date = DateTime.UtcNow
            }).ToList();

            foreach (var userToDel in usersToDelet)
            {
                unitOfWork.BadgeUserRepository.Delete(userToDel);
            }
            foreach (var userToInsert in usersToInsert)
            {
                unitOfWork.BadgeUserRepository.Insert(userToInsert);
            }
        }

    }
}