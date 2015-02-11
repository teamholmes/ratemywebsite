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

    public class ChangePassPhraseViewModel : BaseViewModel
    {

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [StringLength(120)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [StringLength(120,MinimumLength=15)]
        public string NewPassphrase { get; set; }

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(EVAResource))]
        [StringLength(120)]
        public string ValidationCode { get; set; }

        public string result { get; set; }

        public ChangePassPhraseViewModel()
        {

        }
    }

}
