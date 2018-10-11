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
    [Authorize(Roles = "CoAdmin")]
    public class ServiceController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult CompanyServices(int CoId)
        {
            var services = unitOfWork.ServiceRepository.Get(p => p.coID == CoId);
            return View(services);
        }

        [AllowAnonymous]
        public ActionResult CompanyServicesPartial(int CoId)
        {
            var services = unitOfWork.ServiceRepository.Get(p => p.coID == CoId);
            return PartialView(services);
        }

        [AllowAnonymous]
        public ActionResult RelatedServices(int? UId, int? EvId, int? PrId, int? SrId, int? CoId)
        {
            IEnumerable<Service> service = Enumerable.Empty<Service>();
            IEnumerable<Category> categories = Enumerable.Empty<Category>();
            if (UId != null)
            {
                categories = unitOfWork.ActiveUserRepository
                                       .GetByID(UId)
                                       .Professions
                                       .SelectMany(d => d.categories);
            }
            else if (EvId != null)
            {
                categories = unitOfWork.EventRepository
                                       .GetByID(EvId)
                                       .Categories;
            }
            else if (CoId != null)
            {
                service = unitOfWork.ServiceRepository.Get(p => p.coID == CoId);
                categories = unitOfWork.CategoryRepository
                                       .Get(p => p.Coomopanies.Any(c => c.coID == CoId));
            }
            else if (PrId != null)
            {
                categories = unitOfWork.ProductRepository
                                       .GetByID(PrId)
                                       .categories;
            }
            else if (SrId != null)
            {
                service = unitOfWork.ServiceRepository.Get(s => s.serviceID == SrId);
                categories = unitOfWork.ServiceRepository
                                       .GetByID(SrId)
                                       .categories;
            }

            IEnumerable<RelatedServiceViewModel> products = categories
                                                            .SelectMany(p => p.Services)
                                                            .Except(service)
                                                            .GroupBy(g => g)
                                                            .Select(g => new RelatedServiceViewModel { service = g.Key, relevance = g.Count() })
                                                            .OrderByDescending(f => f.relevance);

            return PartialView(products);
        }

        [AllowAnonymous]
        public ActionResult Detail(int SrId)
        {
            var service = unitOfWork.ServiceRepository.GetByID(SrId);
            if (service == null)
            {
                return new RedirectToError();
            }
            return View(service);
        }


        public ActionResult Create(string CoId)
        {
            NullChecker.NullCheck(new object[] { CoId });

            var curentCompany = unitOfWork.NotExpiredCompanyRepository
                                          .GetByID(EncryptionHelper.Unprotect(CoId));
            if (curentCompany == null
                || !curentCompany.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                return new RedirectToError();
            }
            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.countries;

            ViewData["company"] = CoId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create([Bind(Include = "serviceName,serviceNameEN,brandName,brandNameEN,about,aboutEN,serviceCountryID,serviceEra,price,afterSale,regDate")] Service service,
                                   string CoId, string company, string filesToUpload, string Cats)
        {
            NullChecker.NullCheck(new object[] { CoId, company });

            if (ModelState.IsValid)
            {
                var currentUser = WebSecurity.CurrentUserId;
                var coid = EncryptionHelper.Unprotect(company);
                var currenyCompany = unitOfWork.NotExpiredCompanyRepository.GetByID(coid);
                if (CoId == company && currenyCompany.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
                {
                    service.regDate = DateTime.UtcNow;
                    service.coID = currenyCompany.coID;
                    unitOfWork.ServiceRepository.Insert(service);
                    UpdSertProductCats(Cats, service);
                    unitOfWork.Save();

                    var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, String.Empty, "Service");
                    service.image = fileUploadResult.ImagesToUpload;
                    service.document = fileUploadResult.DocsToUpload;

                    unitOfWork.ServiceRepository.Update(service);
                    unitOfWork.Save();
                    UrlHelper Url = new UrlHelper(Request.RequestContext);
                    return Json(new { URL = Url.Action("Detail", "Service", new { SrId = service.serviceID, SrName = StringHelper.URLName(service.CultureServiceName) }) });
                }
                throw new JsonCustomException(ControllerError.ajaxErrorProductAdmin);
            }
            throw new ModelStateException(this.ModelState);
        }

        public ActionResult Edit(string SrId)
        {
            NullChecker.NullCheck(new object[] { SrId });

            var serviceToEdit = unitOfWork.ServiceRepository.GetByID(EncryptionHelper.Unprotect(SrId));
            if (serviceToEdit.company is CompanyExpired
                || !serviceToEdit.company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                return new RedirectToError();
            }

            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.countries;

            ViewData["service"] = SrId;
            var catss = serviceToEdit.categories.Select(u => u.catID);
            ViewData["Cats"] = String.Join(",", catss);
            return View(serviceToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        [SiteMapCacheRelease]
        public ActionResult Edit(string SrId, string service, string filesToUpload, string Cats)
        {
            NullChecker.NullCheck(new object[] { SrId, service });

            var serviceToEdit = unitOfWork.ServiceRepository.GetByID(EncryptionHelper.Unprotect(SrId));
            if (TryUpdateModel(serviceToEdit, "", new string[] {
                    "serviceName","serviceNameEN","brandName","brandNameEN","about","aboutEN",
                    "serviceCountryID","serviceEra","price","afterSale","regDate" }))
            {
                if (serviceToEdit.company.Admins.Any(g => AuthorizationHelper.isRelevant(g.UserId)))
                {

                    UpdSertProductCats(Cats, serviceToEdit);

                    var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, serviceToEdit.image + serviceToEdit.document, "Service");
                    serviceToEdit.image = fileUploadResult.ImagesToUpload;
                    serviceToEdit.document = fileUploadResult.DocsToUpload;

                    unitOfWork.ServiceRepository.Update(serviceToEdit);
                    unitOfWork.Save();
                    UrlHelper Url = new UrlHelper(Request.RequestContext);
                    return Json(new { URL = Url.Action("Detail", "Service", new { SrId = serviceToEdit.serviceID, SrName = StringHelper.URLName(serviceToEdit.CultureServiceName) }) });
                }
                throw new JsonCustomException(ControllerError.ajaxErrorProductAdmin);
            }
            throw new ModelStateException(this.ModelState);
            
        }

        public ActionResult Delete(string SrId)
        {
            NullChecker.NullCheck(new object[] { SrId });

            var serviceToDelete = unitOfWork.ServiceRepository.GetByID(EncryptionHelper.Unprotect(SrId));
            if (serviceToDelete == null || !serviceToDelete.company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                return new RedirectToError();
            }
            return View(serviceToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        public ActionResult Delete(string S, string STD)
        {
            NullChecker.NullCheck(new object[] { STD, S });
            var std = EncryptionHelper.Unprotect(STD);
            var s = EncryptionHelper.Unprotect(S);
            Service serviceToDelete = unitOfWork.ServiceRepository.GetByID(std);

            if (std == s && serviceToDelete.company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                //unitOfWork.LikeCommentRepository.Get(d => d.type == LikeCommentType.Service && serviceToDelete.Comment.Select(t => t.cmtID).Contains(d.elemID)).ToList().ForEach(unitOfWork.LikeCommentRepository.Delete);
                unitOfWork.LikeCommentRepository.Get(d => serviceToDelete.Comment.Select(f => f.cmtID).Contains(d.elemID) && d.type == LikeCommentType.Service).ToList().ForEach(unitOfWork.LikeCommentRepository.Delete);
                //serviceToDelete.Comment.Select(t => t.cmtID).ToList().ForEach(unitOfWork.LikeCommentRepository.Delete); //ProductCommentsLikesDelete
                serviceToDelete.Comment.Select(t => t.cmtID as object).ToList().ForEach(unitOfWork.CommentServiceRepository.Delete);//ProductCommentsDelete
                serviceToDelete.Likes.Select(l => l.likeID as object).ToList().ForEach(unitOfWork.LikeServiceRepository.Delete);//ProductLikesDelete
                //serviceToDelete.Notifications.Select(n => n.notificationID as object).ToList().ForEach(unitOfWork.NotificationRepository.Delete);//ProductNotificationsDelete

                var fileUploadResult = UploadHelper.UpdateUploadedFiles(String.Empty, serviceToDelete.image + serviceToDelete.document, "Question");
                if (String.IsNullOrEmpty(fileUploadResult.ImagesToUpload) && String.IsNullOrEmpty(fileUploadResult.DocsToUpload))
                {
                    unitOfWork.ServiceRepository.Delete(serviceToDelete);
                    unitOfWork.Save();
                }
                UrlHelper Url = new UrlHelper(Request.RequestContext);
                return Json(new { URL = Url.Action("CompanyServices", "Service", new { CoId = serviceToDelete.coID }) });
            }
            throw new JsonCustomException(ControllerError.ajaxErrorProductDelete);
        }


        private void UpdSertProductCats(string selectedItems, Service sericeToUpdate)
        {
            if (sericeToUpdate.categories == null)
            {
                sericeToUpdate.categories = new List<Category>();
            }
            sericeToUpdate.categories = new List<Category>();
            var selectedCategoriesHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' }).Take(ITTConfig.MaxCategoryTagsLimit).Select(u => int.Parse(u)))
                                       : new HashSet<int>();
            var ServiceCategories = sericeToUpdate.categories != null
                                       ? new HashSet<int>(sericeToUpdate.categories.Select(c => c.catID))
                                       : new HashSet<int>();
            IEnumerable<Category> catsToDelet = ServiceCategories.Except(selectedCategoriesHS).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();
            IEnumerable<Category> catsToInsert = selectedCategoriesHS.Except(ServiceCategories).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();

            foreach (var catToDel in catsToDelet)
            {
                sericeToUpdate.categories.Remove(catToDel);
            }
            foreach (var catToInsert in catsToInsert)
            {
                sericeToUpdate.categories.Add(catToInsert);
            }
        }
    }
}
