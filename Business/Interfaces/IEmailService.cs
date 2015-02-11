using System;
using MyApp.Business.DomainObjects.Models;
using System.IO;
namespace MyApp.Business.Services
{
    public interface IEmailService
    {

        Boolean SendTemporaryPassphraseEmail(string toemailaddress, string temppassphrase, string passwordtoken, string userid, string baseurl, string firstname, string surname);
        Boolean SendAccountCreationConfirmation(string identityid, string emailaddressofrecipient, string firstname, string surname, string baseurl);
        Boolean EmailSystemAccountTheirTemporaryPassPhase(string emailaddress, string temppassphrase, string passwordtoken, string userid);
        Boolean SendInformationEmails(string sub, string bodyofmessage, string applicationname);
        Boolean SendDevTestEmails(string fromaddress, string toaddress, string subject, string body, string bcc);


    }
}
