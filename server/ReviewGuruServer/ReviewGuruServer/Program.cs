using ReviewGuru.API.Extensions;

namespace ReviewGuruServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            // Add services to the container.
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

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
              /* .RequireAuthorization()*/;

            app.Run();
        }
    }
}
