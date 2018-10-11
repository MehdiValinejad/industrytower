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
    public class CertificateController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult CertificatesPartial(int? UId, int? CoId)
        {
            
            IEnumerable<Certificate> certificates =  Enumerable.Empty<Certificate>();
            if (UId != null)
            {
                certificates = unitOfWork.CertificateRepository.Get(filter: C => C.userID == UId);
                ViewData["UId"] = UId;
                ViewData["isAdmin"] = AuthorizationHelper.isRelevant((int)UId);
            }
            else if (CoId != null)
            {
                certificates = unitOfWork.CertificateRepository.Get(filter: C => C.coID == CoId);
                ViewData["company"] = EncryptionHelper.Protect(CoId);

                var company = unitOfWork.NotExpiredCompanyRepository.GetByID(CoId);
                ViewData["isAdmin"] = company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId));
            }
            return PartialView(certificates);
        }

        public ActionResult CertificateFeed(int certId)
        { 
            var cert = unitOfWork.CertificateRepository.GetByID(certId);
            return PartialView(cert);
        }

        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create(string CoId)
        {
            ViewData["company"] = CoId;
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Create([Bind(Include = "Certificator,CertificatorEN,Name,NameEN,licenceNo,certificatorURL,certificationDate")] Certificate cert, string company)
        {
            var companyToadd = unitOfWork.CompanyRepository.GetByID(EncryptionHelper.Unprotect(company));

            if (ModelState.IsValid)
            {
                

                if (!String.IsNullOrEmpty(company))
                {

                    if (companyToadd.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
                    {
                        cert.coID = EncryptionHelper.Unprotect(company);
                        unitOfWork.CertificateRepository.Insert(cert);
                        unitOfWork.Save();
                        UnitOfWork newContext = new UnitOfWork();
                        var newCertificate = newContext.CertificateRepository.GetByID(cert.certID);
                        return Json(new
                        {
                            Result = RenderPartialViewHelper.RenderPartialView(this, "CertificatePartial", newCertificate),
                            Message = Resource.Resource.createdSuccessfully
                        });
                    }
                    throw new JsonCustomException(ControllerError.ajaxErrorCertificateCoAdmin);


                }
                else
                {
                    cert.userID = WebSecurity.CurrentUserId;
                    unitOfWork.CertificateRepository.Insert(cert);
                    unitOfWork.Save();
                    FeedHelper.FeedInsert(FeedType.UserCertificate,
                                            cert.certID,
                                          (int)cert.userID
                                          );
                    UnitOfWork newContext = new UnitOfWork();
                    var newCertificate = newContext.CertificateRepository.GetByID(cert.certID);
                    return Json(new
                    {
                        Result = RenderPartialViewHelper.RenderPartialView(this, "CertificatePartial", newCertificate),
                        Message = Resource.Resource.createdSuccessfully
                    });
                }

            }
            throw new ModelStateException(this.ModelState);
            
        }


        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Edit(string CertID)
        {
            NullChecker.NullCheck(new object[] { CertID });

            var certToEdit = unitOfWork.CertificateRepository.GetByID(EncryptionHelper.Unprotect(CertID));
            if (certToEdit.coID != null)
            {
                if (!certToEdit.Company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
                {
                    throw new JsonCustomException(ControllerError.ajaxError);
                }
            }
            else
            { 
                if(!AuthorizationHelper.isRelevant((int)certToEdit.userID))
                {
                    throw new JsonCustomException(ControllerError.ajaxError);
                }
            }
            return PartialView(certToEdit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Edit(string CertID, FormCollection formCollection)
        {
            NullChecker.NullCheck(new object[] { CertID });

            Certificate certEntryToEdit = unitOfWork.CertificateRepository.GetByID(EncryptionHelper.Unprotect(CertID));

            if (TryUpdateModel(certEntryToEdit, "", new string[] { "Certificator", "CertificatorEN", "Name", "NameEN", "licenceNo", "certificatorURL", "certificationDate" }))
            {
                
                if (certEntryToEdit.coID != null)
                {
                    if (certEntryToEdit.Company.Admins.Any(a => AuthorizationHelper.isRelevant(a.UserId)))
                    {
                        unitOfWork.CertificateRepository.Update(certEntryToEdit);
                        unitOfWork.Save();
                        UnitOfWork newContext = new UnitOfWork();
                        var editedCertificate = newContext.CertificateRepository.GetByID(certEntryToEdit.certID);
                        return Json(new
                        {
                            Result = RenderPartialViewHelper.RenderPartialView(this, "CertificatePartial", editedCertificate),
                            Message = Resource.Resource.editedSuccessfully
                        });
                    }
                    throw new JsonCustomException(ControllerError.ajaxErrorCertificateCoAdmin);
                }
                else
                {
                    if (AuthorizationHelper.isRelevant((int)certEntryToEdit.userID))
                    {
                        unitOfWork.CertificateRepository.Update(certEntryToEdit);
                        unitOfWork.Save();
                        UnitOfWork newContext = new UnitOfWork();
                        var editedCertificate = newContext.CertificateRepository.GetByID(certEntryToEdit.certID);
                        return Json(new
                        {
                            Result = RenderPartialViewHelper.RenderPartialView(this, "CertificatePartial", editedCertificate),
                            Message = Resource.Resource.editedSuccessfully
                        });
                    }
                    throw new JsonCustomException(ControllerError.ajaxErrorCertificateCoAdmin);
                }
            }
            throw new ModelStateException(this.ModelState);
        }


        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Delete(string CertID)
        {
            NullChecker.NullCheck(new object[] { CertID });

            var certToDelete = unitOfWork.CertificateRepository.GetByID(EncryptionHelper.Unprotect(CertID));
            if (certToDelete.coID != null)
            {
                if (!certToDelete.Company.Admins.Any(u => AuthorizationHelper.isRelevant(u.UserId)))
                {
                    throw new JsonCustomException(ControllerError.ajaxError);
                }
            }
            else
            {
                if (!AuthorizationHelper.isRelevant((int)certToDelete.userID))
                {
                    throw new JsonCustomException(ControllerError.ajaxError);
                }
            }
            return PartialView(certToDelete);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult Delete([Bind(Include = "C,certID")]Certificate certToDelet, string CertID, string C, string U, string CY)
        {
            NullChecker.NullCheck(new object[] { CertID, C });
            var certid = EncryptionHelper.Unprotect(CertID);
            var c = EncryptionHelper.Unprotect(C);
            if (certid == c)
            {
                var cy = EncryptionHelper.Unprotect(CY);
                if (cy != null
                    && unitOfWork.CompanyRepository.GetByID(cy).Admins.Any(t => AuthorizationHelper.isRelevant(t.UserId)))
                {
                    unitOfWork.CertificateRepository.Delete(certid);
                    unitOfWork.Save();
                    return Json(new { Message = Resource.Resource.deletedSuccessfully });
                }
                else if (AuthorizationHelper.isRelevant((int)EncryptionHelper.Unprotect(U)))
                {
                    unitOfWork.CertificateRepository.Delete(certid);
                    unitOfWork.Save();
                    return Json(new { Message = Resource.Resource.deletedSuccessfully });
                }
            }
            throw new ModelStateException(this.ModelState);
        }

        
    }
}
