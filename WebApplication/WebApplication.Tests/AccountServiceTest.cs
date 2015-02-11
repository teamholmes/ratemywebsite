using System;
using MyApp.Business.DomainObjects.Models;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Collections.Generic;
using Rhino.Mocks;
using MyApp.Business.Services;
using OP.General.Dal;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Reflection;
using OP.General.Extensions;

namespace MyApp.Tests
{

    [TestClass]
    public class AccountServiceTest : BusinessTestBase
    {

        private string _UniqueSystemAccountEmail = "xy@op.co.uk";

        private string _UniqueSystemAccountEmail1 = "xyx@op.co.uk";

        private ApplicationUser _User_ID1 = new ApplicationUser
        {
            Email = "1russell.holmes@op.co.uk",
            UserName = "1russell.holmes@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            IsTemporaryPassPhrase = false,
            IsConfirmed = true
        };

        private ApplicationUser _User_ID2 = new ApplicationUser
        {
            Email = "testuser2@op.co.uk",
            UserName = "testuser2@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            IsTemporaryPassPhrase = true,
            IsConfirmed = true

        };

        private ApplicationUser _User_ID3 = new ApplicationUser
        {
            Email = "tony3.mcKenzie@op.co.uk",
            UserName = "tony3.mcKenzie@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            IsTemporaryPassPhrase = false,
            IsConfirmed = true
        };

        private ApplicationUser _User_ID4 = new ApplicationUser
        {
            Email = "tony4.mcKenzie@op.co.uk",
            UserName = "tony4.mcKenzie@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            IsTemporaryPassPhrase = false,
            IsConfirmed = true

        };

        private ApplicationUser _User_ID5 = new ApplicationUser
        {
            Email = "tony5.mcKenzie@op.co.uk",
            UserName = "tony5.mcKenzie@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            IsTemporaryPassPhrase = false,
            IsConfirmed = true

        };

        private ApplicationUser _User_ID6 = new ApplicationUser
        {
            Email = "tony6@op.co.uk",
            UserName = "tony6@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = false,
            IsTemporaryPassPhrase = false,
            IsConfirmed = true

        };

        private ApplicationUser _User_ID7 = new ApplicationUser
        {
            Email = "tony7@op.co.uk",
            UserName = "tony7@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            IsTemporaryPassPhrase = false
        };

        private ApplicationUser _User_ID8 = new ApplicationUser
        {
            Email = "tony8@op.co.uk",
            UserName = "tony8@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            IsTemporaryPassPhrase = false,
            IsConfirmed = true
            
        };

        private ApplicationUser _User_ID9 = new ApplicationUser
        {
            Email = "tony9@op.co.uk",
            UserName = "tony9@op.co.uk",
            Id = Guid.NewGuid().ToString(),
            IsActive = true,
            IsTemporaryPassPhrase = false
        };




        private PassphraseWord _PassWord1 = new PassphraseWord
        {
            Id = Guid.NewGuid().ToString(),
            Group1 = "myword1".Encrypt(),
            Group2 = "myword2".Encrypt(),
            Group3 = "myword3".Encrypt(),
            Group4 = "myword".Encrypt()
        };

        private PassphraseWord _PassWord2 = new PassphraseWord
        {
            Id = Guid.NewGuid().ToString(),
            Group1 = "myword12".Encrypt(),
            Group2 = "myword13".Encrypt(),
            Group3 = "myword14".Encrypt(),
            Group4 = "myword15".Encrypt()
        };

        private PassphraseWord _PassWord3 = new PassphraseWord
        {
            Id = Guid.NewGuid().ToString(),
            Group1 = "myword42".Encrypt(),
            Group2 = "myword43".Encrypt(),
            Group3 = "myword44".Encrypt(),
            Group4 = "myword145".Encrypt()
        };

        private AppConfiguration _AppConfig1 = new AppConfiguration
        {
            Id = 1,
            Key = "FromEmail",
            Value = "test@op.co.uk"
        };

        private AppConfiguration _AppConfig2 = new AppConfiguration
        {
            Id = 1,
            Key = "SmtpSenderName",
            Value = "test"
        };





