using System;
using System.Collections.Generic;
using OP.General.Extensions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.Business.Services;
using MyApp.Web.ViewModels;
using MyApp.Business.DomainObjects.Models;

using OP.General.Encryption;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.Mail;


namespace MyApp.Web.Controllers
{
    public class ErrorController : Controller
    {

        private IAccountService _AccountService;
        private IConfiguration _Configuration;


        public ErrorController( IAccountService accountService,IConfiguration configuration)
        {
            _AccountService = accountService;
            _Configuration = configuration;
        }

        public ActionResult Index(string str)
        {
            return View("Error");
        }

        public ActionResult NotFound(string str)
        {
            throw new Exception("The page you requested cannot be found");
        }


        public ActionResult NoAccess(string str)
        {
            throw new Exception("You do not have permissions to view this page");
        }



        


        



    }
}
