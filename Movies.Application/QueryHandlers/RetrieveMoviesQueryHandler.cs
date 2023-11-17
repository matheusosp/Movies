using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Movies.Application.Models;
using Movies.Application.Queries;
using Movies.Domain.Interfaces;

namespace Movies.Application.QueryHandlers
{
    public class RetrieveMoviesQueryHandler : IRequestHandler<RetrieveMoviesQuery,
        IGenericCommandResult<IEnumerable<MovieResponse>>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IGenericCommandResult<IEnumerable<MovieResponse>> _result;

        public RetrieveMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper,
            IGenericCommandResult<IEnumerable<MovieResponse>> result)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _result = result;
        }
        public async Task<IGenericCommandResult<IEnumerable<MovieResponse>>> Handle(RetrieveMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.GetAll(cancellationToken);

            var moviesResponse = _mapper.Map<IEnumerable<MovieResponse>>(movies);

            return _result.Ok(moviesResponse);
        }
    }
}
