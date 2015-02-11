using MyApp.Business.DomainObjects.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace MyApp.Business.Services
{


    public class SmtpService : ISmtpService
    {
        private IConfiguration _Configuration;
        private ILog _Log;

        public SmtpService(ILog log, IConfiguration configuration)
        {
            _Configuration = configuration;
            _Log = log;
        }



        public Boolean Send(string to, string fromAddress, string fromName, string subject, string body, string[] cc = null, string bcc = null, bool isHTML = false, List<Attachment> attachments = null, Boolean sendAsyncronously = false)
        {

            _Log.Debug(string.Format("Send : {0} | {1} | {2} | {3}", to, fromAddress, fromName, subject));
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(fromAddress, fromName),
                    Subject = subject,
                    Body = body // + Resource.EmailResource.emailfooter,
                };

                message.IsBodyHtml = isHTML;

                if (cc != null)
                {
                    foreach (string ccEmail in cc)
                    {
                        message.CC.Add(new MailAddress(ccEmail));
                    }
                }

                if (bcc != null)
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }

                message.To.Add(new MailAddress(to));

                if (attachments != null)
                {

                    foreach (Attachment item in attachments)
                    {
                        message.Attachments.Add(item);
                    }
                }


                SmtpClient smtpClient = CreateSmtpClient();

                if (!sendAsyncronously)
                {
                    smtpClient.Send(message);
                }
                else
                {
                    smtpClient.SendCompleted += (s, e) =>
                    {
                        SmtpClient callbackClient = s as SmtpClient;
                        MailMessageWrapper callbackMailMessage = e.UserState as MailMessageWrapper;

                        // the unique id of the message is stored in the mailmessage wrapper so you can access it and update any db you need to.

                        if (e.Error != null)
                            Console.WriteLine("Error sending email.");
                        else if (e.Cancelled)
                            Console.WriteLine("Sending of email cancelled.");
                        else
                            Console.WriteLine("Message sent.");

                        callbackClient.Dispose();
                        callbackMailMessage.Dispose();
                    };


                    smtpClient.SendAsync(message, new MailMessageWrapper(12345, message));
                }

                _Log.Debug(string.Format("SMTP Send {0} | {1}", to, "PASS"));
                return true;




            }
            catch (Exception err)
            {
                _Log.Debug(string.Format("SMTP Send {0} | {1} | {2}", to, "FAIL", err.Message));
                return false;
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            SmtpClient smtpClient = new SmtpClient(_Configuration.SmtpHost, _Configuration.SmtpPort);

            smtpClient.EnableSsl = _Configuration.SmtpSmtpRequiresSsl;

            if (_Configuration.SmtpCredentials != null)
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = _Configuration.SmtpCredentials;
            }

            return smtpClient;
        }



    }
}
