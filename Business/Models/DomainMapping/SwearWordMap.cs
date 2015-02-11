using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class SwearWordMap : EntityTypeConfiguration<SwearWord>
    {
        internal SwearWordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SwearWord");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Word).HasColumnName("Word");
            
            
        }
    }
}

