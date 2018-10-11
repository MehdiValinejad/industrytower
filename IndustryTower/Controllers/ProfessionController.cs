using IndustryTower.DAL;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{
    [Authorize(Roles = "ITAdmin")]
    public class ProfessionController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public ActionResult Index()
        {
            var mainProfs = from item in unitOfWork.ProfessionRepository.Get()
                           select item.categories;
            
            return View();
        }

        public ActionResult List(string q)
        {
            var mainProfs = from item in unitOfWork.ProfessionRepository.Get()
                            select item;
            if (!String.IsNullOrEmpty(q))
            {
                mainProfs = mainProfs.Where(s => s.professionName.ToUpper().Contains(q.ToUpper())
                || s.professionNameEN.ToUpper().Contains(q.ToUpper()));
            }
            if (mainProfs != null)
            {
                return PartialView(mainProfs);
            }
            else return new EmptyResult();
            
        }

        [AllowAnonymous]
        public ActionResult ListPartial(string q)
        {
            var mainProfs = from item in unitOfWork.ProfessionRepository.Get()
                            select item;
            if (!String.IsNullOrEmpty(q))
            {
                mainProfs = mainProfs.Where(s => s.professionName.ToUpper().Contains(q.ToUpper())
                || s.professionNameEN.ToUpper().Contains(q.ToUpper()));
            }
            if (mainProfs != null)
            {

                return PartialView(mainProfs);
            }
            else return new EmptyResult();

        }


        public ActionResult Detail(string profId)
        {
            var prof = unitOfWork.ProfessionRepository.GetByID(EncryptionHelper.Unprotect(profId));
            return View(prof);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "professionName,professionNameEn,professionDescription,professionDescriptionEN")] Profession profession, string[] selectedCategories)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.ProfessionRepository.Insert(profession);
                    InsertProfessionCats(selectedCategories, profession);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your");
            }
            return View(profession);
        }


        public ActionResult Edit(string proffID)
        {
            NullChecker.NullCheck(new object[] { proffID });

            if (!String.IsNullOrEmpty(proffID))
            {
                Profession proff = unitOfWork.ProfessionRepository
                                             .GetByID(EncryptionHelper.Unprotect(proffID));
                return View(proff);
            }
            else
                return HttpNotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection formCollection, string[] selectedCategories, string proffID)
        {
            NullChecker.NullCheck(new object[] { proffID });

            if (ModelState.IsValid)
            {
                var proffIDUprotect = EncryptionHelper.Unprotect(proffID);
                var professionToUpdate = unitOfWork.ProfessionRepository.GetByID(proffIDUprotect);
                if (TryUpdateModel(professionToUpdate, "", new string[] { "professionName", "professionNameEN", "professionDescription", "professionDescriptionEN" }))
                {
                    UpdateProfessionCats(selectedCategories, professionToUpdate);
                    unitOfWork.ProfessionRepository.Update(professionToUpdate);
                    unitOfWork.Save();

                    return RedirectToAction("Index");
                }
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string ProfId) 
        {
            NullChecker.NullCheck(new object[] { ProfId });

            var proffIDUprotect = EncryptionHelper.Unprotect(ProfId);
            Profession professionToDelete = unitOfWork.ProfessionRepository.GetByID(proffIDUprotect);
                unitOfWork.ProfessionRepository.Delete(professionToDelete);
                unitOfWork.Save();
                return RedirectToAction("Index");

        }


        [HostControl]
        public ActionResult MainCatsCreate(string proffID = null)
        {
            if (proffID != null)
            {
                var proffIDUprotect = EncryptionHelper.Unprotect(proffID);
                var profcats = unitOfWork.ProfessionRepository.GetByID(proffIDUprotect);
                ViewBag.profff = new List<int>(profcats.categories.Select(c => c.catID));
            }
            else ViewBag.profff = new List<int>();
            var cats = unitOfWork.CategoryRepository.Get();
            if (cats.Count() > 0)
                return PartialView(cats);
            else return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult RelatedProfessions(int? QId, int? UId, int? GId, int? SnId, int? DId, int? BId)
        {
            IList<Profession> relProf = new List<Profession>();

            SqlParameter pram = null;

            if (QId != null) 
            {
                pram = new SqlParameter("QId", QId);
            }
            else if (UId != null)
            {
                pram = new SqlParameter("UId", UId);
            }
            else if (GId != null)
            {
                pram = new SqlParameter("GId", GId);
            }
            else if (SnId != null)
            {
                pram = new SqlParameter("SnId", SnId);
            }
            else if (DId != null)
            {
                pram = new SqlParameter("DId", DId);
            }
            else if (BId != null)
            {
                pram = new SqlParameter("BId", BId);
            }
            var reader = unitOfWork.ReaderRepository.GetSPDataReader("RelProfs", pram);
            while (reader.Read())
            {
                relProf.Add(new Profession
                {
                    profID = reader.GetInt32(0),
                    professionName = reader[1] as string,
                    professionNameEN = reader[2] as string
                });
            }
            return PartialView(relProf);
        }

        [AllowAnonymous]
        public ActionResult _ProfessionSearchPartial(string searchString)
        {
            var searchWords = searchString.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Where(w => w.Length > 2);
            var Proffs = unitOfWork.ProfessionRepository
                                            .Get(s => searchWords.Any(q => s.professionName.ToUpper().Contains(q)
                                                                  || s.professionNameEN.ToUpper().Contains(q)))
                                            .Select(f => new ProfessionSearchViewModel
                                            {
                                                professionResult = f,
                                                relevance = String.Concat(f.professionName, ' ', f.professionNameEN).ToUpper()
                                                    .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                    .Distinct()
                                                    .Intersect(searchWords)
                                                    .Count()
                                            })
                                            .OrderByDescending(o => o.relevance);
            return PartialView(Proffs);
        }

        [Authorize(Roles = "ITAdmin")]
        private void InsertProfessionCats(string[] selectedItems, Profession professionToInsert) 
        {
            if (selectedItems == null)
            {
                professionToInsert.categories = new List<Category>();
                return;
            }
            professionToInsert.categories = new List<Category>();
            var selectedItemsUnprotected = selectedItems.Where(x => x != "false").Select(i => (int)EncryptionHelper.Unprotect(i)).ToArray();
            var selectedCoursesHS = new HashSet<int>(selectedItemsUnprotected);
            foreach (var cat in selectedCoursesHS)
            {
                var CategoryToAdd = unitOfWork.CategoryRepository.GetByID(cat);
                professionToInsert.categories.Add(CategoryToAdd);
            }

        }


        private void UpdateProfessionCats(string[] selectedItems, Profession professionToUpdate)
        {
            if (selectedItems == null)
            {
                professionToUpdate.categories = new List<Category>();
                return;
            }
            var selectedItemsUnprotected = selectedItems.Where(x => x != "false").Select(i => (int)EncryptionHelper.Unprotect(i)).ToArray();
            var selectedCoursesHS = new HashSet<int>(selectedItemsUnprotected);
            var instructorCourses = new HashSet<int>(professionToUpdate.categories.Select(c => c.catID));
            foreach (var cat in unitOfWork.CategoryRepository.Get())
            {
                if (selectedCoursesHS.Contains(cat.catID))
                {
                    if (!instructorCourses.Contains(cat.catID))
                    {
                        professionToUpdate.categories.Add(cat);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(cat.catID))
                    {
                        professionToUpdate.categories.Remove(cat);
                    }
                }
            }
        }
        

        
    }
}