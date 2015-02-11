using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyApp.Business.Services;
using System.Web;

namespace MyApp.Business.DomainObjects.Models
{
    public class Configuration : IConfiguration
    {

        private IAppConfigurationService _appConfigurationService;
        private ILog _Log;
        private System.Configuration.AppSettingsReader _SettingsReader;


        public Configuration(IAppConfigurationService appConfigurationService, ILog _log)
        {
            _appConfigurationService = appConfigurationService;
            _Log = _log;
            _SettingsReader = new System.Configuration.AppSettingsReader();
        }



        public bool RequiresAccountVerificationAfterAccountCreation
        {
            get
            {
                return false;

            }
        }



        public string APPDiscriminator
        {
            get
            {
                return "BASE_APP";
            }
        }


        public string AppVersion
        {
            get
            {
                return "1.35 18/11/2014 SVN xxx";
            }
        }





        public Boolean ForceCAPTCHAtoShow
        {
            get
            {
                return Boolean.Parse(_SettingsReader.GetValue("ForceCAPTCHAtoShow", typeof(string)).ToString());
            }
        }
        public string SmtpSenderEmailAddress
        {
            get
            {
                return _SettingsReader.GetValue("SmtpSenderEmailAddress", typeof(String)).ToString();
            }
        }


        public string BCCEmailAddress
        {
            get
            {
                return _SettingsReader.GetValue("BCCEmailAddress", typeof(String)).ToString();
            }
        }




        public string SmtpSenderName
        {
            get
            {
                return _SettingsReader.GetValue("SmtpSenderName", typeof(String)).ToString();
            }
        }

        public string FromEmailAddress
        {
            get
            {
                return _SettingsReader.GetValue("SmtpSenderEmailAddress", typeof(String)).ToString();
            }
        }


        public string SmtpHost
        {
            get
            {
                return _SettingsReader.GetValue("SmtpHost", typeof(String)).ToString();
            }
        }

        public int SmtpPort
        {
            get
            {
                return int.Parse(_SettingsReader.GetValue("SmtpPort", typeof(string)).ToString());
            }
        }






        public bool SmtpSmtpRequiresSsl
        {
            get
            {
                return Boolean.Parse(_SettingsReader.GetValue("SmtpSmtpRequiresSsl", typeof(string)).ToString());

            }
        }

        public NetworkCredential SmtpCredentials
        {
            get
            {


                return new NetworkCredential(_SettingsReader.GetValue("SmtpCredentials_Username", typeof(String)).ToString(), _SettingsReader.GetValue("SmtpCredentials_Password", typeof(String)).ToString());
            }
        }


        public int PassphraseMinimumLength
        {
            get
            {
                return 15;
            }
        }

        public string PleaseSelect
        {
            get
            {
                return "Please Select";
            }
        }

        public int MonthsPassphraseisValidFor
        {
            get
            {
                return 12;
            }
        }

        public int NumberHistoricPassphrasestoRecover
        {
            get
            {
                return 14;
            }
        }

        public int MaximumNumberOfFailedLoginAttempts
        {
            get
            {
                return 5;
            }
        }
        public int AccountLockoutPeriodInMins
        {
            get
            {
                return 15;
            }
        }



        public string RoleKey
        {
            get
            {
                return "Role";
            }
        }

        public string RoleDev
        {
            get
            {
                return "DEV";
            }
        }

        public int RoleDevID
        {
            get
            {
                return 1;
            }
        }

        public string RoleAdmin
        {
            get
            {
                return "ADMIN";
            }
        }

        public int RoleAdminID
        {
            get
            {
                return 2;
            }
        }

        public string RoleUser
        {
            get
            {
                return "USER";
            }
        }

        public int RoleUserID
        {
            get
            {
                return 4;
            }
        }

        public string RoleSuperUser
        {
            get
            {
                return "SUPERUSER";
            }
        }

        public int RoleSuperUserID
        {
            get
            {
                return 3;
            }
        }

        public string BaseURL
        {
            get
            {
                try
                {
                    return string.Format(@"{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority);
                }
                catch (Exception err)
                {
                    return "Error obtaining BASEURL :" + err.Message;
                }
            }
        }


        public int DurationofAutheticationTicket
        {
            get
            {
                return 20;
            }
        }

        public string PostinstalaltionCheckEmailDestinationemailaddress
        {
            get
            {
                return _SettingsReader.GetValue("Postinstllationtestemailaddress_to", typeof(String)).ToString();
            }
        }


        public int Thumbnailwidth
        {
            get
            {
                return 48;
            }
        }

        public int Thumbnailheight
        {
            get
            {
                return 48;
            }
        }


        public Boolean DisableLogin
        {
            get
            {
                return _appConfigurationService.GetConfigurationByKey<Boolean>("DisableLogin");
            }
        }

        public Boolean SendInformationEmails
        {
            get
            {
                return Boolean.Parse(_SettingsReader.GetValue("SendInformationEmails", typeof(string)).ToString());
            }
        }


    }
}
