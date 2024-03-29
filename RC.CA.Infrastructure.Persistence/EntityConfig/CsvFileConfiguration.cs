﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RC.CA.Domain.Entities.Account;
using RC.CA.Domain.Entities.CSV;
using RC.CA.Infrastructure.Persistence.Identity;

namespace RC.CA.Infrastructure.Persistence.EntityConfig;

public class CsvFileConfiguration: IEntityTypeConfiguration<CsvFile>
{
    public void Configure(EntityTypeBuilder<CsvFile> builder)
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
        builder.Property(e => e.Status).IsRequired();

        builder.Property(e => e.ColumnMap).HasColumnType(ColumnTypes.TextCol);
    }
}
