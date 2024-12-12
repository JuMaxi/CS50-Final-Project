using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(a => a.Id);
            builder.Property(b => b.Photo).IsRequired().HasMaxLength(200).HasColumnName("Photo");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(40).HasColumnName("Name");
            builder.Property(d => d.LastName).IsRequired().HasMaxLength(60).HasColumnName("Last_Name");
            builder.Property(e => e.Email).IsRequired().HasMaxLength(80).HasColumnName("Email");
            builder.Property(f => f.Password).IsRequired().HasMaxLength(64).HasColumnName("Hash");
            builder.Property(g => g.Birthday).IsRequired().HasColumnName("Birthday");
            builder.Property(h => h.PostCode).IsRequired().HasMaxLength(7).HasColumnName("Post_Code");

        }
    }
}
