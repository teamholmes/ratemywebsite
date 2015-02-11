
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using MyApp.Business.DomainObjects.Models;
using System.Web.Mvc;
using MyApp.Business.Services;
using System.Web;
using Microsoft.Practices.Unity;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.ViewModels
{

    public class BaseViewModel
    {

        public string Feedbackmessage { get; set; }

        public int RandomInt { get; set; }


        public BaseViewModel() // ICacheService cacheservice
        {

            RandomInt = new Random().Next(1, 1000);

            Feedbackmessage = string.Empty;

        }
    }

}
