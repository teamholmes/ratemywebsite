using Microsoft.Practices.Unity;
using MyApp.Business.DomainObjects.Models;
using MyApp.Business.Services;
using MyApp.DAL.Repository;
using OP.General.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Mvc5;

namespace WebApplication
{
    public class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here  
            //This is the important line to edit  
            container.RegisterType<ILog, Log>();
            container.RegisterType<IRepository, ApplicationRepository>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IAppConfigurationService, AppConfigurationService>();
            container.RegisterType<IConfiguration, Configuration>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<ISmtpService, SmtpService>();
            container.RegisterType<ICaptchaService, CaptchaService>();
            container.RegisterType<ISwearWordService, SwearWordService>();
            container.RegisterType<IAdminService, AdminService>();


            RegisterTypes(container);
            return container;
        }
        public static void RegisterTypes(IUnityContainer container)
        {

        }
    } 
}