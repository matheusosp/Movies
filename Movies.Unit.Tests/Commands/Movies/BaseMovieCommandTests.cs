using System;
using System.Text;
using System.Threading;
using AutoMapper;
using Moq;
using Movies.Application.CommandHandlers.Movies;
using Movies.Application.Commands.Movies;
using Movies.Application.Mappings;
using Movies.Application.Validators.Movies;
using Movies.Domain.Entities;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;
using NUnit.Framework;

namespace Movies.Unit.Tests.Commands.Movies
{
    public class BaseMovieCommandTests
    {
        public Mock<IMovieRepository>? MoqMovieRepository;
        public Mock<IBaseMovieHandler>? MoqBaseMovieHandler;
        
        public Mock<IUnitOfWork>? MoqUnitOfWork;
        public Mock<ICommandResult>? MoqCommandResult;
        public IMapper? MoqMapper;
        
        public Movie? Movie;
        public Genre? Genre;

        public ICommandResult CommandResultSuccess;
        
        public void SetUp()
        {
            CreateEntities();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            MoqMapper = mapper;
            
            MoqUnitOfWork = new(MockBehavior.Default);
            MoqCommandResult = new(MockBehavior.Default);
            MoqMovieRepository = new(MockBehavior.Default);
            MoqBaseMovieHandler = new(MockBehavior.Default);
            MoqBaseMovies();
        }
        public void MoqBaseMovies()
        {
            MoqCommandResult?
                .Setup(s => s.Ok())
                .Returns(CommandResultSuccess);
            MoqCommandResult?
                .Setup(s => s.Fail(It.IsAny<string>()))
                .Returns((string error) => new CommandResult(false, error));
            MoqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(1);
            
            MoqBaseMovieHandler?.Setup(s => s.Mapper).Returns(MoqMapper);
            MoqBaseMovieHandler?.Setup(s => s.Result).Returns(MoqCommandResult!.Object);
            MoqBaseMovieHandler?.Setup(s => s.UnitOfWork).Returns(MoqUnitOfWork!.Object);
            MoqBaseMovieHandler?.Setup(s => s.MovieRepository).Returns(MoqMovieRepository!.Object);
            
            
        }

        public void CreateEntities()
        {
            CommandResultSuccess = new CommandResult
            (
                true,
                string.Empty
            );
            
            Genre = new()
            {
                Id = 1,
                Name = "Aventura",
                Active = true,
                RegistrationDate = new DateTime(2010, 1, 10)
            };
            Movie = new()
            {
                Id = 1,
                Name = "A volta dos que não foram",
                Active = true,
                RegistrationDate = new DateTime(2010, 1, 11),
                Genre = Genre,
                GenreId = Genre.Id,
            };
        }
        
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}
