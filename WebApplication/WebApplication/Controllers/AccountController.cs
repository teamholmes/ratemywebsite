using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using WebApplication.Models;
using MyApp.Business.Services;
using MyApp.Web.ViewModels;
using OP.General.Extensions;
using MyApp.Business.DomainObjects.Models;
using MyApp.Web.Controllers;
using System.Threading;
using MyApp.DAL.Repository;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Resources;

namespace WebApplication.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {

        private IAccountService _AccountService;
        private IConfiguration _Configuration;
        private ICaptchaService _CaptchaService;
        private IEmailService _EmailService;
        private ILog _Log;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(IAccountService accountService, IConfiguration configuration, ICaptchaService captchaservice, IEmailService emailservice, ILog log) //UserManager<ApplicationUser> userManager, 
        {
            //UserManager = userManager;
            _AccountService = accountService;
            _Configuration = configuration;
            _CaptchaService = captchaservice;
            _EmailService = emailservice;
            _Log = log;
        }





        ////
        //// GET: /Account/Login
        //[AllowAnonymous]
        //public ActionResult Login(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}

        [AllowAnonymous]
        public FileContentResult GetCaptchaImage(string enid, string cachecode, string rnd)
        {
            if (enid.IsNullOrEmpty()) return null;

            return new FileContentResult(_CaptchaService.GenerateCaptchaImageAsJpeg(enid, 140, 32), "image/jpeg");  // was 185 50 
        }


        [AllowAnonymous]
        //[ClearAllTokensFilter]
        public ActionResult Login(int? redirect, string rid)
        {

            _Log.Debug(string.Format("AccountController ActionResult Login() {0} ", ""));
            try
            {

                AuthenticationManager.SignOut();

                Response.Cookies.Clear();
            }
            catch (Exception err) { }

            try
            {
                Response.Cookies.Clear();

            }
            catch (Exception err) { }

            AuthenticationManager.SignOut();


            // section that redirects to clear the cookie to help resolve the antiforgerytoken issues
            if (!redirect.HasValue)
            {
                return RedirectToAction("Login", new { redirect = 1, rid = DateTime.Now.Ticks });
            }


            string captchacodeused = _CaptchaService.GenerateRandomCode(Request.Url.Host);

            LoginViewModel viewmodel = new LoginViewModel();
            viewmodel.EncryptedCaptcha = captchacodeused.Encrypt();


            return View(viewmodel);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel viewmodel, string returnUrl)
        {

            Log.Debug(string.Format("AccountController ActionResult Login() {0} ", ""));

            BusinessEnum.AuthenticationResult AuthenticationResult;

            if (ModelState.IsValid)
            {
                if (viewmodel.Captcha.TrimCheckForNull() != viewmodel.EncryptedCaptcha.Decrypt())
                {
                    ModelState.AddModelError("Captcha", EVAResource.invalidacaptchacode);
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationResult = _AccountService.Login(viewmodel.EmailAddress, viewmodel.Passphrase);

                    try
                    {
                        System.Web.Helpers.AntiForgery.Validate();
                    }

                    catch (HttpAntiForgeryException err)
                    {
                        Log.Debug(string.Format("HttpAntiForgeryException {0} ", err.Message));
                        return RedirectToAction("LogOff");
                    }

                    if (AuthenticationResult == BusinessEnum.AuthenticationResult.Passed)
                    {

                        // Bounce to the relevant home page - user should be logged in

                        SignIn(_AccountService.GetUserByEmailAddress(viewmodel.EmailAddress), false);

                        _AccountService.UpdateLoginDate(viewmodel.EmailAddress);

                        return RedirectToAction("DetermineHomePage");
                    }
                    else if (AuthenticationResult == BusinessEnum.AuthenticationResult.AccountNotActive)
                    {
                        ModelState.AddModelError("EmailAddress", String.Format(EVAResource.accounthasbeendisabled, _Configuration.AccountLockoutPeriodInMins));
                    }
                    else if (AuthenticationResult == BusinessEnum.AuthenticationResult.Locked)
                    {
                        ModelState.AddModelError("EmailAddress", String.Format(EVAResource.accounthasbeenlocked, _Configuration.AccountLockoutPeriodInMins));
                    }
                    else if (AuthenticationResult == BusinessEnum.AuthenticationResult.AccountNotConfirmed_Validated)
                    {
                        ModelState.AddModelError("EmailAddress", EVAResource.Accoutnotvalidated);
                    }
                    else if (AuthenticationResult == BusinessEnum.AuthenticationResult.PassphraseExpired || AuthenticationResult == BusinessEnum.AuthenticationResult.TempPasswordIssued)
                    {
                        ApplicationUser currentuser = _AccountService.GetUserByEmailAddress(viewmodel.EmailAddress);

                        UpdatePassPhraseAccountViewModel vmodelpass = new UpdatePassPhraseAccountViewModel();
                        vmodelpass.P1 = currentuser.Email.Encrypt();
                        vmodelpass.P2 = currentuser.Id.Encrypt();
                        vmodelpass.ReasonEnum = AuthenticationResult;
                        vmodelpass.ShowPassPhraseUpdateSuccess = false;
                        vmodelpass.HasComeFromHomePage = true;

                        string captchacodeused = _CaptchaService.GenerateRandomCode(Request.Url.Host);
                        vmodelpass.EncryptedCaptcha = captchacodeused.Encrypt();


                        try
                        {
                            ModelState["Captcha"].Value = new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentUICulture);
                            ModelState["EncryptedCaptcha"].Value = new ValueProviderResult(captchacodeused.Encrypt(), captchacodeused.Encrypt(), System.Globalization.CultureInfo.CurrentUICulture);
                        }
                        catch (Exception err) { }


                        return View("UpdatePassphrase", vmodelpass);
                    }
                    else if (viewmodel.Passphrase.NumberOfUpperCaseCharacters() >= 3)
                    {
                        ModelState.AddModelError("EmailAddress", EVAResource.LoginDetailsIncorrectCheckCapsLock);
                    }
                    else
                    {
                        ModelState.AddModelError("EmailAddress", EVAResource.logindetailsincorrect);
                    }
                }
            }

