using System.Text.Json.Serialization;
using MediatR;
using Movies.Domain.Generic;

namespace Movies.Application.Commands.Movies
{
    public class UpdateMovieCommand : IRequest<ICommandResult>
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public long GenreId { get; set; }
    }
}
