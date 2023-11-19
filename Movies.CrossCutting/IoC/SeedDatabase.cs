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
using Movies.Domain.Interfaces;

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

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Tenant>>();
            var movieRepository = scope.ServiceProvider.GetRequiredService<IMovieRepository>();
            var movieGenreRepository = scope.ServiceProvider.GetRequiredService<IMovieGenreRepository>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await CreateRoles(roleManager);

            await AddTenant(userManager);

            await AddGenres(movieGenreRepository, unitOfWork);

            await AddMovies(movieRepository, movieGenreRepository, unitOfWork);
        }

        private async Task AddGenres(IMovieGenreRepository genreRepository, IUnitOfWork unitOfWork)
        {
            var genres = new List<Genre>
            {
                new()
                {
                    Name = "Drama",
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "Romance",
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "Terror",
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "Suspense",
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "Musical",
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "Fantasia",
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
             
            };

            await genreRepository.CreateGenres(genres, default);
            await unitOfWork.SaveChangesAsync(default);
        }

        private async Task AddMovies(IMovieRepository movieRepository, IMovieGenreRepository genreRepository, IUnitOfWork unitOfWork)
        {
            var genres = await genreRepository.GetAll(default);
            var random = new Random();
            var movies = new List<Movie> 
            { 
                new()
                { 
                    Name = "The Shawshank Redemption",
                    GenreId = genres.ToArray()[random.Next(0, genres.Count())].Id,
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "The Godfather",
                    GenreId = genres.ToArray()[random.Next(0, genres.Count())].Id,
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "The Dark Knight",
                    GenreId = genres.ToArray()[random.Next(0, genres.Count())].Id,
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "The Godfather: Part II",
                    GenreId = genres.ToArray()[random.Next(0, genres.Count())].Id,
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "The Lord of the Rings: The Return of the King",
                    GenreId = genres.ToArray()[random.Next(0, genres.Count())].Id,
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "Pulp Fiction",
                    GenreId = genres.ToArray()[random.Next(0, genres.Count())].Id,
                    Active = true,
                    RegistrationDate = DateTime.Now
                },
                new()
                {
                    Name = "Schindler's List",
                    GenreId = genres.ToArray()[random.Next(0, genres.Count())].Id,
                    Active = true,
                    RegistrationDate = DateTime.Now
                },

            };
            await movieRepository.CreateMovies(movies, default);
            await unitOfWork.SaveChangesAsync(default);
        }

        private async Task AddTenant(UserManager<Tenant> userManager)
        {
            var userMaster = _configuration["MasterUser:Email"] ?? string.Empty;
            if (userMaster == string.Empty)
                return;

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

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in Enum.GetNames(typeof(Roles)))
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null) continue;

                role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
