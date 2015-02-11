using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using MyApp.Business.DomainObjects.Models;
using System.Web.Mvc;
using WebApplication.Resources;

namespace MyApp.Web.ViewModels
{

    public class LoginViewModel : BaseViewModel
    {

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.EmailAddressRegex, ErrorMessageResourceName = "InvalidEmailaddress", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "emailaddress", ResourceType = typeof(EVAResource))]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "passphrase", ResourceType = typeof(EVAResource))]
        [StringLength(100)]
        public string Passphrase { get; set; }


        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "catchacode", ResourceType = typeof(EVAResource))]
        public string Captcha { get; set; }

        public string EncryptedCaptcha { get; set; }

        public LoginViewModel()
        {
        }
    }

}
