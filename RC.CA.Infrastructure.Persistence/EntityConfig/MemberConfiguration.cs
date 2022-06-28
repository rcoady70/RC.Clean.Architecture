
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Infrastructure.Persistence.EntityConfig;
public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.Id).IsRequired().HasColumnType(ColumnTypes.Guid).HasDefaultValueSql("newid()");
        builder.HasKey(m => new { m.Id });

        builder.HasMany(m => m.Experiences);

        builder.Property(m => m.Name).IsRequired().HasColumnType(ColumnTypes.Name50);
        builder.Property(m => m.Gender).IsRequired().HasColumnType(ColumnTypes.NameCol25);
        builder.Property(m => m.Qualification).IsRequired().HasColumnType(ColumnTypes.DescriptionCol50);
    }
}
