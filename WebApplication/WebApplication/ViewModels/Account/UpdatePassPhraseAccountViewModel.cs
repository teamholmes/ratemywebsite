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

    public class UpdatePassPhraseAccountViewModel :BaseViewModel
    {
        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.EmailAddressRegex, ErrorMessageResourceName = "InvalidEmailaddress", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "emailaddress", ResourceType = typeof(EVAResource))]
        [StringLength(100)]
        public string EmailAddress { get; set; }


        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [StringLength(100)]
        [Display(Name = "existingpassphrase", ResourceType = typeof(EVAResource))]
        public string ExistingPassword { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [StringLength(100)]
        [Display(Name = "newpassphrase", ResourceType = typeof(EVAResource))]
        public string NewPassPhrase { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [StringLength(100)]
        [Display(Name = "confirmnewpassphrase", ResourceType = typeof(EVAResource))]
        [System.ComponentModel.DataAnnotations.CompareAttribute("NewPassPhrase", ErrorMessage = "Passphrases do not match.")]
        public string ConfirmNewPassPhrase { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "catchacode", ResourceType = typeof(EVAResource))]
        public string Captcha { get; set; }

        public string EncryptedCaptcha { get; set; }


        

        // the usernmae
        public string P1 {get;set;}

        // the db userid
        public string P2 {get;set;}

        // password reset token
        public string P3 { get; set; }

        // password reset token
        public string P4 { get; set; }

        public BusinessEnum.AuthenticationResult ReasonEnum { get; set; }

        public Boolean ShowPassPhraseUpdateSuccess { get; set; }

        public Boolean HasComeFromHomePage { get; set; }

        public string Homepage { get; set; }

        public UpdatePassPhraseAccountViewModel()
        {
        }
    }

}
