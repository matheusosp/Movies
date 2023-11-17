using AutoMapper;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.CommandHandlers
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
