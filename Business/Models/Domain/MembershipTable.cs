using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.General;

namespace MyApp.Business.DomainObjects.Models
{
    public class MembershipTable
    {
        public int Id { get; set; }

        public int UserProfileId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string ConfirmationToken { get; set; }

        public Boolean IsConfirmed { get; set; }

        public DateTime? LastPasswordFailureDate { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        public string Password { get; set; }    /// note this is not used

        public byte[] HashedPassphrase { get; set; }

        public DateTime? PasswordChangedDate { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordVerificationToken { get; set; }

        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public virtual UserProfile UserProfile { get; set; }


        public MembershipTable()
        {

        }

    }



}
