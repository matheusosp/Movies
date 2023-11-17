using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Application.Commands;

namespace Movies.Application.Validators.Movie
{
    public class DeleteMoviesCommandValidator : AbstractValidator<DeleteMoviesCommand>
    {
        public DeleteMoviesCommandValidator()
        {
            RuleFor(c => c.Ids)
                .ForEach(id => id.GreaterThan(0).WithMessage("Cada ID deve ser maior que 0"));
        }
    }
}
