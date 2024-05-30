using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraSoft.Api.Data.Mappings
{
    public class BookMapping : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
            .IsRequired(true);

            builder.Property(x => x.Isbn)
           .IsRequired(true);

            builder.Property(x => x.Publisher)
            .IsRequired(true);

            builder.Property(x => x.PublicationAt)
            .IsRequired(true);

            builder.Property(x => x.Status)
            .IsRequired(true);

            builder.Property(x => x.CopiesAvailable)
          .IsRequired(true);

            builder.Property(x => x.AuthorId)
          .IsRequired(true);
        }
    }
}
