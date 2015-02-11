using System;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Linq;
using MyApp.Business.Resource;
using OP.General.Encryption;
using OP.General.Extensions;
using MyApp.Business.DomainObjects.Models;
using OP.General.Dal;
using OP.General.Model;
using System.Web;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;

namespace MyApp.Business.Services
{
    public class EmailService : IEmailService
    {
        private IRepository _Repository;
        private ISmtpService _SmtpService;
        private ResourceManager _ResourceManager;
        private IConfiguration _Configuration;
        private ILog _Log;

        public EmailService(ISmtpService smtpService, IConfiguration configuration, ILog log, IRepository repository)
        {
            _SmtpService = smtpService;
            _ResourceManager = new ResourceManager(typeof(Resource.EmailResource));
            _Configuration = configuration;
            _Log = log;
            _Repository = repository;
        }


        public Boolean SendAcceptancePackEmail(string emailaddressofrecipient, string firstname, string surname, MemoryStream certificate, MemoryStream welcomeletter)
        {
            _Log.Debug(string.Format("SendAcceptancePackEmail {0} ", emailaddressofrecipient));

            string toEmailAddress = emailaddressofrecipient.ToLowerCheckForNull();

            string body = string.Format(_ResourceManager.GetString("welcomepackbody").Replace("##emailaddress##", _ResourceManager.GetString("standardemaiaddress")));
            body = body.Replace("##firstname##", firstname);
            body = body.Replace("##surname##", surname);
            string subject = string.Format(_ResourceManager.GetString("WelcomepackSubject"));

            string defaultemailtemplate = GetDefaultEmailTemplate();

            defaultemailtemplate = defaultemailtemplate.Replace("##HTMLBODYCONTENT##", body);
            defaultemailtemplate = defaultemailtemplate.Replace("##FOOTER##", GetDefaultEmailFooter());

            certificate.Position = 0;
            welcomeletter.Position = 0;

            Attachment att_cert = new Attachment(certificate, "Certificate.pdf");
            Attachment att_welcletter = new Attachment(welcomeletter, "WelcomeLetter.pdf");

            return _SmtpService.Send(toEmailAddress.TrimCheckForNull(), _Configuration.FromEmailAddress, _Configuration.SmtpSenderName, subject, defaultemailtemplate, null, _Configuration.BCCEmailAddress,true, new List<Attachment>() { att_welcletter, att_cert });


        }

        public Boolean SendAccountCreationConfirmation(string identityid, string emailaddressofrecipient,string firstname, string surname, string baseurl)
        {
            _Log.Debug(string.Format("SendAccountCreationConfirmation {0} ", emailaddressofrecipient));

            string toEmailAddress = emailaddressofrecipient.ToLowerCheckForNull();

            try
            {
              //  SendInformationEmails("Account created for " + toEmailAddress, firstname + " " + surname, "SW KIF");
            }
            catch (Exception err)
            {
                // do nothing
            }

            string body = string.Format(_ResourceManager.GetString("accouncreationbody").Replace("##emailaddress##", _ResourceManager.GetString("standardemaiaddress")));
            body = body.Replace("##baseurl##", baseurl);
            body = body.Replace("##firstname##", firstname);
            body = body.Replace("##surname##", surname);
            string subject = string.Format(_ResourceManager.GetString("accouncreationsubject"));

            string defaultemailtemplate = GetDefaultEmailTemplate();

            defaultemailtemplate = defaultemailtemplate.Replace("##HTMLBODYCONTENT##", body);
            defaultemailtemplate = defaultemailtemplate.Replace("##FOOTER##", GetDefaultEmailFooter());
            defaultemailtemplate = defaultemailtemplate.Replace("##EVENTBANNER##", baseurl + String.Format("/Public/GetEmailBannerCache/{0}", "-1".Encrypt()));


           // Attachment att = new Attachment(new MemoryStream(qrcode), "qrcode.jpg");
            return _SmtpService.Send(toEmailAddress.TrimCheckForNull(), _Configuration.FromEmailAddress, _Configuration.SmtpSenderName, subject, defaultemailtemplate, null, null, true,null,true);

        }



        public Boolean SendTemporaryPassphraseEmail(string toemailaddress, string temppassphrase, string passwordtoken, string userid, string baseurl, string firstname, string surname)
        {

            _Log.Debug(string.Format("SendTemporaryPassphraseEmail  {0} | {1} | {2} | {3}", toemailaddress, temppassphrase, userid, baseurl));

            string urllink = string.Format(@"{0}/Account/ResetPassphrase?t1={1}&t2={2}&t3={3}&t4={4}", _Configuration.BaseURL, Encryption.EncryptTripleDES(toemailaddress, true, true), Encryption.EncryptTripleDES(userid.ToString(), true, true), passwordtoken, "pr");

            string body = _ResourceManager.GetString("passwordresetbody").Replace("##LINK##", urllink);
            body = body.Replace("##firstname##", firstname);
            body = body.Replace("##surname##", surname);
            body = string.Format(body, temppassphrase);
            string subject = string.Format(_ResourceManager.GetString("passwordresetsubject"));

            string defaultemailtemplate = GetDefaultEmailTemplate();

            defaultemailtemplate = defaultemailtemplate.Replace("##HTMLBODYCONTENT##", body);
            defaultemailtemplate = defaultemailtemplate.Replace("##FOOTER##", GetDefaultEmailFooter());
            defaultemailtemplate = defaultemailtemplate.Replace("##EVENTBANNER##", baseurl + String.Format("/Public/GetEmailBannerCache/{0}", "-1".Encrypt()));

            Boolean retval = _SmtpService.Send(toemailaddress.TrimCheckForNull().ToLowerCheckForNull(), _Configuration.FromEmailAddress, _Configuration.SmtpSenderName, subject, defaultemailtemplate, null, null, true);

            // SendInformationEmails("SendTemporaryPassphraseEmail : " + toemailaddress.TrimCheckForNull(), "Send Result : " + retval.ToString() + " userid " + userid.ToString(), "My special app");

            return retval;
        }

