using AutoMapper;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;

namespace Movies.Application.CommandHandlers.Movies
{
    public interface IBaseMovieHandler
    {
        IMapper Mapper { get; }
        IMovieRepository MovieRepository { get; }
        IUnitOfWork UnitOfWork { get; }
        ICommandResult Result { get; }
    }
}
