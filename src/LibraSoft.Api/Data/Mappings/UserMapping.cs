﻿using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraSoft.Api.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(i => i.Email).IsUnique();

            builder.Property(x => x.Name)
            .IsRequired(true);

            builder.Property(x => x.Telephone)
            .IsRequired(true);

            builder.Property(x => x.Password)
            .IsRequired(true);

            builder.OwnsOne(x => x.Address);

            builder.Property(x => x.Role)
            .IsRequired(true);

            builder.Property(x => x.Status)
            .IsRequired(true);
        }
    }
}
