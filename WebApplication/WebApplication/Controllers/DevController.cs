using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.Business.Services;
using MyApp.Web.ViewModels;
using MyApp.Business.DomainObjects.Models;

using OP.General.Encryption;
using System.Text;
using System.Configuration;
using System.Collections;
using OP.General.Extensions;
using System.Resources;
using System.IO;
using OP.General.Performance;
using WebApplication.Controllers;
using Thinktecture.IdentityModel.Authorization.WebApi;
using System.IdentityModel.Services;
using System.Security.Permissions;
using System.Security.Claims;


namespace MyApp.Web.Controllers
{
   //[ClaimsPrincipalPermission(SecurityAction.Demand, Resource="DEV", Operation="ROLE")]
    //[ClaimsAuthorize("1ROLE","FISHING")]
    //[SimpleAuthorize()]
    //[Authorize()]
    public class DevController : BaseController
    {

        private IAccountService _AccountService;
        private IConfiguration _Configuration;
        private IEmailService _EmailService;
        private ISmtpService _SmtpService;


        public DevController(IAccountService accountService, IConfiguration configuration, IEmailService emailservice, ISmtpService smtpservice)
        {
            _AccountService = accountService;

            _Configuration = configuration;

            _EmailService = emailservice;

            _SmtpService = smtpservice;
        }


        [HttpGet]
        public ActionResult WriteOutApplicationLog()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0}Writing out application method trace {0}{0}", "</br>"));

            string applogkey = "BACKLOGKEYFORAPP";

            if (HttpContext.Application[applogkey] != null)
            {
                foreach (string str in (List<string>)HttpContext.Application[applogkey])
                {
                    sb.Append(String.Format("{0}{1}", str, "</br>"));
                }
            }

