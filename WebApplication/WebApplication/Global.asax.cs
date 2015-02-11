using Microsoft.Practices.Unity;
using MyApp.Business.DomainObjects.Models;
using MyApp.Business.Services;
using MyApp.DAL.Repository;
using OP.General.Dal;
using OP.General.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication.Controllers;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {

        private static IUnityContainer _Container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();  
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


           // CreateContainer();
            Bootstrapper.Initialise();


            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;
        }

        //protected virtual void CreateContainer()
        //{
        //    IUnityContainer container = new UnityContainer();
        //    container.RegisterType<IAccountService, AccountService>();
        //    container.RegisterType<IRepository, ApplicationRepository>();
        //    container.RegisterType<ILog, Log>();
        //    container.RegisterType<IConfiguration, Configuration>();

        //    _Container = container;
        //}
    }
}
