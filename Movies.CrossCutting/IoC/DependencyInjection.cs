using System;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Movies.Application;
using Movies.CrossCutting.IdentityErrors;
using Movies.Domain.Entities;
using Movies.Domain.Entities.Enums;
using Movies.Infrastructure.Context;

namespace Movies.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddValidators();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationHandler).Assembly));
        }
        public static void AddValidators(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(ApplicationHandler));
            var types = assembly?.GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains("Movies.Application"))
                .Select(t => t.Assembly);

            services.AddValidatorsFromAssemblies(types);
        }
        public static void AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            var useMemory = configuration.GetSection("UseInMemory").Value;
            if (useMemory != null && bool.Parse(useMemory))
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("Movies"));
            else
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                );
        }
        public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<Tenant, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Lockout.MaxFailedAccessAttempts = 0;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false; //(a-A-0-9)
                })
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = configuration["TokenConfiguration:Audience"],
                    ValidIssuer = configuration["TokenConfiguration:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:key"] ?? string.Empty))
                };
            });

            services.AddAuthorization(auth =>
            {
                foreach (var roleName in Enum.GetNames(typeof(Roles)))
                {
                    auth.AddPolicy(roleName, policy =>
                        policy
                            .RequireRole(roleName)
                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    );
                }
            });
        }

    }
}
