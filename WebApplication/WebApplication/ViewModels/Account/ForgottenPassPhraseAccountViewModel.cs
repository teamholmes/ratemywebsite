using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using MyApp.Business.DomainObjects.Models;
using System.Web.Mvc;
using Resources;

namespace MyApp.Web.ViewModels
{

    public class ForgottenPassPhraseAccountViewModel : BaseViewModel
    {

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(AccountResource))]
        [StringLength(75)]
        [Display(Name = "emailaddress", ResourceType = typeof(AccountResource))]
        public string EmailAddress { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(AccountResource))]
        [Display(Name = "catchacode", ResourceType = typeof(AccountResource))]
        public string Captcha { get; set; }

        public Boolean? IsResetSuccessful { get; set; }

        public string CaptchaEncrypted { get; set; }


        public ForgottenPassPhraseAccountViewModel()
        {
        }
    }

}
