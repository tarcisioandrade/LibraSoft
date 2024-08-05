using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraSoft.Api.Database.Mappings
{
    public class RentMapping : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RentDate)
           .IsRequired(true);

            builder.Property(x => x.ExpectedReturnDate)
           .IsRequired(true);

            builder.Property(x => x.Status)
           .IsRequired(true).HasConversion<string>(); ;

            builder.Property(x => x.UserId)
           .IsRequired(true);

            builder.HasMany(r => r.Books).WithMany(b => b.Rents).UsingEntity<Dictionary<string, object>>("RentBook",
                    r => r.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                    b => b.HasOne<Rent>().WithMany().HasForeignKey("RentId"));
        }
    }
}
