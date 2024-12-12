using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings
{
    public class MessageMapping : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(a => a.Id);
            builder.Property(b => b.Text).IsRequired().HasMaxLength(1500).HasColumnName("Text");
            builder.Property(c => c.Date).IsRequired().HasColumnName("Date");
            builder.Property(d => d.Status).IsRequired().HasColumnName("Status");
            builder.HasOne(e => e.From).WithMany();
            builder.HasOne(f => f.To).WithMany();
        }
    }
}
