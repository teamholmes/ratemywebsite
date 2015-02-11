using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OP.General;
using OP.General.Attributes;

namespace MyApp.Business.DomainObjects.Models
{
    public class BusinessEnum
    {

        public enum UnconfirmedTeamCreateAccountIssues
        {
            EmailAlreadyExists = -1,
            Success = -2,
            InvalidPassphrase = -3,
            Failure = -4
        };

        public enum ApplicationChecklistUpdateResponse
        {
            Success = 10,
            UpdateFailed = 20,
            EmailRequestingEmployerToCheckDetailsIssuedSuccess = 30,
            EmailRequestingEmployerToCheckDetailsIssuedSFailure = 40,
            EmailAcceptance_Success = 50,
            EmailAcceptance_Fail = 60,
            InvalidPNumberEntered = 70,
            InvalidScenario = 80,
            InvalidEmailAddress = 90,
            EmailAddressAlreadyUsed = 100
        }



        public enum AuthenticationResult
        {
            Failed = 1,
            Locked = 2,
            PassphraseExpired = 3,
            Passed = 4,
            AccountValidated = 5,
            TempPasswordIssued = 6,
            AccountNotActive = 7,
            AccountNotConfirmed_Validated = 8
        };

        public enum PassphraseCreation
        {
            PreviouslyUsed = 1,
            NotValidPassPhrase = 2,
            NoUserFound = 3,
            SameAsExisiting = 4,
            Success = 5,
            Failure = 6,
            Existingpasswordnotmatched = 7
        };




        public enum AccountVerificationStatus
        {
            Success = 1,
            AlreadyVerified = 2,
            Failed = 3

        };


        public enum PassphraseReset
        {
            Success = 1,
            Fail = 2,
            NoSuchUser = 3,
            Accountnotvalidated = 4

        };



        public enum CreateSystemAccountStatus
        {
            Success = 1,
            EmailAlreadyExists = 2,
            Fail = 3

        };

        public enum ResendAccountSignupEmails
        {
            Success = 1,
            Fail = 2,
            Accountalreadyvalidated = 3

        };


        public enum FormAction
        {
            Add = 1,
            Edit = 2

        };


     

        public BusinessEnum()
        {

        }

    }



}
