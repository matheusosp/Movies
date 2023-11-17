using MediatR;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Commands
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