        //public Boolean SendDeveloperLoginEmailNotification(string sub, string applicationname, string remoteIpAddress)
        //{
        //    _Log.Debug(string.Format("SendDeveloperLoginNotification {0} ", applicationname));

        //    string body = String.Format("Email sent from system : {0}<br/><br/>Remote IP address of : {1}", applicationname, remoteIpAddress);

        //    string subject = String.Format("{0} : {1} at {2}", sub, applicationname, DateTime.Now.ToLongTimeString());

        //    return _SmtpService.Send(_Configuration.PostinstalaltionCheckEmailDestinationemailaddress.ToLowerCheckForNull(), _Configuration.SmtpSenderEmailAddress, _Configuration.SmtpSenderName, subject, body, null, true);
        //}

        public Boolean SendInformationEmails(string sub, string bodyofmessage, string applicationname)
        {
            _Log.Debug(string.Format("SendInformationMessage {0} ", applicationname));

            if (_Configuration.SendInformationEmails)
            {
                string body = bodyofmessage.TrimCheckForNull();

                string subject = String.Format("{0} : {1} at {2}", sub, applicationname, DateTime.Now.ToLongTimeString());

                // add application log

                string applogkey = "BACKLOGKEYFORAPP";

                try
                {
                    if (HttpContext.Current.Application[applogkey] != null)
                    {
                        body += "<br><br />";

                        foreach (string str in ((List<string>)HttpContext.Current.Application[applogkey]).AsEnumerable().Reverse())
                        {
                            body += (String.Format("{0}{1}", str, "<br><br />"));
                        }
                    }
                }
                catch (Exception err)
                {
                    // do nothing   
                }

                return _SmtpService.Send(_Configuration.PostinstalaltionCheckEmailDestinationemailaddress.ToLowerCheckForNull(), _Configuration.SmtpSenderEmailAddress, _Configuration.SmtpSenderName, subject, body, null, null, true,null,false);
            }
            return false;
        }



        public Boolean SendEmailRequestingDetailsCheck(string emailaddressofrecipient, string firstname, string surname, string urllink)
        {
            _Log.Debug(string.Format("SendEmailRequestingDetailsCheck {0} ", emailaddressofrecipient));

            string toEmailAddress = emailaddressofrecipient.ToLowerCheckForNull();

            string body = string.Format(_ResourceManager.GetString("ConfirmDetailsCorrect").Replace("##emailaddress##", emailaddressofrecipient));
            body = body.Replace("##firstname##", firstname);
            body = body.Replace("##surname##", surname);
            body = body.Replace("##LINK##", urllink);
            string subject = string.Format(_ResourceManager.GetString("ConfirmDetailsCorrectSubject"));

            string defaultemailtemplate = GetDefaultEmailTemplate();

            defaultemailtemplate = defaultemailtemplate.Replace("##HTMLBODYCONTENT##", body);
            defaultemailtemplate = defaultemailtemplate.Replace("##FOOTER##", GetDefaultEmailFooter());
           
            return _SmtpService.Send(toEmailAddress.TrimCheckForNull(), _Configuration.FromEmailAddress, _Configuration.SmtpSenderName, subject, defaultemailtemplate, null, _Configuration.BCCEmailAddress, true, null);
        }


        //public Boolean SendTestMessage(string applicationname)
        //{
        //    _Log.Debug(string.Format("SendTestMessage {0} ", applicationname));

        //    string body = String.Format("Test email sent from system : {0}<br/>", applicationname);
        //    string subject = String.Format("Test email sent from system : {0} at {1}", applicationname, DateTime.Now.ToLongTimeString());

        //    return _SmtpService.Send(_Configuration.PostinstalaltionCheckEmailDestinationemailaddress.ToLowerCheckForNull(), _Configuration.SmtpSenderEmailAddress, _Configuration.SmtpSenderName, subject, body, null, true);
        //}

        //public Boolean SendTestMessageToEmailAddress(string applicationname, string emailaddress, string fromemailaddress, string bodycopy)
        //{
        //    _Log.Debug(string.Format("SendTestMessage {0} ", applicationname));

        //    string body = String.Format("The time the email was sent : '{0}'<br/><br/><br/>From : {1} | To: {2}", DateTime.Now.ToLongTimeString(), fromemailaddress, emailaddress);

