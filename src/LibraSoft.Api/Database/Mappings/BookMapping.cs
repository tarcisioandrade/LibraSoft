using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraSoft.Api.Database.Mappings
{
    public class BookMapping : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Title).IsRequired();
            builder.Property(b => b.Isbn).IsRequired();
            builder.HasIndex(b => b.Isbn).IsUnique();
            builder.Property(b => b.Image).IsRequired();
            builder.Property(b => b.Publisher).IsRequired();
            builder.Property(b => b.PublicationAt).IsRequired();
            builder.Property(b => b.Status).IsRequired().HasConversion<string>();
            builder.Property(b => b.CopiesAvailable).IsRequired();
            builder.Property(b => b.AuthorId).IsRequired();
            builder.HasMany(b => b.Reviews).WithOne(b => b.Book).HasForeignKey(b => b.BookId);
            builder.OwnsOne(b => b.Dimensions);
            builder.Property(b => b.Sinopse).IsRequired();
            builder.Property(b => b.Language).IsRequired();
            builder.Property(b => b.CoverType).IsRequired().HasConversion<string>();
            builder.Property(b => b.PageCount).IsRequired();
            builder.HasMany(b => b.Categories).WithMany(b => b.Books).UsingEntity<Dictionary<string, object>>(
                "BookCategory", b => b.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                c => c.HasOne<Book>().WithMany().HasForeignKey("BookId"));
        }
    }
}
