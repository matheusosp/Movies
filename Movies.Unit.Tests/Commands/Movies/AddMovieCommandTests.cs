using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentValidation.Results;
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
    public class AddMovieCommandTests
    {
        [Test]
        public async Task DadoUmAddMovieCommandValidoDeveAdicionarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            
            // Act
            var result = await handler.Handle(_successAddMovieCommand);
            
            // Assert
            result.Success.Should().BeTrue();
        }
        [Test]
        public async Task DadoUmAddMovieCommandComActiveFalseValidoDeveAdicionarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successAddMovieCommand!.Active = false;
            
            // Act
            var result = await handler.Handle(_successAddMovieCommand);
            
            // Assert
            result.Success.Should().BeTrue();
        } 
        [Test]
        public async Task DadoUmAddMovieCommandComGenreIdInvalidoNaoDeveAdicionarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successAddMovieCommand!.GenreId = 0;
            
            // Act
            var result = await handler.Handle(_successAddMovieCommand);

            // Assert
            result.Success.Should().BeFalse();
        }
        [Test]
        public async Task DadoUmAddMovieCommandComNameMinimoInvalidoNaoDeveAdicionarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successAddMovieCommand!.Name = string.Empty;
            
            // Act
            var result = await handler.Handle(_successAddMovieCommand);

            // Assert
            result.Success.Should().BeFalse();
        }
        [Test]
        public async Task DadoUmAddMovieCommandValidNaoDeveAdicionarOFilmeCasoOcorraFalhaAoSalvarNoBanco()
        {
            // Arrange
            var handler = GetCommandHandler();
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(0);
            
            // Act
            var result = await handler.Handle(_successAddMovieCommand);

            // Assert
            result.Success.Should().BeFalse();
            result.Error.Should().Be(BusinessErrors.FailToCreateMovie.ToString());
        }
        
        [Test]
        public async Task DadoUmAddMovieCommandComNameMaximoInvalidoNaoDeveAdicionarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successAddMovieCommand!.Name = GenerateRandomString(201);;
            
            // Act
            var result = await handler.Handle(_successAddMovieCommand);

            // Assert
            result.Success.Should().BeFalse();
        }
        
        private Mock<IMovieRepository>? _moqMovieRepository;
        private Mock<IBaseMovieHandler>? _moqBaseMovieHandler;
        
        private Mock<IUnitOfWork>? _moqUnitOfWork;
        private Mock<ICommandResult>? _moqCommandResult;
        private IMapper? _moqMapper;
        
        private AddMovieCommand? _successAddMovieCommand;
        private Movie? _movie;
        private Genre? _genre;

        private ICommandResult _commandResultSuccess;
        private AddMovieCommandValidator _validator;

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
            _validator = new AddMovieCommandValidator();
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
            _successAddMovieCommand = new AddMovieCommand
            {
                Name = "A volta dos que não foram",
                Active = true,
                GenreId = 1
            };
        }

        private AddMovieCommandHandlerFake GetCommandHandler()
        {
            return new AddMovieCommandHandlerFake(
                _moqBaseMovieHandler?.Object,
                _validator
            );
        }
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
    public class AddMovieCommandHandlerFake
    {
        private readonly IBaseMovieHandler _baseMovieHandler;
        private readonly AddMovieCommandValidator _validator;

        public AddMovieCommandHandlerFake(IBaseMovieHandler baseMovieHandler, AddMovieCommandValidator validator)
        {
            _baseMovieHandler = baseMovieHandler;
            _validator = validator;
        }

        public async Task<ICommandResult> Handle(AddMovieCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return _baseMovieHandler.Result.Fail(string.Join('\n', validationResult.Errors));
            }

            var commandHandler = new AddMovieCommandHandler(_baseMovieHandler);
            return await commandHandler.Handle(command, default);
        }
    }
}
