
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using MyApp.Business.DomainObjects.Models;
using System.Web.Mvc;

namespace MyApp.Web.ViewModels
{

    public class DecryptViewModel : BaseViewModel
    {

        public string encrypted { get; set; }

        public string decrypted { get; set; }

        public string result { get; set; }

        public DecryptViewModel()
        {

        }
    }

}
