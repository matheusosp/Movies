using System.Collections.Generic;
using MediatR;
using Movies.Domain.Generic;

namespace Movies.Application.Commands.Movies
{
    public class DeleteMoviesCommand : IRequest<ICommandResult>
    {
        public DeleteMoviesCommand(IEnumerable<long> ids)
        {
            Ids = ids;
        }

        public IEnumerable<long> Ids { get; set; }
    }
}
