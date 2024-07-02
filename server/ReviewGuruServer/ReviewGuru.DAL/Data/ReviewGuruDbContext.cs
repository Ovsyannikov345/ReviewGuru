using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Entities.Models;

namespace ReviewGuru.DAL.Data
{
    public class ReviewGuruDbContext : DbContext
    {
        public ReviewGuruDbContext(DbContextOptions<ReviewGuruDbContext> options) : base(options) { }
        public DbSet<Author> Author { get; set; }
        public DbSet<Media> Media { get; set; }
    }
}
