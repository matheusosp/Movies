using System.Collections.Generic;
using MediatR;
using Movies.Domain.Generic;

namespace Movies.Application.Commands.MoviesRent
{
    public class AddMoviesRentCommand : IRequest<ICommandResult>
    {
        public List<long> MoviesIds { get; set; }
        public string CPFClient { get; set; }
    }
}
