//Useful information
//
https://www.learnentityframeworkcore.com/configuration/fluent-api

https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties

//https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations
        // Serialize value type
        //builder.Property(a => a.Email)
        //    .HasConversion(
        //        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        //        v => JsonSerializer.Deserialize<System.Net.Mail.MailAddress>(v, (JsonSerializerOptions)null))
        //    .HasColumnType(ColumnTypes.EmailCol);


//https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key



-- Cheat sheet --

PK
    builder.HasKey(e => new { e.Id, e.Id1  });

FK member can have one or many experiences
  builder.HasOne(m => m.Member)
               .WithMany(e => e.Experiences)
               .HasForeignKey(m => m.MemberId)
               .OnDelete(DeleteBehavior.NoAction);