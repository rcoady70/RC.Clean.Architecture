using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RC.CA.Domain.Entities.Account;
using RC.CA.Infrastructure.Persistence.Identity;

namespace RC.CA.Infrastructure.Persistence.EntityConfig;

public class JwtRefreshTokenConfiguration: IEntityTypeConfiguration<JwtRefreshToken>
{
    /// <summary>
    /// Jwt refresh tokens
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<JwtRefreshToken> builder)
    {
        //https://www.learnentityframeworkcore.com/configuration/fluent-api
        //https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties
        builder.Property(c => c.Id).IsRequired().HasColumnType(ColumnTypes.Guid);
        builder.Property(a => a.UserId).HasColumnType(ColumnTypes.EmailCol150);
        builder.HasKey(c => new { c.Id, c.UserId});

        builder.Property(a => a.CreatedByIp).HasColumnType(ColumnTypes.IPCol50);
        builder.Property(a => a.RevokedByIp).HasColumnType(ColumnTypes.IPCol50);
        builder.Property(a => a.RevokedByIp).HasColumnType(ColumnTypes.DescriptionCol50);

        builder.Property(a => a.Token).HasColumnType(ColumnTypes.UserCol100);
        builder.Property(a => a.ReasonRevoked).HasColumnType(ColumnTypes.DescriptionCol50);
    }
}

