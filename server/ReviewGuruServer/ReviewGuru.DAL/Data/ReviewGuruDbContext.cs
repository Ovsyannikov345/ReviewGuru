using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Entities.Models;

namespace ReviewGuru.DAL.Data
{
    public class ReviewGuruDbContext(DbContextOptions<ReviewGuruDbContext> options) : DbContext(options)
    {
        public DbSet<Author> Authors { get; set; }

        public DbSet<Media> Media { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.MediaId);
                entity.Property(e => e.MediaType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            });
        }
    }
}
