using Microsoft.Practices.Unity;
using MyApp.Business.DomainObjects.Models;
using MyApp.Business.Services;
using MyApp.DAL.Repository;
using OP.General.Dal;
using System.Web;
using System.Web.SessionState;

namespace MyApp.Business.Services
{
    public class UnityBuilder
    {
        public static void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<IRepository, ApplicationRepository>();
            container.RegisterType<IAppConfigurationService, AppConfigurationService>();

            container.RegisterType<IConfiguration, Configuration>();
            container.RegisterType<ILog, Log>();

            container.RegisterType<IAccountService, AccountService>();

            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<ISmtpService, SmtpService>();

            container.RegisterType<IUserService, UserService>();

            container.RegisterType<ICaptchaService, CaptchaService>();

            container.RegisterType<ISessionService, SessionService>();

            container.RegisterType<IAdminService, AdminService>();



        }
    }
}
