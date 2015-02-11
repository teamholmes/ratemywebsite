using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OP.General.Dal;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Business.Services;
using MyApp.Business.DomainObjects.Models;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Reflection;
using System.Web.Mvc;
using MyApp.DAL.Repository;
using WebApplication;

namespace MyApp.Tests
{
    public class BusinessTestBase
    {

        public IRepository _RepositoryMock;

        protected IConfiguration MockConfiguration;
        protected IConfiguration Configuration;

        protected IAppConfigurationService TestAppConfigurationService;
        protected IAppConfigurationService MockAppConfigurationService;

        protected ISmtpService SmtpService;
        protected ISmtpService MockSmtpService;

        protected IEmailService _TestEmailService;
        protected IEmailService _MockEmailService;

        protected ISmtpService TestSMTPService;
        protected ISmtpService MockSMTPService;

        protected ILog _TestLog;
        protected ILog _MockLog;

        protected IAccountService _TestAccountService;
        protected IAccountService _MockAccountService;

        protected IUserService _MockUserService;
        protected IUserService _UserService;

        protected ISessionService _TestSessionService;
        protected ISessionService _MockSessionService;

        protected ICaptchaService _CaptchaService;

        protected IRepository _TestRepository;

     
        protected IAdminService _TestAdminService;
        protected IAdminService _MockAdminService;



        public BusinessTestBase()
        {
           


            ///
            //real

            _TestLog = new MyApp.Business.DomainObjects.Models.Log();

            IUnityContainer container = new UnityContainer();


            Bootstrapper.Initialise();
            //UnityBuilder.ConfigureContainer(container);

            //MyApp.Business.Services.UnityBuilder.ConfigureContainer(container);

            _TestRepository = new TestRepository();

            _RepositoryMock = MockRepository.GenerateMock<IRepository>();
            _MockAccountService = MockRepository.GenerateMock<IAccountService>();

            MockConfiguration = MockRepository.GenerateMock<IConfiguration>();

            TestAppConfigurationService = new AppConfigurationService(_TestRepository, _TestLog);
            MockAppConfigurationService = MockRepository.GenerateMock<IAppConfigurationService>();
            Configuration = new Configuration(TestAppConfigurationService, _MockLog);

            TestSMTPService = new SmtpService(_TestLog, MockConfiguration);
            MockSMTPService = MockRepository.GenerateMock<ISmtpService>();

            _MockEmailService = MockRepository.GenerateMock<IEmailService>();


            _TestEmailService = new EmailService(MockSMTPService, MockConfiguration, _TestLog, _TestRepository);

            _MockAccountService = new AccountService(_RepositoryMock, _MockLog, MockConfiguration, _MockEmailService); 
            _TestAccountService = new AccountService(_TestRepository, _TestLog, MockConfiguration, _TestEmailService); 

            _MockAdminService = new AdminService(_RepositoryMock, _MockLog, Configuration, _MockAccountService); 
            _TestAdminService = new AdminService(_TestRepository, _TestLog, Configuration, _TestAccountService); 

            _CaptchaService = new CaptchaService(_TestRepository, _TestLog, Configuration);



            GenetareMockExpectations();
           


        }


        public void GenetareMockExpectations()
        {
            MockSMTPService.Expect(x => x.Send("xx", "xx", "xx", "xx", "xx", null, null, true)).Return(true).IgnoreArguments();
            MockConfiguration.Expect(x => x.APPDiscriminator).Return("BASE_APP").IgnoreArguments();
            MockConfiguration.Expect(x => x.NumberHistoricPassphrasestoRecover).Return(15).IgnoreArguments();
            MockConfiguration.Expect(x => x.PassphraseMinimumLength).Return(10).IgnoreArguments();
            MockConfiguration.Expect(x => x.MaximumNumberOfFailedLoginAttempts).Return(5).IgnoreArguments();
            MockConfiguration.Expect(x => x.AccountLockoutPeriodInMins).Return(15).IgnoreArguments();
            MockConfiguration.Expect(x => x.BaseURL).Return("http://www.test.com").IgnoreArguments();
            MockConfiguration.Expect(x => x.FromEmailAddress).Return("test@op.co.uk").IgnoreArguments();
            MockConfiguration.Expect(x => x.MonthsPassphraseisValidFor).Return(12).IgnoreArguments();

            



            
        }

     
    }


}



