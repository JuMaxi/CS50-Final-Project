using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings;

public class GardeningHelpMapping : IEntityTypeConfiguration<GardeningHelp>
{
    public void Configure(EntityTypeBuilder<GardeningHelp> builder)
    {
        builder.ToTable("GardeningHelpRequests");
        builder.HasKey(a => a.Id);
        builder.Property(b => b.Title).IsRequired().HasMaxLength(100).HasColumnName("Title");
        builder.Property(c => c.PostCode).IsRequired().HasMaxLength(7).HasColumnName("Post_Code");
        builder.Property(d => d.Description).IsRequired().HasMaxLength(1000).HasColumnName("Description");
        builder.HasOne(e => e.User).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
