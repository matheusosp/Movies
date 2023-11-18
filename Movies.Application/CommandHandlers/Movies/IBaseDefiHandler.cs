using AutoMapper;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Domain.Generic;

namespace Movies.Application.CommandHandlers
{
    public interface IBaseMovieHandler
    {
        IMapper Mapper { get; }
        IMovieRepository MovieRepository { get; }
        IUnitOfWork UnitOfWork { get; }
        ICommandResult Result { get; }
    }
}
