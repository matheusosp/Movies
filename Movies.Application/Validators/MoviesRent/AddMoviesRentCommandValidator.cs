using FluentValidation;
using Movies.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Application.Commands.MoviesRent;

namespace Movies.Application.Validators.MoviesRent
{
    public class AddMoviesRentCommandValidator : AbstractValidator<AddMoviesRentCommand>
    {
        public AddMoviesRentCommandValidator()
        {
            RuleFor(c => c.MoviesIds)
                .NotEmpty().WithMessage("Propriedade {PropertyName} deve estar preenchida.")
                .ForEach(id => id.GreaterThan(0).WithMessage("Cada ID deve ser maior que 0"));

            RuleFor(c => c.CPFClient)
                .NotEmpty().WithMessage("Propriedade {PropertyName} deve estar preenchida.")
                .Length(2, 14).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} characters");

        }
    }
}
