using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Resource;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;



namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class PlanRequestController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Choose(string UId, string CoId, string StId)
        {
            var currentUser = unitOfWork.ActiveUserRepository.GetByID(WebSecurity.CurrentUserId);
            var allRequests = currentUser.PlanRequests.Concat(currentUser.PlanRequestsForUserPlan)
                                                      .Where(r => r.payment == null || r.approve == false);
            SentRequests resuls = new SentRequests();
            resuls.RequestsFoNew = allRequests.Where(r => r is RequestForNew).Select(g => (RequestForNew)g);
            resuls.RevialRequests = allRequests.Where(r => r is RevivalRequest).Select(g => (RevivalRequest)g);
            resuls.CoId = CoId;
            resuls.StId = StId;
            resuls.UId = UId;
            return View(resuls);

        }

        public ActionResult SentRequests()
        {
            return View();
        }


        public ActionResult PlanRequest(string Plan, PlanType planType)
        {
            if (Plan == null)
            {
                return new RedirectToError();
            }
            
            PlanRequestViewModel viewmodel = new PlanRequestViewModel();

            if (planType == PlanType.Company && !PlansPrices.CompanyPlans.ContainsKey(Plan))
            {
                return new RedirectToError();
            }
            else if (planType == PlanType.Store && !PlansPrices.StorePlans.ContainsKey(Plan))
            {
                return new RedirectToError();
            }
            else
            {
                viewmodel.plan = Plan;
                CountriesViewModel countries = new CountriesViewModel();
                viewmodel.country = countries.countries;

                return View(viewmodel);
            }

            //var payTypes = from PayType e in Enum.GetValues(typeof(PayType))
            //               select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            //viewmodel.payTypeToSelectList = new SelectList(payTypes, "Id", "Name");
            ////PayBank SelectList
            //var payBanks = from PayBank e in Enum.GetValues(typeof(PayBank))
            //               select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            //viewmodel.bankTypeToSelectList = new SelectList(payBanks, "Id", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult PlanRequest(PlanRequestViewModel planModel, string Plan, PlanType planType)
        {
            if (Plan == null)
            {
                return new RedirectToError();
            }
            PaymentController planctlr = new PaymentController();
            if (ModelState.IsValid)
            {
                RequestForNew RequestToInsert = new RequestForNew();
                if (planType == PlanType.Company && PlansPrices.CompanyPlans.ContainsKey(Plan))
                {
                    RequestToInsert.planType = PlanType.Company;
                    RequestToInsert.plan = planModel.plan;
                }
                else if (planType == PlanType.Store && PlansPrices.StorePlans.ContainsKey(Plan))
                {
                    RequestToInsert.planType = PlanType.Store;
                    RequestToInsert.plan = planModel.plan;
                }
                else
                {
                    ModelState.AddModelError("", Resource.ControllerError.planQuery);
                    return View(planModel);
                }
                RequestToInsert.requesterUserID = WebSecurity.CurrentUserId;
                RequestToInsert.name = planModel.name;
                RequestToInsert.address = planModel.address;
                RequestToInsert.about = planModel.about;
                RequestToInsert.Email = planModel.email;
                RequestToInsert.phoneNo = planModel.phoneNo;
                RequestToInsert.regCode = planModel.regCode;
                RequestToInsert.reqDate = DateTime.UtcNow;
                RequestToInsert.stateID = planModel.stateID;

                unitOfWork.NewPlanRequestRepository.Insert(RequestToInsert);
                unitOfWork.Save();
                UrlHelper Url = new UrlHelper(Request.RequestContext);
                return Json(new { URL = Url.Action("PayForPlan", "Payment", new { reqId = EncryptionHelper.Protect(RequestToInsert.reqID) }) });
            }
            else throw new ModelStateException(this.ModelState);
        }


        public ActionResult RevivalRequest(string UId, string CoId, string StId, string Plan, PlanType planType)
        {
            PlanReivalRequestViewModel viewmodel = new PlanReivalRequestViewModel();
            var uid = EncryptionHelper.Unprotect(UId);
            var coid = EncryptionHelper.Unprotect(CoId);
            var stid = EncryptionHelper.Unprotect(StId);
            if (coid != null && PlansPrices.CompanyPlans.ContainsKey(Plan))
            {
                var company = unitOfWork.CompanyRepository.GetByID(coid);
                if (company.Admins.Any(u => u.UserId == WebSecurity.CurrentUserId))
                {
                    viewmodel.companyToRevive = company;
                    viewmodel.coId = CoId;
                }
                else return new RedirectToError();
            }
            else if (stid != null && PlansPrices.StorePlans.ContainsKey(Plan))
            {
                var store = unitOfWork.StoreRepository.GetByID(stid);
                if (store.Admins.Any(u => u.UserId == WebSecurity.CurrentUserId))
                {
                    viewmodel.storeToRevive = store;
                    viewmodel.storeId = StId;
                }
                else return new RedirectToError();
                
            }
            else if (uid != null && PlansPrices.UserPlans.ContainsKey(Plan))
            {
                var user = unitOfWork.ActiveUserRepository.GetByID(uid);
                if (user.UserId == WebSecurity.CurrentUserId)
                {
                    viewmodel.userToRevive = user;
                    viewmodel.userId = UId;
                }
                else return new RedirectToError();
                
            }
            else return new RedirectToError();
            
            viewmodel.plan = Plan;
            viewmodel.planType = planType;
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxRequestOnly]
        [HostControl]
        public ActionResult RevivalRequest(PlanReivalRequestViewModel ReviveRequest, string UId, string CoId, string StId, string Plan, PlanType planType)
        {
            var uid = EncryptionHelper.Unprotect(UId);
            var ruid = EncryptionHelper.Unprotect(ReviveRequest.userId);
            var coid = EncryptionHelper.Unprotect(CoId);
            var rcoid = EncryptionHelper.Unprotect(ReviveRequest.coId);
            var stid = EncryptionHelper.Unprotect(StId);
            var rstid = EncryptionHelper.Unprotect(ReviveRequest.storeId);
            if (ModelState.IsValid)
            {
                RevivalRequest revivalToInsert = new RevivalRequest();
                if (rcoid == coid && coid != null
                    && rstid == null
                    && ruid == null
                    && planType == PlanType.Company
                    && PlansPrices.CompanyPlans.ContainsKey(Plan))
                {
                    revivalToInsert.coID = EncryptionHelper.Unprotect(ReviveRequest.coId);

                }
                else if (rstid == stid && stid != null
                    && rcoid == null
                    && ruid == null
                    && planType == PlanType.Store
                    && PlansPrices.StorePlans.ContainsKey(Plan))
                {
                    revivalToInsert.storeID = EncryptionHelper.Unprotect(ReviveRequest.storeId);
                }
                else if (ruid == uid && uid != null
                    && rcoid == null
                    && rstid == null
                    && planType == PlanType.User
                    && PlansPrices.UserPlans.ContainsKey(Plan))
                {
                    revivalToInsert.userID = EncryptionHelper.Unprotect(ReviveRequest.userId);
                }
                else throw new JsonCustomException(ControllerError.ajaxError);

                revivalToInsert.reqDate = DateTime.UtcNow;
                revivalToInsert.planType = ReviveRequest.planType;
                revivalToInsert.plan = ReviveRequest.plan;
                revivalToInsert.requesterUserID = WebSecurity.CurrentUserId;

                unitOfWork.RevivalPlanRequestRepository.Insert(revivalToInsert);
                unitOfWork.Save();
                return Json(new { URL = Url.Action("PayForPlan", "Payment", new { reqId = EncryptionHelper.Protect(revivalToInsert.reqID) }) });
                //return RedirectToAction("PayForPlan", "Payment", new { reqId = EncryptionHelper.Protect(revivalToInsert.reqID) });
            }
            else throw new ModelStateException(this.ModelState);
        }

        [Authorize(Roles = "ITAdmin")]
        public ActionResult AllRequests()
        {
            var allRequests = unitOfWork.PlanRequetRepository.Get(p => p.approve == false && p.payment != null);
            return View(allRequests);
        }

        [Authorize(Roles = "ITAdmin")]
        public ActionResult RequestAudit(string reqId)
        {
            NullChecker.NullCheck(new object[] { reqId });

            var planToApprove = unitOfWork.PlanRequetRepository.GetByID(EncryptionHelper.Unprotect(reqId));
            if (planToApprove.payment == null)
            {
                return new RedirectToNotFound();
            }
            PlanToApprove viewmodel = new PlanToApprove();
            viewmodel.requesterUser = planToApprove.requesterUser;
            viewmodel.requester = EncryptionHelper.Protect(planToApprove.requesterUserID);
            viewmodel.plantype = planToApprove.planType;
            viewmodel.plan = planToApprove.plan;
            viewmodel.requestDate = planToApprove.reqDate;
            viewmodel.req = EncryptionHelper.Protect(planToApprove.reqID);

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ITAdmin")]
        public ActionResult RequestAudit(PlanToApprove planRequestToApprove)
        {
            NullChecker.NullCheck(new object[] { planRequestToApprove.req });

            if (ModelState.IsValid)
            {
                var planToApprove = unitOfWork.PlanRequetRepository
                                              .GetByID(EncryptionHelper.Unprotect(planRequestToApprove.req));
                if (planToApprove.payment == null)
                {
                    return new RedirectToNotFound();
                }

                if (planRequestToApprove.approve && planRequestToApprove.plantype == PlanType.Company)
                {
                    if (planToApprove is RequestForNew)
                    {
                        var New = planToApprove as RequestForNew;

                        CompanyNotExpired newCompany = new CompanyNotExpired();
                        if (planToApprove.plan == "Imperial") { newCompany = new Imperial(); }
                        else if (planToApprove.plan == "Luxury") { newCompany = new Luxury(); }
                        else if (planToApprove.plan == "HighClass") { newCompany = new HighClass(); }
                        else { return RedirectToAction("AllRequests"); }

                        planToApprove.payment.payAcceptDate = DateTime.UtcNow;

                        newCompany.regCode = (long)New.regCode;
                        newCompany.coName = New.name;
                        newCompany.coNameEN = New.name;
                        newCompany.about = New.about;
                        newCompany.aboutEN = New.about;
                        newCompany.address = New.address;
                        newCompany.addressEN = New.address;
                        newCompany.email = New.Email;
                        newCompany.phoneNo = (long)New.phoneNo;
                        newCompany.stateID = New.stateID;
                        newCompany.InsertionDate = DateTime.UtcNow;
                        newCompany.registerDate = DateTime.UtcNow;

                        newCompany.Admins = new List<ActiveUser>();
                        newCompany.Admins.Add(planToApprove.requesterUser);

                        newCompany.Followers = new List<Following>();
                        newCompany.Followers.Add(new Following
                        {
                            FollowerUser = planToApprove.requesterUser,
                            followDate = DateTime.UtcNow
                        });


                        Setting newSetting = new Setting();
                        newCompany.Setting = newSetting;

                        unitOfWork.NotExpiredCompanyRepository.Insert(newCompany);
                        planToApprove.approve = true;

                        unitOfWork.Save();

                        

                        if (!Roles.IsUserInRole(planToApprove.requesterUser.UserName, "CoAdmin"))
                        {
                            Roles.AddUserToRole(planToApprove.requesterUser.UserName, "CoAdmin");
                        }
                        

                        return RedirectToAction("AllRequests");
                    }
                    else if (planToApprove is RevivalRequest)
                    {
                        var New = planToApprove as RevivalRequest;
                        New.approve = true;
                        DateTime lastPayement = unitOfWork.RevivalPlanRequestRepository.Get(r => r.coID == New.coID).Max(c => c.payment.payDate);
                        New.payment.payDate = lastPayement.AddDays(365);
                        unitOfWork.RevivalPlanRequestRepository.Update(New);
                        unitOfWork.CompanyPlanChange((int)New.coID, New.plan);
                        unitOfWork.Save();
                        return RedirectToAction("AllRequests");
                    }
                    return new RedirectToError();
                }
                else if (planRequestToApprove.approve && planRequestToApprove.plantype == PlanType.Store)
                {
                    if (planToApprove is RequestForNew)
                    {
                        var New = planToApprove as RequestForNew;
                            StoreNotExpired newStore = new StoreNotExpired();
                            if (planToApprove.plan == "GroundFloor") { newStore = new GroundFloor(); }
                            else if (planToApprove.plan == "FirstFloor") { newStore = new FirstFloor(); }
                            else if (planToApprove.plan == "SecondFloor") { newStore = new SecondFloor(); }
                            else { return RedirectToAction("AllRequests"); }

                            planToApprove.payment.payAcceptDate = DateTime.UtcNow;

                            newStore.regCode = New.regCode;
                            newStore.storeName = New.name;
                            newStore.storeNameEN = New.name;
                            newStore.about = New.about;
                            newStore.aboutEN = New.about;
                            newStore.address = New.address;
                            newStore.addressEN = New.address;
                            newStore.email = New.Email;
                            newStore.phoneNo = (long)New.phoneNo;
                            newStore.stateID = New.stateID;
                            newStore.registerDate = DateTime.Now.ToLocalTime();

                            newStore.Admins = new List<ActiveUser>();
                            newStore.Admins.Add(planToApprove.requesterUser);

                            newStore.Followers = new List<Following>();
                            newStore.Followers.Add(new Following
                            {
                                FollowerUser = planToApprove.requesterUser,
                                followDate = DateTime.UtcNow
                            });

                            Setting newSetting = new Setting();
                            newStore.Setting = newSetting;

                            unitOfWork.StoreNotExpiredRepository.Insert(newStore);
                            planToApprove.approve = true;
                            unitOfWork.Save();

                            

                            if (!Roles.IsUserInRole(planToApprove.requesterUser.UserName, "StAdmin"))
                            {
                                Roles.AddUserToRole(planToApprove.requesterUser.UserName, "StAdmin");
                            }

                            return RedirectToAction("AllRequests");

                    }
                    else if (planToApprove is RevivalRequest)
                    {
                        var New = planToApprove as RevivalRequest;
                        New.approve = true;
                        DateTime lastPayement = unitOfWork.RevivalPlanRequestRepository.Get(r => r.storeID == New.storeID).Max(c => c.payment.payDate);
                        New.payment.payDate = lastPayement.AddDays(365);
                        unitOfWork.RevivalPlanRequestRepository.Update(New);
                        unitOfWork.StorePlanChange((int)New.storeID, New.plan);
                        unitOfWork.Save();
                        return RedirectToAction("AllRequests");
                    }
                    return new RedirectToError();
                }
                else if (planRequestToApprove.approve && planRequestToApprove.plantype == PlanType.User)
                {
                    if (planToApprove is RevivalRequest)
                    {
                        var New = planToApprove as RevivalRequest;

                            UserProfile newUser = new UserProfile();
                            if (planToApprove.plan.ToLower() == "Professional") { newUser = new Professional(); }
                            else if (planToApprove.plan.ToLower() == "Premium") { newUser = new Premium(); }
                            else if (planToApprove.plan.ToLower() == "Basic") { newUser = new Basic(); }
                            else { return RedirectToAction("AllRequests"); }

                            New.approve = true;
                            DateTime lastPayement = unitOfWork.RevivalPlanRequestRepository.Get(r => r.userID == New.userID).Max(c => c.payment.payDate);
                            New.payment.payDate = lastPayement.AddDays(365);
                            unitOfWork.RevivalPlanRequestRepository.Update(New);
                            unitOfWork.UserPlanChange((int)New.userID, New.plan);
                            planToApprove.approve = true;
                            unitOfWork.Save();
                            return RedirectToAction("AllRequests");

                    }
                    else return new RedirectToError();
                }
            }
            return new RedirectToError();
        }
    }
}
