using AutoMapper;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;

namespace Movies.Application.CommandHandlers.Movies
{
    public class BaseMovieHandler : IBaseMovieHandler
    {
        public IMapper Mapper { get; }
        public IMovieRepository MovieRepository { get; }
        public IUnitOfWork UnitOfWork { get; }
        public ICommandResult Result { get; }

        public BaseMovieHandler(IMovieRepository movieRepository, IMapper mapper,
            IUnitOfWork unitOfWork, ICommandResult commandResult)
        {
            MovieRepository = movieRepository;
            Mapper = mapper;
            UnitOfWork = unitOfWork;
            Result = commandResult;
        }


    }
}
