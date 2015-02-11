using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class ApplicationUserClaimMap : EntityTypeConfiguration<ApplicationUserClaim>
    {
        internal ApplicationUserClaimMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            //this.HasKey(t => t.UserId);

            // Properties
            // Table & Column Mappings
            this.ToTable("AspNetUserClaims");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ClaimType).HasColumnName("ClaimType");
            this.Property(t => t.ClaimValue).HasColumnName("ClaimValue");
            this.Property(t => t.UserId).HasColumnName("User_Id");



            //this.HasOptional(t => t.ApplicationUser).WithMany(t => t.Claimz).HasForeignKey(d => d.UserId);
        }
    }
}

