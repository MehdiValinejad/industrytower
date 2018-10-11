using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]

    public class EventController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult EventsPartial(int? UId, int? CoId, int? StId)
        {
            IEnumerable<Event> events = Enumerable.Empty<Event>();
            if (UId != null)
            {
                events = unitOfWork.EventRepository.Get(filter: e => e.Attendors.Any(u => u.UserId == UId));
            }
            else if (CoId != null)
            {
                events = unitOfWork.EventRepository.Get(filter: e => e.AttendorCompanies.Any(u => u.coID == CoId));
            }
            else if (StId != null)
            {
                events = unitOfWork.EventRepository.Get(filter: e => e.AttendorStores.Any(u => u.storeID == StId));
            }
            return PartialView(events);

        }

        [AllowAnonymous]
        public ActionResult Detail(int EvId)
        {
            var ev = unitOfWork.EventRepository.GetByID(EvId);
            return View(ev);
        }

        [AllowAnonymous]
        public ActionResult RelatedEvents(int? EvId, int? QId, int? PjId, int? JId)
        {
            IEnumerable<Category> categories = Enumerable.Empty<Category>();
            IEnumerable<Event> Event = Enumerable.Empty<Event>();
            if (EvId != null)
            {
                categories = unitOfWork.EventRepository.GetByID(EvId).Categories;
                Event = unitOfWork.EventRepository.Get(e => e.eventID == EvId);
            }
            else if (PjId != null)
            {

                categories = unitOfWork.ProjectRepository
                                       .GetByID(PjId)
                                       .Proffessions.SelectMany(s => s.categories);
            }
            else if (JId != null)
            {
                categories = unitOfWork.JobRepository
                                       .GetByID(JId)
                                       .Professtions.SelectMany(s => s.categories);
            }
            else if (QId != null)
            {
                categories = unitOfWork.QuestionRepository
                                       .GetByID(QId)
                                       .Professions.SelectMany(s => s.categories);
            }
            else
            {
                categories = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId).Professions.SelectMany(s => s.categories);
            }

            IEnumerable<RelatedEventViewModel> companies = categories.SelectMany(c => c.Events)
                                                      .Except(Event)
                                                      .GroupBy(g => g)
                                                      .Select(g => new RelatedEventViewModel { Event = g.Key, relevance = g.Count() })
                                                      .OrderByDescending(f => f.relevance)
                                                      .Take(7);

            return PartialView(companies);
        }


        public ActionResult Create()
        {
            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.countries;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create([Bind(Include = "eventSubj,eventSubjEN,description,descriptionEN,address,addressEN,eventDate,untilDate,stateID")]Event ev, string Cats)
        {
            if (ModelState.IsValid)
            {
                UpdSertEventCats(Cats, ev);

                ev.createDate = DateTime.UtcNow;
                ev.creatorUserID = WebSecurity.CurrentUserId;
                unitOfWork.EventRepository.Insert(ev);
                unitOfWork.Save();
                return Json(new { URL = Url.Action("Detail", "Event", new { EvId = ev.eventID }), EvName = StringHelper.URLName(ev.CultureEventSubj) });
            }
            throw new ModelStateException(this.ModelState);
        }


        public ActionResult Edit(string EvId)
        {
            NullChecker.NullCheck(new object[] { EvId });

            var eventToEdit = unitOfWork.EventRepository
                                        .GetByID(EncryptionHelper.Unprotect(EvId));
            if (!AuthorizationHelper.isRelevant(eventToEdit.creatorUserID))
            {
                return new RedirectToError();
            }
            var catss = eventToEdit.Categories.Select(u => u.catID);
            ViewData["Cats"] = String.Join(",", catss);

            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.counts(eventToEdit.State);
            ViewBag.StateDropDown = countries.states(eventToEdit.State);

            return View(eventToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Edit(Event ev, string EvId, string Cats)
        {
            NullChecker.NullCheck(new object[] { EvId });

            Event eventEntryToEdit = unitOfWork.EventRepository
                                               .GetByID(EncryptionHelper.Unprotect(EvId));
            if (!AuthorizationHelper.isRelevant(eventEntryToEdit.creatorUserID))
            {
                return new RedirectToError();
            }
            if (TryUpdateModel(eventEntryToEdit, "", new string[] { "eventSubj", "eventSubjEN", "description", "dscriptionEN", "address", "addressEN", "eventDate", "untilDate", "stateID" }))
            {
              

                UpdSertEventCats(Cats, eventEntryToEdit);
                unitOfWork.EventRepository.Update(eventEntryToEdit);
                unitOfWork.Save();
                return Json(new { URL = Url.Action("Detail", "Event", new { EvId = eventEntryToEdit.eventID }), EvName = StringHelper.URLName(eventEntryToEdit.CultureEventSubj) });
            }
            throw new ModelStateException(this.ModelState);
        }

        public ActionResult Attend(Event ev)
        {
            return PartialView(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult AttendInsert(string ETA, string CA, string SA)
        {
            NullChecker.NullCheck(new object[] { ETA });
            var evtoattend = EncryptionHelper.Unprotect(ETA);

            var eventToAttend = unitOfWork.EventRepository.GetByID(evtoattend);
            var currentUser = WebSecurity.CurrentUserId;
            if (!String.IsNullOrEmpty(CA) && String.IsNullOrEmpty(SA))
            {
                var companyattend = EncryptionHelper.Unprotect(CA);
                var companyToAdd = unitOfWork.CompanyRepository.GetByID(companyattend);
                if (companyToAdd.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
                {
                    if (eventToAttend.AttendorCompanies.Any(u => u.coID == companyattend))
                    {
                        eventToAttend.AttendorCompanies.Remove(companyToAdd);
                        unitOfWork.Save();
                    }
                    else
                    {
                        eventToAttend.AttendorCompanies.Add(companyToAdd);
                        unitOfWork.Save();
                    }
                    
                } 
            }
            else if (!String.IsNullOrEmpty(SA) && String.IsNullOrEmpty(CA))
            {
                var storeattend = EncryptionHelper.Unprotect(SA);
                var storeToAdd = unitOfWork.StoreRepository.GetByID(storeattend);
                if (storeToAdd.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
                {
                    if (eventToAttend.AttendorStores.Any(u => u.storeID == storeattend))
                    {
                        eventToAttend.AttendorStores.Remove(storeToAdd);
                        unitOfWork.Save();
                    }
                    else
                    {
                        eventToAttend.AttendorStores.Add(storeToAdd);
                        unitOfWork.Save();
                    }
                } 
            }
            else if (String.IsNullOrEmpty(SA) && String.IsNullOrEmpty(CA))
            {
                var userToAdd = unitOfWork.ActiveUserRepository.GetByID(currentUser);
                if (eventToAttend.Attendors.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
                {
                    eventToAttend.Attendors.Remove(userToAdd);
                    unitOfWork.Save();
                }
                else
                {
                    eventToAttend.Attendors.Add(userToAdd);
                    unitOfWork.Save();
                }
            }
            UnitOfWork newContext = new UnitOfWork();
            var newAttend = newContext.EventRepository.GetByID(evtoattend);
            return Json(new { Result = RenderPartialViewHelper.RenderPartialView(this, "Attend", newAttend) });
        }

        [HostControl]
        [AjaxRequestOnly]
         public ActionResult Delete(string EvId)
         {
             NullChecker.NullCheck(new object[] { EvId });
             var eventtodel = unitOfWork.EventRepository.GetByID(EncryptionHelper.Unprotect(EvId));
             if (!AuthorizationHelper.isRelevant(eventtodel.creatorUserID))
             {
                 throw new JsonCustomException(ControllerError.ajaxErrorEventDelete);
             }
             ViewData["ETD"] = EvId;
             return PartialView();
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         [HostControl]
         [AjaxRequestOnly]
         public ActionResult Delete(string EvId, string ETD)
         {
             NullChecker.NullCheck(new object[] { EvId, ETD });

             var evtodel = EncryptionHelper.Unprotect(ETD);
             var evid = EncryptionHelper.Unprotect(EvId);
             var eventToDelete = unitOfWork.EventRepository.GetByID(evtodel);
             if (evtodel == evid && AuthorizationHelper.isRelevant(eventToDelete.Creator.UserId))
             {
                 unitOfWork.EventRepository.Delete(evtodel);
                 unitOfWork.Save();
                 return Json(new
                 {
                     Message = Resource.Resource.deletedSuccessfully,
                     RedirectURL = Url.Action("TotalSearch", "Home", new { searchType = SearchType.Event })
                 });
             }
             throw new JsonCustomException(ControllerError.ajaxErrorEventDelete);
         }


         private void UpdSertEventCats(string selectedItems, Event eventToUpdate)
         {
             if (eventToUpdate.Categories == null)
             {
                 eventToUpdate.Categories = new List<Category>();
             }
             var selectedCategoriesHS =!String.IsNullOrWhiteSpace(selectedItems)
                                       ? new HashSet<int>(selectedItems.Split(new char[] { ',' })
                                                    .Take(ITTConfig.MaxCategoryTagsLimit)
                                                    .Select(u => int.Parse(u)))
                                       : new HashSet<int>(); 
             var EventCategories = eventToUpdate.Categories != null
                                   ? new HashSet<int>(eventToUpdate.Categories.Select(c => c.catID))
                                   : new HashSet<int>();
             var catsToDelet = EventCategories.Except(selectedCategoriesHS).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();
             var catsToInsert = selectedCategoriesHS.Except(EventCategories).Select(t => unitOfWork.CategoryRepository.GetByID(t)).ToList();

             foreach (var catToDel in catsToDelet)
             {
                 eventToUpdate.Categories.Remove(catToDel);
             }
             foreach (var catToInsert in catsToInsert)
             {
                 eventToUpdate.Categories.Add(catToInsert);
             }

         }

    }
}
