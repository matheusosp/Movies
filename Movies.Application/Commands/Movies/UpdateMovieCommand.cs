using MediatR;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Domain.Generic;
using System.Text.Json.Serialization;

namespace Movies.Application.Commands
{
    public class UpdateMovieCommand : IRequest<ICommandResult>
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public long GenderId { get; set; }
    }
}
