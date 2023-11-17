using Movies.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Movies.Application.Validators.Movie
{
    public class AddMovieCommandValidator : AbstractValidator<AddMovieCommand>
    {
        public AddMovieCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Propriedade {PropertyName} deve estar preenchida.")
                .Length(2, 200).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} characters");

            RuleFor(c => c.GenderId)
                .GreaterThan(0).WithMessage("{PropertyName} deve ser maior que 0");

        }
    }
}
