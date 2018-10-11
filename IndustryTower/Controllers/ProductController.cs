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
    [Authorize(Roles="CoAdmin")]
    public class ProductController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult CompanyProducts(int CoId)
        {
            var products = unitOfWork.ProductRepository.Get(p => p.coID == CoId);
            return View(products);
        }

        [AllowAnonymous]
        public ActionResult CompanyProductsPartial(int CoId)
        {
            var products = unitOfWork.ProductRepository.Get(p => p.coID == CoId);
            return PartialView(products);
        }


        [AllowAnonymous]
        public ActionResult RelatedProducts(int? UId, int? EvId, int? PrId, int? SrId, int? CoId)
        {
            IEnumerable<Product> product = Enumerable.Empty<Product>();
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
                product = unitOfWork.ProductRepository.Get(p => p.coID == CoId);
                categories = unitOfWork.CategoryRepository
                                       .Get(p => p.Coomopanies.Any(c => c.coID == CoId));
            }
            else if (PrId != null)
            {
                product = unitOfWork.ProductRepository
                                    .Get(p => p.productID == PrId);
                categories = product.SelectMany(f=>f.categories);
            }
            else if (SrId != null)
            {
                categories = unitOfWork.ServiceRepository
                                       .GetByID(SrId)
                                       .categories;
            }

            IEnumerable<RelatedProductViewModel> products = categories
                                                            .SelectMany(p => p.Products)
                                                            .Except(product)
                                                            .GroupBy(g => g)
                                                            .Select(g => new RelatedProductViewModel { product = g.Key, relevance = g.Count() })
                                                            .OrderByDescending(f => f.relevance);

            return PartialView(products);
        }

        [AllowAnonymous]
        public ActionResult Detail(int PrId)
        {
            var product = unitOfWork.ProductRepository
                                    .GetByID(PrId);
            if (product == null) 
            {
                return new RedirectToError();
            }
            return View(product);
        }


        public ActionResult Create(string CoId)
        {
            NullChecker.NullCheck(new object[] { CoId });

            var curentCompany = unitOfWork.NotExpiredCompanyRepository
                                          .GetByID(EncryptionHelper.Unprotect(CoId));
            if (curentCompany == null 
                || !curentCompany.Admins.Any(u=>AuthorizationHelper.isRelevant(u.UserId)))
            {
                return new RedirectToError();
            }
            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.countries;


            //PorT SelectList
            var PorTs = from PorT e in Enum.GetValues(typeof(PorT))
                           select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewBag.portDropdown = new SelectList(PorTs, "Id", "Name");

            ViewData["company"] = CoId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create([Bind(Include = "productName,productNameEN,brandName,brandNameEN,about,aboutEN,PorT,productionCountryID,weight,height,width,length,power,shabnam,iranCode,packCount,price,shipFree,wuarantee,guarantee,afterSale")] Product product,
                                   string CoId, string company, string filesToUpload, string Cats)
        {
            NullChecker.NullCheck(new object[] { CoId, company });
            var coid = EncryptionHelper.Unprotect(CoId);
            var comid = EncryptionHelper.Unprotect(company);

            if (ModelState.IsValid)
            {
                var currentUser = WebSecurity.CurrentUserId;
                var currenyCompany = unitOfWork.NotExpiredCompanyRepository
                                               .GetByID(comid);
                if (coid == comid && currenyCompany.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
                {
                    product.regDate = DateTime.UtcNow;
                    product.coID = currenyCompany.coID;
                    unitOfWork.ProductRepository.Insert(product);
                    UpdSertProductCats(Cats, product);
                    unitOfWork.Save();

                    var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, String.Empty, "Product");
                    product.image = fileUploadResult.ImagesToUpload;
                    product.document = fileUploadResult.DocsToUpload;

                    unitOfWork.ProductRepository.Update(product);
                    unitOfWork.Save();
                    UrlHelper Url = new UrlHelper(Request.RequestContext);
                    return Json(new { URL = Url.Action("Detail", "Product", new { PrId = product.productID }), PrName = StringHelper.URLName(product.CultureProductName) });
                }
                throw new JsonCustomException(ControllerError.ajaxErrorProductAdmin);
            }
            throw new ModelStateException(this.ModelState);
        }

        public ActionResult Edit(string PrId)
        {
            NullChecker.NullCheck(new object[] { PrId });

            var productToEdit = unitOfWork.ProductRepository
                                          .GetByID(EncryptionHelper.Unprotect(PrId));
            if (productToEdit.company is CompanyExpired 
                || !productToEdit.company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                return new RedirectToError();
            }

            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.countries;


            //PorT SelectList
            var PorTs = from PorT e in Enum.GetValues(typeof(PorT))
                        select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewBag.portDropdown = new SelectList(PorTs, "Id", "Name");

            ViewData["product"] = PrId;
            var catss = productToEdit.categories.Select(u => u.catID);
            ViewData["Cats"] = String.Join(",", catss);
            return View(productToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [SiteMapCacheRelease]
        [HostControl]
        public ActionResult Edit(string PrId, string product, string filesToUpload, string Cats)
        {
            NullChecker.NullCheck(new object[] { PrId, product });

            var productToEdit = unitOfWork.ProductRepository
                                          .GetByID(EncryptionHelper.Unprotect(PrId));

            if (TryUpdateModel(productToEdit, "", new string[] {
                    "productName","productNameEN","brandName","brandNameEN","about","aboutEN",
                    "PorT","productionCountryID","weight","height","width","length",
                    "power","shabnam","iranCode","packCount","price",
                    "shipFree","wuarantee","guarantee","afterSale" }))
            {
                if (productToEdit.company.Admins.Any(g => AuthorizationHelper.isRelevant(g.UserId)))
                {

                    UpdSertProductCats(Cats, productToEdit);

                    var fileUploadResult = UploadHelper.UpdateUploadedFiles(filesToUpload, productToEdit.image + productToEdit.document, "Product");
                    productToEdit.image = fileUploadResult.ImagesToUpload;
                    productToEdit.document = fileUploadResult.DocsToUpload;

                    unitOfWork.ProductRepository.Update(productToEdit);
                    unitOfWork.Save();
                    UrlHelper Url = new UrlHelper(Request.RequestContext);
                    return Json(new { URL = Url.Action("Detail", "Product", new { PrId = productToEdit.productID, PrName = StringHelper.URLName(productToEdit.CultureProductName) }) });
                }
                throw new JsonCustomException(ControllerError.ajaxErrorProductAdmin);
            }
            throw new ModelStateException(this.ModelState);
            
        }


        public ActionResult Delete(string PrId)
        {
            NullChecker.NullCheck(new object[] { PrId });

            var productToDelete = unitOfWork.ProductRepository.GetByID(EncryptionHelper.Unprotect(PrId));
            if (productToDelete == null || !productToDelete.company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                return new RedirectToError();
            }
            return View(productToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        public ActionResult Delete(string P, string PTD)
        {
            NullChecker.NullCheck(new object[] { PTD, P });
            var ptd = EncryptionHelper.Unprotect(PTD);
            var p = EncryptionHelper.Unprotect(P);
            Product productToDelete = unitOfWork.ProductRepository
                                                .GetByID(ptd);
            if (!productToDelete.company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                return new RedirectToError();
            }

            if (ptd == p && productToDelete.company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
            {
                unitOfWork.LikeCommentRepository.Get(d => productToDelete.Comment.Select(f => f.cmtID).Contains(d.elemID) && d.type == LikeCommentType.Product).ToList().ForEach(unitOfWork.LikeCommentRepository.Delete);
                //productToDelete.Comment.SelectMany(t => t.Likes.Select(l => l.likeID as object)).ToList().ForEach(unitOfWork.LikeRepository.Delete); //ProductCommentsLikesDelete
                productToDelete.Comment.Select(t => t.cmtID as object).ToList().ForEach(unitOfWork.CommentProductRepository.Delete);//ProductCommentsDelete
                productToDelete.Likes.Select(l => l.likeID as object).ToList().ForEach(unitOfWork.LikeProductRepository.Delete);//ProductLikesDelete
                //productToDelete.Notifications.Select(n => n.notificationID as object).ToList().ForEach(unitOfWork.NotificationRepository.Delete);//ProductNotificationsDelete

                var fileUploadResult = UploadHelper.UpdateUploadedFiles(String.Empty, productToDelete.image + productToDelete.document, "Question");
                if (String.IsNullOrEmpty(fileUploadResult.ImagesToUpload) && String.IsNullOrEmpty(fileUploadResult.DocsToUpload))
                {
                    unitOfWork.ProductRepository.Delete(productToDelete);
                    unitOfWork.Save();
                }
                UrlHelper Url = new UrlHelper(Request.RequestContext);
                return Json(new { URL = Url.Action("CompanyProducts", "Product", new { CoId = productToDelete.coID }) });
            }
            throw new JsonCustomException(ControllerError.ajaxErrorProductDelete);
        }


        private void UpdSertProductCats(string selectedItems, Product productToUpdate)
        {
            if (productToUpdate.categories == null)
            {
                productToUpdate.categories = new List<Category>();
            }
            var selectedCategoriesHS = !String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' }).Take(ITTConfig.MaxCategoryTagsLimit).Select(u => int.Parse(u))) 
                                       : new HashSet<int>();
            var ProductCategories = productToUpdate.categories != null 
                                       ? new HashSet<int>(productToUpdate.categories.Select(c => c.catID))
                                       : new HashSet<int>();
            IEnumerable<Category> catsToDelet = ProductCategories.Except(selectedCategoriesHS).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();
            IEnumerable<Category> catsToInsert = selectedCategoriesHS.Except(ProductCategories).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();

            foreach (var catToDel in catsToDelet)
            {
                productToUpdate.categories.Remove(catToDel);
            }
            foreach (var catToInsert in catsToInsert)
            {
                productToUpdate.categories.Add(catToInsert);
            }
        }
    }
}
