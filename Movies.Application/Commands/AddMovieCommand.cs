using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediatR;
using Movies.Domain.Interfaces;

namespace Movies.Application.Commands
{
    public class AddMovieCommand : IRequest<ICommandResult>
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public long GenderId { get; set; }
    }
}
