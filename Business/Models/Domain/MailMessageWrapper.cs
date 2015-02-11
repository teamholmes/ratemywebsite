using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.General;
using System.Net.Mail;

namespace MyApp.Business.DomainObjects.Models
{


    public class MailMessageWrapper : MailMessage
    {
        public int UniqueId { get; set; }

        public MailMessage MailMessage { get; set; }

        public MailMessageWrapper(int uniqueId, MailMessage msg)
        {
            UniqueId = uniqueId;
            MailMessage = msg;
        }

    }

}



