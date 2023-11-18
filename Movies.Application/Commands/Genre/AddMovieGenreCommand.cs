using MediatR;
using Movies.Domain.Generic;

namespace Movies.Application.Commands.Genre
{
    public class AddMovieGenreCommand : IRequest<ICommandResult>
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
