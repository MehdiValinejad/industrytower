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
    [Authorize(Roles="CoAdmin,ITAdmin")]
    public class CompanyController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Edit(string CoId)
        {
            NullChecker.NullCheck(new object[] { CoId });

            var CoToEdit = unitOfWork.NotExpiredCompanyRepository
                                     .GetByID(EncryptionHelper.Unprotect(CoId));
            if (!CoToEdit.Admins.Any(c => AuthorizationHelper.isRelevant(c.UserId)))
            {
                return new RedirectToNotFound();
            }

            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.counts(CoToEdit.State);
            ViewBag.StateDropDown = countries.states(CoToEdit.State);

            //PorT SelectList
            var PorTs = from CompanySize e in Enum.GetValues(typeof(CompanySize))
                        select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewBag.CoSizeDropdown = new SelectList(PorTs, "Id", "Name");

            var catss = CoToEdit.Categories.Select(u => u.catID);
            ViewData["Cats"] = String.Join(",", catss);
            ViewData["company"] = CoId;

            return View(CoToEdit);
        }

        [HttpPost]
        [AjaxRequestOnly]
        [HostControl]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        public ActionResult Edit(string CoId, string company, string Cats)
        {
            NullChecker.NullCheck(new object[] { CoId, company });

            var coToEdit = unitOfWork.NotExpiredCompanyRepository
                                     .GetByID(EncryptionHelper.Unprotect(CoId));
            if (coToEdit.Admins.Any(g => AuthorizationHelper.isRelevant(g.UserId)))
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(coToEdit, "", new string[] {
                    "coName","coNameEN","about","aboutEN",
                    "companySize","stateID","countryCode","stateCode","regCode","iranCode",
                    "phoneNo","faxNo","email","website","address","addressEN" }))
                    {

                        UpSertCompanyCats(Cats, coToEdit);

                        unitOfWork.CompanyRepository.Update(coToEdit);
                        unitOfWork.Save();
                        UrlHelper Url = new UrlHelper(Request.RequestContext);
                        return Json(new { URL = Url.Action("CProfile", "Company", new { CoId = coToEdit.coID, CoName = StringHelper.URLName(coToEdit.CultureCoName) }) });

                    }
                }
                throw new ModelStateException(this.ModelState);
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        [AllowAnonymous]
       
        [ValidateInput(false)]
        public ActionResult CProfile(int CoId)
        {
            var company = unitOfWork.NotExpiredCompanyRepository
                                    .GetByID(CoId);
            return View(company);
        }


        [AjaxRequestOnly]
        [HostControl]
        public ActionResult LogoPic(string CoId)
        {
            NullChecker.NullCheck(new object[] { CoId });
            var com = unitOfWork.NotExpiredCompanyRepository.GetByID(EncryptionHelper.Unprotect(CoId));
            if (com.Admins.Any(g => !AuthorizationHelper.isRelevant(g.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            ViewData["CoId"] = CoId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult LogoPic(int x, int y, int w, int h, string picToUpload, string company)
        {
            NullChecker.NullCheck(new object[] { company });

            var currentUser = WebSecurity.CurrentUserId;
            var companyToEdit = unitOfWork.NotExpiredCompanyRepository
                                          .GetByID(EncryptionHelper.Unprotect(company));
            if (companyToEdit.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                UploadHelper.CropImage(x, y, w, h, picToUpload);
                UploadHelper.moveFile(picToUpload, "Company");
                if (companyToEdit.logo != null)
                {
                    UploadHelper.deleteFile(companyToEdit.logo, "Company");
                }
                companyToEdit.logo = picToUpload;
                unitOfWork.CompanyRepository.Update(companyToEdit);
                unitOfWork.Save();
                return Json(new { Success = true });
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        public ActionResult LogoPicDelete(string CoId, int? co)
        {
            NullChecker.NullCheck(new object[] { CoId });
            var com = unitOfWork.NotExpiredCompanyRepository.GetByID(EncryptionHelper.Unprotect(CoId));
            if (com.Admins.Any(g => !AuthorizationHelper.isRelevant(g.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxError);
            }
            ViewData["CoId"] = CoId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult LogoPicDelete(string company)
        {
            NullChecker.NullCheck(new object[] { company });

            var currentUser = WebSecurity.CurrentUserId;
            var companyToDelete = unitOfWork.NotExpiredCompanyRepository
                                            .GetByID(EncryptionHelper.Unprotect(company));
            if (companyToDelete.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
            {
                UploadHelper.deleteFile(companyToDelete.logo, "Company");
                companyToDelete.logo = null;
                unitOfWork.CompanyRepository.Update(companyToDelete);
                unitOfWork.Save();
                return Json(new { Success = true });
            }
            throw new JsonCustomException(ControllerError.ajaxError);
        }

        [AllowAnonymous]
        public ActionResult RelatedCompanies(int? EvId, int? CoId, int? StId, int? PrId, int? SrId, int? UId)
        {

            IEnumerable<Category> categories = Enumerable.Empty<Category>();
            IEnumerable<Company> company = Enumerable.Empty<Company>();
            if (EvId != null)
            {
                categories = unitOfWork.EventRepository
                                       .GetByID(EvId).Categories;
            }
            else if (CoId != null)
            {
                categories = unitOfWork.NotExpiredCompanyRepository.GetByID(CoId).Categories;
                company = unitOfWork.CompanyRepository.Get(u => u.coID == CoId);
            }
            else if (StId != null)
            {
                categories = unitOfWork.StoreNotExpiredRepository
                                       .GetByID(StId).Categories;
            }
            else if (SrId != null)
            {
                categories = unitOfWork.StoreNotExpiredRepository
                                       .GetByID(SrId).Categories;
            }
            else if (PrId != null)
            {
                categories = unitOfWork.ProductRepository
                                       .GetByID(PrId).categories;
            }
            else if (UId != null)
            {
                categories = unitOfWork.ActiveUserRepository
                                       .GetByID(UId)
                                       .Professions
                                       .SelectMany(c => c.categories);
            }

            IEnumerable<RelatedCompanyViewModel> companies = categories.SelectMany(c => c.Coomopanies.Where(f => f is CompanyNotExpired))
                                                      .Except(company)
                                                      .GroupBy(g => g)
                                                      .Select(g => new RelatedCompanyViewModel { company = g.Key, relevance = g.Count() })
                                                      .OrderByDescending(f => f.relevance)
                                                      .Take(7);

            return PartialView(companies);
        }

        [AllowAnonymous]
        public ActionResult _CompaniesSearchPartial(string searchString)
        {
            var companies = unitOfWork.NotExpiredCompanyRepository.Get(c => c.coName.Contains(searchString)
                                                                       || c.coNameEN.Contains(searchString)).Take(10);
            return PartialView(companies);
        }


        private void UpSertCompanyCats(string selectedItems, CompanyNotExpired companyToUpdate)
        {
            if (companyToUpdate.Categories == null)
            {
                companyToUpdate.Categories = new List<Category>();
            }
            var selectedCategoriesHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                    .Take(ITTConfig.MaxCategoryTagsLimit)
                                                    .Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var ProductCategories = companyToUpdate.Categories != null
                                       ? new HashSet<int>(companyToUpdate.Categories.Select(c => c.catID))
                                       : new HashSet<int>();
            IEnumerable<Category> catsToDelet = ProductCategories.Except(selectedCategoriesHS).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();
            IEnumerable<Category> catsToInsert = selectedCategoriesHS.Except(ProductCategories).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();

            foreach (var catToDel in catsToDelet)
            {
                companyToUpdate.Categories.Remove(catToDel);
            }
            foreach (var catToInsert in catsToInsert)
            {
                companyToUpdate.Categories.Add(catToInsert);
            }
        }

    }
}
