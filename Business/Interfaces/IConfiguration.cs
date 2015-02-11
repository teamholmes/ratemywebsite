using System;
using System.Net;
namespace MyApp.Business.Services
{
    public interface IConfiguration
    {
        string RoleKey { get; }
        Boolean ForceCAPTCHAtoShow { get; }
        bool RequiresAccountVerificationAfterAccountCreation { get; }
        string APPDiscriminator { get;}
        string SmtpHost { get; }
        int SmtpPort { get; }
        string SmtpSenderEmailAddress { get; }
        string BCCEmailAddress { get; }
        string SmtpSenderName { get; }
        bool SmtpSmtpRequiresSsl { get; }
        NetworkCredential SmtpCredentials { get; }
        int PassphraseMinimumLength { get; }
        int MonthsPassphraseisValidFor { get; }
        int NumberHistoricPassphrasestoRecover { get; }
        int AccountLockoutPeriodInMins { get; }
        int MaximumNumberOfFailedLoginAttempts { get; }
        string RoleDev { get; }
        int RoleDevID { get; }
        string RoleAdmin { get; }
        int RoleAdminID { get; }
        string RoleUser { get; }
        int RoleUserID { get; }

        string RoleSuperUser { get; }
        int RoleSuperUserID { get; }

        string FromEmailAddress { get; }

        string PleaseSelect { get; }

        string BaseURL { get; }
        int DurationofAutheticationTicket { get; }
        string PostinstalaltionCheckEmailDestinationemailaddress { get; }
        string AppVersion { get; }
        int Thumbnailheight { get; }
        int Thumbnailwidth { get; }
        Boolean DisableLogin { get; }


        Boolean SendInformationEmails { get; }



    }
}
