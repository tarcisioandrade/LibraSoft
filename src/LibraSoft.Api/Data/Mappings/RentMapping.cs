using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraSoft.Api.Data.Mappings
{
    public class RentMapping : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RentDate)
           .IsRequired(true);

            builder.Property(x => x.ReturnDate)
           .IsRequired(true);

            builder.Property(x => x.Status)
           .IsRequired(true);

            builder.Property(x => x.UserId)
           .IsRequired(true);
        }
    }
}
