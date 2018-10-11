using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [Authorize(Roles="StAdmin,ITAdmin")]
    public class StoreController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public ActionResult Edit(string StId)
        {
            NullChecker.NullCheck(new object[] { StId });
            var StToEdit = unitOfWork.StoreNotExpiredRepository
                                     .GetByID((int)EncryptionHelper.Unprotect(StId));
            if (!StToEdit.Admins.Any(c => AuthorizationHelper.isRelevant(c.UserId)))
            {
                return new RedirectToNotFound();
            }

            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.counts(StToEdit.State);
            ViewBag.StateDropDown = countries.states(StToEdit.State);

            var catss = StToEdit.Categories.Select(u => u.catID);
            ViewData["Cats"] = String.Join(",", catss);
            ViewData["store"] = StId;

            return View(StToEdit);
        }

        [HttpPost]
        [AjaxRequestOnly]
        [HostControl]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        public ActionResult Edit(string StId, string store, string Cats)
        {
            NullChecker.NullCheck(new object[] { StId, store });
            var stid = EncryptionHelper.Unprotect(StId);
            var storeid = EncryptionHelper.Unprotect(store);

            var stToEdit = unitOfWork.StoreNotExpiredRepository
                                     .GetByID(stid);
            if (!stToEdit.Admins.Any(c => AuthorizationHelper.isRelevant(c.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            if (stid == storeid) 
            {
                if (TryUpdateModel(stToEdit, "", new string[] {
                    "storeName","storeNameEN","about","aboutEN",
                    "stateID","countryCode","stateCode","regCode","iranCode",
                    "phoneNo","faxNo","email","website","address","addressEN" }))
                {
                    UpSertStoreCats(Cats, stToEdit);

                    unitOfWork.StoreNotExpiredRepository.Update(stToEdit);
                    unitOfWork.Save();
                    UrlHelper Url = new UrlHelper(Request.RequestContext);
                    return Json(new { URL = Url.Action("SProfile", "Store", new { StId = stToEdit.storeID, StName = StringHelper.URLName(stToEdit.CultureStoreName) }) });
                }

                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        [AllowAnonymous]
        public ActionResult SProfile(int StId)
        {
            var store = unitOfWork.StoreNotExpiredRepository
                                  .GetByID(StId);
            return View(store);
        }


        [AjaxRequestOnly]
        [HostControl]
        public ActionResult LogoPic(string StId)
        {
            NullChecker.NullCheck(new object[] { StId });
            var store = unitOfWork.StoreNotExpiredRepository.GetByID(EncryptionHelper.Unprotect(StId));
            if (store.Admins.Any(g=>!AuthorizationHelper.isRelevant(g.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            ViewData["StId"] = StId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult LogoPic(int x, int y, int w, int h, string picToUpload, string store)
        {
            NullChecker.NullCheck(new object[] { store });

            var currentUser = WebSecurity.CurrentUserId;
            var storeToEdit = unitOfWork.StoreNotExpiredRepository
                                        .GetByID(EncryptionHelper.Unprotect(store));
            if (storeToEdit.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                UploadHelper.CropImage(x, y, w, h, picToUpload);
                UploadHelper.moveFile(picToUpload, "Store");
                if (storeToEdit.logo != null)
                {
                    UploadHelper.deleteFile(storeToEdit.logo, "Store");
                }
                storeToEdit.logo = picToUpload;
                unitOfWork.StoreNotExpiredRepository.Update(storeToEdit);
                unitOfWork.Save();
                return Json(new { Success = true });
            }
            throw new JsonCustomException(ControllerError.ajaxErrorProfilePicUpload);
        }


        public ActionResult LogoPicDelete(string StId,string gid)
        {
            NullChecker.NullCheck(new object[] { StId });
            var store = unitOfWork.StoreNotExpiredRepository.GetByID(EncryptionHelper.Unprotect(StId));
            if (store.Admins.Any(g => !AuthorizationHelper.isRelevant(g.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            ViewData["StId"] = StId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult LogoPicDelete(string store)
        {
            NullChecker.NullCheck(new object[] { store });

            var currentUser = WebSecurity.CurrentUserId;
            var storeToDelete = unitOfWork.StoreNotExpiredRepository
                                          .GetByID(EncryptionHelper.Unprotect(store));
            if (storeToDelete.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                UploadHelper.deleteFile(storeToDelete.logo, "Store");
                storeToDelete.logo = null;
                unitOfWork.StoreNotExpiredRepository.Update(storeToDelete);
                unitOfWork.Save();
                return Json(new { Success = true });
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        [AllowAnonymous]
        public ActionResult RelatedStores(int? EvId, int? CoId, int? StId, int? PrId, int? SrId, int? UId)
        {
            IEnumerable<Category> categories = Enumerable.Empty<Category>();
            IEnumerable<Store> store = Enumerable.Empty<Store>();
            if (EvId != null)
            {
                categories = unitOfWork.EventRepository
                                       .GetByID(EvId)
                                       .Categories;
            }
            else if (CoId != null)
            {
                categories = unitOfWork.NotExpiredCompanyRepository
                                       .GetByID(CoId)
                                       .Categories;
            }
            else if (StId != null)
            {
                categories = unitOfWork.StoreNotExpiredRepository.GetByID(StId).Categories;
                store = unitOfWork.StoreRepository.Get(f => f.storeID == StId);
            }
            else if (PrId != null)
            {
                categories = unitOfWork.ProductRepository
                                       .GetByID(PrId)
                                       .categories;
            }
            else if (SrId != null)
            {
                categories = unitOfWork.ServiceRepository
                                       .GetByID(SrId)
                                       .categories;
            }
            else if (UId != null)
            {
                categories = unitOfWork.ActiveUserRepository
                                       .GetByID(UId)
                                       .Professions
                                       .SelectMany(c => c.categories);
            }

            IEnumerable<RelatedStoreViewModel> stores = categories.SelectMany(c => c.Stores.Where(f => f is StoreNotExpired))
                                                      .Except(store)
                                                      .GroupBy(g => g)
                                                      .Select(g => new RelatedStoreViewModel { store = g.Key, relevance = g.Count() })
                                                      .OrderByDescending(f => f.relevance)
                                                      .Take(7);

            return PartialView(stores);
        }


        [AllowAnonymous]
        public ActionResult _StoresSearchPartial(string searchString)
        {
            var stores = unitOfWork.StoreNotExpiredRepository.Get(c => c.storeName.Contains(searchString)
                                                                       || c.storeNameEN.Contains(searchString)).Take(10);
            return PartialView(stores);
        }

        private void UpSertStoreCats(string selectedItems, StoreNotExpired storeToUpdate)
        {
            if (storeToUpdate.Categories == null)
            {
                storeToUpdate.Categories = new List<Category>();
            }
            var selectedCategoriesHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                        .Take(ITTConfig.MaxCategoryTagsLimit)
                                                        .Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var ProductCategories = storeToUpdate.Categories != null
                                       ? new HashSet<int>(storeToUpdate.Categories.Select(c => c.catID))
                                       : new HashSet<int>();
            IEnumerable<Category> catsToDelet = ProductCategories.Except(selectedCategoriesHS).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();
            IEnumerable<Category> catsToInsert = selectedCategoriesHS.Except(ProductCategories).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();

            foreach (var catToDel in catsToDelet)
            {
                storeToUpdate.Categories.Remove(catToDel);
            }
            foreach (var catToInsert in catsToInsert)
            {
                storeToUpdate.Categories.Add(catToInsert);
            }
        }

    }
}
