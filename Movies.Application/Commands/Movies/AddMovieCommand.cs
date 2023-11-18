using MediatR;
using Movies.Domain.Generic;

namespace Movies.Application.Commands.Movies
{
    public class AddMovieCommand : IRequest<ICommandResult>
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public long GenreId { get; set; }
    }
}