        private PassphraseHistory _PHistory1 = new PassphraseHistory
        {
            DateOfChange = DateTime.UtcNow.AddMonths(-1),
            Id = Guid.NewGuid().ToString(),
        };

        private PassphraseHistory _PHistory2 = new PassphraseHistory
        {
            DateOfChange = DateTime.UtcNow.AddMonths(-2),
            Id = Guid.NewGuid().ToString(),
        };

        private PassphraseHistory _PHistory3 = new PassphraseHistory
        {
            DateOfChange = DateTime.UtcNow.AddMonths(-3),
            Id = Guid.NewGuid().ToString(),
        };

     
        private PassphraseWord ppw1 = new PassphraseWord() { Id = Guid.NewGuid().ToString(), Group1 = "gp1", Group2 = "gp2", Group3 = "gp3", Group4 = "gp4" };

        private PassphraseWord ppw2 = new PassphraseWord() { Id = Guid.NewGuid().ToString(), Group1 = "gp2", Group2 = "gp3", Group3 = "gp4", Group4 = "gp5" };

        private PassphraseWord ppw3 = new PassphraseWord() { Id = Guid.NewGuid().ToString(), Group1 = "1gp2", Group2 = "1gp3", Group3 = "1gp4", Group4 = "1gp5" };

        private ApplicationUser _TestApplicationUser = new ApplicationUser()
        {
            Email = "russell@op.co.uk",
            Firstname = "russell",
            IsActive = true,
            IsTemporaryPassPhrase = false,
            Surname = "Holmes",
            UserName = "russellUserName"
        };



        private List<PassphraseWord> _ListofPassphraseWords;

        private List<ApplicationUser> _ListofApplicationUsers;

        private List<PassphraseHistory> _ListofPassHistory;



        [TestInitialize]
        public void ClassInitialise()
        {

            SetupTestEnvironment();

        }



        [TestMethod]
        public void WillNotLoginNonExistantAccount()
        {
            BusinessEnum.AuthenticationResult result = _TestAccountService.Login("bogus@bogus.com", "account");
            Assert.IsTrue(result == BusinessEnum.AuthenticationResult.Failed);
        }

        [TestMethod]
        public void WIllLoginaValidCOnfirmedAccount()
        {
            BusinessEnum.AuthenticationResult result = _TestAccountService.Login(_User_ID1.Email, "_User_ID1");
            Assert.IsTrue(result == BusinessEnum.AuthenticationResult.Passed);
        }


        [TestMethod]
        public void WIllFailToLoginWithIncorrectPassword()
        {
            BusinessEnum.AuthenticationResult result = _TestAccountService.Login(_User_ID1.Email, "incorrectpassword");
            Assert.IsTrue(result == BusinessEnum.AuthenticationResult.Failed);
        }

        [TestMethod]
        public void WillRequireToResetPassPhraseDueToTemporary()
        {
            BusinessEnum.AuthenticationResult result = _TestAccountService.Login(_User_ID2.Email, "_User_ID2");
            Assert.IsTrue(result == BusinessEnum.AuthenticationResult.TempPasswordIssued);
        }

        [TestMethod]
        public void WillNotLoginAsAccountNotActive()
        {
            BusinessEnum.AuthenticationResult result = _TestAccountService.Login(_User_ID6.Email, "_User_ID6");
            Assert.IsTrue(result == BusinessEnum.AuthenticationResult.AccountNotActive);
        }

        [TestMethod]
        public void WillNotLoginAsAccountNotConfirmedOrActivated()
        {
            BusinessEnum.AuthenticationResult result = _TestAccountService.Login(_User_ID7.Email, "_User_ID7");
            Assert.IsTrue(result == BusinessEnum.AuthenticationResult.AccountNotConfirmed_Validated);
        }

        [TestMethod]
        public void WillNotLoginALockedAccount()
        {

            _User_ID8.PasswordFailuresSinceLastSuccess = Configuration.MaximumNumberOfFailedLoginAttempts;
            _User_ID8.LastPasswordFailureDate = DateTime.Now.AddMinutes(-1);
            BusinessEnum.AuthenticationResult result = _TestAccountService.Login(_User_ID8.Email, "wrongpassword");
            Assert.IsTrue(result == BusinessEnum.AuthenticationResult.Locked);
        }

