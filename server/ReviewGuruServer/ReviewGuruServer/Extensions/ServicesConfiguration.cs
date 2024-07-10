using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReviewGuru.DAL.Data;
using System;
using System.Text;
using ReviewGuru.BLL.Utilities.Mapping;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Services;
using ReviewGuru.DAL.Repositories.IRepositories;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.BLL.Utilities.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using ReviewGuru.BLL.Utilities.EmailSender;


namespace ReviewGuru.API.Extensions
{
    public static class ServicesConfiguration
    {
        public static void AddIdentityDbContext(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<ReviewGuruDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        public static void AddAuthenticationBearer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudiences = configuration.GetSection("Jwt:Audiences").Get<string[]>(),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:AccessSecretKey"]!))
                };
            });
        }

        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                        builder => builder.WithOrigins("http://localhost:3000")
                                          .AllowAnyMethod()
                                          .AllowAnyHeader()
                                          .AllowCredentials());
            });
        }

        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }


        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

            services.AddScoped<ITokenService, TokenService>()
                    .AddScoped<IAuthService, AuthService>()
                    .AddScoped<IAuthorService, AuthorService>()
                    .AddScoped<IMediaService, MediaService>()
                    .AddScoped<IReviewService, ReviewService>()
                    .AddScoped<IUserService, UserService>();
        }

        public static void AddDataAccessRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
                    .AddScoped<IUserRepository, UserRepository>()
                    .AddScoped<IAuthorRepository, AuthorRepository>()
                    .AddScoped<IMediaRepository, MediaRepository>()
                    .AddScoped<IReviewRepository, ReviewRepository>();
        }

        public static void AddAutoValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegistrationValidator>();
        }

        public static void AddEmailSender(this IServiceCollection services)
        {
            services.AddTransient<ReviewGuru.BLL.Utilities.EmailSender.IEmailSender, EmailSender>();
        }
    }
}