        //    body += "<br/>" + bodycopy;

        //    string subject = String.Format("Test email sent from : '{0}' at {1}", applicationname, DateTime.Now.ToLongTimeString());

        //    return _SmtpService.Send(emailaddress.Trim().ToLowerCheckForNull(), fromemailaddress, _Configuration.SmtpSenderName, subject, body, new string[] { _Configuration.PostinstalaltionCheckEmailDestinationemailaddress }, true);
        //}



        public Boolean EmailSystemAccountTheirTemporaryPassPhase(string emailaddress, string temppassphrase, string passwordtoken, string userid)
        {
            _Log.Debug(string.Format("EmailSystemAccountTheirTemporaryPassPhase {0} | {1} | {2}", emailaddress, temppassphrase, userid));

            string urllink = string.Format(@"{0}/Account/ResetPassphrase?t1={1}&t2={2}&t3={3}&t4={4}", _Configuration.BaseURL, Encryption.EncryptTripleDES(emailaddress, true, true), Encryption.EncryptTripleDES(userid.ToString(), true, true), passwordtoken, "pr");

            string body = _ResourceManager.GetString("createsyatemaccountsendwelcomemeial").Replace("##LINK##", urllink);
            body = string.Format(body, temppassphrase);

            string subject = string.Format(_ResourceManager.GetString("systemaccountcreatedsubject"));

            return _SmtpService.Send(emailaddress.TrimCheckForNull().ToLowerCheckForNull(), _Configuration.SmtpSenderEmailAddress, _Configuration.SmtpSenderName, subject, body, null, null, true);
        }

        //public string GetEmailAddressForEventByEventIdCache(int eventId)
        //{
        //    _Log.Debug(string.Format("GetEmailAddressForEventByEventId {0} ", eventId));

        //    string uniquekey = "GetEmailAddressForEventByEventIdCache_" + eventId;

        //    if (Utilities.GetFromCache(uniquekey) != null)
        //    {
        //        return (string)Utilities.GetFromCache(uniquekey);
        //    }
        //    else
        //    {
        //        string retval = string.Empty;

        //        // Region selectedregion = _Repository.GetAll<Region>("Event").Where(x => x.Events.Any(y => y.Id == eventId)).SingleOrDefault();
        //        Region selectedregion = _Repository.GetFiltered<Region>((x => x.Events.Any(y => y.Id == eventId)), "Event").SingleOrDefault();

        //        if (selectedregion != null)
        //        {
        //            retval = selectedregion.EmailAddress.ToLowerCheckForNull();
        //        }

        //        // send default if necessary
        //        if (String.IsNullOrEmpty(retval))
        //        {
        //            retval = _Configuration.SmtpSenderEmailAddress.ToLowerCheckForNull();
        //        }

        //        return Utilities.SetandGetCache<string>(uniquekey, retval, 60);
        //    }
        //}


        //public string GetEmailAddressForRegionbyCountryIdCache(int countryId)
        //{

        //    _Log.Debug(string.Format("GetEmailAddressForRegionbyCountryId {0} ", countryId));

        //    string uniquekey = "GetEmailAddressForRegionbyCountryIdCache" + countryId;

        //    if (Utilities.GetFromCache(uniquekey) != null)
        //    {
        //        return (string)Utilities.GetFromCache(uniquekey);
        //    }
        //    else
        //    {

        //        string retval = string.Empty;

        //        // Country selectedcountry = _Repository.GetAll<Country>("Region").Where(x => x.Id == countryId).SingleOrDefault();
        //        Country selectedcountry = _Repository.GetFiltered<Country>((x => x.Id == countryId), "Region").SingleOrDefault();

        //        if (selectedcountry != null)
        //        {
        //            retval = selectedcountry.Region.EmailAddress;
        //        }

        //        // send default if necessary
        //        if (String.IsNullOrEmpty(retval))
        //        {
        //            retval = _Configuration.SmtpSenderEmailAddress;
        //        }

        //        return Utilities.SetandGetCache<string>(uniquekey, retval, 60);
        //    }

        //}



        public Boolean SendDevTestEmails(string fromaddress, string toaddress, string subject, string body, string bcc)
        {
            _Log.Debug(string.Format("SendDevTestEmails {0} | {1}", fromaddress, toaddress));

            string toEmailAddress = toaddress.ToLowerCheckForNull();

            body += "<br/><br/>";

            body += String.Format("From:{0}, To:{1}, BCC:{2}", fromaddress, toaddress, bcc);

          
            // Attachment att = new Attachment(new MemoryStream(qrcode), "qrcode.jpg");
            return _SmtpService.Send(toEmailAddress.TrimCheckForNull(), fromaddress.TrimCheckForNull(), _Configuration.SmtpSenderName, subject, body, null, bcc, true, null,false);

        }



        private string GetDefaultEmailTemplate()
        {
            return _ResourceManager.GetString("baseemailtemplate");
        }

        private string GetDefaultEmailFooter()
        {
            return _ResourceManager.GetString("emailfooter");
        }





    }
}
