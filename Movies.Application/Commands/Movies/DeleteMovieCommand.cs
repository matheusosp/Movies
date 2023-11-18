using MediatR;
using Movies.Domain.Generic;

namespace Movies.Application.Commands.Movies
{
    public class DeleteMovieCommand : IRequest<ICommandResult>
    {
        public DeleteMovieCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
