using MyApp.Business.DomainObjects;
using OP.General.Extensions;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OP.General.Dal;
using MyApp.Business.DomainObjects.Models;
using System.Collections.Generic;
using WebMatrix.WebData;
using System.Web.Security;
using System.Web;
using System.Web.Helpers;
using System.Collections;
using OP.General.Model;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyApp.Business.Services
{
    public class AccountService : IAccountService
    {
        private IRepository _Repository;
        private ILog _Log;
        private IConfiguration _Configuration;
        private IEmailService _EmailService;


        public AccountService(IRepository repository, ILog log, IConfiguration configuration, IEmailService emailservice)
        {
            _Repository = repository;
            _Log = log;
            _Configuration = configuration;
            _EmailService = emailservice;
        }



        public Boolean UpdateLoginDate(string username)
        {

            _Log.Debug(String.Format("UpdateLoginDate {0} | {1}", username, ""));

            ApplicationUser currentuser = _Repository.GetFiltered<ApplicationUser>((x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Discriminator == _Configuration.APPDiscriminator)).SingleOrDefault();

            if (currentuser == null || currentuser.Id.IsNullOrEmpty()) return  false;

            currentuser.LastLoginDate = DateTime.UtcNow;

            return _Repository.Update(currentuser.Id, currentuser);

        }


        public BusinessEnum.AuthenticationResult Login(string username, string password)
        {

            _Log.Debug("Login - user " + username + " attempting to logging in");


            // make sure there is a valid type of password before we need to go to the database
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) return BusinessEnum.AuthenticationResult.Failed;

            // // is there a userprofile?
            //UserProfile currentuser = GetUser(username);


            ApplicationUser currentuser = _Repository.GetFiltered<ApplicationUser>((x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Discriminator == _Configuration.APPDiscriminator)).SingleOrDefault();

            if (currentuser == null) return BusinessEnum.AuthenticationResult.Failed;

            if (!currentuser.IsActive) return BusinessEnum.AuthenticationResult.AccountNotActive;

            string hashedpassword = GetHashedPassPhraseForStringAsString(password);

            if (currentuser == null || currentuser.Id.IsNullOrEmpty()) return BusinessEnum.AuthenticationResult.Failed;

            if (!currentuser.IsConfirmed) return BusinessEnum.AuthenticationResult.AccountNotConfirmed_Validated;

            if (!HashedPassPhrasesMatch(currentuser.PasswordHash, hashedpassword))
            {

                if (currentuser.PasswordFailuresSinceLastSuccess >= _Configuration.MaximumNumberOfFailedLoginAttempts && currentuser.LastPasswordFailureDate.HasValue && currentuser.LastPasswordFailureDate.Value.AddMinutes(_Configuration.AccountLockoutPeriodInMins) > DateTime.UtcNow)
                {
                    return BusinessEnum.AuthenticationResult.Locked;
                }

                if (currentuser.PasswordFailuresSinceLastSuccess >= _Configuration.MaximumNumberOfFailedLoginAttempts && currentuser.LastPasswordFailureDate.HasValue && currentuser.LastPasswordFailureDate.Value.AddMinutes(_Configuration.AccountLockoutPeriodInMins) <= DateTime.UtcNow)
                {
                    // reset the number of attempts
                    ResetPasswordFailures(currentuser);
                }

                RecordFailedLogin(currentuser);

                return BusinessEnum.AuthenticationResult.Failed;
            }
            else
            {
                if (currentuser.PasswordFailuresSinceLastSuccess >= _Configuration.MaximumNumberOfFailedLoginAttempts && currentuser.LastPasswordFailureDate.HasValue && currentuser.LastPasswordFailureDate.Value.AddMinutes(_Configuration.AccountLockoutPeriodInMins) > DateTime.UtcNow)
                {
                    return BusinessEnum.AuthenticationResult.Locked;
                }

                if (currentuser.PasswordFailuresSinceLastSuccess >= _Configuration.MaximumNumberOfFailedLoginAttempts && currentuser.LastPasswordFailureDate.HasValue && currentuser.LastPasswordFailureDate.Value.AddMinutes(_Configuration.AccountLockoutPeriodInMins) <= DateTime.UtcNow)
                {
                    // reset the number of attempts
                    ResetPasswordFailures(currentuser);
                }
            }

            if (currentuser.IsTemporaryPassPhrase) return BusinessEnum.AuthenticationResult.TempPasswordIssued;

            // has the password expired?
            if (HasPasswordExpired(currentuser)) return BusinessEnum.AuthenticationResult.PassphraseExpired;

            ResetLoginFailureCounter(currentuser);

            return BusinessEnum.AuthenticationResult.Passed;
        }


        public BusinessEnum.PassphraseCreation UpdatePassPhrase(string passwordtoken, string emailaddress, string existingpassphrase, string userId, string newpassphrase)
        {
            _Log.Debug(String.Format("UpdatePassPhrase : {0} | {1} ", emailaddress, userId));

            if (!IsValidPassphrase(newpassphrase)) return BusinessEnum.PassphraseCreation.NotValidPassPhrase;

            // are the new and old passwords identical
            if (HashedPassPhrasesMatch(GetHashedPassPhraseForStringAsString(existingpassphrase), GetHashedPassPhraseForStringAsString(newpassphrase))) return BusinessEnum.PassphraseCreation.SameAsExisiting;

            // have they already used the passphrase before
            if (!IsNewPassphrase(userId, newpassphrase)) return BusinessEnum.PassphraseCreation.PreviouslyUsed;

            ApplicationUser selecteduser = _Repository.GetFiltered<ApplicationUser>((x => x.Email.Equals(emailaddress, StringComparison.InvariantCultureIgnoreCase) && x.Discriminator == _Configuration.APPDiscriminator &&  x.Id.Equals(userId, StringComparison.InvariantCultureIgnoreCase))).SingleOrDefault();

            if (selecteduser == null)
            {
                return BusinessEnum.PassphraseCreation.NoUserFound;
            }
            else
            {

                if (!selecteduser.Email.Equals(emailaddress, StringComparison.InvariantCultureIgnoreCase)) return BusinessEnum.PassphraseCreation.NoUserFound;

                // get the current passphrase

                string currentpassphrasehash = selecteduser.PasswordHash;

                string existinghashedpassphraseenteredbyuser = GetHashedPassPhraseForStringAsString(existingpassphrase);

                string newhashedpassphrase = GetHashedPassPhraseForStringAsString(newpassphrase);

                if (!HashedPassPhrasesMatch(existinghashedpassphraseenteredbyuser, currentpassphrasehash)) return BusinessEnum.PassphraseCreation.Existingpasswordnotmatched;

                if (ChangePassPhraseAndStoreOldPassPhraseInHistory(selecteduser, currentpassphrasehash, newhashedpassphrase))
                {
                    LogoutUser();
                    return BusinessEnum.PassphraseCreation.Success;
                }

                return BusinessEnum.PassphraseCreation.Existingpasswordnotmatched;
            }

        }


        public Boolean HasPasswordExpired(ApplicationUser selectedmembership)
        {
            _Log.Debug(String.Format("HasPasswordExpired : selectedmembership {0}", selectedmembership.Id));
            if (selectedmembership.PasswordChangedDate.HasValue && selectedmembership.PasswordChangedDate.Value.AddMonths(_Configuration.MonthsPassphraseisValidFor) < DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }


        public Boolean ChangePassPhraseAndStoreOldPassPhraseInHistory(ApplicationUser selecteduser, string currentpassphrasehash, string newhashedpassphrase)
        {
            _Log.Debug(String.Format("ChangePassPhraseAndStoreOldPassPhraseInHistory :  {0} ", selecteduser.UserName));
            if (selecteduser == null) return false;

            // remove temporary password status
            selecteduser.IsTemporaryPassPhrase = false;
            _Repository.Update<ApplicationUser>(selecteduser.Id, selecteduser);

            AssignUserNewPassword(selecteduser, newhashedpassphrase);

            AddPassPhraseToUserHistory(selecteduser.Id, currentpassphrasehash);
            return true;
        }

        public void LogoutUser()
        {
            _Log.Debug(String.Format("LogoutUser"));
            try
            {
                FormsAuthentication.SignOut();
            }
            catch (Exception err) { }


            try
            {
                HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                cookie1.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Add(cookie1);

                //// clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                //HttpCookie cookie2 = new HttpCookie(_Configuration.SessionKeyForUserProfile, "");
                //cookie2.Expires = DateTime.Now.AddYears(-1);
                //HttpContext.Current.Response.Cookies.Add(cookie2);
            }
            catch (Exception err) { }

            try
            {
                // HttpContext.Current.Session[_Configuration.SessionKeyForUserProfile] = null;
            }
            catch (Exception err) { }
        }


        public Boolean RecordFailedLogin(ApplicationUser currentUser)
        {
            _Log.Debug(String.Format("RecordFailedLogin : currentUser {0}", currentUser.UserName));

            currentUser.PasswordFailuresSinceLastSuccess += 1;
            return _Repository.Update<ApplicationUser>(currentUser.Id, currentUser);
        }

        public Boolean ResetLoginFailureCounter(ApplicationUser selectedmembership)
        {
            _Log.Debug(String.Format("ResetLoginFailureCounter : selectedmembership {0}", selectedmembership.Id));
            selectedmembership.PasswordFailuresSinceLastSuccess = 0;
            selectedmembership.LastLoginDate = DateTime.UtcNow;
            return _Repository.Update<ApplicationUser>(selectedmembership.Id, selectedmembership);
        }


        //public string GetRolesForUser(UserProfile cuser)
        //{
        //    _Log.Debug(String.Format("GetRolesForUser : cuser {0}", cuser.Username));
        //    List<Role> listforoles = new List<Role>();

        //    // get all of the roles for the user
        //    var listofroleidsforuser = cuser.UserInRole;

        //    var listofallroles = GetAllSystemRolesCache();

        //    if (listofroleidsforuser == null || !listofroleidsforuser.Any()) return string.Empty;

        //    var designatedrolesfouser = from r in listofallroles
        //                                join c in listofroleidsforuser on r.RoleId equals c.RoleId
        //                                where c.RoleId == r.RoleId
        //                                select r.RoleName;

        //    if (designatedrolesfouser == null || !designatedrolesfouser.Any()) return string.Empty;

        //    return String.Join("|", designatedrolesfouser);

        //}

        //public List<Role> GetAllSystemRolesCache()
        //{
        //    string uniquekey = "GetAllSystemRolesCache";

        //    if (Utilities.GetFromCache(uniquekey) != null)
        //    {
        //        return (List<Role>)Utilities.GetFromCache(uniquekey);
        //    }
        //    else
        //    {
        //        return Utilities.SetandGetCache<List<Role>>(uniquekey, _Repository.GetAll<Role>().ToList(), 60);
        //    }
        //}


        //public List<Role> GetAllRoles()
        //{
        //    _Log.Debug(String.Format("GetAllRoles {0}", ""));

        //    string uniquekey = "GetAllRoles";

        //    if (Utilities.GetFromCache(uniquekey) != null)
        //    {
        //        return (List<Role>)Utilities.GetFromCache(uniquekey);
        //    }
        //    else
        //    {
        //        return Utilities.SetandGetCache<List<Role>>(uniquekey, _Repository.GetAll<Role>().ToList(), 60);
        //    }
        //}

        //public int GetFirstRoleIdForUserbyUserId(int userprofileId)
        //{
        //    _Log.Debug(String.Format("GetFirstRoleIdForUserbyUserId - {0} ", userprofileId));

        //    return _Repository.GetFiltered<UserInRole>().Where(x => x.UserId == userprofileId).SingleOrDefault().RoleId;
        //}


        public BusinessEnum.UnconfirmedTeamCreateAccountIssues CreateNewAccount(string emailaddress, string passphrase, string firstname, string surname, string contactnumber, Dictionary<string, string> claims, Boolean isConfirmed)
        {
            _Log.Debug("CreateNewAccount - emailaddress " + emailaddress);

            if (!IsValidPassphrase(passphrase))
            {
                return BusinessEnum.UnconfirmedTeamCreateAccountIssues.InvalidPassphrase;
            }

            if (DoesAccountExistByEmailAddress(emailaddress, null)) return BusinessEnum.UnconfirmedTeamCreateAccountIssues.EmailAlreadyExists;

            if (AddAccountToDB(emailaddress, passphrase, firstname, surname, contactnumber, claims, isConfirmed)) return BusinessEnum.UnconfirmedTeamCreateAccountIssues.Success;

            return BusinessEnum.UnconfirmedTeamCreateAccountIssues.Failure;
        }




        private Boolean AddAccountToDB(string emailaddress, string passphrase, string firstname, string surname, string contactnumber, Dictionary<string, string> claims, Boolean isConfirmed)
        {

           
            _Log.Debug(string.Format("AddAccountToDB {0}", emailaddress));
            // instantiate new objects
            ApplicationUser createduser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = emailaddress.TrimCheckForNull(),
                IsTemporaryPassPhrase = false,
                UserName = emailaddress.TrimCheckForNull(),
                IsActive = true,
                PhoneNumber = contactnumber.TrimCheckForNull(),
                Firstname = firstname.TrimCheckForNull(),
                Surname = surname.TrimCheckForNull(),
                IsConfirmed = isConfirmed,
                PasswordChangedDate = DateTime.UtcNow,
                PasswordFailuresSinceLastSuccess = 0,
                EmailConfirmed = false,
                AccessFailedCount = 0,
                Discriminator = _Configuration.APPDiscriminator
            };

            createduser.PasswordHash = GetHashedPassPhraseForStringAsString(passphrase.TrimCheckForNull());

            createduser.Claims = new List<ApplicationUserClaim>();

            List<ApplicationUserClaim> userclaims = new List<ApplicationUserClaim>();
         
            foreach (KeyValuePair<string, string> entry in claims)
            {

                createduser.Claims.Add(new ApplicationUserClaim()
                {
                    UserId = createduser.Id,
                    ClaimType = entry.Key,
                    ClaimValue = entry.Value
                });

            }

            string dbid =  _Repository.Add<ApplicationUser>(createduser);

            return dbid.IsNotNullOrEmpty();
        }

        public BusinessEnum.AccountVerificationStatus ValidateUserAccount(string emailaddress)
        {

            throw new Exception("ValidateUserAccount has not been implemented yet");

            throw new Exception("Not coded for");
            //_Log.Debug("ConfirmUserAccount - emailaddress " + emailaddress);

            //UserProfile currentUser = _Repository.GetFiltered<UserProfile>((x => x.Username == emailaddress)).SingleOrDefault();

            //currentUser.IsActive = true;
            //currentUser.TeamConfirmationURL = string.Empty;

            //MembershipTable mt = currentUser.MembershipTables.SingleOrDefault();

            //if (mt == null) return BusinessEnum.AccountVerificationStatus.Failed;

            //if (mt.IsConfirmed) return BusinessEnum.AccountVerificationStatus.AlreadyVerified;

            //mt.IsConfirmed = true;
            //_Repository.Update(currentUser.Id, currentUser);

            //// add the roles
            //if (String.IsNullOrEmpty(GetRolesForUser(currentUser)))
            //{
            //    // TODO: this should be done via virtual properties
            //    UserInRole urole = new UserInRole { RoleId = _Configuration.RoleTeamID, UserId = currentUser.Id };
            //    _Repository.Add<UserInRole>(urole);
            //}
            return BusinessEnum.AccountVerificationStatus.Success;


        }

        //public Boolean UpdateSystemAccount(ApplicationUser uprofile)
        //{
        //    _Log.Debug("UpdateSystemAccount - uprofile " + uprofile.Id);

        //    return _Repository.Update<ApplicationUser>(uprofile.Id, uprofile);
        //}

        //public Boolean UpdateExistingRoleForUserProfile(string userprofileId, int roleidtochangeto)
        //{
        //    //_Log.Debug(String.Format("UpdateExistingRoleForUserProfile : {0} {1} ", userprofileId, roleidtochangeto));

        //    //UserInRole userRole = _Repository.GetFiltered<UserInRole>().Where(x => x.Id == userprofileId).SingleOrDefault();

        //    //if (userRole != null)
        //    //{
        //    //    userRole.RoleId = roleidtochangeto;
        //    //    return _Repository.Update<UserInRole>(userRole.Id, userRole);
        //    //}

        //    return false;

        //}

        //public BusinessEnum.CreateSystemAccountStatus CreateSystemAccount(string emailaddress, string firstname, string surname, int roleId, Boolean isActive)
        //{
        //    _Log.Debug(String.Format("CreateSystemAccount : {0} {1} {2} {3}", emailaddress, firstname, surname, roleId));

        //    ApplicationUser up = new ApplicationUser();

        //    up.Firstname = firstname.TrimCheckForNull();
        //    up.Surname = surname.TrimCheckForNull();
        //    up.Email = emailaddress.TrimCheckForNull();
        //    up.IsActive = isActive;
        //    up.UserName = up.Email;
        //    up.IsTemporaryPassPhrase = true;
        //    up.PhoneNumber = "na";

        //    return CreateSystemAccount(up, roleId);
        //}

        //public BusinessEnum.CreateSystemAccountStatus CreateSystemAccount(ApplicationUser uprofile, int roleId)
        //{
        //    _Log.Debug(String.Format("CreateSystemAccount : {0} {1}", uprofile.Id, roleId));

        //    if (DoesAccountExistByEmailAddress(uprofile.Email, null)) return BusinessEnum.CreateSystemAccountStatus.EmailAlreadyExists;

        //    //MembershipTable mt = new MembershipTable
        //    //{
        //    //    CreateDate = DateTime.UtcNow,
        //    //    Password = "na",
        //    //    IsConfirmed = true,
        //    //    PasswordChangedDate = DateTime.UtcNow,
        //    //    PasswordFailuresSinceLastSuccess = 0,
        //    //    PasswordSalt = string.Empty
        //    //};

        //    string tempassphrase = GenerateRandomPassPhraseFromDatabase();

        //    uprofile.PasswordHash = GetHashedPassPhraseForStringAsString(tempassphrase);

        //    //uprofile.MembershipTables = new List<MembershipTable>() { mt };

        //    string dbid = _Repository.Add<ApplicationUser>(uprofile);

        //    if (dbid.IsNullOrEmpty()) return BusinessEnum.CreateSystemAccountStatus.Fail;

        //    //UserInRole urole = new UserInRole()
        //    //{
        //    //    RoleId = roleId,
        //    //    UserId = dbid
        //    //};

        //    //if (String.IsNullOrEmpty(GetRolesForUser(uprofile)))
        //    //{
        //    //    _Repository.Add<UserInRole>(urole);
        //    //}

        //    _EmailService.EmailSystemAccountTheirTemporaryPassPhase(uprofile.Email, tempassphrase, "TBD", dbid);

        //    return BusinessEnum.CreateSystemAccountStatus.Success;

        //}

        public string ReIssueWelcomeEmail(string userprofileId)
        {
            _Log.Debug(String.Format("ReIssueWelcomeEmail : {0}", userprofileId));

            ApplicationUser uprofile = GetUserProfileById(userprofileId);

            string tempassphrase = GenerateRandomPassPhraseFromDatabase();

            uprofile.PasswordHash = GetHashedPassPhraseForStringAsString(tempassphrase);

            uprofile.PasswordChangedDate = DateTime.UtcNow;

            _Repository.Update<ApplicationUser>(uprofile.Id, uprofile);

            _EmailService.EmailSystemAccountTheirTemporaryPassPhase(uprofile.Email, tempassphrase, "TBD", uprofile.Id);

            return String.Format("{0} {1}", uprofile.Firstname, uprofile.Surname);
        }





        public void ResendAccountSetupEmailAndResetPassphraseToSystemUser(ApplicationUser uprofile)
        {

            _Log.Debug(String.Format("ResendAccountSetupEmailAndResetPassphraseToSystemUser : {0}", uprofile.Id));

            string tempassphrase = GenerateRandomPassPhraseFromDatabase();

            ApplicationUser mt = GetUserProfileById(uprofile.Id);

            mt.PasswordHash = GetHashedPassPhraseForStringAsString(tempassphrase);

            _Repository.Update<ApplicationUser>(mt.Id, mt);

            // _EmailService.EmailSystemAccountTheirTemporaryPassPhase(uprofile.EmailAddress, tempassphrase, "TBD", uprofile.Id);
        }


        public bool IsNewPassphrase(string userId, string passphrase)
        {
            _Log.Debug(String.Format("IsNewPassphrase  {0} | {1}", userId, passphrase));

            List<PassphraseHistory> oldpassphrases = _Repository.GetFiltered<PassphraseHistory>((x => x.AspNetUsersId.Equals(userId, StringComparison.InvariantCultureIgnoreCase))).OrderByDescending(x => x.DateOfChange).Take(_Configuration.NumberHistoricPassphrasestoRecover).ToList();

            if (oldpassphrases == null || !oldpassphrases.Any()) return true;

            string hashedpassphrase = GetHashedPassPhraseForStringAsString(passphrase);

            foreach (PassphraseHistory history in oldpassphrases)
            {
                if (HashedPassPhrasesMatch(history.PassphraseHash, hashedpassphrase)) return false;
            }
            return true;
        }


        public bool IsValidPassphrase(string passphrase)
        {
            _Log.Debug("IsValidPassphrase");

            if (string.IsNullOrEmpty(passphrase)) return false;

            if (passphrase.Length < _Configuration.PassphraseMinimumLength) return false;

            return true;
        }


        //public bool DisableUser(UserProfile user)
        //{
        //    //_Log.Debug("DisableUser - user " + user.Username);

        //    //UserProfile currentUser = _Repository.GetAll<UserProfile>().Where(x => x.Username == user.Username).SingleOrDefault();

        //    //if (currentUser == null)
        //    //    throw new UserDoesNotExistsException();

        //    //currentUser.IsEnabled = false;

        //    //return _Repository.Update(currentUser.Id, currentUser);
        //    return true;
        //}


        //public bool PassphraseErrorSetLockOutOnAccount(UserProfile user, Boolean isLocked)
        //{
        //    //_Log.Debug("PassphraseLockOutUser - user " + user.Username);

        //    // UserProfile currentUser = _Repository.GetAll<UserProfile>().Where(x => x.Username == user.Username).SingleOrDefault();

        //    //if (currentUser == null)
        //    //    throw new UserDoesNotExistsException();

        //    //currentUser.IsLocked = isLocked;

        //    //if (!isLocked) currentUser.FailedAttempts = 0;

        //    //return _Repository.Update(currentUser.Id, currentUser);
        //    return true;
        //}


        //public bool EnableUser(UserProfile user)
        //{
        //    //_Log.Debug("EnableUser - user " + user.Username);

        //    //UserProfile enabledUser = _Repository.GetAll<UserProfile>().Where(x => x.Username == user.Username).SingleOrDefault();

        //    //if (enabledUser == null)
        //    //    throw new UserDoesNotExistsException();

        //    //enabledUser.IsEnabled = true;

        //    //return _Repository.Update(enabledUser.Id, enabledUser);
        //    return false;
        //}


        public Boolean ResetPasswordFailures(ApplicationUser member)
        {
            _Log.Debug("ResetPasswordFailures ");

            member.PasswordFailuresSinceLastSuccess = 0;
            return _Repository.Update<ApplicationUser>(member.Id, member);
        }

        //public UserProfile GetUser(string username)
        //{
        //    _Log.Debug("GetUser - user " + username);
        //    // eagerly load the membershiptable and the userinroles
        //    return _Repository.GetFiltered<UserProfile>((x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)), "MembershipTables")
        //       .SingleOrDefault();
        //}

        //public UserProfile GetUserCache(string username)
        //{
        //    _Log.Debug("GetUserCache " + username);

        //    string uniquekey = "GetUserCache_" + username;

        //    if (Utilities.GetFromCache(uniquekey) != null)
        //    {
        //        return (UserProfile)Utilities.GetFromCache(uniquekey);
        //    }
        //    else
        //    {
        //        UserProfile selecteduser = _Repository.GetFiltered<UserProfile>((x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)))
        //       .SingleOrDefault();
        //        return Utilities.SetandGetCache<UserProfile>(uniquekey, selecteduser, 15);
        //    }
        //}

        //public UserProfile GetUserCachebyUserProfileId(int userProfileId)
        //{
        //    _Log.Debug("GetUserCachebyUserProfileId " + userProfileId.ToString());

        //    string uniquekey = "GetUserCachebyUserProfileId_" + userProfileId.ToString();

        //    if (Utilities.GetFromCache(uniquekey) != null)
        //    {
        //        return (UserProfile)Utilities.GetFromCache(uniquekey);
        //    }
        //    else
        //    {
        //        UserProfile selecteduser = _Repository.GetFiltered<UserProfile>((x => x.Id == userProfileId)).SingleOrDefault();
        //        return Utilities.SetandGetCache<UserProfile>(uniquekey, selecteduser, 15);
        //    }
        //}


        public ApplicationUser GetUserByEmailAddress(string emailaddress)
        {
            _Log.Debug("GetUserByEmailAddress - emailaddress " + emailaddress);

            return _Repository.GetFiltered<ApplicationUser>((x => x.Email.Equals(emailaddress, StringComparison.InvariantCultureIgnoreCase) && x.Discriminator == _Configuration.APPDiscriminator)).SingleOrDefault();
        }


        public Boolean DoesAccountExistByEmailAddress(string emailaddress, string currentuserprofielId)
        {
            _Log.Debug("DoesAccountExist - emailaddress " + emailaddress);

            if (string.IsNullOrEmpty(emailaddress)) return true;

            string trimmedemailaddress = emailaddress.TrimCheckForNull();

            if (currentuserprofielId.IsNullOrEmpty())
            {
                // UserProfile selecteduser = _Repository.GetFiltered<UserProfile>((x => x.EmailAddress.Equals(trimmedemailaddress, StringComparison.InvariantCultureIgnoreCase))).SingleOrDefault();

                ApplicationUser selecteduser = _Repository.GetFiltered<ApplicationUser>((x => x.Email.Equals(trimmedemailaddress, StringComparison.InvariantCultureIgnoreCase) && x.Discriminator == _Configuration.APPDiscriminator)).SingleOrDefault();

                if (selecteduser == null) return false;
            }

            if (currentuserprofielId.IsNotNullOrEmpty())
            {
                ApplicationUser selecteduser1 = _Repository.GetFiltered<ApplicationUser>((x => x.Email.Equals(trimmedemailaddress, StringComparison.InvariantCultureIgnoreCase) && x.Discriminator == _Configuration.APPDiscriminator && x.Id.Equals(currentuserprofielId, StringComparison.InvariantCultureIgnoreCase))).SingleOrDefault();

                if (selecteduser1 == null) return false;
            }

            return true;
        }



        //public Boolean DoesAccountExistByUsername(string username)
        //{
        //    _Log.Debug("DoesAccountExistByUsername - username " + username);

        //    if (string.IsNullOrEmpty(username)) return true;

        //    UserProfile selecteduser = _Repository.GetFiltered<UserProfile>((x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) )).SingleOrDefault();

        //    if (selecteduser == null) return false;

        //    return true;
        //}

        public Boolean HashedPassPhrasesMatch(string p1, string p2)
        {
            _Log.Debug("HashedPassPhrasesMatch");
            if (p1.ToUpperCheckForNull() == p2.ToUpperCheckForNull()) return true;

            return false;
        }

        public byte[] GetHashedPassPhraseForString(string stringtohash)
        {
            _Log.Debug("GetHashedPassPhraseFromString");

            if (string.IsNullOrEmpty(stringtohash))
                return null;

            SHA256 hash = SHA256.Create();

            return hash.ComputeHash(UnicodeEncoding.Default.GetBytes(stringtohash.TrimCheckForNull()));
        }

        public string GetHashedPassPhraseForStringAsString(string stringtohash)
        {
            _Log.Debug("GetHashedPassPhraseFromString");

            if (string.IsNullOrEmpty(stringtohash))
                return null;

            SHA256 hash = SHA256.Create();
            byte[] result = hash.ComputeHash(UnicodeEncoding.Default.GetBytes(stringtohash.TrimCheckForNull()));

            //return Encoding.UTF8.GetString(result, 0, result.Length);
            return Convert.ToBase64String(result);

        }


        public BusinessEnum.PassphraseReset EmailTemporaryPassPhrase(string emailaddress, string baseURL)
        {
            _Log.Debug("EmailTemporaryPassPhrase  " + emailaddress);

            string tempassphrase = GenerateRandomPassPhraseFromDatabase();

            ApplicationUser selecteduser = GetUserByEmailAddress(emailaddress);

            if (selecteduser == null) return BusinessEnum.PassphraseReset.NoSuchUser;

            if (selecteduser.IsConfirmed == false) return BusinessEnum.PassphraseReset.Accountnotvalidated;

            string passwordtoken = "TBD";

            if (_EmailService.SendTemporaryPassphraseEmail(emailaddress, tempassphrase, passwordtoken, selecteduser.Id, baseURL, selecteduser.Firstname, selecteduser.Surname))
            {
                return ((ChangeUserToTemporaryPasswordIssue(selecteduser, tempassphrase) == true) ? BusinessEnum.PassphraseReset.Success : BusinessEnum.PassphraseReset.Fail);
            }

            return BusinessEnum.PassphraseReset.Fail;

        }

        public Boolean ChangeUserToTemporaryPasswordIssue(ApplicationUser selecteduser, string tempassphrase)
        {
            _Log.Debug(string.Format("ChangeUserToTemporaryPasswordIssue {0}", selecteduser.Id));

            selecteduser.IsTemporaryPassPhrase = true;
            _Repository.Update<ApplicationUser>(selecteduser.Id, selecteduser);

            AddPassPhraseToUserHistory(selecteduser.Id, selecteduser.PasswordHash);

            return AssignUserNewPassword(selecteduser, GetHashedPassPhraseForStringAsString(tempassphrase));
        }


        private Boolean AssignUserNewPassword(ApplicationUser mt, string passphrase)
        {
            _Log.Debug(string.Format("AssignUserNewPassword {0}", mt.Id));

            mt.PasswordChangedDate = DateTime.UtcNow;
            mt.PasswordHash = passphrase;

            return _Repository.Update<ApplicationUser>(mt.Id, mt);
        }


        //public MembershipTable GetMemberShipByUserId(int userId)
        //{
        //    _Log.Debug(String.Format("GetMemberShipByUserId - userId {0} ", userId));

        //    return _Repository.GetFiltered<MembershipTable>((x => x.UserProfileId == userId)).SingleOrDefault();
        //}

        public string AddPassPhraseToUserHistory(string userId, string hashedpassphrase)
        {
            _Log.Debug(String.Format("AddPassPhraseToUserHistory - userId {0}", userId));

            TrimPassphraseHistory(userId);

            PassphraseHistory pt = new PassphraseHistory();
            pt.Id = Guid.NewGuid().ToString();
            pt.AspNetUsersId = userId;
            pt.DateOfChange = DateTime.UtcNow;
            pt.PassphraseHash = hashedpassphrase;
            return _Repository.Add<PassphraseHistory>(pt);
        }

        public BusinessEnum.ResendAccountSignupEmails SendAccountConfirmationEmailBasedOnEmailAddress(string emailaddress, string baseUrl)
        {
            _Log.Debug(string.Format("SendAccountConfirmationEmailBasedOnEmailAddress {0}", emailaddress));

            ApplicationUser up = GetUserByEmailAddress(emailaddress.TrimCheckForNull());

            if (up == null || up.Id.IsNullOrEmpty()) return BusinessEnum.ResendAccountSignupEmails.Fail;

            if (up.IsConfirmed) return BusinessEnum.ResendAccountSignupEmails.Accountalreadyvalidated;


            //if (up.UserInRole.Where(x => x.RoleId == _Configuration.RoleUserID || x.RoleId == _Configuration.RoleAdminID).Any())
            //{
            //    
            //    ResendAccountSetupEmailAndResetPassphraseToSystemUser(up);
            //}
            //else
            //{
            //    // check that they are a team
            //    int countryid = 0;
            //    int eventid = 0;

            //    foreach (string str in up.TeamConfirmationURL.Split('&'))
            //    {
            //        if (str.Contains("ci="))
            //        {
            //            countryid = (str.Split('=')[1]).DecryptToInt();
            //            break;
            //        }
            //    }

            //    foreach (string str in up.TeamConfirmationURL.Split('&'))
            //    {
            //        if (str.Contains("tev="))
            //        {
            //            eventid = (str.Split('=')[1]).DecryptToInt();
            //            break;
            //        }
            //    }

            //    // _EmailService.SendAccountConfirmation(countryid, emailaddress.TrimCheckForNull(), up.TeamConfirmationURL, baseUrl, eventid);
            //}
            return BusinessEnum.ResendAccountSignupEmails.Success;
        }


        public void StoreConfirmationURLInUserProfileUntilValidated(string dbid, string urllink)
        {
            _Log.Debug(string.Format("StoreConfirmationURLInUserProfileUntilValidated {0} | {1}", dbid, urllink));

            ApplicationUser up = GetUserProfileById(dbid);
            up.TeamConfirmationURL = urllink;

            _Repository.Update<ApplicationUser>(up.Id, up);

        }

        public void TrimPassphraseHistory(string userId)
        {
            _Log.Debug(String.Format("TrimPassphraseHistory - userId {0}", userId));

            List<PassphraseHistory> oldpassphrases = _Repository.GetAll<PassphraseHistory>().Where(x => x.AspNetUsersId.Equals(userId, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.DateOfChange).Take(_Configuration.NumberHistoricPassphrasestoRecover).ToList();

            if (oldpassphrases != null && oldpassphrases.Any() && oldpassphrases.Count > _Configuration.NumberHistoricPassphrasestoRecover)
            {
                int counter = 0;
                try
                {
                    for (counter = _Configuration.NumberHistoricPassphrasestoRecover; counter < oldpassphrases.Count; counter++)
                    {
                        _Repository.DeleteById<PassphraseHistory>(oldpassphrases[counter].Id);
                    }
                }
                catch (Exception err)
                {
                    _Log.Error(String.Format("TrimPassphraseHistory - counter {0} {1}", counter, err.Message));
                }
            }
        }

        //public Boolean IsUserProfileActive(int userid)
        //{
        //    _Log.Debug(string.Format("IsUserProfileActive {0}", userid));

        //    return _Repository.GetFiltered<UserProfile>((x => x.Id == userid)).SingleOrDefault().IsActive;
        //}

        public Boolean SetActiveStateByUserProfileId(string userid, Boolean isActive)
        {
            _Log.Debug(string.Format("SetActiveStateByUserProfileId {0} {1}", userid, isActive));

            ApplicationUser uprofile = GetUserProfileById(userid);

            uprofile.IsActive = isActive;
            return _Repository.Update<ApplicationUser>(uprofile.Id, uprofile);
        }

        public Boolean UpdateIsConfirmedStatusByUserProfileId(string userprofileId)
        {
            _Log.Debug(string.Format("UpdateIsConfirmedStatusByUserProfileId {0} {1}", userprofileId, ""));

            ApplicationUser mt = GetUserProfileById(userprofileId);


            if (mt.Id == userprofileId && mt.IsConfirmed == false)
            {
                mt.IsConfirmed = true;
                return _Repository.Update<ApplicationUser>(mt.Id, mt);
            }
            return false;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            _Log.Debug(string.Format("GetAllUsers {0}", ""));

            List<ApplicationUser> listofusers = _Repository.GetAll<ApplicationUser>().ToList();

            return listofusers;
        }

        //public List<UserProfile> GetAllSystemUsers()
        //{
        //    _Log.Debug(string.Format("GetAllSystemUsers {0}", ""));

        //    List<UserInRole> listofUsersinroles = _Repository.GetAll<UserInRole>().Where(x => x.RoleId == _Configuration.RoleAdminID || x.RoleId == _Configuration.RoleUserID).ToList();

        //    List<UserProfile> listofusers = _Repository.GetAll<UserProfile>().Join(listofUsersinroles, e => e.Id, m => m.UserId, (e, m) => e).OrderBy(e => e.Surname).ToList();

        //    return listofusers;
        //}

        public ApplicationUser GetUserProfileById(string id)
        {
            return _Repository.GetFiltered<ApplicationUser>(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
        }

        public BusinessEnum.CreateSystemAccountStatus UpdateUserProfilebyId(string id, string firstname, string surname, string emailaddress, Boolean isActive)
        {
            _Log.Debug(string.Format("UpdateUserProfilebyId {0} {1} {2} {3}", id, firstname, surname, emailaddress));

            if (DoesAccountExistByEmailAddress(emailaddress, id)) return BusinessEnum.CreateSystemAccountStatus.EmailAlreadyExists;

            ApplicationUser selecteduser = GetUserProfileById(id);

            selecteduser.Firstname = firstname.TrimCheckForNull();
            selecteduser.Surname = surname.TrimCheckForNull();
            selecteduser.Email = emailaddress.TrimCheckForNull();
            selecteduser.UserName = emailaddress.TrimCheckForNull();
            selecteduser.IsActive = isActive;

            _Repository.Update<ApplicationUser>(id, selecteduser);

            return BusinessEnum.CreateSystemAccountStatus.Success;

        }


        //public string GetFormattedStringOfLastSuccessfulLogin(UserProfile user)
        //{
        //    _Log.Debug(string.Format("GetFormattedStringOfLastSuccessfulLogin {0}", user.Id));

        //    if (user == null || user.MembershipTables == null || user.MembershipTables[0] == null || !user.MembershipTables[0].LastLoginDate.HasValue)
        //    {
        //        return "None recorded";
        //    }

        //    return string.Format("{0} at {1} (UTC time)", user.MembershipTables[0].LastLoginDate.Value.ToShortDateString(), user.MembershipTables[0].LastLoginDate.Value.ToShortTimeString());
        //}


        //public List<UserProfile> GetAllUserProfileWhoHaventConfirmedAccount()
        //{
        //    _Log.Debug(string.Format("GetAllUserProfileWhoHaventConfirmedAccount {0}", ""));

        //    return _Repository.GetFiltered<UserProfile>((x => x.TeamConfirmationURL != null && x.TeamConfirmationURL.Length > 0)).ToList();
        //}

        public void RemoveCachedItemsWithUsernameInKey(string username)
        {

            _Log.Debug(string.Format("RemoveCachedItemsWithUsernameInKey {0}", username));

            if (String.IsNullOrEmpty(username)) return;

            try
            {
                HttpContext oc = HttpContext.Current;
                foreach (var c in oc.Cache)
                {
                    string key = ((DictionaryEntry)c).Key.ToString();
                    if (key.Contains(username.ToUpper()))
                    {
                        oc.Cache.Remove(key);
                    }
                }
            }
            catch (Exception)
            {

            }
        }



        //private bool DoesUserAlreadyExist(string username)
        //{
        //    _Log.Debug("DoesUserAlreadyExist - user " + username);

        //    return GetUser(username) != null;
        //}

        //public byte[] GetHash(string stringtohash)
        //{
        //    if (string.IsNullOrEmpty(stringtohash))
        //        return null;

        //    SHA256 hash = SHA256.Create();

        //    return hash.ComputeHash(UnicodeEncoding.Default.GetBytes(stringtohash));
        //}


        public string GenerateRandomPassPhraseFromDatabase()
        {

            _Log.Debug(String.Format("GenerateRandomPassPhraseFromDatabase {0}", ""));
            List<PassphraseWord> groups = _Repository.GetAll<PassphraseWord>().ToList();

            int[] order = new int[4];
            var cryptoProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[4];
            cryptoProvider.GetBytes(randomBytes);
            int randomNumber = Math.Abs(BitConverter.ToInt32(randomBytes, 0));

            for (var x = 0; x != 4; x++)
            {
                //get a random number 
                var number = (randomNumber % 4) + 1;
                while (order.All(o => order.Contains(number)))
                {
                    //the number is in the list so get other one.
                    randomBytes = new byte[4];
                    cryptoProvider.GetBytes(randomBytes);
                    randomNumber = Math.Abs(BitConverter.ToInt32(randomBytes, 0));
                    number = (randomNumber % 4) + 1;
                }
                order[x] = number;

            }

            StringBuilder sb = new StringBuilder();
            for (var x = 0; x != 4; x++)
            {
                randomBytes = new byte[4];
                cryptoProvider.GetBytes(randomBytes);
                randomNumber = Math.Abs(BitConverter.ToInt32(randomBytes, 0));
                int location = (randomNumber % groups.Count) + 1;
                switch (order[x])
                {
                    case 1:
                        sb.Append(groups[location - 1].Group1.Decrypt());
                        break;
                    case 2:
                        sb.Append(groups[location - 1].Group2.Decrypt());
                        break;
                    case 3:
                        sb.Append(groups[location - 1].Group3.Decrypt());
                        break;
                    case 4:
                        sb.Append(groups[location - 1].Group4.Decrypt());
                        break;
                }
                sb.Append(" ");
            }

            return sb.ToString().TrimEnd();
        }


        public void EncryptPassphraseDB()
        {
            _Log.Debug(string.Format("EncryptPassphraseDB {0}", ""));

            List<PassphraseWord> listofwords = _Repository.GetAll<PassphraseWord>().ToList();

            foreach (PassphraseWord pw in listofwords)
            {
                pw.Group1 = pw.Group1.Encrypt();
                pw.Group2 = pw.Group2.Encrypt();
                pw.Group3 = pw.Group3.Encrypt();
                pw.Group4 = pw.Group4.Encrypt();
            }

            _Repository.UpdateAll<PassphraseWord>(listofwords);
        }


        public string GenerateNewPassphraseDB()
        {
            _Log.Debug(string.Format("GeneratePassphraseDB {0}", ""));


            StringBuilder sb = new StringBuilder();

            List<PassphraseWord> listofwords = _Repository.GetAll<PassphraseWord>().ToList();
            foreach (PassphraseWord pw in listofwords)
            {
                _Repository.DeleteById<PassphraseWord>(pw.Id);
            }

            string[] word1 = new string[] { "Aardvark", "Ant", "Alligator", "Sheep", "Monkey", "Fox", "Walrus", "Cat", "Dog", "Lion", "Tiger", "Gerbil", "Antelope", "Squirrel", "Goat", "Eleven", "Fifteen", "Calculator", "Display", "Mercury", "Stratocumulus" };
            string[] word2 = new string[] { "Table", "Chair", "Sofa", "Floor", "Ceiling", "Carpet", "Wall", "Window", "Curtain", "Door", "Stair", "Cupboard", "Hallway", "Kitchen", "Recliner", "Twelve", "Sixteen", "Computer", "Adapter", "Venus", "Stratus" };
            string[] word3 = new string[] { "Carrot", "Pea", "Turnip", "Leek", "Barley", "Onion", "Asparagus", "Garlic", "Cloves", "Herb", "Celery", "Potatoes", "Cabbage", "Pickle", "Mushroom", "Thirteen", "Seventen", "Abacus", "Keyboard", "Earth", "Cumulus" };
            string[] word4 = new string[] { "Blue", "Green", "Yellow", "Pink", "Cyan", "Beige", "Vanilla", "Fuscia", "Red", "Teal", "Black", "White", "Magenta", "Grey", "Clear", "Fourteen", "Eighteen", "Pencil", "Ethernet", "Mars", "Cumulonimbus" };

            for (int i = 0; i < word1.Length; i++)
            {
                PassphraseWord pw = new PassphraseWord()
                {
                    Id = Guid.NewGuid().ToString(),
                    Group1 = word1[i].Encrypt(),
                    Group2 = word2[i].Encrypt(),
                    Group3 = word3[i].Encrypt(),
                    Group4 = word4[i].Encrypt()
                };
                _Repository.Add<PassphraseWord>(pw);

                sb.Append(String.Format("INSERT [dbo].[Words] ([Id], [Group1], [Group2], [Group3], [Group4]) VALUES ({5}, N'{0}', N'{1}', N'{2}', N'{3}'){4}", pw.Group1, pw.Group2, pw.Group3, pw.Group4, "<br/>", pw.Id));
            }
            return sb.ToString();
        }

        public void DecryptPassphraseDB()
        {
            _Log.Debug(string.Format("DecryptPassphraseDB {0}", ""));

            List<PassphraseWord> listofwords = _Repository.GetAll<PassphraseWord>().ToList();

            foreach (PassphraseWord pw in listofwords)
            {
                pw.Group1 = pw.Group1.Decrypt();
                pw.Group2 = pw.Group2.Decrypt();
                pw.Group3 = pw.Group3.Decrypt();
                pw.Group4 = pw.Group4.Decrypt();
            }

            _Repository.UpdateAll<PassphraseWord>(listofwords);
        }






        //public void SetUserProfileActiveStatusOnlyIfStatusChanged(string userprofileId, Boolean isActiveStatus)
        //{
        //    _Log.Debug(string.Format("SetUserProfileActiveStatusOnlyIfStatusChanged {0}", userprofileId));

        //    ApplicationUser currentProfile = GetUserProfileById(userprofileId);

        //    if (isActiveStatus != currentProfile.IsActive)
        //    {
        //        currentProfile.IsActive = isActiveStatus;

        //        _Repository.Update<ApplicationUser>(currentProfile.Id, currentProfile);
        //    }
        //}


        public BusinessEnum.ApplicationChecklistUpdateResponse UpdateLoginEmailAddressForUser(string userprofileId, string emailaddress)
        {
            _Log.Debug(string.Format("UpdateLoginEmailAddressForUser {0}", userprofileId));

            if (!Regex.IsMatch(emailaddress, RegularExpression.EmailAddressRegex))
            {
                return BusinessEnum.ApplicationChecklistUpdateResponse.InvalidEmailAddress;
            }

            ApplicationUser currentProfile = GetUserProfileById(userprofileId);

            if (!UserNameIsUnique(emailaddress, userprofileId)) return BusinessEnum.ApplicationChecklistUpdateResponse.EmailAddressAlreadyUsed;

            currentProfile.UserName = currentProfile.Email = emailaddress.TrimCheckForNull();

            return _Repository.Update<ApplicationUser>(currentProfile.Id, currentProfile) == true ? BusinessEnum.ApplicationChecklistUpdateResponse.Success : BusinessEnum.ApplicationChecklistUpdateResponse.UpdateFailed;
        }

        private Boolean UserNameIsUnique(string emailaddress, string userprofileId)
        {
            if (_Repository.GetFiltered<ApplicationUser>(x => x.UserName.Equals(emailaddress, StringComparison.InvariantCultureIgnoreCase) && x.Discriminator == _Configuration.APPDiscriminator && !x.Id.Equals(userprofileId, StringComparison.InvariantCultureIgnoreCase)).Any()) return false;

            return true;
        }


        #region Helper methods

        public string BuildPassphrase(string part1, string part2, string part3, string part4)
        {
            _Log.Debug("BuildPassphrase ");
            return string.Format("{0}{1}{2}{3}", part1, part2, part3, part4);
        }

        #endregion

    }
}
