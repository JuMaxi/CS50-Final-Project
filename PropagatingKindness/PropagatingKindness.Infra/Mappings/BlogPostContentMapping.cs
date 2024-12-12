using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings
{
    public class BlogPostContentMapping : IEntityTypeConfiguration<BlogPostContent>
    {
        public void Configure(EntityTypeBuilder<BlogPostContent> builder)
        {
            builder.ToTable("Post_Contents");
            builder.HasKey(a => a.Id);
            builder.Property(b => b.Content).IsRequired().HasColumnName("Content");
        }
    }
}
