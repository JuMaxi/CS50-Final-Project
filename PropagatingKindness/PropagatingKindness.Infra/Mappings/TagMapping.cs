using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Infra.Mappings
{
    public class TagMapping : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            builder.HasKey(a => a.Id);  
            builder.Property(b => b.Text).IsRequired().HasMaxLength(30).HasColumnName("Text");
        }
    }
}
