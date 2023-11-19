using MediatR;
using Movies.Application.Models;
using Movies.Domain.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Queries.Genres
{
    public class RetrieveAllGenresQuery : IRequest<IGenericCommandResult<IEnumerable<GenreResponse>>>
    {
    }
}
