using AutoMapper;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;

namespace Movies.Application.CommandHandlers.Genres
{
    public class BaseMovieGenreHandler : IBaseMovieGenreHandler
    {
        public BaseMovieGenreHandler(IMovieGenreRepository movieGenreRepository, IMapper mapper,
            IUnitOfWork unitOfWork, ICommandResult commandResult)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
            MovieGenreRepository = movieGenreRepository;
            Result = commandResult;
        }

        public IMapper Mapper { get; }
        public IMovieGenreRepository MovieGenreRepository { get; }
        public IUnitOfWork UnitOfWork { get; }
        public ICommandResult Result { get; }
    }
}
