using System.Reflection;
using LibraSoft.Api.Database.Mappings;
using LibraSoft.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraSoft.Api.Database
{
    public class AppDbContext : DbContext
    {
        readonly IConfiguration Configuration;

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Rent> Rents { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new AuthorMapping());
            modelBuilder.ApplyConfiguration(new BookMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new RentMapping());
            modelBuilder.ApplyConfiguration(new ReviewMapping());
            modelBuilder.ApplyConfiguration(new LikeMapping());
        }
    }
}
