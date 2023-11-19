using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Movies.Application.CommandHandlers.Movies;
using Movies.Application.Commands.Movies;
using Movies.Application.Mappings;
using Movies.Application.Validators.Movies;
using Movies.Domain.Entities;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;
using NUnit.Framework;

namespace Movies.Unit.Tests.Commands.Movies
{
    [TestFixture]
    public class DeleteMoviesCommandHandlerTests
    {
        [Test]
        public async Task DadoUmDeleteMoviesCommandValidoDeveDeletarOsFilmes()
        {
            // Arrange
            var handler = GetCommandHandler();
            
            // Act
            var result = await handler.Handle(_successDeleteMoviesCommand);
            
            // Assert
            result.Success.Should().BeTrue();
        }
        [Test]
        public async Task DadoUmDeleteMoviesCommandInValidoNaoDeveDeletarOsFilmes()
        {
            // Arrange
            var handler = GetCommandHandler();
            var command = new DeleteMoviesCommand(new List<long>
            {
                0,1
            });
            
            // Act
            var result = await handler.Handle(command);
            
            // Assert
            result.Success.Should().BeFalse();
        }
        [Test]
        public async Task DadoUmUpdateMovieCommandValidoNaoDeveDeletarOsFilmesCasoOcorraFalhaAoSalvarNoBanco()
        {
            // Arrange
            var handler = GetCommandHandler();
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(0);
            
            // Act
            var result = await handler.Handle(_successDeleteMoviesCommand);
            
            // Assert
            result.Success.Should().BeFalse();
            result.Error.Should().Be(BusinessErrors.FailToDeleteMovies.ToString());
        }
        
        private Mock<IMovieRepository>? _moqMovieRepository;
        private Mock<IBaseMovieHandler>? _moqBaseMovieHandler;
        
        private Mock<IUnitOfWork>? _moqUnitOfWork;
        private Mock<ICommandResult>? _moqCommandResult;
        private IMapper? _moqMapper;
        
        private DeleteMoviesCommand? _successDeleteMoviesCommand;
        private Movie? _movie;
        private Movie? _movie2;
        private Genre? _genre;

        private ICommandResult _commandResultSuccess;
        private DeleteMoviesCommandValidator _validator;
        

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
            _validator = new DeleteMoviesCommandValidator();
            MoqBaseMovies();
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
            _movie2 = new()
            {
                Id = 2,
                Name = "A volta dos que não foram 2",
                Active = true,
                RegistrationDate = new DateTime(2010, 1, 11),
                Genre = _genre,
                GenreId = _genre.Id,
            };
            
            var movies = new List<long>()
            {
                _movie.Id,
                _movie2.Id
            };
            _successDeleteMoviesCommand = new DeleteMoviesCommand(movies);
        }
        private DeleteMoviesCommandHandlerFake GetCommandHandler()
        {
            return new DeleteMoviesCommandHandlerFake(
                _moqBaseMovieHandler?.Object,
                _validator
            );
        }
    }
    public class DeleteMoviesCommandHandlerFake
    {
        private readonly IBaseMovieHandler _baseMovieHandler;
        private readonly DeleteMoviesCommandValidator _validator;

        public DeleteMoviesCommandHandlerFake(IBaseMovieHandler baseMovieHandler, DeleteMoviesCommandValidator validator)
        {
            _baseMovieHandler = baseMovieHandler;
            _validator = validator;
        }

        public async Task<ICommandResult> Handle(DeleteMoviesCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return _baseMovieHandler.Result.Fail(string.Join('\n', validationResult.Errors));
            }

            var commandHandler = new DeleteMoviesCommandHandler(_baseMovieHandler);
            return await commandHandler.Handle(command, default);
        }
    }
}