using Microsoft.AspNet.Identity.EntityFramework;
using MyApp.Business.DomainObjects.Mapping;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MyApp.Business.DomainObjects.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUserClaim //: IdentityUserClaim
    {
        public int Id { get; set; }

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserId { get; set; }


        //[ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}