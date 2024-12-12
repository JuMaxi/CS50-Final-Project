using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings
{
    public class BlogPostMapping : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.ToTable("Blogs");
            builder.HasKey(a => a.Id);
            builder.Property(b => b.Title).IsRequired().HasMaxLength(100).HasColumnName("Title");
            builder.Property(c => c.Photo).IsRequired().HasMaxLength(200).HasColumnName("Photo");
            builder.Property(d => d.ShortDescription).IsRequired().HasMaxLength(200).HasColumnName("Short_Description");
            builder.Property(e => e.Date).IsRequired().HasColumnName("Date");
            builder.HasMany(f => f.Tags).WithOne();
            builder.HasOne(g => g.Content).WithOne(h => h.BlogPost).HasForeignKey<BlogPostContent>("BlogPostId");
        }
    }
}
