using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class WebsiteReviewMap : EntityTypeConfiguration<WebsiteReview>
    {
        internal WebsiteReviewMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("WebsiteReview");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.URL).HasColumnName("URL");
        }
    }
}

