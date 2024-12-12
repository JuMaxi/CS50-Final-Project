using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings
{
    public class AdvertMapping : IEntityTypeConfiguration<Advert>
    {
        public void Configure(EntityTypeBuilder<Advert> builder)
        {
            builder.ToTable("Adverts");
            builder.HasKey(a => a.Id);
            builder.HasOne(b => b.User).WithMany();
            //builder.Property(b => b.User).HasColumnName("User_Id");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100).HasColumnName("Name");
            builder.Property(d => d.Description).IsRequired().HasMaxLength(1000).HasColumnName("Description");
            builder.Property(e => e.Status).IsRequired().HasColumnName("Status");
            builder.HasMany(f => f.Photos).WithOne();
        }
    }
}
