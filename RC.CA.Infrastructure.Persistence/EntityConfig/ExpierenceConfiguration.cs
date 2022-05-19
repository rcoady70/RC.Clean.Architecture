using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Infrastructure.Persistence.EntityConfig;
public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.Property(e => e.Id).IsRequired()
                                   .HasColumnType(ColumnTypes.BigInt);
        builder.HasKey(e => new { e.Id });
                
        builder.Property(e => e.QualificationName).IsRequired().HasColumnType(ColumnTypes.Name50);

        builder.Property(e => e.Description).IsRequired().HasColumnType(ColumnTypes.DescriptionCol50);

    }
}
