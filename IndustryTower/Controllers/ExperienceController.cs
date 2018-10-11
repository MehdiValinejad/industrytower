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
    public class ExperienceController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult ExperiencesPartial(int UId)
        {
            IEnumerable<Experience> experiences = Enumerable.Empty<Experience>();
            experiences = unitOfWork.ExperienceRepository.Get(filter: C => C.userID == UId);
            ViewData["UId"] = UId;
            return PartialView(experiences);
        }

        public ActionResult ExperienceFeed(int expId)
        {
            var exp = unitOfWork.ExperienceRepository.GetByID(expId);
            return PartialView(exp);
        }

        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create()
        {
            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.countries;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create([Bind(Include = "title,titleEN,coName,coNameEN,description,descriptionEN,attendDate,untilDate,stateID")] Experience exp, string CoId)
        {

            if (!String.IsNullOrEmpty(CoId)) 
            {
                ModelState.Remove("coName");
                ModelState.Remove("coNameEN");
            }
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(CoId))
                {
                    var coid = EncryptionHelper.Unprotect(CoId);
                    var companyToExperience = unitOfWork.NotExpiredCompanyRepository.GetByID(coid);
                    exp.coName = companyToExperience.coName;
                    exp.coNameEN = companyToExperience.coNameEN;
                    exp.CoId = coid;
                }
                
                exp.userID = WebSecurity.CurrentUserId;
                unitOfWork.ExperienceRepository.Insert(exp);
                unitOfWork.Save();
                FeedHelper.FeedInsert(FeedType.Experience,
                                        exp.experienceID,
                                          exp.userID
                                          );
                UnitOfWork newContext = new UnitOfWork();
                var newExperience = newContext.ExperienceRepository.GetByID(exp.experienceID);
                return Json(new
                {
                    Result = RenderPartialViewHelper.RenderPartialView(this, "ExperiencePartial", newExperience),
                    Message = Resource.Resource.createdSuccessfully
                });
            }
            throw new ModelStateException(this.ModelState);
        }

        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Edit(string expID)
        {
            NullChecker.NullCheck(new object[] { expID });

            CountriesViewModel countries = new CountriesViewModel();
            
            var expToEdit = unitOfWork.ExperienceRepository
                                      .GetByID(EncryptionHelper.Unprotect(expID));
            if (!AuthorizationHelper.isRelevant(expToEdit.userID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorEducationUser);
            }
            ViewBag.CountryDropDown = countries.counts(expToEdit.State);
            ViewBag.StateDropDown = countries.states(expToEdit.State);
            return PartialView(expToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Edit(string expID, FormCollection formCollection, string CoId)
        {
            NullChecker.NullCheck(new object[] { expID });
            var expid = EncryptionHelper.Unprotect(expID);
            Experience expEntryToEdit = unitOfWork.ExperienceRepository
                                                  .GetByID(expid);
            if (!AuthorizationHelper.isRelevant(expEntryToEdit.userID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorEducationUser);
            }
            if (TryUpdateModel(expEntryToEdit, "", new string[] { "title", "titleEN", "coName", "coNameEN", "description", "descriptionEN", "attendDate", "untilDate", "stateID" }))
            {
                
                if (!String.IsNullOrEmpty(CoId))
                {
                    var coid = EncryptionHelper.Unprotect(CoId);
                    var companyToExperience = unitOfWork.NotExpiredCompanyRepository.GetByID(coid);
                    expEntryToEdit.coName = companyToExperience.coName;
                    expEntryToEdit.coNameEN = companyToExperience.coNameEN;
                    expEntryToEdit.CoId = coid;
                }
                else
                {
                    expEntryToEdit.CoId = null;
                }

                unitOfWork.ExperienceRepository.Update(expEntryToEdit);
                unitOfWork.Save();
                UnitOfWork newContext = new UnitOfWork();
                var editedExperience = newContext.ExperienceRepository.GetByID(expid);
                return Json(new
                {
                    Result = RenderPartialViewHelper.RenderPartialView(this, "ExperiencePartial", editedExperience),
                    Message = Resource.Resource.editedSuccessfully
                });

            }
            throw new ModelStateException(this.ModelState);
            
        }

        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Delete(string expID)
        {
            NullChecker.NullCheck(new object[] { expID });

            var expToDelete = unitOfWork.ExperienceRepository
                                        .GetByID(EncryptionHelper.Unprotect(expID));
            if (!AuthorizationHelper.isRelevant(expToDelete.userID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorEducationUser);
            }
            return PartialView(expToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Delete(string expID, string E)
        {
            NullChecker.NullCheck(new object[] { expID, E });
            var expid = EncryptionHelper.Unprotect(expID);
            var exp = EncryptionHelper.Unprotect(E);
            if (expid == exp)
            {
                var experienceToDelete = unitOfWork.ExperienceRepository.GetByID(expid);
                if (AuthorizationHelper.isRelevant(experienceToDelete.userID))
                {
                    unitOfWork.ExperienceRepository.Delete(expid);
                    unitOfWork.Save();
                    return Json(new { Message = Resource.Resource.deletedSuccessfully });
                }
                throw new JsonCustomException(ControllerError.ajaxErrorEducationUser);
            }
            throw new JsonCustomException(ControllerError.ajaxErrorEducationDelete);
        }
	}
}