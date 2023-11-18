using System.Collections.Generic;
using MediatR;
using Movies.Application.Models;
using Movies.Domain.Generic;

namespace Movies.Application.Queries.Movies
{
    public class RetrieveAllMoviesQuery : IRequest<IGenericCommandResult<IEnumerable<MovieResponse>>>
    {

    }
}
