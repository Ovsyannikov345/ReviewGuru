using Library.API.Extensions;
using Microsoft.EntityFrameworkCore;
using ReviewGuru.API.Extensions;
using ReviewGuru.DAL.Data;
using Serilog;

namespace ReviewGuruServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console()
            .WriteTo.File("debug_log.txt"));
            // Add services to the container.
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureSwagger();
            services.AddHttpContextAccessor();

            services.AddIdentityDbContext(builder.Configuration);
            services.AddAuthenticationBearer(builder.Configuration);
            services.AddMapper();
            services.AddBusinessLogicServices();
            services.AddDataAccessRepositories();
            services.AddAutoValidation();
            services.AddAuthorization();
            services.AddCorsPolicy(builder.Configuration);
            services.AddEmailSender();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ReviewGuruDbContext>();
                context.Database.Migrate();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers()
              .RequireAuthorization();

            app.Run();
        }
    }
}
