using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class RoleMap : EntityTypeConfiguration<Role>
    {
        internal RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleId);

            // Properties
            // Table & Column Mappings
            this.ToTable("webpages_Roles");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
        }
    }
}

