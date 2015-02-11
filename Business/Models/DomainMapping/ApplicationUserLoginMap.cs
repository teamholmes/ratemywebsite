using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class ApplicationUserLoginMap : EntityTypeConfiguration<ApplicationUserLogin>
    {
        internal ApplicationUserLoginMap()
        {
            // Primary Key
            this.HasKey(t => t.LoginProvider);
            this.HasKey(t => t.UserId);
            this.HasKey(t => t.ProviderKey);

            // Properties
            // Table & Column Mappings
            this.ToTable("AspNetUserLogins");
            this.Property(t => t.LoginProvider).HasColumnName("LoginProvider");
            this.Property(t => t.ProviderKey).HasColumnName("ProviderKey");
            this.Property(t => t.UserId).HasColumnName("UserId");
            
        }
    }
}

