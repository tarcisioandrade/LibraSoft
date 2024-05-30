using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraSoft.Api.Data.Mappings
{
    public class AuthorMapping : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
            .IsRequired(true);

            builder.Property(x => x.Status)
          .IsRequired(true);

            builder.Property(x => x.Biography)
          .IsRequired(false);

            builder.Property(x => x.DateBirth)
          .IsRequired(false);
        }
    }
}
