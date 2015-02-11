using System;
using System.Collections.Generic;
using System.Net.Mail;
namespace MyApp.Business.Services
{
    public interface ISmtpService
    {
        Boolean Send(string to, string fromAddress, string fromName, string subject, string body, string[] cc = null, string bcc = null, bool isHTML = false, List<Attachment> attachments = null, Boolean sendAsyncronously = false);
    }
}
