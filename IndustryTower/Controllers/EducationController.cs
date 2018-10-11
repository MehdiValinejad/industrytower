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
    public class EducationController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult EducationsPartial(int UId)
        {
            IEnumerable<Education> educations = Enumerable.Empty<Education>();
            educations = unitOfWork.EducationRepository.Get(filter: C => C.userID == UId);

            ViewData["UId"] = UId;
            return PartialView(educations);
        }


        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create()
        {
            var eduDegree = from EducationDegree e in Enum.GetValues(typeof(EducationDegree))
                           select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewBag.eduDegreeToSelectList = new SelectList(eduDegree, "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create([Bind(Include = "degree,fieldOfStudy,fieldOfStudyEN,school,schoolEN,attendDate,untilDate")] Education edu)
        {
            if (ModelState.IsValid)
            {
                edu.userID = WebSecurity.CurrentUserId;
                unitOfWork.EducationRepository.Insert(edu);
                unitOfWork.Save();
                UnitOfWork newContext = new UnitOfWork();
                var newEducation = newContext.EducationRepository.GetByID(edu.educationID);
                return Json(new
                {
                    Result = RenderPartialViewHelper.RenderPartialView(this, "EducationPartial", newEducation),
                    Message = Resource.Resource.createdSuccessfully
                });
            }
            throw new ModelStateException(this.ModelState);
        }


        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Edit(string eduID)
        {
            NullChecker.NullCheck(new object[] { eduID });

            var eduToEdit = unitOfWork.EducationRepository.GetByID(EncryptionHelper.Unprotect(eduID));
            if (!AuthorizationHelper.isRelevant(eduToEdit.userID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorEducationUser);
            }
            var eduDegree = from EducationDegree e in Enum.GetValues(typeof(EducationDegree))
                            select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            ViewBag.eduDegreeToSelectList = new SelectList(eduDegree, "Id", "Name", eduToEdit.degree);
            return PartialView(eduToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Edit(string eduID, FormCollection formCollection)
        {
            NullChecker.NullCheck(new object[] { eduID });

            Education eduEntryToEdit = unitOfWork.EducationRepository
                                                 .GetByID(EncryptionHelper.Unprotect(eduID));
            if (!AuthorizationHelper.isRelevant(eduEntryToEdit.userID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorEducationUser);
            }

            if (TryUpdateModel(eduEntryToEdit, "", new string[] { "degree", "fieldOfStudy", "fieldOfStudyEN", "school", "schoolEN", "attendDate", "untilDate" }))
            {
                unitOfWork.EducationRepository.Update(eduEntryToEdit);
                unitOfWork.Save();
                UnitOfWork newContext = new UnitOfWork();
                var editedEducation = newContext.EducationRepository.GetByID(eduEntryToEdit.educationID);
                return Json(new
                {
                    Result = RenderPartialViewHelper.RenderPartialView(this, "EducationPartial", editedEducation),
                    Message = Resource.Resource.editedSuccessfully
                });

            }
            throw new ModelStateException(this.ModelState);
          
        }

        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Delete(string eduID)
        {
            NullChecker.NullCheck(new object[] { eduID });

            var eduToDelete = unitOfWork.EducationRepository
                                        .GetByID(EncryptionHelper.Unprotect(eduID));
            if (!AuthorizationHelper.isRelevant(eduToDelete.userID))
            {
                throw new JsonCustomException(ControllerError.ajaxErrorEducationUser);
            }
            return PartialView(eduToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Delete(string eduID, string E)
        {
            NullChecker.NullCheck(new object[] { eduID, E });
            var eduid = EncryptionHelper.Unprotect(eduID);
            var edu = EncryptionHelper.Unprotect(E);
            if (eduid == edu)
            {
                var educationToDelete = unitOfWork.EducationRepository
                                                  .GetByID(eduid);
                if (!AuthorizationHelper.isRelevant(educationToDelete.userID))
                {
                    throw new JsonCustomException(ControllerError.ajaxErrorEducationUser);
                }

                unitOfWork.EducationRepository.Delete(eduid);
                unitOfWork.Save();
                return Json(new { Message = Resource.Resource.deletedSuccessfully }); 
            }
            throw new JsonCustomException(ControllerError.ajaxErrorEducationDelete);
        }
    }
}
