using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        internal UserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserProfile");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.EmailAddress).HasColumnName("EmailAddress");
            this.Property(t => t.Firstname).HasColumnName("Firstname");
            this.Property(t => t.Surname).HasColumnName("Surname");
            this.Property(t => t.IsTemporaryPassPhrase).HasColumnName("IsTemporaryPassPhrase");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            
        }
    }
}

