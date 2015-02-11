using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using MyApp.Business.DomainObjects.Models;
using MyApp.Business.Services;
using OP.General.Extensions;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNet.Identity;
using Resources;

namespace MyApp.Web.Controllers
{
    //[HandleError]
    public class BaseController  : Controller
    {

        //public BaseController()
        //    : this(new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new MyApp.DAL.Repository.ApplicationDbContext())))
        //{
        //}

        public ILog Log;
        public IUnityContainer Container;
       // public UserManager<ApplicationUser> Umanager { get; set; }

        public BaseController() //public BaseController(UserManager<ApplicationUser> userManager)
        {

            var container = new UnityContainer();
            container.RegisterType<ILog, Log>();
            Log = container.Resolve<ILog>();
           // Umanager = userManager;
        }




        public string GetBaseURL()
        {
            return string.Format(@"{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
        }

      
        public string GetLoggedInUserUserName()
        {
            if (User != null && User.Identity != null && User.Identity.Name != null)
            {
                return User.Identity.Name;
            }
            return EVAResource.NoUserName;
        }


        public string GetUserClaimValueForKey(string claimkey)
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            return identity.Claims.Where(c => c.Type.Equals(claimkey,StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value).SingleOrDefault();
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            RedirectToAction("Index", "Error", new { str = "hello" });
             
         //   base.OnException(filterContext);

        }

    }
}
