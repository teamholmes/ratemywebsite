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

    public class ResetClientCofnirmedDetailsViewModel : BaseViewModel
    {

        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(AccountResource))]
        [StringLength(120)]
        public string EmailAddress { get; set; }


        [Required(ErrorMessageResourceName = "requiredfield", ErrorMessageResourceType = typeof(AccountResource))]
        [StringLength(120)]
        public string ValidationCode { get; set; }

        public string result { get; set; }

        public ResetClientCofnirmedDetailsViewModel()
        {

        }
    }

}