        [TestMethod]
        public void WIllRequireToSetNewPassPhrase()
        {
            _User_ID4.PasswordChangedDate = DateTime.Now.AddMonths((Configuration.MonthsPassphraseisValidFor * -1) - 1);
            BusinessEnum.AuthenticationResult result = _TestAccountService.Login(_User_ID4.Email, "_User_ID4");
            Assert.IsTrue(result == BusinessEnum.AuthenticationResult.PassphraseExpired);
        }

        [TestMethod]
        public void CannotCreateAccountWithDuplicateEmail()
        {
            BusinessEnum.UnconfirmedTeamCreateAccountIssues result = _TestAccountService.CreateNewAccount(_User_ID1.Email, "passwordlengthismorethanminimum", "fn", "sn", "0131", GetEmptyClaim(), true);
            Assert.IsTrue(result == BusinessEnum.UnconfirmedTeamCreateAccountIssues.EmailAlreadyExists);
        }

        [TestMethod]
        public void CannotCreateAccountWithInvalidPassphraseOfLowerThanRequiredLength()
        {
            BusinessEnum.UnconfirmedTeamCreateAccountIssues result = _TestAccountService.CreateNewAccount("unique@Email.com", "1", "fn", "sn", "0131", GetEmptyClaim(), true);
            Assert.IsTrue(result == BusinessEnum.UnconfirmedTeamCreateAccountIssues.InvalidPassphrase);
        }

        [TestMethod]
        public void WillCreateAccountWIthValidEmailAndPassPhrase()
        {
            string Email = "addedunique@Email.com";
            BusinessEnum.UnconfirmedTeamCreateAccountIssues result = _TestAccountService.CreateNewAccount(Email, "thisisagoodpassphrase", "fn", "sn", "0131", GetEmptyClaim(), true);
            ApplicationUser selecteduser = _TestAccountService.GetUserByEmailAddress(Email);
            Assert.IsTrue(selecteduser.Email == Email);
        }

        [TestMethod]
        public void WillNotResetPassPhraseForNonExistentUser()
        {
            string Email = "nonexistent@Email.com";
            // _MockAccountService.Expect(x => x.GenerateRandomPassPhraseFromDatabase()).Return("xxx-xxx-xxx-xxx-xxx");
            BusinessEnum.PassphraseReset result = _TestAccountService.EmailTemporaryPassPhrase(Email, "base url");
            Assert.IsTrue(result == BusinessEnum.PassphraseReset.NoSuchUser);
        }

        [TestMethod]
        public void WillNotPasswordResetAnUnconfirmedAccount()
        {
            string Email = "nonexistent@Email.com";
            BusinessEnum.PassphraseReset result = _TestAccountService.EmailTemporaryPassPhrase(_User_ID7.Email, "base url");
            Assert.IsTrue(result == BusinessEnum.PassphraseReset.Accountnotvalidated);
        }

        [TestMethod]
        public void WillResetPassPhrase()
        {
            BusinessEnum.PassphraseReset result = _TestAccountService.EmailTemporaryPassPhrase(_User_ID2.Email, "base url");
            Assert.IsTrue(result == BusinessEnum.PassphraseReset.Success);
        }



        [TestMethod]
        public void WillRequireTOResetPassphrase()
        {
            BusinessEnum.PassphraseReset result = _TestAccountService.EmailTemporaryPassPhrase(_User_ID2.Email, "base url");
            Assert.IsTrue(result == BusinessEnum.PassphraseReset.Success);
        }

        [TestMethod]
        public void CannotChangePassPhraseAsPassphrasePreviouslyUsed()
        {
            BusinessEnum.PassphraseCreation result = _TestAccountService.UpdatePassPhrase("TBD", _User_ID2.Email, "_User_ID2", _User_ID2.Id, "_User_ID21");
            Assert.IsTrue(result == BusinessEnum.PassphraseCreation.PreviouslyUsed);
        }

