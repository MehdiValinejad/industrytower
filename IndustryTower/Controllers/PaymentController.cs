using IndustryTower.DAL;
using IndustryTower.Exceptions;
using IndustryTower.Filters;
using IndustryTower.Helpers;

using IndustryTower.Models;
using IndustryTower.ViewModels;
using Resource;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class PaymentController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        



        
        void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate(
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }



        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult Pay(string StId, string UId, string CoId)
        {
            var tt = typeof(Company).Assembly.GetTypes().Where(t => t.BaseType == typeof(Company));
            PayViewModel viewmodel = new PayViewModel();
            viewmodel.companyToPay = CoId != null ? unitOfWork.CompanyRepository
                                                              .GetByID(EncryptionHelper.Unprotect(CoId)) 
                                                  : null;
            viewmodel.StoreToPay = StId != null ? unitOfWork.StoreRepository
                                                            .GetByID(EncryptionHelper.Unprotect(CoId)) 
                                                : null;
            viewmodel.UserToPay = UId != null ? unitOfWork.ActiveUserRepository
                                                          .GetByID(EncryptionHelper.Unprotect(CoId)) 
                                              : null;
            //PayBank SelectList
            var payBanks = from PayBank e in Enum.GetValues(typeof(PayBank))
                           select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
            viewmodel.bankTypeToSelectList = new SelectList(payBanks, "Id", "Name");

            return View(viewmodel);
        }

        [ITTAuthorizeAttribute]
        public ActionResult UserPayments(string UId)
        {
            var payments = unitOfWork.PaymentRepository.Get(p => p.userID == WebSecurity.CurrentUserId);
            return View(payments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult Pay(PayViewModel paymentVM, string CoId, string StId, string UId)
        {
            string[] formcontrol = { paymentVM.CoId,
                                   paymentVM.StId,
                                   paymentVM.UId };
            if (formcontrol.Count(t => !String.IsNullOrEmpty(t)) == 1)
            {
                Payment payment = new Payment();
                payment.payDate = DateTime.UtcNow;
                payment.payBank = paymentVM.payBank;
                if (!String.IsNullOrEmpty(paymentVM.CoId) && CoId == paymentVM.CoId)
                {
                    var coid = EncryptionHelper.Unprotect(paymentVM.CoId);
                    payment.coID = coid;
                    payment.payAmount = PlansPrices.CompanyPlans[paymentVM.Name];
                    unitOfWork.PaymentRepository.Insert(payment);
                    unitOfWork.CompanyPlanChange((int)coid, paymentVM.Name);
                    unitOfWork.Save();
                }
                else if (!String.IsNullOrEmpty(paymentVM.StId) && StId == paymentVM.StId)
                {
                    var stid = EncryptionHelper.Unprotect(paymentVM.StId);
                    payment.storeID = stid;
                    payment.payAmount = PlansPrices.StorePlans[paymentVM.Name];
                    unitOfWork.PaymentRepository.Insert(payment);
                    unitOfWork.StorePlanChange((int)stid, paymentVM.Name);
                    unitOfWork.Save();

                }
                else if (paymentVM.UId != null && UId == paymentVM.UId)
                {
                    var uid = EncryptionHelper.Unprotect(paymentVM.UId);
                    payment.userID = uid;
                    payment.payAmount = PlansPrices.UserPlans[paymentVM.Name];
                    unitOfWork.PaymentRepository.Insert(payment);
                    unitOfWork.UserPlanChange((int)uid, paymentVM.Name);
                    unitOfWork.Save();
                }
                else //create 
                { 

                }
            }
            return View();
        }


        public ActionResult PayForPlan(string reqId)
        {

            NullChecker.NullCheck(new object[] { reqId });

            var request = unitOfWork.PlanRequetRepository.GetByID(EncryptionHelper.Unprotect(reqId));
            if (request.payment == null)
            {
                if (request.requesterUserID == WebSecurity.CurrentUserId
                && request.approve == false)
                {
                    PayForPlanViewModel viewmodel = new PayForPlanViewModel();
                    viewmodel.reqId = EncryptionHelper.Protect(request.reqID);
                    viewmodel.requesterUserId = EncryptionHelper.Protect(request.requesterUserID);
                    viewmodel.plan = request.plan;
                    if (request is RequestForNew)
                    {
                        viewmodel.PlanRequest = (RequestForNew)request;
                    }
                    else if (request is RevivalRequest)
                    {
                        viewmodel.PlanRequestRevival = (RevivalRequest)request;
                    }

                    var payBanks = from PayBank e in Enum.GetValues(typeof(PayBank))
                                   select new { Id = e, Name = Resource.EnumTypes.ResourceManager.GetString(e.ToString()) };
                    viewmodel.bankTypeToSelectList = new SelectList(payBanks, "Id", "Name");

                    return View(viewmodel);
                }
                else return new RedirectToError();
            }
            return View("PayBefore");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HostControl]
        public ActionResult PayForPlan(PayForPlanViewModel payForPlanToInsert, string reqId)
        {
            NullChecker.NullCheck(new object[] { reqId });
            var reqid = EncryptionHelper.Unprotect(reqId);
            var requestToPay = unitOfWork.PlanRequetRepository.GetByID(reqid);
            if (ModelState.IsValid)
            {
                if (requestToPay.requesterUserID == WebSecurity.CurrentUserId && reqId == payForPlanToInsert.reqId)
                {
                    if (payForPlanToInsert.payBank == PayBank.PayPal)
                    {
                        string result;
                        String[] resultArray = {};
                        long token = 0;
                        BypassCertificateError();
                        
                       //Payment process goes here *****

                        //TempData["res"] = result;

                        if (resultArray[0] == "0")
                        {
                            requestToPay.reqToken = token;
                            unitOfWork.Save();
                            
                            ViewData["RefId"] = resultArray[1];
                            return View("~/Views/Payment/PaymentRedirect.cshtml");
                        }
                        else
                        {
                            ViewData["errorMess"] = Resource.Resource.payTransactionError;
                            return View("~/Views/Error/CustomError.cshtml");
                        }
                    }
                    else return new RedirectToError();
                }
                else return new RedirectToError();
            }
            else throw new ModelStateException(this.ModelState);
        }

        


        long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxRequestOnly]
        //[HostControl]
        //public ActionResult PayForPlan(PayForPlanViewModel payForPlanToInsert ,string reqId)
        //{
        //    NullChecker.NullCheck(new object[] { reqId });
        //    var reqid = EncryptionHelper.Unprotect(reqId);
        //    var requestToPay = unitOfWork.PlanRequetRepository.GetByID(reqid);
        //    if (payForPlanToInsert.payType == PayType.Cash)
        //    {
        //        this.ModelState.Remove("payCode");
        //        this.ModelState.Remove("payAmount");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        if (requestToPay.requesterUserID == WebSecurity.CurrentUserId && reqId == payForPlanToInsert.reqId)
        //        {
        //            if (payForPlanToInsert.payAmount == PlansPrices.AllPlans[requestToPay.plan])
        //            {
        //                Payment paymentForRequest = new Payment();
        //                if (payForPlanToInsert.payType == PayType.Cash)
        //                {
        //                    paymentForRequest.payType = PayType.Cash;
        //                    paymentForRequest.reqID = requestToPay.reqID;
        //                    paymentForRequest.payAmount = (long)payForPlanToInsert.payAmount;
        //                }
        //                else if (payForPlanToInsert.payType == PayType.Bank)
        //                {
        //                    paymentForRequest.payType = PayType.Bank;
        //                    paymentForRequest.reqID = requestToPay.reqID;
        //                    paymentForRequest.payBank = payForPlanToInsert.payBank;
        //                    paymentForRequest.payCode = (long)payForPlanToInsert.payCode;
        //                    paymentForRequest.payAmount = (long)payForPlanToInsert.payAmount;
        //                    //paymentForRequest.payType = payForPlanToInsert.payType;
        //                }
        //                else throw new JsonCustomException(ControllerError.ajaxError);

        //                paymentForRequest.payDate = DateTime.UtcNow;
        //                unitOfWork.PaymentRepository.Insert(paymentForRequest);
        //                unitOfWork.Save();
        //                return Json(new { URL = Url.Action("PaymentSuccessful", "Payment") });
        //                //return View("PaymentSuccessful");
        //            }
                    
        //        }
        //        throw new JsonCustomException(ControllerError.ajaxError);
        //    }
        //    else throw new ModelStateException(this.ModelState);
        //}



        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult ExpiredAccounts()
        {

            return View();
        }

        [ITTAuthorizeAttribute(Roles = "ITAdmin")]
        public ActionResult ExpiredSearch(string q)
        {
            ExpiredViewModel viewmodel = new ExpiredViewModel();
            viewmodel.expiredCompanies = unitOfWork.ExpiredCompanyRepository.Get(filter: s => s.coName.Contains(q.ToLower()) 
                                                                                         || s.coNameEN.Contains(q.ToLower()));
            viewmodel.expiredStores = unitOfWork.StoreExpiredRepository.Get(filter: s => s.storeName.Contains(q.ToLower())
                                                                                         || s.storeNameEN.Contains(q.ToLower()));
            return View(viewmodel);
        }

        
    }
}