            return Content(sb.ToString());
        }


        [HttpGet]
        public ActionResult Throwerror()
        {
            throw new Exception("Test Exception created by developer");
            return Content("hello");
        }


       

        [HttpGet]
        public ActionResult ChangePassphrase()
        {
            ChangePassPhraseViewModel viewmodel = new ChangePassPhraseViewModel();
            return View(viewmodel);
        }



        [HttpPost]
        public ActionResult ChangePassphrase(ChangePassPhraseViewModel viewmodel)
        {

            if (viewmodel.ValidationCode.TrimCheckForNull() != "westpoint" + DateTime.Now.Month.ToString())
            {
                ModelState.AddModelError("ValidationCode", "Invalid validation code");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser selecteduser = _AccountService.GetUserByEmailAddress(viewmodel.EmailAddress);

                if (selecteduser != null && selecteduser.Id.IsNotNullOrEmpty())
                {

                    Boolean success = _AccountService.ChangeUserToTemporaryPasswordIssue(selecteduser, viewmodel.NewPassphrase.TrimCheckForNull());

                    if (!success)
                    {
                        viewmodel.result = "Unable to change passphrase";
                    }
                    else
                    {
                        viewmodel.result = "Passphrase has been changed. Note - no emails have been sent and the user must change passphrase at next login.";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Cannot find user");
                }
            }
            return View(viewmodel);
        }






        [HttpGet]
        public ActionResult SendTestEmail()
        {
            SendTestEmailViewModel viewmodel = new SendTestEmailViewModel();
            viewmodel.senderName = _Configuration.SmtpSenderName;
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendTestEmail(SendTestEmailViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {

                _EmailService.SendDevTestEmails(viewmodel.fromemailaddress, viewmodel.emailaddress, viewmodel.subject, viewmodel.boodycopy, viewmodel.bccEmailAddress);

                ModelState.AddModelError("", "Email sent to " + viewmodel.emailaddress);

            }
            return View(viewmodel);
        }


        [HttpGet]
        public ActionResult Decrypt()
        {
            DecryptViewModel viewmodel = new DecryptViewModel();
            return View(viewmodel);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Decrypt(DecryptViewModel viewmodel)
        {
            if (!String.IsNullOrEmpty(viewmodel.encrypted))
            {
                viewmodel.decrypted = string.Empty;
                viewmodel.result = viewmodel.encrypted.Decrypt();
            }
            else if (!String.IsNullOrEmpty(viewmodel.decrypted))
            {
                viewmodel.encrypted = string.Empty;
                viewmodel.result = viewmodel.decrypted.Encrypt();
            }
            return View(viewmodel);
        }



        [HttpGet]
        public ActionResult ClearApplicationCache()
        {

            StringBuilder sb = new StringBuilder();
            List<string> toRemove = new List<string>();
            foreach (DictionaryEntry cacheItem in HttpRuntime.Cache)
            {
                toRemove.Add(cacheItem.Key.ToString());
            }
            foreach (string key in toRemove)
            {
                sb.Append(String.Format("Removing cache for key : <strong>{0}</strong></br>", key));
                HttpRuntime.Cache.Remove(key);
            }

            sb.Append("<script>alert('Application Data Cache will be cleared - Please press F5(Refresh)');history.go(-1)</script><br/><strong>Your page will now be loaded with a fresh set of data</strong><br/>");
            return Content(sb.ToString());
        }

        //[HttpGet]
        //public ActionResult WriteOutApplicationLog()
        //{

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(String.Format("{0}Writing out application method trace {0}{0}", "</br>"));

        //    string applogkey = "BACKLOGKEYFORAPP";

        //    if (HttpContext.Application[applogkey] != null)
        //    {
        //        foreach (string str in (List<string>)HttpContext.Application[applogkey])
        //        {
        //            sb.Append(String.Format("{0}{1}", str, "</br>"));
        //        }
        //    }

        //    return Content(sb.ToString());
        //}


        [HttpGet]
        public ActionResult Postinstallationcheck(Boolean? op)
        {
            if (!op.HasValue || op.Value != true) return Content("Incorrect paramaters expected");

            string nline = "<br/><br/>";
            StringBuilder sb = new StringBuilder();

            // test the email system
            sb.Append(String.Format("Sending test email to '{0}' - <strong>{1}</strong>{2}", _Configuration.PostinstalaltionCheckEmailDestinationemailaddress, (_EmailService.SendDevTestEmails(_Configuration.SmtpSenderEmailAddress, _Configuration.PostinstalaltionCheckEmailDestinationemailaddress, "Test Email - " + DateTime.Now.ToShortTimeString(), "Body copy", null) == true ? "Pass" : "Fail"), nline));

            // test the db
            Boolean result = true;
            int num = 0;
            try
            {
                List<ApplicationUser> rolesindb = _AccountService.GetAllUsers();
                num = rolesindb.Count();
            }
            catch (Exception)
            {
                result = false;
            }
            sb.Append(String.Format("Accessing database - <strong>{0}</strong> (returned value '{1}'){2}", (result == true ? "Pass" : "Fail"), num, nline));

            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            foreach (String str in connstring.Split(';'))
            {
                if (str.Contains("Catalog="))
                {
                    sb.Append(String.Format("DB Catalog - <strong>{0}</strong>{1}", str, nline));
                }
                else if (str.Contains("Data Source="))
                {
                    sb.Append(String.Format("DB Datasource - <strong>{0}</strong>{1}", str, nline));
                }

            }


            // writing out some basic config settings
            sb.Append(String.Format("App Version   '{0}' {1}", _Configuration.AppVersion, nline));
            sb.Append(String.Format("SmtpHost  '{0}' {1}", _Configuration.SmtpHost, nline));
            sb.Append(String.Format("SmtpPort  '{0}' {1}", _Configuration.SmtpPort, nline));
            sb.Append(String.Format("SmtpCredentials.UserName  '{0}' {1}", _Configuration.SmtpCredentials.UserName, nline));
            sb.Append(String.Format("SmtpSenderEmailAddress  '{0}' {1}", _Configuration.SmtpSenderEmailAddress, nline));
            sb.Append(String.Format("SmtpSenderName  '{0}' {1}", _Configuration.SmtpSenderName, nline));
            sb.Append(String.Format("SmtpSmtpRequiresSsl  '{0}' {1}", _Configuration.SmtpSmtpRequiresSsl, nline));
            sb.Append(String.Format("PostinstalaltionCheckEmailDestinationemailaddress  '{0}' {1}", _Configuration.PostinstalaltionCheckEmailDestinationemailaddress, nline));
            sb.Append(String.Format("AccountLockoutPeriodInMins  '{0}' {1}", _Configuration.AccountLockoutPeriodInMins, nline));


            // write out contents of folder

            sb.Append(GetFilePresent("~\\Content\\Template\\", "xxxxxx.pdf"));


            // write out web config

            sb.Append(WriteOutWebConfig());

            // write out application methods called

            sb.Append(String.Format("{0}{0}Writing out application method trace {0}", nline));

            string applogkey = "BACKLOGKEYFORAPP";

            if (HttpContext.Application[applogkey] != null)
            {
                foreach (string str in (List<string>)HttpContext.Application[applogkey])
                {
                    sb.Append(String.Format("{0}{1}", str, nline));
                }
            }




            return Content(sb.ToString());

        }

        //public DevToolsController()
        //{

        //}

        //[Authorize(Roles = "DEV")]
        //public ActionResult QUnit()
        //{
        //    return View();
        //}

        //[Authorize(Roles = "DEV")]
        //public ActionResult ViewLog()
        //{
        //    ViewLogsViewModel viewLogs = new ViewLogsViewModel();
        //    viewLogs.logs = _DevService.GetAllLogs();
        //    return View(viewLogs);
        //}


        /// <summary>
        /// Method that checks for files being present
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        protected string GetFilePresent(string folder, string filename)
        {
            string retVal = string.Empty;

            retVal += "<br/>";

            // checking pdf template exists
            // string files = String.Format("~\\{0}\\", path);
            string FolderPathLIC = Server.MapPath(folder);
            string actualfile = filename;
            string filer = FolderPathLIC + actualfile;

            //retVal += (String.Format("<br/>Checking {0} Exists '{1}'<br/>", filename, filer));
            try
            {

                retVal += (System.IO.File.Exists(filer) ? String.Format("<strong>PASS</strong> - '{0}' Exists<br/>", actualfile) : String.Format("<strong>FAIL</strong> - '{0}' Does NOT EXIST<br/>", actualfile));
            }
            catch
            {
                retVal += (String.Format("<strong>CATCH FAIL</strong> - {0}<br/>", actualfile));
            }
            finally
            {

            }

            return retVal;

        }

        protected string WriteFilesInDirectory(string directory)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("-- Directory Listing : {0}{1}", directory, "<br/>"));
            string[] files = Directory.GetFiles(directory);

            // iterate through each of the files
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);

                sb.Append(String.Format("--- File : {0}{1}", fi.FullName, "<br/>"));
            }
            return sb.ToString();
        }



        protected string WriteOutWebConfig()
        {
            string retVal = string.Empty;

            retVal += "<br/>";

            string FolderPathLIC = Server.MapPath("~\\");
            string actualfile = "Web.config";
            string filer = FolderPathLIC + actualfile;

            //retVal += (String.Format("<br/>Checking {0} Exists '{1}'<br/>", filename, filer));
            try
            {
                string filecontents = System.IO.File.ReadAllText(filer);

                retVal += (String.Format("<br/><strong>WEB CONFIG</strong><br/>{0}<br/>", Server.HtmlEncode(filecontents)));

            }
            catch
            {
                // do nothing
            }
            finally
            {

            }

            return retVal;

        }
    }
}
