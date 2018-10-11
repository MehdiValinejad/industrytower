using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{

    [ITTAuthorizeAttribute]
    public class PatentController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult PatentsPartial(int UId)
        {
            IEnumerable<Patent> patents = Enumerable.Empty<Patent>();
            patents = unitOfWork.PatentRepository.Get(filter: C => C.Inventors.Any(t => t.UserId == UId));
            ViewData["UId"] = UId;
            return PartialView(patents);
        }

        [AllowAnonymous]
        public ActionResult Detail(int patId)
        {
            var patent = unitOfWork.PatentRepository.GetByID(patId);

            return View(patent);
        }

        [AjaxRequestOnly]
        public ActionResult Create()
        {
            var patStatus = from PatentStatus e in Enum.GetValues(typeof(PatentStatus))
                            select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewBag.patStatusToSelectList = new SelectList(patStatus, "Id", "Name");

            var country = unitOfWork.CountstateRepository.Get(c => c.countryID == null);
            ViewBag.countrySelectList = new SelectList(country, "stateID", "CultureStateName"); //CultureHelper.SelectListCulture(country, "stateID", "stateName","stateNameEN",null);
            
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult Create([Bind(Include = "status,officeStateID,patentTitle,patentTitleEN,patentNo,patentURL,issueDate,description,descriptionEN")] Patent pat)
        {
            if (ModelState.IsValid)
            {
                

                var currentUser = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
                pat.Inventors = new List<ActiveUser>();
                pat.Inventors.Add(currentUser);
                unitOfWork.PatentRepository.Insert(pat);
                unitOfWork.Save();
                UnitOfWork newContext = new UnitOfWork();
                var newPatent = newContext.PatentRepository.GetByID(pat.patentID);
                return Json(new
                {
                    Success = true,
                    Result = RenderPartialViewHelper.RenderPartialView(this, "PatentPartial", newPatent),
                    Message = Resource.Resource.createdSuccessfully
                }); 
            }
            throw new ModelStateException(this.ModelState);
        }

        [AjaxRequestOnly]
        public ActionResult Edit(string patID)
        {
            NullChecker.NullCheck(new object[] { patID });

            var patToEdit = unitOfWork.PatentRepository.GetByID(EncryptionHelper.Unprotect(patID));
            if (!patToEdit.Inventors.Any(i => AuthorizationHelper.isRelevant(i.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorPatentUser);
            }
            var patStatus = from PatentStatus e in Enum.GetValues(typeof(PatentStatus))
                            select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewBag.patStatusToSelectList = new SelectList(patStatus, "Id", "Name", patToEdit.status);

            var country = unitOfWork.CountstateRepository.Get(c => c.countryID == null);
            ViewBag.countrySelectList = new SelectList(country, "stateID", "CultureStateName");   // CultureHelper.SelectListCulture(country, "stateID", "stateName", "stateNameEN", patToEdit.officeStateID);
            return PartialView(patToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        public ActionResult Edit(string patID, FormCollection formCollection)
        {
            NullChecker.NullCheck(new object[] { patID });

            Patent patentEntryToEdit = unitOfWork.PatentRepository
                                       .GetByID(EncryptionHelper.Unprotect(patID));
            if (!patentEntryToEdit.Inventors.Any(i => AuthorizationHelper.isRelevant(i.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorPatentUser);
            }
            if (TryUpdateModel(patentEntryToEdit, "", new string[] { "status", "officeStateID", "patentTitle", "patentTitleEN", "patentNo", "patentURL", "issueDate", "description", "descriptionEN" }))
            {
                

                unitOfWork.PatentRepository.Update(patentEntryToEdit);
                unitOfWork.Save();
                UnitOfWork newContext = new UnitOfWork();
                var editedPatent = newContext.PatentRepository.GetByID(patentEntryToEdit.patentID);
                return Json(new
                {
                    Result = RenderPartialViewHelper.RenderPartialView(this, "PatentPartial", editedPatent),
                    Message = Resource.Resource.editedSuccessfully
                });

            }
            throw new ModelStateException(this.ModelState);
        }


        public ActionResult Delete(string patID)
        {
            NullChecker.NullCheck(new object[] { patID });

            var patToDelete = unitOfWork.PatentRepository.GetByID(EncryptionHelper.Unprotect(patID));
            if (!patToDelete.Inventors.Any(i => AuthorizationHelper.isRelevant(i.UserId)))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorPatentUser);
            }
            return PartialView(patToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string patID, string P)
        {
            NullChecker.NullCheck(new object[] { patID, P });
            var patid = EncryptionHelper.Unprotect(patID);
            var p = EncryptionHelper.Unprotect(P);
            if (patid == p)
            {
                var patentToDelet = unitOfWork.PatentRepository.GetByID(p);
                if (patentToDelet.Inventors.Any(u=>AuthorizationHelper.isRelevant(u.UserId)))
                {
                    unitOfWork.PatentRepository.Delete(patentToDelet.patentID);
                    unitOfWork.Save();
                    return Json(new { Success = true, Message = Resource.Resource.deletedSuccessfully });
                }
                throw new JsonCustomException(ControllerError.ajaxErrorPatentUser);
            }
            throw new ModelStateException(this.ModelState);
        }

    }
}
