using MediatR;
using System.Collections.Generic;
using Movies.Application.Models;
using Movies.Domain.Generic;

namespace Movies.Application.Queries
{
    public class RetrieveAllMoviesQuery : IRequest<IGenericCommandResult<IEnumerable<MovieResponse>>>
    {

    }
}
