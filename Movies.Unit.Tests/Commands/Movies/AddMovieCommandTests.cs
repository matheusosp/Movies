using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Movies.Application.CommandHandlers.Movies;
using Movies.Application.Commands.Movies;
using Movies.Application.Mappings;
using Movies.Domain.Entities;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;
using NUnit.Framework;

namespace Movies.Unit.Tests.Commands.Movies
{
    [TestFixture]
    public class AddMovieCommandTests
    {
        private Mock<IMovieRepository>? _moqMovieRepository;
        private Mock<IBaseMovieHandler>? _moqBaseMovieHandler;
        
        private Mock<IUnitOfWork>? _moqUnitOfWork;
        private Mock<ICommandResult>? _moqCommandResult;
        private IMapper? _moqMapper;
        
        private AddMovieCommand? _successAddMovieCommand;
        private Movie? _movie;
        private Genre? _genre;

        private ICommandResult _commandResultSuccess;
        [SetUp]
        public void SetUp()
        {
            CreateEntities();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            _moqMapper = mapper;
            
            _moqUnitOfWork = new(MockBehavior.Default);
            _moqCommandResult = new(MockBehavior.Default);
            _moqMovieRepository = new(MockBehavior.Default);
            _moqBaseMovieHandler = new(MockBehavior.Default);
            
            MoqBaseMovies();
        }

        [Test]
        public async Task DadoUmAddMovieCommandValidoDeveAdicionarOFilme()
        {
            // Arrange
            _successAddMovieCommand!.GenreId = 0;
            _successAddMovieCommand!.Name = "";
            
            // Act
            var handler = GetCommand();
            
            var result = await handler.Handle(_successAddMovieCommand, default);

            // Assert
            result.Success.Should().BeTrue();
        }
        [Test]
        public async Task DadoUmAddMovieCommandInvalidoNaoDeveAdicionarOFilme()
        {
            // Arrange
            _successAddMovieCommand!.GenreId = 0;
            _successAddMovieCommand!.Name = "";
            
            // Act
            var handler = GetCommand();
            
            var result = await handler.Handle(_successAddMovieCommand, default);

            // Assert
            result.Success.Should().BeFalse();
        }
        private void MoqBaseMovies()
        {
            _moqCommandResult?
                .Setup(s => s.Ok())
                .Returns(_commandResultSuccess);
            _moqCommandResult?
                .Setup(s => s.Fail(It.IsAny<string>()))
                .Returns((string error) => new CommandResult(false, error));
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(1);
            
            _moqBaseMovieHandler?.Setup(s => s.Mapper).Returns(_moqMapper);
            _moqBaseMovieHandler?.Setup(s => s.Result).Returns(_moqCommandResult!.Object);
            _moqBaseMovieHandler?.Setup(s => s.UnitOfWork).Returns(_moqUnitOfWork!.Object);
            _moqBaseMovieHandler?.Setup(s => s.MovieRepository).Returns(_moqMovieRepository!.Object);
            
            
        }

        private void CreateEntities()
        {
            _commandResultSuccess = new CommandResult
            (
                true,
                string.Empty
            );
            
            _genre = new()
            {
                Id = 1,
                Name = "Aventura",
                Active = true,
                RegistrationDate = new DateTime(2010, 1, 10)
            };
            _movie = new()
            {
                Id = 1,
                Name = "A volta dos que não foram",
                Active = true,
                RegistrationDate = new DateTime(2010, 1, 11),
                Genre = _genre,
                GenreId = _genre.Id,
            };
            _successAddMovieCommand = new AddMovieCommand
            {
                Name = "A volta dos que não foram",
                Active = true,
                GenreId = 0
            };
        }
        
        private AddMovieCommandHandler GetCommand()
        {
            return new AddMovieCommandHandler(
                _moqBaseMovieHandler?.Object
            );
        }
    }
}