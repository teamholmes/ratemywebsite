using Microsoft.AspNet.Identity.EntityFramework;
using MyApp.Business.DomainObjects.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MyApp.Business.DomainObjects.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser //: IdentityUser
    {
        public bool IsActive { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
        //public string Discriminator { get; set; }
        public Boolean IsTemporaryPassPhrase { get; set; }

        public string TeamConfirmationURL { get; set; }

        public Boolean IsConfirmed { get; set; }

        public DateTime? LastPasswordFailureDate { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        public DateTime? PasswordChangedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }


        public string Id { get; set; }
        public string Email { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }

        public Boolean PhoneNumberConfirmed { get; set; }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        public Boolean TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }
        public Boolean LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Discriminator { get; set; }




        //[ForeignKey("UserId")]
        public virtual List<ApplicationUserClaim> Claims { get; set; }

    }
}