        [TestMethod]
        public void CanotChangePassPhraseTooShort()
        {
            BusinessEnum.PassphraseCreation result = _TestAccountService.UpdatePassPhrase("TBD", _User_ID2.Email, "_User_ID2", _User_ID2.Id, "short");
            Assert.IsTrue(result == BusinessEnum.PassphraseCreation.NotValidPassPhrase);
        }


        [TestMethod]
        public void CanotChangePassPhraseSameAsExisting()
        {
            BusinessEnum.PassphraseCreation result = _TestAccountService.UpdatePassPhrase("TBD", _User_ID2.Email, "thisismycurrentpassphrase", _User_ID2.Id, "thisismycurrentpassphrase");
            Assert.IsTrue(result == BusinessEnum.PassphraseCreation.SameAsExisiting);
        }

        [TestMethod]
        public void CanotChangePassPhraseNotValidNewPassphraseDueToLength()
        {
            BusinessEnum.PassphraseCreation result = _TestAccountService.UpdatePassPhrase("TBD", _User_ID2.Email, "thisismycurrentpassphrase", _User_ID2.Id, "in");
            Assert.IsTrue(result == BusinessEnum.PassphraseCreation.NotValidPassPhrase);
        }

        [TestMethod]
        public void CanotChangePassPhraseExistingPassphrtaseNotCorrect()
        {
            BusinessEnum.PassphraseCreation result = _TestAccountService.UpdatePassPhrase("TBD", _User_ID2.Email, "notcorrectexisting", _User_ID2.Id, "thisisanewvalidpassphrase");
            Assert.IsTrue(result == BusinessEnum.PassphraseCreation.Existingpasswordnotmatched);
        }

        [TestMethod]
        public void CanUpdatePassphrase()
        {
            BusinessEnum.PassphraseCreation result = _TestAccountService.UpdatePassPhrase("TBD", _User_ID2.Email, "_User_ID2", _User_ID2.Id, "thisismycurrentpassphraseupdated");
            Assert.IsTrue(result == BusinessEnum.PassphraseCreation.Success);
        }





        private void SetupTestEnvironment()
        {

           
            
            _User_ID1.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID1.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID1");

            _User_ID2.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID2.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID2");

            _User_ID3.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID3.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID3");

            _User_ID4.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID4.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID4");

            _User_ID5.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID5.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID5");

            _User_ID6.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID6.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID6");

            _User_ID7.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID7.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID7");

            _User_ID8.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID8.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID8");

            _User_ID9.Discriminator = MockConfiguration.APPDiscriminator;
            _User_ID9.PasswordHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID9");

            _TestRepository.Add(_User_ID1);
            _TestRepository.Add(_User_ID2);
            _TestRepository.Add(_User_ID3);
            _TestRepository.Add(_User_ID4);
            _TestRepository.Add(_User_ID5);
            _TestRepository.Add(_User_ID6);
            _TestRepository.Add(_User_ID7);
            _TestRepository.Add(_User_ID8);
            _TestRepository.Add(_User_ID9);

            _TestRepository.Add(_PassWord1);
            _TestRepository.Add(_PassWord2);
            _TestRepository.Add(_PassWord3);

            _TestRepository.Add(_AppConfig2);
            _TestRepository.Add(_AppConfig1);


            _PHistory1.PassphraseHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID21");
            _PHistory2.PassphraseHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID22");
            _PHistory3.PassphraseHash = _TestAccountService.GetHashedPassPhraseForStringAsString("_User_ID23");

            ///

            _PHistory1.AspNetUsersId = _User_ID2.Id;
            _PHistory2.AspNetUsersId = _User_ID2.Id;
            _PHistory3.AspNetUsersId = _User_ID3.Id;



            ///

            _TestRepository.Add(_PHistory1);
            _TestRepository.Add(_PHistory2);
            _TestRepository.Add(_PHistory3);

          

        }


        [TestCleanup]
        public void ClassCleanUp()
        {
            _TestRepository = null;
        }

        public Dictionary<string, string> GetEmptyClaim()
        {
            Dictionary<string, string> claimsavailable = new Dictionary<string, string>();

            return claimsavailable; // claimsavailable.Add("fish", "soup");
        }


    }
}
