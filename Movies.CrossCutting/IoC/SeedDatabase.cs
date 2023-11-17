using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Entities;
using Movies.Infrastructure.Context;

namespace Movies.CrossCutting.IoC
{
    public class SeedDatabase : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public SeedDatabase(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var roleName in Enum.GetNames(typeof(Roles)))
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null) continue;

                role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);
            }

            var userMaster = _configuration["MasterUser:Email"] ?? string.Empty;
            if (userMaster == string.Empty)
                return;

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Tenant>>();
            var user = await userManager.FindByEmailAsync(userMaster);
            if (user != null) return;

            user = new Tenant
            {
                Name = userMaster,
                UserName = userMaster,
                Email = userMaster,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
            };
            var password = _configuration["MasterUser:Password"] ?? string.Empty;
            if (password == string.Empty)
                return;
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, Roles.Master.ToString());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
