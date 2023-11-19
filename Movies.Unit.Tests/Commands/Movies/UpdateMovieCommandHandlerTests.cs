using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public class UpdateMovieCommandHandlerTests
    {
        [Test]
        public async Task DadoUmUpdateMovieCommandValidoDeveAtualizarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _moqMovieRepository?
                .Setup(x => x.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), CancellationToken.None))
                .ReturnsAsync(_movie);;
            
            // Act
            var result = await handler.Handle(_successUpdateMovieCommand);
            
            // Assert
            result.Success.Should().BeTrue();
        }
        [Test]
        public async Task DadoUmUpdateMovieCommandComActiveFalseValidoDeveAtualizarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successUpdateMovieCommand!.Active = false;
            _moqMovieRepository?
                .Setup(x => x.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), CancellationToken.None))
                .ReturnsAsync(_movie);;
            
            // Act
            var result = await handler.Handle(_successUpdateMovieCommand);
            
            // Assert
            result.Success.Should().BeTrue();
        }
        [Test]
        public async Task DadoUmUpdateMovieCommandInvalidoNaoDeveAtualizarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successUpdateMovieCommand!.Name = string.Empty;
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(0);
            _moqMovieRepository?
                .Setup(x => x.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), CancellationToken.None))
                .ReturnsAsync(_movie);;
            
            // Act
            var result = await handler.Handle(_successUpdateMovieCommand);
            
            // Assert
            result.Success.Should().BeFalse();
        }
        [Test]
        public async Task DadoUmUpdateMovieCommandInvalidoComIdNaoDeveAtualizarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successUpdateMovieCommand!.Id = 0;
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(0);
            _moqMovieRepository?
                .Setup(x => x.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), CancellationToken.None))
                .ReturnsAsync(_movie);;
            
            // Act
            var result = await handler.Handle(_successUpdateMovieCommand);
            
            // Assert
            result.Success.Should().BeFalse();
        }
        [Test]
        public async Task DadoUmUpdateMovieCommandInvalidoComGenreNaoDeveAtualizarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successUpdateMovieCommand!.GenreId = 0;
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(0);
            _moqMovieRepository?
                .Setup(x => x.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), CancellationToken.None))
                .ReturnsAsync(_movie);;
            // Act
            var result = await handler.Handle(_successUpdateMovieCommand);
            
            // Assert
            result.Success.Should().BeFalse();
        }
        [Test]
        public async Task DadoUmUpdateMovieCommandValidNaoDeveAlterarOFilmeCasoOcorraFalhaAoSalvarNoBanco()
        {
            // Arrange
            var handler = GetCommandHandler();
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(0);
            _moqMovieRepository?
                .Setup(x => x.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), CancellationToken.None))
                .ReturnsAsync(_movie);;
            
            // Act
            var result = await handler.Handle(_successUpdateMovieCommand);

            // Assert
            result.Success.Should().BeFalse();
            result.Error.Should().Be(BusinessErrors.FailToUpdateMovie.ToString());
        }
        [Test]
        public async Task DadoUmUpdateMovieCommandComMovieInexistenteNaoDeveAlterarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(0);
            _moqMovieRepository?
                .Setup(x => x.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), CancellationToken.None))
                .ReturnsAsync(null as Movie);;
            
            // Act
            var result = await handler.Handle(_successUpdateMovieCommand);
        
            // Assert
            result.Success.Should().BeFalse();
            result.Error.Should().Be(BusinessErrors.MovieNotFound.ToString());
        }
        [Test]
        public async Task DadoUmAddMovieCommandComGenreInativoNaoDeveAlterarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            var movie = new Movie
            {
                Id = _movie!.Id,
                Name = _movie.Name,
                Active = _movie.Active,
                RegistrationDate = _movie.RegistrationDate,
                Genre = _movie.Genre,
                GenreId = _movie.GenreId
            };
            movie.Genre.Active = false;
            _moqMovieRepository?
                .Setup(x => x.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), CancellationToken.None))
                .ReturnsAsync(movie);;
            
            // Act
            var result = await handler.Handle(_successUpdateMovieCommand);

            // Assert
            result.Success.Should().BeFalse();
            result.Error.Should().Be(BusinessErrors.MovieGenreIsInactive.ToString());
        }
        private Mock<IMovieRepository>? _moqMovieRepository;
        private Mock<IBaseMovieHandler>? _moqBaseMovieHandler;
        
        private Mock<IUnitOfWork>? _moqUnitOfWork;
        private Mock<ICommandResult>? _moqCommandResult;
        private IMapper? _moqMapper;
        
        private UpdateMovieCommand? _successUpdateMovieCommand;
        private Movie? _movie;
        private Genre? _genre;

        private ICommandResult _commandResultSuccess;
        private UpdateMovieCommandValidator _validator;
        

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
            _validator = new UpdateMovieCommandValidator();
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

            _successUpdateMovieCommand = new UpdateMovieCommand
            {
                Id = _movie.Id,
                Active = _movie.Active,
                Name = _movie.Name,
                GenreId = _movie.GenreId
            };
        }
        private UpdateMovieCommandHandlerFake GetCommandHandler()
        {
            return new UpdateMovieCommandHandlerFake(
                _moqBaseMovieHandler?.Object,
                _validator
            );
        }
    }
    public class UpdateMovieCommandHandlerFake
    {
        private readonly IBaseMovieHandler _baseMovieHandler;
        private readonly UpdateMovieCommandValidator _validator;

        public UpdateMovieCommandHandlerFake(IBaseMovieHandler baseMovieHandler, UpdateMovieCommandValidator validator)
        {
            _baseMovieHandler = baseMovieHandler;
            _validator = validator;
        }

        public async Task<ICommandResult> Handle(UpdateMovieCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return _baseMovieHandler.Result.Fail(string.Join('\n', validationResult.Errors));
            }

            var commandHandler = new UpdateMovieCommandHandler(_baseMovieHandler);
            return await commandHandler.Handle(command, default);
        }
    }
}