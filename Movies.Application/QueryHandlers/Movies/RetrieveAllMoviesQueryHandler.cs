using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Movies.Application.Models;
using Movies.Application.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;

namespace Movies.Application.QueryHandlers.Movies
{
    public class RetrieveAllMoviesQueryHandler : IRequestHandler<RetrieveAllMoviesQuery,
        IGenericCommandResult<IEnumerable<MovieResponse>>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IGenericCommandResult<IEnumerable<MovieResponse>> _result;

        public RetrieveAllMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper,
            IGenericCommandResult<IEnumerable<MovieResponse>> result)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _result = result;
        }
        public async Task<IGenericCommandResult<IEnumerable<MovieResponse>>> Handle(RetrieveAllMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.GetAll(cancellationToken);
            var moviesResponse = _mapper.Map<IEnumerable<Movie>, List<MovieResponse>>(movies);

            return _result.Ok(moviesResponse);
        }
    }
}
