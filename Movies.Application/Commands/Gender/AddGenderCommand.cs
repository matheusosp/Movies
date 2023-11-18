using MediatR;
using Movies.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Domain.Generic;

namespace Movies.Application.Commands.Gender
{
    public class AddGenderCommand : IRequest<ICommandResult>
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
