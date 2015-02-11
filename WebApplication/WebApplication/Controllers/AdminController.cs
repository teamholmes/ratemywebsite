using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.Business.DomainObjects.Models;
using MyApp.Business.Services;
using MyApp.Web.ViewModels;
using OP.General.Extensions;
using System.Resources;
using System.IO;
using System.Text.RegularExpressions;
using OP.General.Performance;
using System.Security.Claims;

namespace MyApp.Web.Controllers
{
    //[SimpleAuthorize(ClaimType = "role", ClaimValue = "devi|devp")]
    public class AdminController : BaseController
    {
        private IAccountService _AccountService;
        private IAdminService _AdminService;
        private IConfiguration _Configuration;


        public AdminController(IAccountService accountService, IAdminService adminService, IConfiguration configuration)
        {
            _AccountService = accountService;
            _AdminService = adminService;
            _Configuration = configuration;

        }



        public ActionResult Overview()
        {

            return Content("Im in teh admin section");
        }




    }
}
