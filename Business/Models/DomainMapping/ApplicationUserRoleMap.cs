using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class ApplicationUserRoleMap : EntityTypeConfiguration<ApplicationUserRole>
    {
        internal ApplicationUserRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);
            this.HasKey(t => t.RoleId);

            // Properties
            // Table & Column Mappings
            this.ToTable("AspNetUserRoles");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            
        }
    }
}

