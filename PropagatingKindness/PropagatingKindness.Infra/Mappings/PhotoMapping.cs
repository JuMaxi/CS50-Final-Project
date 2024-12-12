using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings
{
    public class PhotoMapping : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.ToTable("Photos");
            builder.HasKey(a => a.Id);
            builder.Property(b => b.Location).IsRequired().HasMaxLength(200).HasColumnName("Location");
        }
    }
}
