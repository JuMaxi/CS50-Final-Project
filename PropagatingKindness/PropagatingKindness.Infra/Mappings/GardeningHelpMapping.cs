using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings;

public class GardeningHelpMapping : IEntityTypeConfiguration<GardeningHelp>
{
    public void Configure(EntityTypeBuilder<GardeningHelp> builder)
    {
        builder.ToTable("GardeningHelpRequests");
        builder.HasKey(x => x.Id);
        builder.Property(h => h.PostCode).IsRequired().HasMaxLength(7).HasColumnName("Post_Code");
        builder.Property(h => h.Description).IsRequired().HasMaxLength(1000).HasColumnName("Description");
        builder.HasOne(h => h.User).WithMany();
        builder.Property(h => h.User).HasColumnName("User_Id");
    }
}
