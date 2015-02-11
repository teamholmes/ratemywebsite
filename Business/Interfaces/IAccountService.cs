using MyApp.Business.DomainObjects.Models;
using System;
using System.Collections.Generic;
namespace MyApp.Business.Services
{
    public interface IAccountService
    {
        string AddPassPhraseToUserHistory(string userId, string hashedpassphrase);
        string BuildPassphrase(string part1, string part2, string part3, string part4);
        Boolean ChangePassPhraseAndStoreOldPassPhraseInHistory(ApplicationUser selecteduser, string currentpassphrasehash, string newhashedpassphrase);
        Boolean DoesAccountExistByEmailAddress(string emailaddress, string currentuserprofielId);
        BusinessEnum.PassphraseReset EmailTemporaryPassPhrase(string emailaddress, string baseURL);
        string GenerateRandomPassPhraseFromDatabase();
       // MembershipTable GetMemberShipByUserId(int userId);
       // UserProfile GetUser(string username);
        ApplicationUser GetUserByEmailAddress(string emailaddress);
        Boolean HasPasswordExpired(ApplicationUser selectedmembership);
        bool IsNewPassphrase(string userId, string passphrase);
        bool IsValidPassphrase(string passphrase);
        BusinessEnum.AuthenticationResult Login(string username, string password);
        //bool GenerateFormsAuthenticationTicket(global::MyApp.Business.DomainObjects.Models.UserProfile cuser, MembershipTable selectedmembership);
        void LogoutUser();
        Boolean RecordFailedLogin(ApplicationUser currentUser);
        Boolean ResetLoginFailureCounter(ApplicationUser selectedmembership);
        Boolean ResetPasswordFailures(ApplicationUser member);
        void TrimPassphraseHistory(string userId);
        BusinessEnum.PassphraseCreation UpdatePassPhrase(string passwordtoken, string emailaddress, string existingpassphrase, string userId, string newpassphrase);
        Boolean ChangeUserToTemporaryPasswordIssue(ApplicationUser selecteduser, string tempassphrase);
        //List<Role> GetAllRoles();
        byte[] GetHashedPassPhraseForString(string stringtohash);
        Boolean HashedPassPhrasesMatch(string p1, string p2);
        BusinessEnum.UnconfirmedTeamCreateAccountIssues CreateNewAccount(string emailaddress, string passphrase, string firstname, string surname, string contactnumber, Dictionary<string, string> claims, Boolean isConfirmed);
       // Boolean DoesAccountExistByUsername(string username);
        BusinessEnum.AccountVerificationStatus ValidateUserAccount(string emailaddress);
        //Boolean IsUserProfileActive(int userid);
        Boolean SetActiveStateByUserProfileId(string userid, Boolean isActive);
       // List<UserProfile> GetAllSystemUsers();
        ApplicationUser GetUserProfileById(string id);
        //BusinessEnum.CreateSystemAccountStatus CreateSystemAccount(ApplicationUser uprofile, int roleId);
        //Boolean UpdateSystemAccount(ApplicationUser uprofile);
        void RemoveCachedItemsWithUsernameInKey(string username);
        //Boolean UpdateExistingRoleForUserProfile(string userprofileId, int roleidtochangeto);
       // int GetFirstRoleIdForUserbyUserId(int userprofileId);
        //string GetRolesForUser(UserProfile cuser);
        void ResendAccountSetupEmailAndResetPassphraseToSystemUser(ApplicationUser uprofile);
        BusinessEnum.ResendAccountSignupEmails SendAccountConfirmationEmailBasedOnEmailAddress(string emailaddress, string baseUrl);
        void StoreConfirmationURLInUserProfileUntilValidated(string dbid, string urllink);
        void EncryptPassphraseDB();
        void DecryptPassphraseDB();
       // void ClearAuthenticationTicket();
        //List<UserProfile> GetAllUserProfileWhoHaventConfirmedAccount();
        Boolean UpdateIsConfirmedStatusByUserProfileId(string userprofileId);
        List<ApplicationUser> GetAllUsers();

       // UserProfile GetUserCache(string username);
       // UserProfile GetUserCachebyUserProfileId(int userProfileId);

        string GenerateNewPassphraseDB();
        //BusinessEnum.CreateSystemAccountStatus CreateSystemAccount(string emailaddress, string firstname, string surname, int roleId, Boolean isActive);
        BusinessEnum.CreateSystemAccountStatus UpdateUserProfilebyId(string id, string firstname, string surname, string emailaddress, Boolean isActive);
        string ReIssueWelcomeEmail(string userprofileId);
       // string GetFormattedStringOfLastSuccessfulLogin(UserProfile user);
       // void SetUserProfileActiveStatusOnlyIfStatusChanged(string userprofileId, Boolean isActiveStatus);

        string GetHashedPassPhraseForStringAsString(string stringtohash);

        Boolean UpdateLoginDate(string username);

    }
}
