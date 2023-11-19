using System;
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
    public class DeleteMovieCommandHandlerTests
    {
        [Test]
        public async Task DadoUmDeleteMovieCommandValidoDeveDeletarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            
            // Act
            var result = await handler.Handle(_successDeleteMovieCommand);
            
            // Assert
            result.Success.Should().BeTrue();
        }
        [Test]
        public async Task DadoUmDeleteMovieCommandInValidoNaoDeveDeletarOFilme()
        {
            // Arrange
            var handler = GetCommandHandler();
            _successDeleteMovieCommand!.Id = 0;
                
            // Act
            var result = await handler.Handle(_successDeleteMovieCommand);
            
            // Assert
            result.Success.Should().BeFalse();
        }
        [Test]
        public async Task DadoUmDeleteMovieCommandValidoNaoDeveDeletarOFilmeCasoOcorraFalhaAoSalvarNoBanco()
        {
            // Arrange
            var handler = GetCommandHandler();
            _moqUnitOfWork?
                .Setup(x => x.SaveChangesWithTransactionAsync(CancellationToken.None))
                .ReturnsAsync(0);
            
            // Act
            var result = await handler.Handle(_successDeleteMovieCommand);
            
            // Assert
            result.Success.Should().BeFalse();
            result.Error.Should().Be(BusinessErrors.FailToDeleteMovie.ToString());
        }
        
        private Mock<IMovieRepository>? _moqMovieRepository;
        private Mock<IBaseMovieHandler>? _moqBaseMovieHandler;
        
        private Mock<IUnitOfWork>? _moqUnitOfWork;
        private Mock<ICommandResult>? _moqCommandResult;
        private IMapper? _moqMapper;
        
        private DeleteMovieCommand? _successDeleteMovieCommand;
        private Movie? _movie;
        private Genre? _genre;

        private ICommandResult _commandResultSuccess;
        private DeleteMovieCommandValidator _validator;
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
            _validator = new DeleteMovieCommandValidator();
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
            _successDeleteMovieCommand = new DeleteMovieCommand(_movie.Id);
        }

        private DeleteMovieCommandHandlerFake GetCommandHandler()
        {
            return new DeleteMovieCommandHandlerFake(
                _moqBaseMovieHandler?.Object,
                _validator
            );
        }
    }
    public class DeleteMovieCommandHandlerFake
    {
        private readonly IBaseMovieHandler _baseMovieHandler;
        private readonly DeleteMovieCommandValidator _validator;

        public DeleteMovieCommandHandlerFake(IBaseMovieHandler baseMovieHandler, DeleteMovieCommandValidator validator)
        {
            _baseMovieHandler = baseMovieHandler;
            _validator = validator;
        }

        public async Task<ICommandResult> Handle(DeleteMovieCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return _baseMovieHandler.Result.Fail(string.Join('\n', validationResult.Errors));
            }

            var commandHandler = new DeleteMovieCommandHandler(_baseMovieHandler);
            return await commandHandler.Handle(command, default);
        }
    }
    
}
