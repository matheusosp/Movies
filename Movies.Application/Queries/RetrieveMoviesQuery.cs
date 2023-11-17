using MediatR;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Domain.Entities;
using Movies.Application.Models;

namespace Movies.Application.Queries
{
    public class RetrieveMoviesQuery : IRequest<IGenericCommandResult<IEnumerable<MovieResponse>>>
    {
    }
}
