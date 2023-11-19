using System;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using Moq;
using Movies.Application.CommandHandlers.Movies;
using Movies.Application.Commands.Movies;
using Movies.Application.Mappings;
using Movies.Application.Models;
using Movies.Application.Queries.Movies;
using Movies.Application.Validators.Movies;
using Movies.Domain.Entities;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;
using NUnit.Framework;

namespace Movies.Unit.Tests.Commands.Movies
{
    [TestFixture]
    public class RetrieveAllMoviesQueryHandlerTests
    {
        private Mock<IMovieRepository>? _moqMovieRepository;
        private Mock<IBaseMovieHandler>? _moqBaseMovieHandler;
        
        private Mock<IUnitOfWork>? _moqUnitOfWork;
        private Mock<IGenericCommandResult<IEnumerable<MovieResponse>>>? _moqCommandResult;
        private IMapper? _moqMapper;
        
        private RetrieveAllMoviesQuery? _successRetrieveAllMoviesQuery;
        private Movie? _movie;
        private Genre? _genre;

        private IGenericCommandResult<IEnumerable<MovieResponse>> _commandResultSuccess;
        

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
        private void MoqBaseMovies()
        {
            _moqCommandResult?
                .Setup(s => s.Ok(new List<MovieResponse>()
                {
                    new ()
                    {
                        Id = _movie.Id,
                        Name = _movie.Name,
                        Active = _movie.Active,
                        RegistrationDate = _movie.RegistrationDate,
                        Genre = _movie.Genre
                    }
                }))
                .Returns(_commandResultSuccess);
            _moqCommandResult?
                .Setup(s => s.Fail(It.IsAny<string>()))
                .Returns((string error) => _commandResultSuccess(false, error));
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
}