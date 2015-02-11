using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class UserInRoleMap : EntityTypeConfiguration<UserInRole>
    {
        internal UserInRoleMap()
        {
            // Primary Key
            //this.HasKey(t => t.UserId);
            //this.HasKey(t => t.RoleId);
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("webpages_UsersInRoles");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
        }
    }
}

