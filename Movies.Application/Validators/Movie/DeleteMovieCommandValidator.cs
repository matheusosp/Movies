using FluentValidation;
using Movies.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators.Movie
{
    public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
    {
        public DeleteMovieCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("{PropertyName} deve ser maior que 0");
        }
    }
}
