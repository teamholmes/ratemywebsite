using System.Data.Entity.ModelConfiguration;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.DomainObjects.Mapping
{
    internal class WebsiteReviewDetailMap : EntityTypeConfiguration<WebsiteReviewDetail>
    {
        internal WebsiteReviewDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("WebsiteReviewDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WebsiteReviewId).HasColumnName("WebsiteReviewId");
            this.Property(t => t.DesignRating).HasColumnName("DesignRating");
            this.Property(t => t.FunctionalityRating).HasColumnName("FunctionalityRating");
            this.Property(t => t.ContentRating).HasColumnName("ContentRating");
            this.Property(t => t.Comment).HasColumnName("Comment");
        }
    }
}

