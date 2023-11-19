using MediatR;
using Movies.Application.Models;
using Movies.Application.Queries.Movies;
using Movies.Domain.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Movies.Application.Queries.Genres;
using AutoMapper;
using Movies.Domain.Interfaces;
using Movies.Domain.Entities;

namespace Movies.Application.QueryHandlers.Genres
{
    public class RetrieveAllGenresQueryHandler : IRequestHandler<RetrieveAllGenresQuery,
        IGenericCommandResult<IEnumerable<GenreResponse>>>
    {
        private readonly IMovieGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly IGenericCommandResult<IEnumerable<GenreResponse>> _result;
        

        public RetrieveAllGenresQueryHandler(IMovieGenreRepository genreRepository, IMapper mapper,
            IGenericCommandResult<IEnumerable<GenreResponse>> result)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
            _result = result;
        }
        public async Task<IGenericCommandResult<IEnumerable<GenreResponse>>> Handle(RetrieveAllGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await _genreRepository.GetAll(cancellationToken);
            var genresResponse = _mapper.Map<IEnumerable<Genre>, List<GenreResponse>>(genres);

            return _result.Ok(genresResponse);
        }
    }
}
