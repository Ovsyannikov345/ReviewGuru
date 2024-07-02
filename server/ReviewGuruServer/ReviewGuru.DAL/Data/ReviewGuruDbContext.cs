using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Entities.Models;

namespace ReviewGuru.DAL.Data
{
    public class ReviewGuruDbContext : DbContext
    {
        public ReviewGuruDbContext(DbContextOptions<ReviewGuruDbContext> options) : base(options) { }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MediaAuthor> MediaAuthor { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
