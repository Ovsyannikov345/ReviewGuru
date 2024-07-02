using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Data;
using System;

namespace ReviewGuru.API.Extensions
{
    public static class ServicesConfiguration
    {
        public static void AddIdentityDbContext(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<ReviewGuruDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}
