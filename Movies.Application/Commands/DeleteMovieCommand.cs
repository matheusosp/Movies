using MediatR;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators.Movie
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
