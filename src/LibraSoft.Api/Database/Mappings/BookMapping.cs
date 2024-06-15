using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraSoft.Api.Database.Mappings
{
    public class BookMapping : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);


            builder.Property(x => x.Title) .IsRequired(true);

            builder.Property(x => x.Isbn).IsRequired(true);
            builder.HasIndex(x => x.Isbn).IsUnique();

            builder.Property(x => x.Publisher).IsRequired(true);

            builder.Property(x => x.PublicationAt).IsRequired(true);

            builder.Property(x => x.Status).IsRequired(true).HasConversion<string>(); ;

            builder.Property(x => x.CopiesAvailable).IsRequired(true);

            builder.Property(x => x.AuthorId).IsRequired(true);

            builder.HasMany(b => b.Categories).WithMany(b => b.Books).UsingEntity<Dictionary<string, object>>(
                "BookCategory", b => b.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                c => c.HasOne<Book>().WithMany().HasForeignKey("BookId"));
        }
    }
}
