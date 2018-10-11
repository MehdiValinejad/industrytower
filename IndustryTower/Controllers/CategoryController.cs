using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace IndustryTower.Controllers
{
    public class CategoryController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //[AjaxRequestOnly]
        [Authorize(Roles="ITAdmin")]
        public ActionResult Index(string parentId)
        {
            var children = unitOfWork.CategoryRepository.Get();
              
            return PartialView(children);
        }

        [Authorize(Roles = "ITAdmin")]
        //[HostControl]
        public ActionResult PartialCategories(string parentId)
        {
            var children = from item in unitOfWork.CategoryRepository.Get()
                           where parentId == null ? item.parent1ID == null : item.parent1ID == EncryptionHelper.Unprotect(parentId)
                           select item;
            ViewBag.parentIDD = parentId;
            if (children.Count() > 0)
                return PartialView(children);
            else return new EmptyResult();
        }

        [AllowAnonymous]
        [OutputCache(CacheProfile = "1Month" , Location= OutputCacheLocation.Server)]
        public ActionResult CategoryTags()
        {
            var children = unitOfWork.CategoryRepository.Get();
            if (children.Count() > 0)
                return PartialView(children);
            else return new EmptyResult();
        }

        [Authorize(Roles = "ITAdmin")]
        public ActionResult Management()
        {
            return View();
        }

        [Authorize(Roles = "ITAdmin")]
        [HostControl]
        public ActionResult Create(string parentId)
        {
            Category newCat = new Category();
            if (parentId != null)
            {
                var parent = unitOfWork.CategoryRepository.GetByID(EncryptionHelper.Unprotect(parentId));
                
                newCat.parent1 = parent;
                newCat.parent2 = parent.parent1;
                newCat.parent3 = parent.parent2;
                newCat.parent4 = parent.parent3;
            }
            return View(newCat);
        }

        [Authorize(Roles = "ITAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        public ActionResult Create([Bind(Include = "catName,catNameEn,parent1ID,parent2ID,parent3ID,parent4ID")] Category category)
        {
            try
            {
                if (ModelState.IsValid && Request.UrlReferrer.Host == Request.Url.Host)
                {
                    //var Parent = unitOfWork.CategoryRepository.GetByID(category.parent1ID);

                    //category.parent1ID = Parent.catID;
                    //category.parent2ID = Parent.parent1ID;
                    //category.parent3ID = Parent.parent2ID;
                    //category.parent4ID = Parent.parent3ID;
                    unitOfWork.CategoryRepository.Insert(category);
                    unitOfWork.Save();
                    return RedirectToAction("Management", "Category");
                }
            }
            catch (Exception e )
            {
                ModelState.AddModelError("", e.Message);
            }
            return PartialView(category);
        }


        [Authorize(Roles = "ITAdmin")]
        [HostControl]
        public ActionResult Edit(string catId)
        {
            var catToEdit = unitOfWork.CategoryRepository.GetByID(EncryptionHelper.Unprotect(catId));
            return View(catToEdit);
        }

        [Authorize(Roles = "ITAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        public ActionResult Edit(FormCollection form, string catId)
        {
            if (ModelState.IsValid)
            {
                var professionToUpdate = unitOfWork.CategoryRepository.GetByID(EncryptionHelper.Unprotect(catId));
                if (TryUpdateModel(professionToUpdate, "", new string[] { "catName", "catNameEN" }))
                {
                    try
                    {
                        unitOfWork.CategoryRepository.Update(professionToUpdate);
                        unitOfWork.Save();

                        return RedirectToAction("Management", "Category");
                    }
                    catch(Exception e)
                    {
                        ModelState.AddModelError("", e.Message);
                    }
                }
                
            }
            return View();
        }


        public ActionResult Detail(string catId)
        {
            var cat = unitOfWork.CategoryRepository.GetByID(EncryptionHelper.Unprotect(catId));
            return View(cat);
        }
        
        //[HttpPost]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public ActionResult Edit(FormCollection form, Category categoryEdit, int catId)
        //{
            
        //    try
        //    {
        //        if (ModelState.IsValid && Request.IsAjaxRequest())
        //        {

        //            unitOfWork.CategoryRepository.PartialUpdate(form, categoryEdit, catId);
        //            unitOfWork.Save();

        //            return RedirectToAction("Management", "Category");
        //        }
                

        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("", "Can not update");
        //    }
        //    return View();

            
        //}

        [Authorize(Roles = "ITAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        public ActionResult Delete(string catId)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    var catIdUnprotect = EncryptionHelper.Unprotect(catId);
                    var allChilds = unitOfWork.CategoryRepository.Get(filter: c => c.parent1ID == catIdUnprotect);
                    Category catToDelete = unitOfWork.CategoryRepository.GetByID(catIdUnprotect);
                    foreach (var childCats in allChilds)
                    {
                        unitOfWork.CategoryRepository.Delete(childCats);
                    }
                    unitOfWork.CategoryRepository.Delete(catToDelete);
                    unitOfWork.Save();
                    return RedirectToAction("Management", "Category");
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            throw new ModelStateException(this.ModelState);
        }

        [AllowAnonymous]
        public ActionResult RelatedCategories(int? CoId, int? StId, int? PrId, int? SrId, int? EvId, int? GId, int? SnId)
        {
            IList<Category> relcats = new List<Category>();
            SqlParameter pram = null;

            if (CoId != null)
            {
                pram = new SqlParameter("CoId", CoId);
            }
            else if (StId != null)
            {
                pram = new SqlParameter("StId", StId);
            }
            else if (PrId != null)
            {
                pram = new SqlParameter("PrId", PrId);
            }
            else if (SrId != null)
            {
                pram = new SqlParameter("SrId", SrId);
            }
            else if (EvId != null)
            {
                pram = new SqlParameter("EvId", EvId);
            }
            else if (GId != null)
            {
                pram = new SqlParameter("GId", GId);
            }
            else if (SnId != null)
            {
                pram = new SqlParameter("SnId", SnId);
            }

            var reader = unitOfWork.ReaderRepository.GetSPDataReader("RelCats", pram);
            while (reader.Read())
            {
                relcats.Add(new Category
                {
                    catID = reader.GetInt32(0),
                    catName = reader[1] as string,
                    catNameEN = reader[2] as string
                });
            }

            return PartialView(relcats);
        }

        [AllowAnonymous]
        public ActionResult _CategorySearchPartial(string searchString)
        {
            var cats = unitOfWork.CategoryRepository.Get(c => c.catName.Contains(searchString)
                                                               || c.catNameEN.Contains(searchString)).Take(10);
            return PartialView(cats);
        }

    }
}
