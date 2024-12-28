using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings;

public class ChatMapping : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");
        builder.HasKey(a => a.Id);
        builder.Property(b => b.LastUpdate).IsRequired().HasColumnName("Last_Update");
        builder.HasOne(c => c.FromUser).WithMany().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d=> d.ToUser).WithMany().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(f => f.Advert).WithMany().OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(g => g.Messages).WithOne(g => g.Chat).OnDelete(DeleteBehavior.NoAction);
    }
}
