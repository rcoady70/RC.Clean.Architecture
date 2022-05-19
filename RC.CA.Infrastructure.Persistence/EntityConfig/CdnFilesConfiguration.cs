using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RC.CA.Domain.Entities.Cdn;

namespace RC.CA.Infrastructure.Persistence.EntityConfig
{
    public class CdnFilesConfiguration :IEntityTypeConfiguration<CdnFiles>
    {
        //https://andrewhalil.com/2021/06/23/how-to-implement-a-file-uploader-using-net-core-web-api/
        public void Configure(EntityTypeBuilder<CdnFiles> builder)
        {
            builder.Property(e => e.Id).IsRequired()
                                       .HasColumnType(ColumnTypes.Guid);
            builder.Property(e => e.FileName).IsRequired()
                                             .HasColumnType(ColumnTypes.FileName250);
            builder.Property(e => e.OrginalFileName).IsRequired()
                                                    .HasColumnType(ColumnTypes.FileName250);
            builder.Property(e => e.ContentType).IsRequired()
                                                .HasColumnType(ColumnTypes.Name50);
            builder.Property(e => e.CdnLocation).IsRequired()
                                                .HasColumnType(ColumnTypes.URLCol250);
        }
    }
}
