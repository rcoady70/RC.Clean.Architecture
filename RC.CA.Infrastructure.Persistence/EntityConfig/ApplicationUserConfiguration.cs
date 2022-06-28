using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RC.CA.Infrastructure.Persistence.Identity;

namespace RC.CA.Infrastructure.Persistence.EntityConfig;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        //https://www.learnentityframeworkcore.com/configuration/fluent-api
        //https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties
        builder.Property(a => a.FirstName).IsRequired().HasColumnType(ColumnTypes.NameCol25);
        builder.Property(a => a.LastName).IsRequired().HasColumnType(ColumnTypes.NameCol25);
    }
}
