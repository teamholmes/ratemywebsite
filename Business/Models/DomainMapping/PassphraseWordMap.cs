using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class PassphraseWordMap : EntityTypeConfiguration<PassphraseWord>
    {
        internal PassphraseWordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Words");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Group1).HasColumnName("Group1");
            this.Property(t => t.Group2).HasColumnName("Group2");
            this.Property(t => t.Group3).HasColumnName("Group3");
            this.Property(t => t.Group4).HasColumnName("Group4");
        }
    }
}