            string captchacodeused1 = _CaptchaService.GenerateRandomCode(Request.Url.Host);
            viewmodel.EncryptedCaptcha = captchacodeused1.Encrypt();

            try
            {
                ModelState["Captcha"].Value = new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentUICulture);
                ModelState["EncryptedCaptcha"].Value = new ValueProviderResult(captchacodeused1.Encrypt(), captchacodeused1.Encrypt(), System.Globalization.CultureInfo.CurrentUICulture);
            }
            catch (Exception err)
            { }
            return View(viewmodel);
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CreateAccount(CreateAccountViewModel viewmodel)
        {

            _Log.Debug(string.Format("AccountController ActionResult CreateAccount {0} ", ""));

            if (viewmodel.Captcha.TrimCheckForNull() != viewmodel.EncryptedCaptch.Decrypt())
            {
                ModelState.AddModelError("Captcha", EVAResource.invalidacaptchacode);
            }

            if (ModelState.IsValid)
            {
                Dictionary<string, string> claimsavailable = new Dictionary<string, string>();

                claimsavailable.Add(_Configuration.RoleKey, _Configuration.RoleUser);

                BusinessEnum.UnconfirmedTeamCreateAccountIssues accountcreationresult;

                accountcreationresult = _AccountService.CreateNewAccount(viewmodel.EmailAddress, viewmodel.Passphrasepart1, viewmodel.FirstName, viewmodel.Surname, viewmodel.ContactNumber, claimsavailable, true);

                if (accountcreationresult == BusinessEnum.UnconfirmedTeamCreateAccountIssues.EmailAlreadyExists)
                {
                    ModelState.AddModelError("EmailAddress", EVAResource.emailalreadyexists);
                }
                else if (accountcreationresult == BusinessEnum.UnconfirmedTeamCreateAccountIssues.InvalidPassphrase)
                {
                    ModelState.AddModelError("Passphrasepart1", EVAResource.invalidpassphrasecreation);
                }
                else
                {

                    if (!_Configuration.RequiresAccountVerificationAfterAccountCreation)
                    {
                        SignIn(_AccountService.GetUserByEmailAddress(viewmodel.EmailAddress), false);
                        _EmailService.SendAccountCreationConfirmation(_AccountService.GetUserByEmailAddress(viewmodel.EmailAddress).Id, viewmodel.EmailAddress, viewmodel.FirstName.ToAllowableFirstNameSurnameCharacters(), viewmodel.Surname.ToAllowableFirstNameSurnameCharacters(), GetBaseURL());
                        return RedirectToAction("DetermineHomePage");
                    }
                    else
                    {
                        throw new Exception("Not coded for");
                        return RedirectToAction("DetermineHomePage");
                    }

                }

            }

            string captchacodeused = _CaptchaService.GenerateRandomCode(Request.Url.Host);
            viewmodel.EncryptedCaptch = captchacodeused.Encrypt();
            viewmodel.Captcha = string.Empty;

            try
            {
                ModelState["EncryptedCaptch"].Value = new ValueProviderResult(captchacodeused.Encrypt(), captchacodeused.Encrypt(), System.Globalization.CultureInfo.CurrentUICulture);
                ModelState["Captcha"].Value = new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentUICulture);
            }
            catch (Exception err)
            { }


            return View("CreateAccount", viewmodel);
        }


        [AllowAnonymous]
        [OutputCache(Duration = 60, VaryByParam = "EmailAddress")]
        public JsonResult IsEmailAvailable(string EmailAddress)
        {

            _Log.Debug(string.Format("AccountController JsonResult IsEmailAvailable {0} ", ""));

            if (_AccountService.DoesAccountExistByEmailAddress(EmailAddress, null))
            {
                return Json(String.Format("'{0}' has already been taken", EmailAddress), JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);

        }


        [AllowAnonymous]
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CreateAccount()
        {

            _Log.Debug(string.Format("AccountController ActionResult CreateAccount {0} ", ""));

            CreateAccountViewModel viewmodel = new CreateAccountViewModel();

            string captchacodeused = _CaptchaService.GenerateRandomCode(Request.Url.Host);

            viewmodel.EncryptedCaptch = captchacodeused.Encrypt();

            return View(viewmodel);

        }



        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgottenPassPhrase()
        {
            Log.Debug(string.Format("AccountController ActionResult ForgottenPassPhrase() {0} ", ""));

            ForgottenPassPhraseAccountViewModel viewmodel = new ForgottenPassPhraseAccountViewModel();

            string captchacodeused = _CaptchaService.GenerateRandomCode(Request.Url.Host);

            viewmodel.CaptchaEncrypted = captchacodeused;


            return View(viewmodel);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgottenPassPhrase(ForgottenPassPhraseAccountViewModel viewmodel, string submitbutton)
        {
            Log.Debug(string.Format("AccountController ActionResult ForgottenPassPhrase() {0} ", submitbutton));


            if (viewmodel.Captcha.TrimCheckForNull() != viewmodel.CaptchaEncrypted.Decrypt())
            {
                ModelState.AddModelError("Captcha", EVAResource.invalidacaptchacode);
            }


            if (ModelState.IsValid)
            {
                if (_AccountService.DoesAccountExistByEmailAddress(viewmodel.EmailAddress, null))
                {
                    // user wants to get their passphrase emailed
                    if (submitbutton == @EVAResource.sendmemypassphrase)
                    {
                        BusinessEnum.PassphraseReset result = _AccountService.EmailTemporaryPassPhrase(viewmodel.EmailAddress, GetBaseURL());

                        if (result == BusinessEnum.PassphraseReset.Success)
                        {
                            viewmodel.IsResetSuccessful = true;
                        }
                        else if (result == BusinessEnum.PassphraseReset.NoSuchUser)
                        {
                            ModelState.AddModelError("EmailAddress", EVAResource.nosuchuser);
                        }
                        else if (result == BusinessEnum.PassphraseReset.Accountnotvalidated)
                        {
                            ModelState.AddModelError("EmailAddress", EVAResource.accountnotvalidated);
                        }
                        else
                        {
                            ModelState.AddModelError("EmailAddress", EVAResource.unabletoreset);
                        }
                    }
                    else if (submitbutton == @EVAResource.resendaccountconfirmation)
                    {
                        // determine the account
                        BusinessEnum.ResendAccountSignupEmails result = _AccountService.SendAccountConfirmationEmailBasedOnEmailAddress(viewmodel.EmailAddress, GetBaseURL());

                        if (result == BusinessEnum.ResendAccountSignupEmails.Accountalreadyvalidated)
                        {
                            ModelState.AddModelError("EmailAddress", EVAResource.accountalreadyactivated);
                        }
                        else if (result == BusinessEnum.ResendAccountSignupEmails.Fail)
                        {
                            ModelState.AddModelError("EmailAddress", EVAResource.problemobtainingyouraccount);
                        }
                        else
                        {

                            viewmodel.Feedbackmessage = EVAResource.waitforemailaccountproblems;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("EmailAddress", EVAResource.forgottenpassphraseemaildoesntexists);
                }

            }

            string captchacodeused = _CaptchaService.GenerateRandomCode(Request.Url.Host);

            viewmodel.CaptchaEncrypted = captchacodeused;

            try
            {
                ModelState["CaptchaEncrypted"].Value = new ValueProviderResult(captchacodeused.Encrypt(), captchacodeused.Encrypt(), System.Globalization.CultureInfo.CurrentUICulture);
                ModelState["Captcha"].Value = new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentUICulture);
            }
            catch (Exception err)
            { }

            return View(viewmodel);
        }



        [AllowAnonymous]
        public ActionResult ResetPassphrase(string t1, string t2, string t3, string t4)
        {
            Log.Debug(string.Format("AccountController ActionResult ResetPassphrase() {0} ", t1));

            if (String.IsNullOrEmpty(t1) || string.IsNullOrEmpty(t2) || string.IsNullOrEmpty(t3)) return Content("Missing parameters");

            string emailaddress = t1.Decrypt();
            string userId = t2.Decrypt();

            string captchacodeused = _CaptchaService.GenerateRandomCode(Request.Url.Host);

            UpdatePassPhraseAccountViewModel vmodel = new UpdatePassPhraseAccountViewModel();
            vmodel.P1 = emailaddress.Encrypt();
            vmodel.P2 = userId.Encrypt();
            vmodel.P3 = t3;
            vmodel.P4 = t4;
            vmodel.ShowPassPhraseUpdateSuccess = false;

            vmodel.EncryptedCaptcha = captchacodeused;

            if (t4 == "pr") vmodel.ReasonEnum = BusinessEnum.AuthenticationResult.TempPasswordIssued;

            vmodel.HasComeFromHomePage = false;

            return View("UpdatePassphrase", vmodel);
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassphrase(UpdatePassPhraseAccountViewModel viewmodel)
        {
            Log.Debug(string.Format("AccountController ActionResult UpdatePassphrase() {0} ", ""));

            if (String.IsNullOrEmpty(viewmodel.P1) || string.IsNullOrEmpty(viewmodel.P2)) return Content("Invalid parameters");


            if (viewmodel.Captcha.TrimCheckForNull() != viewmodel.EncryptedCaptcha.Decrypt())
            {
                ModelState.AddModelError("Captcha", EVAResource.invalidacaptchacode);
            }

            // regenerate a captcha code
            string captchacodeused = _CaptchaService.GenerateRandomCode(Request.Url.Host);

            viewmodel.EncryptedCaptcha = captchacodeused;

            try
            {
                ModelState["EncryptedCaptcha"].Value = new ValueProviderResult(captchacodeused.Encrypt(), captchacodeused.Encrypt(), System.Globalization.CultureInfo.CurrentUICulture);
                ModelState["Captcha"].Value = new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentUICulture);
            }
            catch (Exception err)
            { }

            if (ModelState.IsValid)
            {


                string username = viewmodel.P1.Decrypt();
                string userId = viewmodel.P2.Decrypt();
                string passwordtoken = viewmodel.P3;

                if (!viewmodel.EmailAddress.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    ModelState.AddModelError("EmailAddress", EVAResource.theemailaddresseneteredwasnotexpected);
                    return View(viewmodel);
                }

                BusinessEnum.PassphraseCreation creationstatus = _AccountService.UpdatePassPhrase(passwordtoken, viewmodel.EmailAddress, viewmodel.ExistingPassword, userId, viewmodel.NewPassPhrase);

                if (creationstatus == BusinessEnum.PassphraseCreation.PreviouslyUsed)
                {
                    ModelState.AddModelError("NewPassPhrase", AccountResource.previouslyused);
                }
                else if (creationstatus == BusinessEnum.PassphraseCreation.SameAsExisiting)
                {
                    ModelState.AddModelError("NewPassPhrase", AccountResource.sameasexisitng);
                }
                else if (creationstatus == BusinessEnum.PassphraseCreation.NotValidPassPhrase)
                {
                    ModelState.AddModelError("NewPassPhrase", GeneralResource.invalidpassphrasecreation);
                }
                else if (creationstatus == BusinessEnum.PassphraseCreation.Existingpasswordnotmatched)
                {
                    string t = GeneralResource.existingpasswordisnotcorrect;
                    ModelState.AddModelError("ExistingPassword", GeneralResource.existingpasswordisnotcorrect);
                }
                else if (creationstatus == BusinessEnum.PassphraseCreation.Success)
                {
                    // autologin
                    BusinessEnum.AuthenticationResult loginresult = _AccountService.Login(viewmodel.EmailAddress, viewmodel.NewPassPhrase);

                    if (loginresult == BusinessEnum.AuthenticationResult.Passed)
                    {
                        return RedirectToAction("DetermineHomePage");
                    }

                    viewmodel.Homepage = GetBaseURL();
                    viewmodel.ShowPassPhraseUpdateSuccess = true;

                }
                else
                {
                    ModelState.AddModelError("NewPassPhrase", EVAResource.passphrasefailedtochange);
                }

            }

            return View(viewmodel);
        }



        [HttpGet]
        //[ClaimsAuthorize("Role", "user")]
        [ClaimsAuthorize("Role", "apple", "user", "banana")]
        public ActionResult DetermineHomePage()
        {
            Log.Debug(string.Format("AccountController ActionResult DetermineHomePage() {0} ", ""));

            string id = GetUserClaimValueForKey("Id");

            string roleclaimvalue = GetUserClaimValueForKey(_Configuration.RoleKey);

            if (roleclaimvalue.ToUpperCheckForNull() == _Configuration.RoleDev.ToUpperCheckForNull()) return RedirectToAction("Overview", "Admin");

            if (roleclaimvalue.ToUpperCheckForNull() == _Configuration.RoleAdmin.ToUpperCheckForNull()) return RedirectToAction("Overview", "Admin");

            if (roleclaimvalue.ToUpperCheckForNull() == _Configuration.RoleUser.ToUpperCheckForNull()) return RedirectToAction("Overview", "Employer");


            return Content("Unable to redirect you to the appropriate page");
        }








        ////
        //// GET: /Account/Register
        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser() { UserName = model.UserName };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        ////
        //// POST: /Account/Disassociate
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        //{
        //    ManageMessageId? message = null;
        //    IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
        //    if (result.Succeeded)
        //    {
        //        message = ManageMessageId.RemoveLoginSuccess;
        //    }
        //    else
        //    {
        //        message = ManageMessageId.Error;
        //    }
        //    return RedirectToAction("Manage", new { Message = message });
        //}

        ////
        //// GET: /Account/Manage
        //public ActionResult Manage(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //        : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //        : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : "";
        //    ViewBag.HasLocalPassword = HasPassword();
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    return View();
        //}

        ////
        //// POST: /Account/Manage
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Manage(ManageUserViewModel model)
        //{
        //    bool hasPassword = HasPassword();
        //    ViewBag.HasLocalPassword = hasPassword;
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    if (hasPassword)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            }
        //            else
        //            {
        //                AddErrors(result);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // User does not have a password so remove any validation errors caused by a missing OldPassword field
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //            }
        //            else
        //            {
        //                AddErrors(result);
        //            }
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        ////
        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        ////
        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var user = await UserManager.FindAsync(loginInfo.Login);
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        // If the user does not have an account, then prompt the user to create an account
        //        ViewBag.ReturnUrl = returnUrl;
        //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
        //    }
        //}

        ////
        //// POST: /Account/LinkLogin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LinkLogin(string provider)
        //{
        //    // Request a redirect to the external login provider to link a login for the current user
        //    return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        //}

        ////
        //// GET: /Account/LinkLoginCallback
        //public async Task<ActionResult> LinkLoginCallback()
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //    }
        //    var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Manage");
        //    }
        //    return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser() { UserName = model.UserName };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInAsync(user, isPersistent: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Account", "Login");
        }

        ////
        //// GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        //[ChildActionOnly]
        //public ActionResult RemoveAccountList()
        //{
        //    var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
        //    ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
        //    return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && Umanager != null)
        //    {
        //        Umanager.Dispose();
        //        Umanager = null;
        //    }
        //    base.Dispose(disposing);
        //}

        #region Helpers
        // Used for XSRF protection when adding external logins
        //private const string XsrfKey = "XsrfId";



        //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //    //var identity = await Umanager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        //    //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        //}



        private void SignIn(ApplicationUser user, bool isPersistent)
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);


            ClaimsIdentity identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id)
            }, DefaultAuthenticationTypes.ApplicationCookie);


            foreach (ApplicationUserClaim clm in user.Claims)
            {
                identity.AddClaim(new Claim(clm.ClaimType, clm.ClaimValue));
            }


            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

            System.Security.Claims.ClaimsPrincipal cp = new ClaimsPrincipal(User.Identity);


            string t = "41";
        }

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        //private bool HasPassword()
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        return user.PasswordHash != null;
        //    }
        //    return false;
        //}

        //public enum ManageMessageId
        //{
        //    ChangePasswordSuccess,
        //    SetPasswordSuccess,
        //    RemoveLoginSuccess,
        //    Error
        //}

        //private ActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        //private class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}
        #endregion
    }
}