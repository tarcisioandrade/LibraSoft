using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraSoft.Api.Database.Mappings
{
    public class ReviewMapping : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Comment).IsRequired();
            builder.Property(x => x.Status).IsRequired(true).HasConversion<string>();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.BookId).IsRequired();
            builder.HasMany(x => x.Likes).WithOne(x => x.Review).HasForeignKey(x => x.ReviewId);
        }
    }
}
