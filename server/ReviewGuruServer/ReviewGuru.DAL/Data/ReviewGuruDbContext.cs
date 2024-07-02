using Microsoft.EntityFrameworkCore;

namespace ReviewGuruServer.Database
{
    public class ReviewGuruDbContext : DbContext
    {
        public ReviewGuruDbContext()
        {
        }

        public ReviewGuruDbContext(DbContextOptions<ReviewGuruDbContext> options)
        : base(options)
        {
            Database.Migrate();
        }
    }
}
