using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using MyApp.Business.DomainObjects.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication.Resources;

namespace MyApp.Web.ViewModels
{

    public class CreateAccountViewModel : BaseViewModel
    {

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.EmailAddressRegex, ErrorMessageResourceName = "InvalidEmailaddress", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "emailaddress", ResourceType = typeof(EVAResource))]
        [StringLength(100)]
        [Remote("IsEmailAvailable","Account","On no. not again")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.EmailAddressRegex, ErrorMessageResourceName = "InvalidEmailaddress", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "confirmemailaddress", ResourceType = typeof(EVAResource))]
        [System.Web.Mvc.CompareAttribute("EmailAddress", ErrorMessage = "Email addresses do not match")]
        [StringLength(100)]
        public string ConfirmEmailAddress { get; set; }


        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [StringLength(100)]
        [Display(Name = "passphrase", ResourceType = typeof(EVAResource))]
        public string Passphrasepart1 { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [StringLength(100)]
        [System.Web.Mvc.CompareAttribute("Passphrasepart1", ErrorMessage = "Passphrases do not match.")]
        [Display(Name = "confirmpassphrase", ResourceType = typeof(EVAResource))]
        public string Passphrasepart2 { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "firstname", ResourceType = typeof(EVAResource))]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "surname", ResourceType = typeof(EVAResource))]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "telephonenumber", ResourceType = typeof(EVAResource))]
        [StringLength(100)]
        public string ContactNumber { get; set; }

      
        public string ValidationURLForDebugging { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [Display(Name = "catchacode", ResourceType = typeof(EVAResource))]
        public string Captcha { get; set; }

        public string EncryptedCaptch { get; set; }

        public string EncryptedType { get; set; }

        public CreateAccountViewModel()
        {
        }
    }

}
