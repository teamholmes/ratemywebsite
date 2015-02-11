using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class AppConfigurationMap : EntityTypeConfiguration<AppConfiguration>
    {
        internal AppConfigurationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("AppConfiguration");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Value).HasColumnName("Value");
        }
    }
}
