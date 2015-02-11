using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using MyApp.Business.DomainObjects.Models;
using System.Web.Mvc;

namespace MyApp.Web.ViewModels
{

    public class SendTestEmailViewModel : BaseViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string emailaddress {get;set;}

        [DataType(DataType.EmailAddress)]
        [Required]
        public string fromemailaddress { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string bccEmailAddress { get; set; }

        public string boodycopy { get; set; }

        public string subject { get; set; }

        public string senderName { get; set; }


        public SendTestEmailViewModel()
        {

        }
    }

}
