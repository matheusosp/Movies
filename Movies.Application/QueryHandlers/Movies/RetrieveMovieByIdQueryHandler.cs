using AutoMapper;
using MediatR;
using Movies.Application.Models;
using Movies.Application.Queries.Movies;
using Movies.Domain.Entities;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Generic;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Application.QueryHandlers.Movies
{
    public class RetrieveMovieByIdQueryHandler : IRequestHandler<RetrieveMovieByIdQuery,
        IGenericCommandResult<MovieResponse>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IGenericCommandResult<MovieResponse> _result;
        public RetrieveMovieByIdQueryHandler(IMovieRepository movieRepository, IMapper mapper,
            IGenericCommandResult<MovieResponse> result)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _result = result;
        }
        public async Task<IGenericCommandResult<MovieResponse>> Handle(RetrieveMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetBy(d => d.Id == request.Id, cancellationToken);

            var moviesResponse = _mapper.Map<Movie, MovieResponse>(movie);
            return movie == null ? _result.Fail(BusinessErrors.EntityNotFoundInDataBase.ToString()) : _result.Ok(moviesResponse);
        }
    }
}
