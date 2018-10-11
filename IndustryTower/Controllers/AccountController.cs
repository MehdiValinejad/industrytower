using DotNetOpenAuth.AspNet;
using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Filters;
using IndustryTower.Mailers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using Microsoft.Web.WebPages.OAuth;
using Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class AccountController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }



        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Home", "UserProfile");
            }
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }


        

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", Resource.ControllerError.nameOrPassIncorrect);
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Home", "UserProfile");
            }
            CountriesViewModel countries = new CountriesViewModel();
            ViewBag.CountryDropDown = countries.countries;
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    
                    var isEmail = unitOfWork.UserRepository.Get(u => u.Email == model.Email).Any();
                    if (isEmail)
                    {
                        throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);
                    }

                    string confirmationToken = WebSecurity.CreateUserAndAccount(
                        model.UserName,
                        model.Password, new
                        {
                            model.Birthday,
                            model.Email,
                            model.stateID,
                            model.firstName,
                            model.firstNameEN,
                            model.lastName,
                            model.lastNameEN,
                            model.gender
                        }, true);

                    try
                    {
                        UserMailer.Registraion(new MailRgisterConfirmationViewModel
                        {
                            EmailAddress = model.Email,
                            DisplayName = model.firstName + " " + model.lastName,
                            UserName = model.UserName,
                            Token = confirmationToken
                        }).Send();
                    }
                    catch
                    {
                        ViewData["errorMess"] = ControllerError.registerActivationEmailError;
                        ViewData["link"] = Url.Action("ResendConfirmationToken", "Account");
                        return View("~/Views/Error/CustomError.cshtml");
                    }
                    return RedirectToAction("RegisterStepTwo", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    CountriesViewModel countries = new CountriesViewModel();
                    ViewBag.CountryDropDown = countries.countries;
                    ModelState.AddModelError("", GetErrorMessage(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return  ControllerError.MembershipError_DuplicateUserName;

                case MembershipCreateStatus.DuplicateEmail:
                    return ControllerError.MembershipError_DuplicateEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return ControllerError.MembershipError_InvalidPassword;

                case MembershipCreateStatus.InvalidEmail:
                    return ControllerError.MembershipError_InvalidEmail;

                case MembershipCreateStatus.InvalidAnswer:
                    return ControllerError.MembershipError_InvalidAnswer;

                case MembershipCreateStatus.InvalidQuestion:
                    return ControllerError.MembershipError_InvalidQuestion;

                case MembershipCreateStatus.InvalidUserName:
                    return ControllerError.MembershipError_InvalidUserName;

                case MembershipCreateStatus.ProviderError:
                    return ControllerError.MembershipError_ProviderError;

                case MembershipCreateStatus.UserRejected:
                    return ControllerError.MembershipError_UserRejected;

                default:
                    return ControllerError.MembershipError_GeneralError;
            }
        }


        [AllowAnonymous]
        public ActionResult RegisterStepTwo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult AlreadyConfirmed()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResendConfirmationToken()
        {
            return View();
        }


        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResendConfirmationToken(string email)
        {
            //using (var db = new ITTContext())
            //{
            //    var tsqlQuery = string.Format("SELECT [ConfirmationToken] FROM [webpages_Membership] WHERE [UserId] IN (SELECT [UserId] FROM [UserProfile] WHERE [Email] LIKE '{0}')", email);
            //    var Token =  db.Database.SqlQuery<string>(tsqlQuery).First();
            //}
            ActiveUser user = unitOfWork.ActiveUserRepository.Get(u => u.Email == email).SingleOrDefault();
            if (user != null)
            {
                var Membership = unitOfWork.Webpages_MembershipRepository.Get(m => m.UserId == user.UserId).Single();
                if (!WebSecurity.IsConfirmed(user.UserName))
                {
                    try
                    {
                        UserMailer.ConfirmResend(new MailResendConfirmationViewModel
                        {
                            EmailAddress = user.Email,
                            DisplayName = user.firstName + " " + user.lastName,
                            UserName = user.UserName,
                            Token = Membership.ConfirmationToken
                        }).Send();
                        return RedirectToAction("RegisterStepTwo", "Account");
                    }
                    catch
                    {
                        ModelState.AddModelError("", ControllerError.resendConfirmationEmailError);
                    }
                }
                else
                {
                    return RedirectToAction("AlreadyConfirmed", "Account");
                }


            }
            else ModelState.AddModelError("", ControllerError.emailWasNotFound);
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string CToken)
        {
            if (!String.IsNullOrEmpty(CToken)) //&& (Regex.IsMatch(Id, @"[0-9a-f]{8}\-([0-9a-f]{4}\-){3}[0-9a-f]{12}")))
            {
                if (WebSecurity.ConfirmAccount(CToken))
                {
                    var membership = unitOfWork.Webpages_MembershipRepository.Get(c => c.ConfirmationToken == CToken).Single();
                    var user = unitOfWork.ActiveUserRepository.GetByID(membership.UserId);
                    Setting newsetting = new Setting();
                    user.Setting = newsetting;
                    unitOfWork.ActiveUserRepository.Update(user);
                    unitOfWork.Save();
                    return RedirectToAction("ConfirmationSuccess");
                }
            }
            return RedirectToAction("ConfirmationFailure");
        }

        [AllowAnonymous]
        public ActionResult ConfirmationSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmationFailure()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string UserName)
        {
            //check user existance
            var user = Membership.GetUser(UserName);
            if (user == null)
            {
                ViewData["message"] = Resource.Resource.userNotExist;
            }
            else
            {
                //generate password token
                var token = WebSecurity.GeneratePasswordResetToken(UserName);
                //create url with above token
                //var resetLink = Url.Action("ResetPassword", "Account", new { un = UserName, rt = token }, "http") ;
                //get user emailid
                var User = unitOfWork.ActiveUserRepository.Get(i => i.UserName == UserName)
                                                     .FirstOrDefault();

                try
                {
                    UserMailer.PasswordReset(new MailPassResetConfirmationViewModel
                    {
                        EmailAddress = User.Email,
                        DisplayName = User.firstName + " " + User.lastName,
                        UserName = User.UserName,
                        Token = token
                    }).Send();

                    return View("ResetEmailSent");
                }
                catch
                {
                    ViewData["message"] = Resource.Resource.passResetMailError;
                }
            }
            return View();
        }


        [AllowAnonymous]
        public ActionResult ResetPassword(string un, string rt)
        {
            //TODO: Check the un and rt matching and then perform following
            //get userid of received username
            var userID = unitOfWork.ActiveUserRepository.Get(i => i.UserName == un)
                                                  .FirstOrDefault().UserId;
            bool Any = unitOfWork.Webpages_MembershipRepository
                         .Get(a => a.UserId == userID 
                           && a.PasswordVerificationToken == rt)
                           //&& a.PasswordVerificationTokenExpirationDate < DateTime.Now)
                         .Any();

            if (Any == true)
            {
                //generate random password
                string newpassword = GenerateRandomPassword(6);
                //reset password
                bool response = WebSecurity.ResetPassword(rt, newpassword);
                if (response == true)
                {
                    //get user emailid to send password
                    var user = unitOfWork.ActiveUserRepository.Get(u => u.UserName == un)
                                                         .FirstOrDefault();

                    try
                    {
                        UserMailer.NewPass(new MailNewPassConfirmationViewModel
                        {
                            EmailAddress = user.Email,
                            DisplayName = user.firstName + " " + user.lastName,
                            UserName = user.UserName,
                            NewPass = newpassword
                        }).Send();
                        ViewData["message"] = Resource.Resource.newPassMailSent;
                    }
                    catch
                    {
                        ViewData["message"] = Resource.Resource.passResetMailError;
                    }

                }
                else
                {
                    return new RedirectToError();
                }
            }
            else
            {
                ViewData["message"] = Resource.Resource.passResetTokenNotMatch;
            }

            return View();
        }

        private string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }


        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.CurrentUserId);
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? Resource.Resource.passwordChanged
                : message == ManageMessageId.SetPasswordSuccess ? Resource.Resource.passwordHasBeenSet
                : message == ManageMessageId.RemoveLoginSuccess ? Resource.Resource.extenralLognRemoved
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.CurrentUserId);
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.CurrentUserId);
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", Resource.ControllerError.passWordChangeError);
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (ITTContext db = new ITTContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.CurrentUserId);
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
