using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class PassphraseHistoryMap : EntityTypeConfiguration<PassphraseHistory>
    {
        internal PassphraseHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PassphraseHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AspNetUsersId).HasColumnName("AspNetUsersId");
            this.Property(t => t.DateOfChange).HasColumnName("DateOfChange");
            this.Property(t => t.PassphraseHash).HasColumnName("PassphraseHash");
        }
    }
}

