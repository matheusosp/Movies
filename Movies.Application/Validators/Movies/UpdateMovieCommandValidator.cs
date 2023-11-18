using FluentValidation;
using Movies.Application.Commands.Movies;

namespace Movies.Application.Validators.Movies
{
    public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
    {
        public UpdateMovieCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("{PropertyName} deve ser maior que 0");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Propriedade {PropertyName} deve estar preenchida.")
                .Length(2, 200).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} characters");

            RuleFor(c => c.GenreId)
                .GreaterThan(0).WithMessage("{PropertyName} deve ser maior que 0");

        }
    }
}
