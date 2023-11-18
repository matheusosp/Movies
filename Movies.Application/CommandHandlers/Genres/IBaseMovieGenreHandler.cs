using AutoMapper;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;

namespace Movies.Application.CommandHandlers.Genres
{
    public interface IBaseMovieGenreHandler
    {
        IMapper Mapper { get; }
        IMovieGenreRepository MovieGenreRepository { get; }
        IUnitOfWork UnitOfWork { get; }
        ICommandResult Result { get; }
    }
}
